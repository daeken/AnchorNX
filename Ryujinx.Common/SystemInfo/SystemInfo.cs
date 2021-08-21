﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Ryujinx.Common.Logging;

namespace Ryujinx.Common.SystemInfo
{
    public class SystemInfo
    {
        public string OsDescription { get; protected set; }
        public string CpuName { get; protected set; }
        public ulong RamTotal { get; protected set; }
        public ulong RamAvailable { get; protected set; }
        protected static int LogicalCoreCount => Environment.ProcessorCount;

        protected SystemInfo()
        {
            OsDescription = $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})";
            CpuName = "Unknown";
        }

        private static string ToMBString(ulong bytesValue) => (bytesValue == 0) ? "Unknown" : $"{bytesValue / 1024 / 1024} MB";

        public void Print()
        {
            Logger.Notice.Print(LogClass.Application, $"Operating System: {OsDescription}");
            Logger.Notice.Print(LogClass.Application, $"CPU: {CpuName}");
            Logger.Notice.Print(LogClass.Application, $"RAM: Total {ToMBString(RamTotal)} ; Available {ToMBString(RamAvailable)}");
        }

        public static SystemInfo Gather()
        {
            if (OperatingSystem.IsWindows())
            {
                return new WindowsSystemInfo();
            }
            else if (OperatingSystem.IsLinux())
            {
                return new LinuxSystemInfo();
            }
            else if (OperatingSystem.IsMacOS())
            {
                return new MacOSSystemInfo();
            }
            else
            {
                Logger.Error?.Print(LogClass.Application, "SystemInfo unsupported on this platform");

                return new SystemInfo();
            }
        }

        // x86 exposes a 48 byte ASCII "CPU brand" string via CPUID leaves 0x80000002-0x80000004.
        internal static string GetCpuidCpuName()
        {
            if (!X86Base.IsSupported)
            {
                return null;
            }

            // Check if CPU supports the query
            if ((uint)X86Base.CpuId(unchecked((int)0x80000000), 0).Eax < 0x80000004)
            {
                return null;
            }

            int[] regs = new int[12];

            for (uint i = 0; i < 3; ++i)
            {
                (regs[4 * i], regs[4 * i + 1], regs[4 * i + 2], regs[4 * i + 3]) = X86Base.CpuId((int)(0x80000002 + i), 0);
            }

            string name = Encoding.ASCII.GetString(MemoryMarshal.Cast<int, byte>(regs)).Replace('\0', ' ').Trim();

            return string.IsNullOrEmpty(name) ? null : name;
        }
    }
}