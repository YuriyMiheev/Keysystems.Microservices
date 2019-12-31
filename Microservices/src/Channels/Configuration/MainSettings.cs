using System.Collections.Generic;

using Microservices.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class MainSettings : AppSettingsBase
	{
		public const string TAG_PREFIX = ".";


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public MainSettings(IDictionary<string, AppConfigSetting> appSettings)
			: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		public int LINK
		{
			get => Parser.ParseInt(GetValue(".LINK"), 0);
		}

		/// <summary>
		/// {Get} Провайдер.
		/// </summary>
		public string Provider
		{
			get => Parser.ParseString(GetValue(".Provider"), "");
		}

		/// <summary>
		/// {Get} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type
		{
			get => Parser.ParseString(GetValue(".Type"), "");
		}

		/// <summary>
		/// {Get} Версия.
		/// </summary>
		public string Version
		{
			get => Parser.ParseString(GetValue(".Version"), "");
		}

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment
		{
			get => Parser.ParseString(GetValue(".Comment"), "");
		}

		/// <summary>
		/// {Get} Имя файла иконки.
		/// </summary>
		public string Icon
		{
			get => Parser.ParseString(GetValue(".Icon"), "favicon.png");
		}

		/// <summary>
		/// {Get} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts
		{
			get => Parser.ParseBool(GetValue(".CanSyncContacts"), false);
		}

		/// <summary>
		/// {Get} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances
		{
			get => Parser.ParseBool(GetValue(".AllowMultipleInstances"), false);
		}

		public string Name
		{
			get => Parser.ParseString(GetValue(".Name"), "");
		}

		public string VirtAddress
		{
			get => Parser.ParseString(GetValue(".VirtAddress"), "");
		}

		public string RealAddress
		{
			get => Parser.ParseString(GetValue(".RealAddress"), "");
		}

		public string SID
		{
			get => Parser.ParseString(GetValue(".SID"), "");
		}

		public int Timeout
		{
			get => Parser.ParseInt(GetValue(".Timeout"), 30);
		}

		public string PasswordIn
		{
			get => Parser.ParseString(GetValue(".PasswordIn"), "");
		}

		public string PasswordOut
		{
			get => Parser.ParseString(GetValue(".PasswordOut"), "");
		}
		#endregion

	}
}
