namespace Microservices.Bus.Addins
{
	public class AddinManagerOptions
	{
		/// <summary>
		/// Каталог с дополнениями.
		/// </summary>
		public string AddinsDirectory { get; set; }

		/// <summary>
		/// Файл с описанием дополнения.
		/// </summary>
		public string DescriptionFile { get; set; }
	}
}
