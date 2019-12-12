using System;
using System.Collections.Generic;

namespace Microservices.Bus.Data.DAO
{
	/// <summary>
	/// Информация о сервисе.
	/// </summary>
	public class ServiceInfo
	{
		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual int LINK { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string InstanceID { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string ServiceName { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string Version { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual DateTime? StartTime { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual DateTime? ShutdownTime { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string ShutdownReason { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual bool? Online { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual string ExternalAddress { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual string InternalAddress { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual bool? DebugEnabled { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual bool? AuthorizeEnabled { get; set; }

		/// <summary>
		/// {Get,Set}
		/// </summary>
		public virtual int MaxUploadSize { get; set; }

		/// <summary>
		/// {Get,Set} 
		/// </summary>
		public virtual IList<ServiceProperty> Properties { get; set; }
	}
}
