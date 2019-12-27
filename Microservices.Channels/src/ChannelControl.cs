using System;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels.Configuration;
using Microservices.Channels.Data;
using Microservices.Channels.Logging;
using Microservices.Configuration;
using Microservices.Data;

namespace Microservices.Channels
{
	public class ChannelControl : IChannelControl
	{
		private readonly IChannelService _channel;
		private CancellationTokenSource _cancellationSource;
		private readonly IAppSettingsConfig _appConfig;
		private readonly IDatabase _database;
		private readonly IChannelDataAdapter _dataAdapter;
		private readonly IMessageScanner _scanner;
		private readonly ILogger _logger;
		private readonly ChannelStatus _status;
		private readonly MainSettings _mainSettings;
		private readonly DatabaseSettings _databaseSettings;
		private readonly MessageSettings _messageSettings;
		private bool _initialized;


		public ChannelControl(IAppSettingsConfig appConfig, IChannelService channel, ChannelStatus status, IDatabase database, IChannelDataAdapter dataAdapter, IMessageScanner scanner, ILogger logger)
		{
			_channel = channel ?? throw new ArgumentNullException(nameof(channel));
			_status = status ?? throw new ArgumentNullException(nameof(status));
			_database = database ?? throw new ArgumentNullException(nameof(database));
			_dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
			_scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_mainSettings = _appConfig.MainSettings();
			_databaseSettings = _appConfig.DatabaseSettings();
			_messageSettings = _appConfig.MessageSettings();
			_cancellationSource = new CancellationTokenSource();
		}


		#region Control
		public Task OpenChannelAsync(CancellationToken cancellationToken = default)
		{
			return Task.Run(() =>
				{
					Initialize();

					_status.Opened = true;
					//_channel.UpdateMyselfContact(this.Info);

					//ServiceInfo serviceInfo = this.MessageService.GetInfo();
					//if (serviceInfo.ChannelsSettings.CheckDatabaseUsed)
					//{
					//	ChannelInfo existChannel;
					//	if (TryCheckDatabaseUsedOtherChannel(out existChannel))
					//	{
					//		if (existChannel.LINK != this.LINK)
					//		{
					//			var error = new ChannelException(this, String.Format("БД уже используется другим каналом: {0}.", existChannel));
					//			error.ErrorCode = ChannelException.DatabaseUsedOtherChannel;
					//			error.Data.Add("OtherChannelInfo", existChannel.ToDto());

					//			throw error;
					//		}
					//	}

					//	Uri rmsUri;
					//	Contact contact;
					//	if (TryCheckDatabaseUsedOtherRms(out rmsUri, out contact))
					//	{
					//		var error = new ChannelException(this, String.Format("БД уже используется другим RMS сервисом: {0}.", rmsUri.Host));
					//		error.ErrorCode = ChannelException.DatabaseUsedOtherRms;
					//		error.Data.Add("OtherRmsUri", rmsUri);
					//		if (contact != null)
					//			error.Data.Add("OtherRmsContact", contact.ToDto());

					//		throw error;
					//	}
					//}
				});
		}

		public async Task RunChannelAsync(CancellationToken cancellationToken = default)
		{
			CheckOpened();

			//if (_cancellationSource.IsCancellationRequested)
			//	_cancellationSource = new CancellationTokenSource();

			if (await TryConnectAsync(out Exception error) == false)
				throw error;

			//_channel.DeleteDeletedMessages();

			//void DeleteDeletedMessages()
			//{
			//	if (_messageSettings.DeleteDeleted)
			//	{
			//		try
			//		{
			//			string sql = $"DELETE FROM {Database.Tables.MESSAGES} WHERE STATUS='{MessageStatus.DELETED}'";
			//			int count = _dataAdapter.ExecuteUpdate(sql);
			//			_logger.LogTrace($"Удалено сообщений: {count}");
			//		}
			//		catch (Exception ex)
			//		{
			//			_logger.LogError(ex);
			//		}
			//	}
			//}

			//void ResetSendingMessages()
			//{
			//	try
			//	{
			//		string statusInfo = "Отправка сообщения была прервана.";
			//		string statusDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
			//		string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}', STATUS_DATE='{statusDate}' WHERE DIRECTION='{MessageDirection.OUT}' AND STATUS='{MessageStatus.SENDING}'";
			//		int count = _dataAdapter.ExecuteUpdate(sql);
			//		_logger.LogTrace($"Найдено недоставленных сообщений: {count}");
			//	}
			//	catch (Exception ex)
			//	{
			//		_logger.LogError(ex);
			//	}
			//}

			//void ResetReceivingMessages()
			//{
			//	try
			//	{
			//		string statusInfo = "Прием сообщения был прерван.";
			//		string statusDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
			//		string sql = $"UPDATE {Database.Tables.MESSAGES} SET STATUS='{MessageStatus.ERROR}', STATUS_INFO='{statusInfo}', STATUS_DATE='{statusDate}' WHERE DIRECTION='{MessageDirection.IN}' AND STATUS='{MessageStatus.RECEIVING}'";
			//		int count = _dataAdapter.ExecuteUpdate(sql);
			//		_logger.LogTrace($"Найдено непринятых сообщений: {count}");
			//	}
			//	catch (Exception ex)
			//	{
			//		_logger.LogError(ex);
			//	}
			//}

			//DeleteDeletedMessages();
			//ResetSendingMessages();
			//ResetReceivingMessages();

			if (_messageSettings.ScanEnabled)
			{
				_scanner.StartScan(_messageSettings.ScanInterval, _messageSettings.ScanPortion, _cancellationSource.Token);
			}

			_status.Running = true;

			//UpdateMyselfContact(this.Info);
		}

