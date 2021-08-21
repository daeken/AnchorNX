using Ryujinx.Graphics.Shader.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

using static Ryujinx.Graphics.Shader.IntermediateRepresentation.OperandHelper;

namespace Ryujinx.Graphics.Shader.Decoders
{
    static class Decoder
    {
        public const ulong ShaderEndDelimiter = 0xe2400fffff87000f;

        public static Block[][] Decode(IGpuAccessor gpuAccessor, ulong startAddress, out bool hasBindless)
        {
            hasBindless = false;

            List<Block[]> funcs = new List<Block[]>();

            Queue<ulong> funcQueue = new Queue<ulong>();
            HashSet<ulong> funcVisited = new HashSet<ulong>();

            void EnqueueFunction(ulong funcAddress)
            {
                if (funcVisited.Add(funcAddress))
                {
                    funcQueue.Enqueue(funcAddress);
                }
            }

            funcQueue.Enqueue(0);

            while (funcQueue.TryDequeue(out ulong funcAddress))
            {
                List<Block> blocks = new List<Block>();
                Queue<Block> workQueue = new Queue<Block>();
                Dictionary<ulong, Block> visited = new Dictionary<ulong, Block>();

                Block GetBlock(ulong blkAddress)
                {
                    if (!visited.TryGetValue(blkAddress, out Block block))
                    {
                        block = new Block(blkAddress);

                        workQueue.Enqueue(block);
                        visited.Add(blkAddress, block);
                    }

                    return block;
                }

                GetBlock(funcAddress);

                while (workQueue.TryDequeue(out Block currBlock))
                {
                    // Check if the current block is inside another block.
                    if (BinarySearch(blocks, currBlock.Address, out int nBlkIndex))
                    {
                        Block nBlock = blocks[nBlkIndex];

                        if (nBlock.Address == currBlock.Address)
                        {
                            throw new InvalidOperationException("Found duplicate block address on the list.");
                        }

                        nBlock.Split(currBlock);
                        blocks.Insert(nBlkIndex + 1, currBlock);

                        continue;
                    }

                    // If we have a block after the current one, set the limit address.
                    ulong limitAddress = ulong.MaxValue;

                    if (nBlkIndex != blocks.Count)
                    {
                        Block nBlock = blocks[nBlkIndex];

                        int nextIndex = nBlkIndex + 1;

                        if (nBlock.Address < currBlock.Address && nextIndex < blocks.Count)
                        {
                            limitAddress = blocks[nextIndex].Address;
                        }
                        else if (nBlock.Address > currBlock.Address)
                        {
                            limitAddress = blocks[nBlkIndex].Address;
                        }
                    }

                    FillBlock(gpuAccessor, currBlock, limitAddress, startAddress, out bool blockHasBindless);
                    hasBindless |= blockHasBindless;

                    if (currBlock.OpCodes.Count != 0)
                    {
                        // We should have blocks for all possible branch targets,
                        // including those from SSY/PBK instructions.
                        foreach (OpCodePush pushOp in currBlock.PushOpCodes)
                        {
                            GetBlock(pushOp.GetAbsoluteAddress());
                        }

                        // Set child blocks. "Branch" is the block the branch instruction
                        // points to (when taken), "Next" is the block at the next address,
                        // executed when the branch is not taken. For Unconditional Branches
                        // or end of program, Next is null.
                        OpCode lastOp = currBlock.GetLastOp();

                        if (lastOp is OpCodeBranch opBr)
                        {
                            if (lastOp.Emitter == InstEmit.Cal)
                            {
                                EnqueueFunction(opBr.GetAbsoluteAddress());
                            }
                            else
                            {
                                currBlock.Branch = GetBlock(opBr.GetAbsoluteAddress());
                            }
                        }
                        else if (lastOp is OpCodeBranchIndir opBrIndir)
                        {
                            // An indirect branch could go anywhere, we don't know the target.
                            // Those instructions are usually used on a switch to jump table
                            // compiler optimization, and in those cases the possible targets
                            // seems to be always right after the BRX itself. We can assume
                            // that the possible targets are all the blocks in-between the
                            // instruction right after the BRX, and the common target that
                            // all the "cases" should eventually jump to, acting as the
                            // switch break.
                            Block firstTarget = GetBlock(currBlock.EndAddress);

                            firstTarget.BrIndir = opBrIndir;

                            opBrIndir.PossibleTargets.Add(firstTarget);
                        }

                        if (!IsUnconditionalBranch(lastOp))
                        {
                            currBlock.Next = GetBlock(currBlock.EndAddress);
                        }
                    }

                    // Insert the new block on the list (sorted by address).
                    if (blocks.Count != 0)
                    {
                        Block nBlock = blocks[nBlkIndex];

                        blocks.Insert(nBlkIndex + (nBlock.Address < currBlock.Address ? 1 : 0), currBlock);
                    }
                    else
                    {
                        blocks.Add(currBlock);
                    }

                    // Do we have a block after the current one?
                    if (currBlock.BrIndir != null && HasBlockAfter(gpuAccessor, currBlock, startAddress))
                    {
                        bool targetVisited = visited.ContainsKey(currBlock.EndAddress);

                        Block possibleTarget = GetBlock(currBlock.EndAddress);

                        currBlock.BrIndir.PossibleTargets.Add(possibleTarget);

                        if (!targetVisited)
                        {
                            possibleTarget.BrIndir = currBlock.BrIndir;
                        }
                    }
                }

                foreach (Block block in blocks.Where(x => x.PushOpCodes.Count != 0))
                {
                    for (int pushOpIndex = 0; pushOpIndex < block.PushOpCodes.Count; pushOpIndex++)
                    {
                        PropagatePushOp(visited, block, pushOpIndex);
                    }
                }

                funcs.Add(blocks.ToArray());
            }

            return funcs.ToArray();
        }

