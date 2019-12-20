using System;
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
		public string Provider
		{
			get { return Parser.ParseString(GetValue(".Provider"), ""); }
		}

		/// <summary>
		/// {Get} Имя типа и сборки, реализующих канал.
		/// </summary>
		public string Type
		{
			get { return Parser.ParseString(GetValue(".Type"), ""); }
		}

		/// <summary>
		/// {Get} Версия.
		/// </summary>
		public string Version
		{
			get { return Parser.ParseString(GetValue(".Version"), ""); }
		}

		/// <summary>
		/// {Get} Комментарий.
		/// </summary>
		public string Comment
		{
			get { return Parser.ParseString(GetValue(".Comment"), ""); }
		}

		/// <summary>
		/// {Get} Имя файла иконки.
		/// </summary>
		public string Icon
		{
			get { return Parser.ParseString(GetValue(".Icon"), "favicon.png"); }
		}

		/// <summary>
		/// {Get} Может обновлять список контактов.
		/// </summary>
		public bool CanSyncContacts
		{
			get { return Parser.ParseBool(GetValue(".CanSyncContacts"), false); }
		}

		/// <summary>
		/// {Get} Поддержка множества экземпляров.
		/// </summary>
		public bool AllowMultipleInstances
		{
			get { return Parser.ParseBool(GetValue(".AllowMultipleInstances"), false); }
		}

		public string Name
		{
			get { return Parser.ParseString(GetValue(".Name"), ""); }
		}

		public string VirtAddress
		{
			get { return Parser.ParseString(GetValue(".VirtAddress"), ""); }
		}

		public string RealAddress
		{
			get { return Parser.ParseString(GetValue(".RealAddress"), ""); }
		}

		public string SID
		{
			get { return Parser.ParseString(GetValue(".SID"), ""); }
		}

		public TimeSpan Timeout
		{
			get { return Parser.ParseTime(GetValue(".Timeout"), TimeSpan.FromSeconds(30)).Value; }
		}

		public string PasswordIn
		{
			get { return Parser.ParseString(GetValue(".PasswordIn"), ""); }
		}

		public string PasswordOut
		{
			get { return Parser.ParseString(GetValue(".PasswordOut"), ""); }
		}
		#endregion

	}
}
