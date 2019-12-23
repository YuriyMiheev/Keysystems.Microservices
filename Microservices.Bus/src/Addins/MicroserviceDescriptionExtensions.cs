namespace Microservices.Bus.Addins
{
	public static class MicroserviceDescriptionExtensions
	{
		public static MicroserviceDescriptionProperty GetProperty(this MicroserviceDescription description, string propName)
		{
			return description.Properties[propName];
		}
	}
}
