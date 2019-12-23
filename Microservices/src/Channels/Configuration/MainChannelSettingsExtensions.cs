namespace Microservices.Channels.Configuration
{
	public static class MainChannelSettingsExtensions
	{
		public static void SetRealAddress(this MainChannelSettings mainSettings, string realAddress)
		{
			mainSettings.SetValue(".RealAddress", realAddress);
		}
	}
}
