using System;

namespace AnchorNX.Devices {
	public class InterruptController : MmioDevice {
		public override (ulong, ulong) AddressRange => (0x50042000, 0x50043FFF);

		readonly (int Id, int Sender)[] ActiveInterrupt = {
			(0x3FF, 0), (0x3FF, 0), (0x3FF, 0), (0x3FF, 0)
		};

		[Mmio(0x50042000)]
		uint Ctlr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50042004)]
		uint Pmr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x50042008)]
		uint Bpr {
			get => throw new NotImplementedException();
			set { }
		}

		[Mmio(0x5004200C)]
		uint Iar {
			get {
				var (id, sender) = ActiveInterrupt[Core.CurrentId];
				return (uint) (id | (sender << 10));
			}
		}

		[Mmio(0x50042010)]
		uint Eoir {
			set => Box.InterruptDistributor.HandleNextInterrupt(Core.CurrentId);
		}

		public bool IsInterruptActive(int core) => ActiveInterrupt[core].Id != 0x3FF;
		public void SetActiveInterrupt(int core, int intId, int sender) {
			ActiveInterrupt[core] = (intId, sender);
			if(intId != 0x3FF)
				Box.Cores[core].Interrupt = true;
		}
	}
}