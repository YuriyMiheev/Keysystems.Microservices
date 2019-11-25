﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microservices.Channels.Client;

namespace ConsoleApp1
{
	class Program
	{
		static CancellationTokenSource _cancellationSource = new CancellationTokenSource();
		//static Process _process;
		//static int _processId;

		static async Task Main(string[] args)
		{
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
			System.Threading.Thread.Sleep(1000);

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


				using var hubClient = new ChannelHubClient("http://localhost:5005");
				{
					hubClient.Connected += hubClient_Connected;
					hubClient.Disconnected += hubClient_Disconnected;

					IChannelHub_v1 api = hubClient as IChannelHub_v1;
					api.ServiceLogEventHandler(hubClient_ServiceLog);
					await api.LoginAsync("");
					IDictionary<string, SettingItem> settings = await api.GetSettingsAsync();
					await api.OpenAsync();
					await api.RunAsync();
					(List<Message>, int) messages = await api.GetLastMessagesAsync(null, null, null);
					Message msg = await api.GetMessageAsync(messages.Item1.First().LINK.Value);
					//int? resMsg = await api.ReceiveMessage(msg.LINK.Value);
					using (TextReader bodyReader = await api.ReadMessageBodyAsync(msg.LINK.Value))
					{
						string body = await bodyReader.ReadToEndAsync();
					}

					MessageContentInfo contentInfo = msg.Contents[0];
					using (TextReader contentReader = await api.ReadMessageContentAsync(contentInfo.LINK))
					{
						string content = await contentReader.ReadToEndAsync();
						byte[] data = Convert.FromBase64String(content);
						File.WriteAllBytes(contentInfo.Name, data);
					}


					//var contentInfo = new MessageContentInfo()
					//{
					//	MessageLINK = msg.LINK.Value,
					//	Name = "Новый2"
					//};
					//var contentStream = new StringReader("Hello, World!");
					//await api.SaveMessageContent(contentInfo, contentStream);

					await api.LogoutAsync();
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
		private static Task hubClient_Disconnected(IChannelHubClient hubClient, Exception error)
		{
			Console.WriteLine("Disconnected");
			return Task.CompletedTask;
		}

		private static void hubClient_Connected(IChannelHubClient hubClient)
		{
			Console.WriteLine("Connected");
		}

		private static void hubClient_ServiceLog(IChannelHubClient hubClient, string traceId, string channel, string logLevel, string text)
		{
			Console.WriteLine($@"[{DateTime.Now:yyyy-MM-dd HH\:mm\:ss}] [{traceId}] [{channel}] [{logLevel}] {text}");
		}
	}
}