using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

namespace Microservices.Bus.VMO
{
	public static class VMOConverter
	{
		private static IMapper mapper;

		/// <summary>
		/// Type initializer.
		/// </summary>
		static VMOConverter()
		{
			var config = new MapperConfiguration(cfg =>
				{
					//cfg.CreateMap<Contact, Contact>();
				});
			mapper = config.CreateMapper();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmo"></param>
		public static VMO.ExceptionWrapper ToVmo(this Exception obj)
		{
			if (obj == null)
				return null;

			return new VMO.ExceptionWrapper()
			{
				InnerException = obj.InnerException?.ToVmo(),
				FullMessage = obj.Message,
				InnerMessages = obj.Message,
				Message = obj.Message,
				Source = obj.Source,
				StackTrace = obj.StackTrace,
			};
		}


	}
}
