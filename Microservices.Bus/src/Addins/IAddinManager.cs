using Microservices.Bus.Channels;

namespace Microservices.Bus.Addins
{
	public interface IAddinManager
	{
		MicroserviceDescription[] RegisteredMicroservices { get; }


		void LoadAddins();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		MicroserviceDescription FindMicroservice(string provider);
	}
}
