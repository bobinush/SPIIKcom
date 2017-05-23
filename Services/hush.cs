namespace SPIIKcom
{
	public interface IDataService
	{
		string PublicKey { get; }
	}

	public class DataService : IDataService
	{
		private readonly string _publicKey;
		private readonly string _privateKey;

		public DataService(string publicKey, string privateKey)
		{
			_publicKey = publicKey;
			_privateKey = privateKey;
		}

		public string PublicKey { get { return _publicKey; } }
	}
}