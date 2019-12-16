using Microservices.Bus.Channels;

namespace Microservices.Bus.Addins
{
	public interface IAddinManager
	{
		ChannelDescription[] RegisteredChannels { get; }


		void LoadAddins();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		ChannelDescription FindChannelDescription(string provider);
	}
}
