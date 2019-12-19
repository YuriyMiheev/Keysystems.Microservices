using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microservices.Bus.Channels
{
	public static class ProcessExtensions
	{
		public static Process FindProcessById(this Process[] processes, int processId)
		{
			return processes.SingleOrDefault(p => p.Id == processId);
		}
	}
}
