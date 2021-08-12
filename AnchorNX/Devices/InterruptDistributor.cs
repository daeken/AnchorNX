using System;
using System.Collections.Generic;
using System.Linq;

namespace AnchorNX.Devices {
	public class InterruptDistributor : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x50041000, 0x50041FFF);

		const uint GICD_SGIR_TARGET_LIST_MASK = 0x03000000;
		const int GICD_SGIR_TARGET_LIST_SHIFT = 24;
		const uint GICD_SGIR_TARGET_MASK = 0x00FF0000;
		const int GICD_SGIR_TARGET_SHIFT = 16;
		const uint GICD_SGIR_INTID_MASK = 0x0000000F;
		const uint GICD_SGIR_TARGET_OTHERS_VAL = 1;
		const uint GICD_SGIR_TARGET_SELF_VAL = 2;

		readonly Queue<(int IntId, int Sender)>[] Queues =
			Enumerable.Range(0, 4).Select(_ => new Queue<(int, int)>()).ToArray();

		[Mmio(0x50041000)]
		uint Ctlr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50041004)]
		uint Typer => 6 | (3 << 5) | (1 << 10) | (0x1F << 11);

		[Mmio(0x50041080)] MmioArray<uint> IGroupR = new(32, writable: true);
		[Mmio(0x50041100)] MmioArray<uint> IsEnableR = new(32, writable: true);
		[Mmio(0x50041180)] MmioArray<uint> IcEnableR = new(32, writable: true);
		[Mmio(0x50041280)] MmioArray<uint> IcPendR = new(32, writable: true);
		[Mmio(0x50041380)] MmioArray<uint> IcActiveR = new(32, writable: true);
		[Mmio(0x50041400)] MmioArray<byte> IPriorityR = new(1020, writable: true);
		[Mmio(0x50041800)] MmioArray<byte> ITargetsR = new(1020, writable: true);
		[Mmio(0x50041C00)] MmioArray<uint> ICfgR = new(32, writable: true);

		[Mmio(0x50041F00)]
		uint Sgir {
			get => 0;
			set {
				var targetList = (value & GICD_SGIR_TARGET_LIST_MASK) >> GICD_SGIR_TARGET_LIST_SHIFT;
				var target = (value & GICD_SGIR_TARGET_MASK) >> GICD_SGIR_TARGET_SHIFT;
				var intid = value & GICD_SGIR_INTID_MASK;
				
				Console.WriteLine($"Interrupt in flight: target {target:X} intid {intid:X}");

				if(target == GICD_SGIR_TARGET_OTHERS_VAL)
					for(var i = 0; i < 4; ++i) {
						if(i != Core.CurrentId)
							EnqueueInterrupt(i, (int) intid, Core.CurrentId);
					}
				else if(target == GICD_SGIR_TARGET_SELF_VAL)
					EnqueueInterrupt(Core.CurrentId, (int) intid, Core.CurrentId);
				else
					for(var i = 0; i < 4; ++i)
						EnqueueInterrupt(i, (int) intid, Core.CurrentId);
			}
		}

		void EnqueueInterrupt(int core, int intId, int sender) {
			if(Box.InterruptController.IsInterruptActive(core))
				Queues[core].Enqueue((intId, sender));
			else {
				Box.InterruptController.SetActiveInterrupt(core, intId, sender);
				Box.Cores[core].Interrupt = true;
			}
		}

		public void HandleNextInterrupt(int core) {
			if(Queues[core].TryDequeue(out var next))
				Box.InterruptController.SetActiveInterrupt(core, next.IntId, next.Sender);
			else
				Box.InterruptController.SetActiveInterrupt(core, 0x3FF, 0);
		}
	}
}