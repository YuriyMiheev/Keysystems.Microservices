
namespace Microservices.Bus.Addins
{
	public static class AddinDescriptionExtensions
	{
		public static AddinDescriptionProperty GetProperty(this IAddinDescription description, string propName)
		{
			return description.Properties[propName];
		}
	}
}
