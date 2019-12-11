using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Bus
{
	public static class ServiceInfoExtensions
	{
		public static ServiceInfo WithoutProperties(this ServiceInfo obj)
		{
			return new ServiceInfo()
				{
					AddinsDir = obj.AddinsDir,
					Administrator = obj.Administrator,
					AuthorizeEnabled = obj.AuthorizeEnabled,
					BaseDir = obj.BaseDir,
					ConfigFileName = obj.ConfigFileName,
					CurrentTime = obj.CurrentTime,
					Database = obj.Database,
					DebugEnabled = obj.DebugEnabled,
					ExternalAddress = obj.ExternalAddress,
					InstanceID = obj.InstanceID,
					InternalAddress = obj.InternalAddress,
					LicenseFile = obj.LicenseFile,
					LINK = obj.LINK,
					LogFilesDir = obj.LogFilesDir,
					MachineName = obj.MachineName,
					MaxUploadSize = obj.MaxUploadSize,
					Online = obj.Online,
					Running = obj.Running,
					ServiceName = obj.ServiceName,
					ShutdownReason = obj.ShutdownReason,
					ShutdownTime = obj.ShutdownTime,
					StartTime = obj.StartTime,
					StartupError = obj.StartupError,
					TempDir = obj.TempDir,
					ToolsDir = obj.ToolsDir,
					Version = obj.Version
				};
		}
	}
}
