using System;
using System.Runtime.CompilerServices;

namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public interface IBinder {
		static readonly Logger Logger = new("IBinder");
		static Action<string> Log = Logger.Log;

		string InterfaceToken { get; }
		uint AdjustRefcount(int addVal, int type);

		void GetNativeHandle(uint typeId, out uint readableEvent);

		uint OnTransact(uint code, uint flags, ReadOnlySpan<byte> inputParcel, Span<byte> outputParcel) {
			inputParcel.Hexdump(Logger);
			var inputParcelReader = new Parcel(inputParcel.ToArray());

			// TODO: support objects?
			var outputParcelWriter = new Parcel((uint) (outputParcel.Length - Unsafe.SizeOf<ParcelHeader>() - 0x28), 0x28);

			var inputInterfaceToken = inputParcelReader.ReadInterfaceToken();

			if(!InterfaceToken.Equals(inputInterfaceToken)) {
				Console.WriteLine($"Invalid interface token {inputInterfaceToken} (expected: {InterfaceToken})");

				return 0;
			}

			OnTransact(code, flags, inputParcelReader, outputParcelWriter);

			outputParcelWriter.Finish().CopyTo(outputParcel);

			return 0;
		}

		void OnTransact(uint code, uint flags, Parcel inputParcel, Parcel outputParcel);
	}
}