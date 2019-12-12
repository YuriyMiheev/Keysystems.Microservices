using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Microservices.Configuration;

namespace Microservices.Bus.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class BusSettings : AppSettingsBase
	{
		public const string TAG_PREFIX = ".";
		private ConnectionStringSetting _connSetting;


		#region Ctor
		public BusSettings(IAppSettingsConfig appConfig, IConnectionStringsConfig connConfig)
			: base(TAG_PREFIX, appConfig.GetAppSettings())
		{
			IDictionary<string, ConnectionStringSetting> connSettings = connConfig.GetConnectionStrings();
			if (!connSettings.ContainsKey("SysDatabase"))
				throw new ConfigSettingsException("Не найдена строка подключения к системной БД сервиса.", "SysDatabase");

			_connSetting = connSettings["SysDatabase"];
		}
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Параметры подключения к системной БД.
		/// </summary>
		public DatabaseInfo Database
		{
			get
			{
				string name = _connSetting.Name;
				string schema = "dbo";
				int index = name.IndexOf('.');
				if (index > -1)
				{
					schema = name.Substring(0, index).TrimStart('[').TrimEnd(']');
					name = name.Substring(index + 1);
				}

				return new DatabaseInfo()
					{
						Name = name,
						Schema = schema,
						Provider = _connSetting.Provider,
						ConnectionString = _connSetting.ConnectionString //HttpUtility.UrlDecode(_connSetting.ConnectionString)
					};
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public CredentialInfo Administrator
		{
			get
			{
				string admin = Parser.ParseString(PropertyValue(".Administrator"), null);
				if (admin != null)
				{
					try
					{
						return CredentialInfo.Parse(admin);
					}
					catch (Exception ex)
					{
						throw new ConfigSettingsException("Некорректное значения св-ва.", "Administrator", ex);
					}
				}

				return new CredentialInfo();
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool DebugEnabled
		{
			get
			{
				bool defaultValue = false;
				return Parser.ParseBool(PropertyValue(".Debug.Enabled"), defaultValue);
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool AuthorizationRequired
		{
			get
			{
				bool defaultValue = true;
				return Parser.ParseBool(PropertyValue(".Authorization.Required"), defaultValue);
			}
		}

		/// <summary>
		/// {Get} байт.
		/// </summary>
		public int MaxUploadFileSize
		{
			get
			{
				int defaultValue = (2 * 1024 * 1024 * 1023);
				return Parser.ParseInt(PropertyValue(".MaxUploadFileSize"), defaultValue);
			}
		}

		/// <summary>
		/// {Get} байт.
		/// </summary>
		public int MessageBufferSize
		{
			get
			{
				int defaultValue = (1024 * 1024); // ServiceEnvironment.DefaultMessageBufferSize;
				return Parser.ParseInt(PropertyValue(".MessageBufferSize"), defaultValue);
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool CheckInstanceID
		{
			get
			{
				bool defaultValue = false;
				return Parser.ParseBool(PropertyValue(".CheckInstanceID"), defaultValue);
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public string LicenseFile
		{
			get
			{
				string defaultValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "rms.lic");
				string licenseFile = Parser.ParseString(PropertyValue(".LicenseFile"), defaultValue);
				return Path.GetFullPath(licenseFile);
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public string TempDir
		{
			get
			{
				string defaultValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TEMP");
				string tempDir = Parser.ParseString(PropertyValue(".TempDir"), defaultValue);
				return Path.GetFullPath(tempDir);
			}
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public string AddinsDir
		{
			get
			{
				string defaultValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "ADDINS");
				string addinsDir = Parser.ParseString(PropertyValue(".AddinsDir"), defaultValue);
				return Path.GetFullPath(addinsDir);
			}
		}

		///// <summary>
		///// {Get}
		///// </summary>
		//public string PluginsDir
		//{
		//	get
		//	{
		//		string defaultValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "PLUGINS");
		//		string pluginsDir = Parser.ParseString(PropertyValue(".PluginsDir"), defaultValue);
		//		return Path.GetFullPath(pluginsDir);
		//	}
		//}

		/// <summary>
		/// {Get}
		/// </summary>
		public string ToolsDir
		{
			get
			{
				string defaultValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "TOOLS");
				string toolsDir = Parser.ParseString(PropertyValue(".ToolsDir"), defaultValue);
				return Path.GetFullPath(toolsDir);
			}
		}
		#endregion

	}
}
