using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace servicebus_sender
{
	public class SenderConsole
	{
		static string connectionString = "Endpoint=sb://serv-bus-mtz-1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gW8Ye7qF1KkSSxkbZh2tGihoTUM1bXdjvk3rj9ZgHSg=";
		static string queuePath = "queue-1";

		static void Main(string[] args)
		{
			var queueClient = new QueueClient(connectionString, queuePath);

			for (int i = 0; i < 10; i++)
			{
				var content = $"Message {i}";

				var message = new Message(Encoding.UTF8.GetBytes(content));
				queueClient.SendAsync(message).Wait();

				Console.WriteLine("Sent " + i);
			}

			queueClient.CloseAsync().Wait();

			Console.WriteLine("Messages sent...");
			Console.ReadLine();
		}
	}
}