        private static bool HasBlockAfter(IGpuAccessor gpuAccessor, Block currBlock, ulong startAdddress)
        {
            if (!gpuAccessor.MemoryMapped(startAdddress + currBlock.EndAddress) ||
                !gpuAccessor.MemoryMapped(startAdddress + currBlock.EndAddress + 7))
            {
                return false;
            }

            ulong inst = gpuAccessor.MemoryRead<ulong>(startAdddress + currBlock.EndAddress);

            return inst != 0UL && inst != ShaderEndDelimiter;
        }

        private static bool BinarySearch(List<Block> blocks, ulong address, out int index)
        {
            index = 0;

            int left  = 0;
            int right = blocks.Count - 1;

            while (left <= right)
            {
                int size = right - left;

                int middle = left + (size >> 1);

                Block block = blocks[middle];

                index = middle;

                if (address >= block.Address && address < block.EndAddress)
                {
                    return true;
                }

                if (address < block.Address)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return false;
        }

        private static void FillBlock(
            IGpuAccessor gpuAccessor,
            Block        block,
            ulong        limitAddress,
            ulong        startAddress,
            out bool     hasBindless)
        {
            ulong address = block.Address;
            hasBindless = false;

            do
            {
                if (address + 7 >= limitAddress)
                {
                    break;
                }

                // Ignore scheduling instructions, which are written every 32 bytes.
                if ((address & 0x1f) == 0)
                {
                    address += 8;

                    continue;
                }

                ulong opAddress = address;

                address += 8;

                long opCode = gpuAccessor.MemoryRead<long>(startAddress + opAddress);

                (InstEmitter emitter, OpCodeTable.MakeOp makeOp) = OpCodeTable.GetEmitter(opCode);

                if (emitter == null)
                {
                    // TODO: Warning, illegal encoding.

                    block.OpCodes.Add(new OpCode(null, opAddress, opCode));

                    continue;
                }

                if (makeOp == null)
                {
                    throw new ArgumentNullException(nameof(makeOp));
                }

                OpCode op = makeOp(emitter, opAddress, opCode);

                // We check these patterns to figure out the presence of bindless access
                hasBindless |= (op is OpCodeImage image && image.IsBindless) ||
                    (op is OpCodeTxd txd && txd.IsBindless) ||
                    (op is OpCodeTld4B) ||
                    (emitter == InstEmit.TexB) ||
                    (emitter == InstEmit.TldB) ||
                    (emitter == InstEmit.TmmlB) ||
                    (emitter == InstEmit.TxqB);

                block.OpCodes.Add(op);
            }
            while (!IsControlFlowChange(block.GetLastOp()));

            block.EndAddress = address;

            block.UpdatePushOps();
        }

        private static bool IsUnconditionalBranch(OpCode opCode)
        {
            return IsUnconditional(opCode) && IsControlFlowChange(opCode);
        }

        private static bool IsUnconditional(OpCode opCode)
        {
            if (opCode is OpCodeExit op && op.Condition != Condition.Always)
            {
                return false;
            }

            return opCode.Predicate.Index == RegisterConsts.PredicateTrueIndex && !opCode.InvertPredicate;
        }

        private static bool IsControlFlowChange(OpCode opCode)
        {
            return (opCode is OpCodeBranch opBranch && !opBranch.PushTarget) ||
                    opCode is OpCodeBranchIndir                              ||
                    opCode is OpCodeBranchPop                                ||
                    opCode is OpCodeExit;
        }

        private enum MergeType
        {
            Brk = 0,
            Sync = 1
        }

        private struct PathBlockState
        {
            public Block Block { get; }

            private enum RestoreType
            {
                None,
                PopPushOp,
                PushBranchOp
            }

            private RestoreType _restoreType;

            private ulong _restoreValue;
            private MergeType _restoreMergeType;

            public bool ReturningFromVisit => _restoreType != RestoreType.None;

            public PathBlockState(Block block)
            {
                Block             = block;
                _restoreType      = RestoreType.None;
                _restoreValue     = 0;
                _restoreMergeType = default;
            }

            public PathBlockState(int oldStackSize)
            {
                Block             = null;
                _restoreType      = RestoreType.PopPushOp;
                _restoreValue     = (ulong)oldStackSize;
                _restoreMergeType = default;
            }

            public PathBlockState(ulong syncAddress, MergeType mergeType)
            {
                Block             = null;
                _restoreType      = RestoreType.PushBranchOp;
                _restoreValue     = syncAddress;
                _restoreMergeType = mergeType;
            }

            public void RestoreStackState(Stack<(ulong, MergeType)> branchStack)
            {
                if (_restoreType == RestoreType.PushBranchOp)
                {
                    branchStack.Push((_restoreValue, _restoreMergeType));
                }
                else if (_restoreType == RestoreType.PopPushOp)
                {
                    while (branchStack.Count > (uint)_restoreValue)
                    {
                        branchStack.Pop();
                    }
                }
            }
        }

        private static void PropagatePushOp(Dictionary<ulong, Block> blocks, Block currBlock, int pushOpIndex)
        {
            OpCodePush pushOp = currBlock.PushOpCodes[pushOpIndex];

            Stack<PathBlockState> workQueue = new Stack<PathBlockState>();

            HashSet<Block> visited = new HashSet<Block>();

            Stack<(ulong, MergeType)> branchStack = new Stack<(ulong, MergeType)>();

            void Push(PathBlockState pbs)
            {
                // When block is null, this means we are pushing a restore operation.
                // Restore operations are used to undo the work done inside a block
                // when we return from it, for example it pops addresses pushed by
                // SSY/PBK instructions inside the block, and pushes addresses poped
                // by SYNC/BRK.
                // For blocks, if it's already visited, we just ignore to avoid going
                // around in circles and getting stuck here.
                if (pbs.Block == null || !visited.Contains(pbs.Block))
                {
                    workQueue.Push(pbs);
                }
            }

            Push(new PathBlockState(currBlock));

            while (workQueue.TryPop(out PathBlockState pbs))
            {
                if (pbs.ReturningFromVisit)
                {
                    pbs.RestoreStackState(branchStack);

                    continue;
                }

                Block current = pbs.Block;

                // If the block was already processed, we just ignore it, otherwise
                // we would push the same child blocks of an already processed block,
                // and go around in circles until memory is exhausted.
                if (!visited.Add(current))
                {
                    continue;
                }

                int pushOpsCount = current.PushOpCodes.Count;

                if (pushOpsCount != 0)
                {
                    Push(new PathBlockState(branchStack.Count));

                    for (int index = pushOpIndex; index < pushOpsCount; index++)
                    {
                        OpCodePush currentPushOp = current.PushOpCodes[index];
                        MergeType pushMergeType = currentPushOp.Emitter == InstEmit.Ssy ? MergeType.Sync : MergeType.Brk;
                        branchStack.Push((currentPushOp.GetAbsoluteAddress(), pushMergeType));
                    }
                }

                pushOpIndex = 0;

                if (current.Next != null)
                {
                    Push(new PathBlockState(current.Next));
                }

                if (current.Branch != null)
                {
                    Push(new PathBlockState(current.Branch));
                }
                else if (current.GetLastOp() is OpCodeBranchIndir brIndir)
                {
                    // By adding them in descending order (sorted by address), we process the blocks
                    // in order (of ascending address), since we work with a LIFO.
                    foreach (Block possibleTarget in brIndir.PossibleTargets.OrderByDescending(x => x.Address))
                    {
                        Push(new PathBlockState(possibleTarget));
                    }
                }
                else if (current.GetLastOp() is OpCodeBranchPop op)
                {
                    MergeType popMergeType = op.Emitter == InstEmit.Sync ? MergeType.Sync : MergeType.Brk;

                    bool found = true;
                    ulong targetAddress = 0UL;
                    MergeType mergeType;

                    do
                    {
                        if (branchStack.Count == 0)
                        {
                            found = false;
                            break;
                        }

                        (targetAddress, mergeType) = branchStack.Pop();

                        // Push the target address (this will be used to push the address
                        // back into the SSY/PBK stack when we return from that block),
                        Push(new PathBlockState(targetAddress, mergeType));
                    }
                    while (mergeType != popMergeType);

                    // Make sure we found the correct address,
                    // the push and pop instruction types must match, so:
                    // - BRK can only consume addresses pushed by PBK.
                    // - SYNC can only consume addresses pushed by SSY.
                    if (found)
                    {
                        if (branchStack.Count == 0)
                        {
                            // If the entire stack was consumed, then the current pop instruction
                            // just consumed the address from out push instruction.
                            op.Targets.Add(pushOp, op.Targets.Count);

                            pushOp.PopOps.TryAdd(op, Local());
                        }
                        else
                        {
                            // Push the block itself into the work "queue" (well, it's a stack)
                            // for processing.
                            Push(new PathBlockState(blocks[targetAddress]));
                        }
                    }
                }
            }
        }
    }
}