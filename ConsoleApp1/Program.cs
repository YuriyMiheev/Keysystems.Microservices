using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microservices.ChannelConnector;

namespace ConsoleApp1
{
	class Program
	{
		static CancellationTokenSource _cancellationSource = new CancellationTokenSource();
		//static Process _process;
		//static int _processId;

		static async Task Main(string[] args)
		{
			System.Threading.Thread.Sleep(1000);

			//TaskScheduler.UnobservedTaskException += (s, e) =>
			//{
			//	if (e.Exception != null)
			//	{
			//		foreach (Exception ex in e.Exception.InnerExceptions)
			//		{
			//			//LogError(ex);
			//		}
			//	}

			//	e.SetObserved();
			//};

			try
			{
				//var startInfo = new ProcessStartInfo()
				//{
				//	FileName = @"C:\Projects\Keysystems.Microservices\WebApplication1\bin\Debug\netcoreapp3.0\WebApplication1.exe",
				//	UseShellExecute = false,
				//	//CreateNoWindow = true,
				//	Arguments = "--Urls http://*:5005"
				//};

				//using (var process = Process.Start(startInfo))
				//{
				//	_processId = process.Id;


				using (IChannelHubClient hub = new ChannelHubClient("http://localhost:5005"))
				{
					hub.Connected += Connected;
					hub.Disconnected += Disconnected;
					hub.LogReceived += LogReceived;
					hub.MessagesReceived += MessagesReceived;
					hub.ChannelStatus.PropertyChanged += StatusChanged;

					await hub.LoginAsync("FD0CFC4A-3D0F-4EA8-B1D4-6FC306FCD73D");
					IDictionary<string, SettingItem> settings = await hub.GetSettingsAsync();
					//var newSettings = new Dictionary<string, string>();
					//newSettings[".RealAddress"] = @"Data Source=.\SQLEXPRESS; Initial Catalog=EsbliteClient_v3.8; User ID=rms; Password=rms";
					//await hub.SetSettingsAsync(newSettings);

					await hub.OpenChannelAsync();
					await hub.RunChannelAsync();
					(List<Message>, int) messages = await hub.GetMessagesAsync(null, null, null);
					if (messages.Item1.Count > 0)
					{
						Message msg = await hub.GetMessageAsync(messages.Item1.First().LINK.Value);
						//int? resMsg = await hub.ReceiveMessage(msg.LINK.Value);
						using (TextReader bodyReader = await hub.ReadMessageBodyAsync(msg.LINK.Value))
						{
							string body = await bodyReader.ReadToEndAsync();
						}

						MessageContentInfo contentInfo = msg.Contents[0];
						using (TextReader contentReader = await hub.ReadMessageContentAsync(contentInfo.LINK))
						{
							string content = await contentReader.ReadToEndAsync();
							byte[] data = Convert.FromBase64String(content);
							File.WriteAllBytes(contentInfo.Name, data);
						}
					}


					//var contentInfo = new MessageContentInfo()
					//{
					//	MessageLINK = msg.LINK.Value,
					//	Name = "Новый2"
					//};
					//var contentStream = new StringReader("Hello, World!");
					//await hub.SaveMessageContent(contentInfo, contentStream);

					Console.ReadLine();
					await hub.CloseChannelAsync();
					await hub.LogoutAsync();
				}

				//Console.ReadLine();
				//_cancellationSource.Cancel();
				//}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				//Process.GetProcessById(_processId)?.Kill();
				Console.ReadLine();
			}
		}

		private static void StatusChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Console.WriteLine($"Изменился статус: {e.PropertyName}");
		}

		private static int _count = 0;
		private static void MessagesReceived(IChannelHubClient hubClient, Message[] messages)
		{
			_count++;
			Console.WriteLine($"{_count}. Messages: {messages.Length}");
		}

		private static void LogReceived(IChannelHubClient hubClient, IDictionary<string, object> logRecord)
		{
			var machineName = logRecord["MachineName"];
			var processId = logRecord["ProcessId"];
			var connectionId = logRecord["ConnectionId"];
			var virtAddress = logRecord["VirtAddress"];
			var logLevel = logRecord["LogLevel"];
			var text = logRecord["Text"];
			Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{machineName}] [{processId}] [{connectionId}] [{virtAddress}] [{logLevel}]");
			Console.WriteLine($"{text}");
		}

		private static void Connected(IChannelHubClient hubClient)
		{
			Console.WriteLine("Connected");
		}

		private static Task Disconnected(IChannelHubClient hubClient, Exception error)
		{
			Console.WriteLine("Disconnected");
			return Task.CompletedTask;
		}
	}
}
