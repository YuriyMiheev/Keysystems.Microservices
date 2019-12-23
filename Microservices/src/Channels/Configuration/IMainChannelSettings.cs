namespace Microservices.Channels.Configuration
{
	public interface IMainChannelSettings
	{
		#region Properties
		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		string Provider { get; }

		/// <summary>
		/// {Get} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type { get; }

		///// <summary>
		///// {Get} Версия.
		///// </summary>
		//public string Version { get; }

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment { get; }

		///// <summary>
		///// {Get} Имя файла иконки.
		///// </summary>
		//public string Icon { get; }

		///// <summary>
		///// {Get} Может обновлять список контактов.
		///// </summary>
		//public bool CanSyncContacts { get; }

		///// <summary>
		///// {Get} Поддержка множества экземпляров.
		///// </summary>
		//public bool AllowMultipleInstances { get; }

		public string Name { get; }

		public string VirtAddress { get; }

		public string RealAddress { get; }

		public string SID { get; }

		public int Timeout { get; }

		public string PasswordIn { get; }

		public string PasswordOut { get; }
		#endregion
	}
}
