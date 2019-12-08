using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Configuration;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// Настройки обработки сообщений.
	/// </summary>
	public class MessageSettings : AppSettingsBase
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="appSettings">Свойства канала.</param>
		public MessageSettings(IDictionary<string, AppConfigSetting> appSettings)
			: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Удалять устаревшие сообщения.
		/// </summary>
		public bool DeleteExpired
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.DELETE.EXPIRED"), false); }
		}

		/// <summary>
		/// {Get} Срок жизни сообщений (дней). (Default: 366)
		/// </summary>
		public int ExpiredDays
		{
			get { return Parser.ParseInt(PropertyValue("MESSAGE.DELETE.EXPIRED_DAYS"), 366); }
		}

		/// <summary>
		/// {Get} Статусы сообщений подпадающие под устаревание. (Default: [COMPLETED, DELETED, ERROR])
		/// </summary>
		public List<string> ExpiredStatuses
		{
			get
			{
				string status = Parser.ParseString(PropertyValue("MESSAGE.DELETE.EXPIRED_STATUSES"), "");
				return status.Trim('[', ']').Split(new char[] { ' ', ',', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
		}

		/// <summary>
		/// {Get} Удалять сообщения после отправки. (Default: false)
		/// </summary>
		public bool DeleteAfterSend
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.DELETE.AFTER_SEND"), false); }
		}

		/// <summary>
		/// {Get} Удалять сообщения после приема. (Default: false)
		/// </summary>
		public bool DeleteAfterReceive
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.DELETE.AFTER_RECEIVE"), false); }
		}

		/// <summary>
		/// {Get} Удалять сообщения со статусом 'DELETED'. (Default: No)
		/// </summary>
		public bool DeleteDeleted
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.DELETE.DELETED"), false); }
		}

		/// <summary>
		/// {Get} Проверять наличие новых сообщений. (Default: true)
		/// </summary>
		public bool ScanEnabled
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.SCAN.ENABLED"), true); }
		}

		/// <summary>
		/// {Get} Интервал проверки новых сообщений. (Default: 10 сек)
		/// </summary>
		public TimeSpan ScanInterval
		{
			get { return Parser.ParseTime(PropertyValue("MESSAGE.SCAN.INTERVAL"), TimeSpan.Zero).Value; }
		}

		/// <summary>
		/// {Get} Число потоков обработки. (Default: 1)
		/// </summary>
		public int ScanThreads
		{
			get { return Parser.ParseInt(PropertyValue("MESSAGE.SCAN.THREADS"), 1); }
		}

		/// <summary>
		/// {Get} Размер порции сообщений. (Default: 1000)
		/// </summary>
		public int ScanPortion
		{
			get { return Parser.ParseInt(PropertyValue("MESSAGE.SCAN.PORTION"), 1000); }
		}

		/// <summary>
		/// {Get} Гарантированная доставка сообщений. (Default: false)
		/// </summary>
		public bool ReliableEnabled
		{
			get { return Parser.ParseBool(PropertyValue("MESSAGE.RELIABLE.ENABLED"), false); }
		}

		/// <summary>
		/// {Get} Таймаут гарантированной доставки. (Default: -1)
		/// </summary>
		public TimeSpan ReliableTimeout
		{
			get
			{
				TimeSpan timeout = Parser.ParseTime(PropertyValue("MESSAGE.RELIABLE.TIMEOUT"), TimeSpan.Zero).Value;
				if ( timeout == TimeSpan.Zero )
					return TimeSpan.FromMilliseconds(-1);
				else
					return timeout;
			}
		}
		#endregion


		#region Static
		/// <summary>
		/// "MESSAGE."
		/// </summary>
		public const string TAG_PREFIX = "MESSAGE.";

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public static List<ChannelDescriptionProperty> DefaultProperties()
		//{
		//	var properties = new List<ChannelDescriptionProperty>();
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.EXPIRED", Type = "Bool", Value = "No", DefaultValue = "No", Comment = "Удалять устаревшие сообщения" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.EXPIRED_DAYS", Type = "Int", Value = "366", Comment = "Давность сообщений (в днях)" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.EXPIRED_STATUSES", Type = "List", Value = "[COMPLETED, DELETED, ERROR]", Comment = "Статусы просроченных сообщений" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.AFTER_SEND", Type = "Bool", Value = "No", DefaultValue = "No", Comment = "Удалять после отправки" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.AFTER_RECEIVE", Type = "Bool", Value = "No", DefaultValue = "No", Comment = "Удалять после приема" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.DELETED", Type = "Bool", Value = "No", DefaultValue = "No", Comment = "Удалять удаленные сообщения" });
		//	//properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.DELETE.WHAT", Type = "Enum", Value = "MESSAGE", DefaultValue = "MESSAGE", Format = "[MESSAGE | CONTENT]", Comment = "Удалять сообщение или контент" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.SCAN.ENABLED", Type = "Bool", Value = "Yes", DefaultValue = "Yes", Comment = "Вкл/Выкл сканирование новых сообщений" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.SCAN.INTERVAL", Type = "Time", Value = "00:00:10", DefaultValue = "00:00:10", Format = "{чч:мм:сс}", Comment = "Интервал проверки новых сообщений" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.SCAN.THREADS", Type = "Int", Value = "1", DefaultValue = "1", Comment = "Число потоков обработки" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.SCAN.PORTION", Type = "Int", Value = "1000", DefaultValue = "1000", Comment = "Размер порции сообщений" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.RELIABLE.ENABLED", Type = "Bool", Value = "No", DefaultValue = "No", Comment = "Вкл/Выкл гарантированную доставку сообщений" });
		//	properties.Add(new ChannelDescriptionProperty() { Name = "MESSAGE.RELIABLE.TIMEOUT", Type = "Time", Value = "00:00:00", Format = "{чч:мм:сс}", Comment = "Таймаут гарантированной доставки" });
		//	return properties;
		//}
		#endregion

	}
}
