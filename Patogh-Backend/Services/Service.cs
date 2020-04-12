namespace PatoghBackend.Services.Common
{
	using System;
	using PatoghBackend.Contract;

	public class Service : IService
	{
		private bool disposed;

		~Service()
		{
			// Simply call Dispose(false).
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// Free other state (managed objects).
				}

				// Free your own state (unmanaged objects).
				// Set large fields to null.
				disposed = true;
			}
		}

		// Use C# destructor syntax for finalization code.
	}
}