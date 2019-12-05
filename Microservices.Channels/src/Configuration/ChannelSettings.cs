using System;
using System.Collections.Generic;

using Microservices.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// Дополнительные настройки канала.
	/// </summary>
	public class ChannelSettings : AppSettingsBase
	{
		public const string TAG_PREFIX = "CHANNEL.";


		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="properties">Свойства канала.</param>
		public ChannelSettings(IDictionary<string, ConfigFileSetting> appSettings)
			: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Автоматическое открытие.
		/// </summary>
		public bool AutoOpen
		{
			get { return Parser.ParseBool(PropertyValue("CHANNEL.AUTO_OPEN"), false); }
		}

		/// <summary>
		/// {Get} Автоматический запуск.
		/// </summary>
		public bool AutoRun
		{
			get { return Parser.ParseBool(PropertyValue("CHANNEL.AUTO_RUN"), false); }
		}

		/// <summary>
		/// {Get} Вкл/Выкл Ping.
		/// </summary>
		public bool PingEnabled
		{
			get { return Parser.ParseBool(PropertyValue("CHANNEL.PING.ENABLED"), false); }
		}

		/// <summary>
		/// {Get} Периодичность пинга. (Default: 1 мин)
		/// </summary>
		public TimeSpan PingInterval
		{
			get { return Parser.ParseTime(PropertyValue("CHANNEL.PING.INTERVAL"), TimeSpan.FromMinutes(1)).Value; }
		}

		///// <summary>
		///// {Get} Плагин обработки Http запросов.
		///// </summary>
		//public string HttpPlugin
		//{
		//	get { return Parser.ParseString(PropertyValue("CHANNEL.PLUGIN.HTTP"), null); }
		//}

		/// <summary>
		/// {Get} (Default: ERROR)
		/// </summary>
		public string LogLevel
		{
			get { return Parser.ParseString(PropertyValue("CHANNEL.LOG.LEVEL"), "ERROR"); }
		}

		///// <summary>
		///// {Get} (Default: true)
		///// </summary>
		//public bool LogFileEnabled
		//{
		//   get { return Parser.ParseBool(PropertyValue("CHANNEL.LOG.FILE.ENABLED"), true); }
		//}

		/// <summary>
		/// {Get} (Default: true)
		/// </summary>
		public bool LogMailEnabled
		{
			get { return Parser.ParseBool(PropertyValue("CHANNEL.LOG.MAIL.ENABLED"), false); }
		}
		
		/// <summary>
		/// {Get} 
		/// </summary>
		public string LogMailEmail
		{
			get { return Parser.ParseString(PropertyValue("CHANNEL.LOG.MAIL.EMAIL"), ""); }
		}

		/// <summary>
		/// {Get} (Default: true)
		/// </summary>
		public bool LogJabberEnabled
		{
			get { return Parser.ParseBool(PropertyValue("CHANNEL.LOG.JABBER.ENABLED"), false); }
		}
		
		/// <summary>
		/// {Get} 
		/// </summary>
		public string LogJabberAddress
		{
			get { return Parser.ParseString(PropertyValue("CHANNEL.LOG.JABBER.ADDRESS"), ""); }
		}
		#endregion


		#region Static
		///// <summary>
		///// Свойства канала.
		///// </summary>
		///// <returns></returns>
		//public static List<ChannelDescriptionProperty> DefaultProperties()
		//{
		//	var properties = new List<ChannelDescriptionProperty>();
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.AUTO_OPEN", Type = "Bool", Value = "Yes", DefaultValue = "Yes", Format = "Yes|No", Comment = "Автоматически открывать канал" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.AUTO_RUN", Type = "Bool", Value = "Yes", DefaultValue = "Yes", Format = "Yes|No", Comment = "Автоматически запускать канал" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.PING.ENABLED", Type = "Bool", Value = "Yes", DefaultValue = "Yes", Format = "Yes|No", Comment = "Вкл/Выкл Ping" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.PING.INTERVAL", Value = "00:01:00", Type = "Time", Format = "{чч:мм:сс}", Comment = "Ping-интервал" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.PLUGIN.HTTP", Type = "String", Format = "Type, Assembly", Comment = "Имя плагина обработки Http запросов" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.LEVEL", Type = "String", Value = "ERROR", DefaultValue = "ERROR", Format = "[ERROR | TRACE]", Comment = "Уровень логирования" });
		//	//properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.FILE.ENABLED", Type = "Bool", Value = "Yes", DefaultValue = "Yes", Comment = "Разрешить логирование" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.MAIL.ENABLED", Type = "Bool", Value = "No", DefaultValue = "No", Format = "Yes|No", Comment = "Разрешить логирование" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.MAIL.EMAIL", Type = "String", Format = "{user@domain}", Comment = "Email адрес для отправки логов" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.JABBER.ENABLED", Type = "Bool", Value = "No", DefaultValue = "No", Format = "Yes|No", Comment = "Разрешить логирование" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "CHANNEL.LOG.JABBER.ADDRESS", Type = "String", Format = "{user@domain}", Comment = "Адрес для отправки логов по XMPP" });
		//	return properties;
		//}
		#endregion

	}
}
