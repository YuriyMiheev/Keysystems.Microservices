using System.Collections.Generic;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Информация о канале.
	/// </summary>
	public class ChannelInfo //: IMainChannelSettings
	{
		private readonly IDictionary<string, ChannelInfoProperty> _properties;


		public ChannelInfo()
		{
			_properties = new Dictionary<string, ChannelInfoProperty>();
		}


		public IDictionary<string, ChannelInfoProperty> Properties => _properties;

		public int LINK { get; set; }

		public bool Enabled { get; set; }

		public string Provider { get; set; }

		public string Type { get; set; }

		public string Comment { get; set; }

		public string Name { get; set; }

		public string VirtAddress { get; set; }

		public string RealAddress { get; set; }

		public string SID { get; set; }

		public int Timeout { get; set; }

		public string PasswordIn { get; set; }

		public string PasswordOut { get; set; }
	}
}
