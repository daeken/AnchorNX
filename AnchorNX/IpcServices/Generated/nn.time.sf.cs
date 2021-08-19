#pragma warning disable 169, 465, 1998
using System;
using System.Threading.Tasks;
using UltimateOrb;
namespace AnchorNX.IpcServices.Nn.Time.Sf {
	public unsafe struct CalendarAdditionalInfo {
		public uint TmWday;
		public int TmYday;
		public fixed byte TzName[8];
		public bool IsDaylightSavingTime;
		public int UtcOffsetSeconds;
	}
}
