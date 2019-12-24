using System;
using System.Diagnostics;

namespace Microservices.Configuration
{
	/// <summary>
	/// Настройка в конфиг файле.
	/// </summary>
	[DebuggerDisplay("{this.Value}")]
	public class AppConfigSetting : IAppConfigSetting
	{
		public AppConfigSetting()
		{ }

		public AppConfigSetting(string name, string value)
		{
			this.Name = name ?? throw new ArgumentException(nameof(name));
			this.Value = value;
		}


		/// <summary>
		/// Имя.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Значение.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Тип.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Формат.
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// Значение по умолчанию.
		/// </summary>
		public string DefaultValue { get; set; }

		/// <summary>
		/// Комментарий.
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// Только чтение.
		/// </summary>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// Секретная.
		/// </summary>
		public bool Secret { get; set; }

	}
}
