namespace Microservices.Bus.Addins
{
	/// <summary>
	/// Менеджер дополнений.
	/// </summary>
	public interface IAddinManager
	{
		/// <summary>
		/// {Get} Список загруженных дополнений. 
		/// </summary>
		IAddinDescription[] RegisteredChannels { get; }

		/// <summary>
		/// Загрузить дополнения.
		/// </summary>
		void LoadAddins();

		/// <summary>
		/// Найти описание дополнения.
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		IAddinDescription FindDescription(string provider);
	}
}
