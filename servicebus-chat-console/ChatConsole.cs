using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace servicebus_chat_console
{
	class ChatConsole
	{
        static string ConnectionString = "Endpoint=sb://serv-bus-mtz-1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gW8Ye7qF1KkSSxkbZh2tGihoTUM1bXdjvk3rj9ZgHSg=";
        static string TopicPath = "chattopic";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter name:");
            var userName = Console.ReadLine();

            var manager = new ManagementClient(ConnectionString);

            if (!manager.TopicExistsAsync(TopicPath).Result)
            {
                manager.CreateTopicAsync(TopicPath).Wait();
            }

            var description = new SubscriptionDescription(TopicPath, userName)
            {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
            };
            manager.CreateSubscriptionAsync(description).Wait();


            var topicClient = new TopicClient(ConnectionString, TopicPath);
            var subscriptionClient = new SubscriptionClient(ConnectionString, TopicPath, userName);

            subscriptionClient.RegisterMessageHandler(ProcessMessages, ExceptionReceivedHandler);

            var helloMessage = new Message(Encoding.UTF8.GetBytes("Has entered the room..."));
            helloMessage.Label = userName;
            topicClient.SendAsync(helloMessage).Wait();

            while (true)
            {
                string text = Console.ReadLine();
                if (text.Equals("exit")) break;

                var chatMessage = new Message(Encoding.UTF8.GetBytes(text));
                chatMessage.Label = userName;
                topicClient.SendAsync(chatMessage).Wait();
            }

            var goodbyeMessage = new Message(Encoding.UTF8.GetBytes("Has left the building..."));
            goodbyeMessage.Label = userName;
            topicClient.SendAsync(goodbyeMessage).Wait();

            topicClient.CloseAsync().Wait();
            subscriptionClient.CloseAsync().Wait();

        }

		static async Task ProcessMessages(Message message, CancellationToken token)
		{
			// Deserialize the message body.
			var text = Encoding.UTF8.GetString(message.Body);
			Console.WriteLine($"{ message.Label }> { text }");
		}

		static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
        }
    }
}
