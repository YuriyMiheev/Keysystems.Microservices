using System.ComponentModel;
using System.Diagnostics;

namespace Microservices.Channels
{
	/// <summary>
	/// Статус канала.
	/// </summary>
	//[DebuggerDisplay("{this.Created|this.Opened|this.Running|this.Online}")]
	public class ChannelStatus : INotifyPropertyChanged
	{
		/// <summary>
		/// 
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private bool _created;
		/// <summary>
		/// {Get}
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


		public override string ToString()
		{
			return $"{this.Created}|{this.Opened}|{this.Running}|{this.Online}";
		}
	}
}
