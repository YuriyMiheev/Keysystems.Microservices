﻿using System;
using System.ComponentModel;

namespace Microservices.Channels
{
	/// <summary>
	/// Статус канала.
	/// </summary>
	public class ChannelStatus : INotifyPropertyChanged
	{
		/// <summary>
		/// 
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private bool _created;
		/// <summary>
		/// 
		/// </summary>
		public bool Created
		{
			get { return _created; }
			set
			{
				if (_created != value)
				{
					_created = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Created)));
				}
			}
		}

		private bool _opened;
		/// <summary>
		/// 
		/// </summary>
		public bool Opened
		{
			get { return _opened; }
			set
			{
				if (_opened != value)
				{
					_opened = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Opened)));
				}
			}
		}

		private bool _running;
		/// <summary>
		/// {Get}
		/// </summary>
		public bool Running
		{
			get { return _running; }
			set
			{
				if (_running != value)
				{
					_running = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Running)));
				}
			}
		}

		private bool? _online;
		/// <summary>
		/// 
		/// </summary>
		public bool? Online
		{
			get { return _online; }
			set
			{
				if (_online != value)
				{
					_online = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Online)));
				}
			}
		}

		private Exception _lastError;
		/// <summary>
		/// 
		/// </summary>
		public Exception Error
		{
			get { return _lastError; }
			set
			{
				if (_lastError != value)
				{
					_lastError = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Error)));
				}
			}
		}


		public override string ToString()
		{
			return $"{this.Created}|{this.Opened}|{this.Running}|{this.Online}|{this.Error?.Message}";
		}
	}
}
