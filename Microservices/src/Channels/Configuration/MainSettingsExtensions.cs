namespace Microservices.Channels.Configuration
{
	public static class MainSettingsExtensions
	{
		public static void SetRealAddress(this MainSettings mainSettings, string realAddress)
		{
			mainSettings.SetValue(".RealAddress", realAddress);
		}
	}
}
