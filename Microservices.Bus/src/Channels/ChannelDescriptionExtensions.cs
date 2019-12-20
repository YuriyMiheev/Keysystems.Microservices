using Microservices.Configuration;

namespace Microservices.Bus.Channels
{
	public static class ChannelDescriptionExtensions
	{
		public static MicroserviceDescriptionProperty GetProperty(this MicroserviceDescription description, string propName)
		{
			return description.Properties[propName];
		}
	}
}
