using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservices.Channels.Client
{
	/// <summary>
	/// 
	/// </summary>
	public class ChannelStatus : INotifyPropertyChanged
	{
		/// <summary>
		/// 
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;


		private bool _opened;
		/// <summary>
		/// {Get}
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
		/// {Get}
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

	}
}
