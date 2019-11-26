using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Channels.Configuration
{
	/// <summary>
	/// Настройки для работы с БД.
	/// </summary>
	public class DatabaseSettings : SettingsBase
	{

		#region Ctor
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="appSettings">Настройки канала.</param>
		public DatabaseSettings(IDictionary<string, ConfigFileSetting> appSettings)
			: base(TAG_PREFIX, appSettings)
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get} Имя схемы БД. (Default: dbo)
		/// </summary>
		public string Schema
		{
			get { return Parser.ParseString(PropertyValue("DATABASE.SCHEMA"), ""); }
		}

		/// <summary>
		/// {Get} Таймаут выполнения. (Default: 0 сек)
		/// </summary>
		public TimeSpan ExecuteTimeout
		{
			get { return Parser.ParseTime(PropertyValue("DATABASE.TIMEOUT"), TimeSpan.FromSeconds(0)).Value; }
		}

		/// <summary>
		/// {Get} Имя хранимой процедуры пинга БД. (Default: rms_Ping)
		/// </summary>
		public string PingSP
		{
			get { return Parser.ParseString(PropertyValue("DATABASE.PING_SP"), ""); }
		}

		/// <summary>
		/// {Get} Вызывать хранимую процедуру. (Default: true)
		/// </summary>
		public bool PingSPEnabled
		{
			get { return Parser.ParseBool(PropertyValue("DATABASE.PING_SP.ENABLED"), true); }
		}

		/// <summary>
		/// {Get} Имя хранимой процедуры пинга БД. (Default: rms_Repair)
		/// </summary>
		public string RepairSP
		{
			get { return Parser.ParseString(PropertyValue("DATABASE.REPAIR_SP"), ""); }
		}

		/// <summary>
		/// {Get} Вызывать хранимую процедуру. (Default: true)
		/// </summary>
		public bool RepairSPEnabled
		{
			get { return Parser.ParseBool(PropertyValue("DATABASE.REPAIR_SP.ENABLED"), true); }
		}

		/// <summary>
		/// {Get} Имя хранимой процедуры уведомления об изменении статуса сообщения. (Default: rms_MessageStatusChanged)
		/// </summary>
		public string MessageStatusChangedSP
		{
			get { return Parser.ParseString(PropertyValue("DATABASE.STATUS_SP"), ""); }
		}

		/// <summary>
		/// {Get} Вызывать хранимую процедуру. (Default: true)
		/// </summary>
		public bool MessageStatusChangedSPEnabled
		{
			get { return Parser.ParseBool(PropertyValue("DATABASE.STATUS_SP.ENABLED"), true); }
		}

		/// <summary>
		/// {Get} Имя хранимой процедуры обработки входящих сообщений. (Default: rms_ReceiveMessage)
		/// </summary>
		public string ReceiveMessageSP
		{
			get { return Parser.ParseString(PropertyValue("DATABASE.RECEIVE_SP"), ""); }
		}

		/// <summary>
		/// {Get} Вызывать хранимую процедуру. (Default: true)
		/// </summary>
		public bool ReceiveMessageSPEnabled
		{
			get { return Parser.ParseBool(PropertyValue("DATABASE.RECEIVE_SP.ENABLED"), true); }
		}

		/// <summary>
		/// {Get}
		/// </summary>
		public bool ReceiveMessageSPUseOutputParam
		{
			get { return Parser.ParseBool(PropertyValue("DATABASE.RECEIVE_SP.OUTPUT"), false); }
		}
		#endregion


		#region Static
		/// <summary>
		/// "DATABASE."
		/// </summary>
		public const string TAG_PREFIX = "DATABASE.";

		///// <summary>
		///// 
		///// </summary>
		//public const string DEFAULT_PROPERTIES =
		//	"{ Name: 'DATABASE.SCHEMA', Type: 'String', Value: 'dbo', DefaultValue: 'dbo', Comment: 'Имя схемы БД' }, " +
		//	"{ Name: 'DATABASE.TIMEOUT', Type: 'Time', Value: '00:00:00', DefaultValue: '00:00:00', Format: '{чч:мм:сс}', Comment: 'Таймаут выполнения' }, " +
		//	"{ Name: 'DATABASE.PING_SP', Type: 'String', Value: 'rms_Ping', DefaultValue: 'rms_Ping', Comment: 'Имя хранимой процедуры пинга сообщений' }, " +
		//	"{ Name: 'DATABASE.PING_SP.ENABLED', Type: 'Bool', Value: 'Yes', DefaultValue: 'Yes', Comment: 'Вызывать хранимую процедуру' }, " +
		//	"{ Name: 'DATABASE.REPAIR_SP', Type: 'String', Value: 'rms_Repair', DefaultValue: 'rms_Repair', Comment: 'Имя хранимой процедуры восстановления БД' }, " +
		//	"{ Name: 'DATABASE.REPAIR_SP.ENABLED', Type: 'Bool', Value: 'Yes', DefaultValue: 'Yes', Comment: 'Вызывать хранимую процедуру' }, " +
		//	"{ Name: 'DATABASE.STATUS_SP', Type: 'String', Value: 'rms_MessageStatusChanged', DefaultValue: 'rms_MessageStatusChanged', Comment: 'Имя хранимой процедуры уведомления об изменении статуса сообщени' }, " +
		//	"{ Name: 'DATABASE.STATUS_SP.ENABLED', Type: 'Bool', Value: 'Yes', DefaultValue: 'Yes', Comment: 'Вызывать хранимую процедуру' }, " +
		//	"{ Name: 'DATABASE.RECEIVE_SP', Type: 'String', Value: 'rms_ReceiveMessage', DefaultValue: 'rms_ReceiveMessage', Comment: 'Имя хранимой процедуры обработки входящего сообщения' }, " +
		//	"{ Name: 'DATABASE.RECEIVE_SP.ENABLED', Type: 'Bool', Value: 'Yes', DefaultValue: 'Yes', Comment: 'Вызывать хранимую процедуру' }, " +
		//	"{ Name: 'DATABASE.RECEIVE_SP.OUTPUT', Type: 'Bool', Value: 'No', DefaultValue: 'No', Comment: 'Хранимая процедура возвращает значение' }";
		#endregion

	}
}