		public Task StopChannelAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				_cancellationSource.Cancel(false); //_scanner.StopScan();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
			finally
			{
				_status.Running = false;
				_status.Online = null;

				//UpdateMyselfContact(this.Info);
			}
		}

		public Task CloseChannelAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				//_channel.Dispose();
				//Stop();
				_cancellationSource.Cancel(false);

				_status.Running = false;
				_status.Online = null;
				_status.Opened = false;

				//UpdateMyselfContact(myInfo);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}
			//finally
			//{
			//	Dispose();
			//}
		}
		#endregion


		#region Diagnostic
		public Task<bool> TryConnectAsync(out Exception error)
		{
			Initialize();

			if (_database.TryConnect(out ConnectionException ex))
			{
				error = null;
				_status.Online = true;
				return true;
			}
			else
			{
				error = ex;
				_status.Online = false;
				return false;
			}
		}

		/// <summary>
		/// Запустить диагностику канала.
		/// </summary>
		public void CheckState()
		{
			Initialize();

			using DbContext dbContext = _database.ValidateSchema();
		}

		/// <summary>
		/// Восстановить работоспособность канала.
		/// </summary>
		public void Repair()
		{
			Initialize();

			if (_databaseSettings.RepairSPEnabled)
			{
				try
				{
					string repairSP = _databaseSettings.RepairSP;
					if (String.IsNullOrWhiteSpace(repairSP))
						throw new InvalidOperationException("Не указано имя хранимой процедуры восстановления БД.");

					_logger.LogTrace($"Вызов хранимой процедуры \"{repairSP}\".");
					_dataAdapter.CallRepairSP(repairSP);
				}
				catch (Exception ex)
				{
					//SetError(ex);
					_logger.LogError(ex);
				}
			}

			using DbContext dbContext = _database.CreateOrUpdateSchema();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Ping()
		{
			Initialize();

			if (!TryConnect(out Exception error))
				throw error;

			if (_databaseSettings.PingSPEnabled)
			{
				try
				{
					string pingSP = _databaseSettings.PingSP;
					if (String.IsNullOrWhiteSpace(pingSP))
						throw new InvalidOperationException("Не указано имя хранимой процедуры пинга БД.");

					_logger.LogTrace($"Вызов хранимой процедуры \"{pingSP}\".");
					_dataAdapter.CallPingSP(pingSP);
				}
				catch (Exception ex)
				{
					SetError(ex);
					_logger.LogError(ex);
				}
			}
		}
		#endregion


		//#region Error
		///// <summary>
		///// Сбросить ошибку.
		///// </summary>
		//public void ClearError()
		//{
		//}

		/// <summary>
		/// Запомнить ошибку.
		/// </summary>
		/// <param name="error"></param>
		public void SetError(Exception error)
		{
		}

		///// <summary>
		///// Вызвать ошибку.
		///// </summary>
		///// <param name="text"></param>
		///// <returns></returns>
		//public ChannelException ThrowError(string text)
		//{
		//	return new ChannelException(this, text);
		//}
		//#endregion



		private void Initialize()
		{
			if (_initialized)
				return;

			lock (this)
			{
				if (_initialized)
					return;

				if (_cancellationSource.IsCancellationRequested)
					_cancellationSource = new CancellationTokenSource();

				_database.Schema = _databaseSettings.Schema;
				_database.ConnectionString = _mainSettings.RealAddress;
				_dataAdapter.ExecuteTimeout = (int)_databaseSettings.ExecuteTimeout.TotalSeconds;

				_initialized = true;
			}
		}

		private void CheckOpened()
		{
			if (!_status.Opened)
				throw new InvalidOperationException("Сервис-канал закрыт.");
		}

	}
}
