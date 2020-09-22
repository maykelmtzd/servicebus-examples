using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace servicebus_receiver
{
	class ReceiverConsole
	{
		static string connectionString = "Endpoint=sb://serv-bus-mtz-1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gW8Ye7qF1KkSSxkbZh2tGihoTUM1bXdjvk3rj9ZgHSg=";
		static string queuePath = "queue-1";
		static void Main(string[] args)
		{
			var queueClient = new QueueClient(connectionString, queuePath);

			queueClient.RegisterMessageHandler(ProcessMessagesAsync, HandlerExceptionsAsync);

			Console.WriteLine("Press enter to exit");
			Console.ReadLine();

			queueClient.CloseAsync().Wait(); 
		}

		private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
		{
			var content = Encoding.UTF8.GetString(message.Body);
			Console.WriteLine($"Received: {content}");
		}
		private static Task HandlerExceptionsAsync(ExceptionReceivedEventArgs arg)
		{
			throw new NotImplementedException();
		}
	}
}
