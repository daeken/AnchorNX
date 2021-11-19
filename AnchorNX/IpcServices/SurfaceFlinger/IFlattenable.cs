namespace AnchorNX.IpcServices.Nns.Hosbinder {
	public interface IFlattenable {
		uint GetFlattenedSize();

		uint GetFdCount();

		void Flatten(Parcel parcel);

		void Unflatten(Parcel parcel);
	}
}