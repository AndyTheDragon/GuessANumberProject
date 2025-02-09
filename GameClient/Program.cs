using System.Net.Sockets;
using System.Text;

namespace GameClient
{
    class Program
    {
        private static string _serverAddress = "localhost";
        private static List<string> _servers = ["localhost", "wyrmlings.dk"];
        private static int _serverPort = 7700;
        
        static async Task Main(string[] args)
        {
            _serverAddress = ChooseServer();
            
            Console.WriteLine($"Connecting to server {_serverAddress}...");
            using TcpClient client = new TcpClient(_serverAddress, _serverPort);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };


            var cts = new CancellationTokenSource();
            var receiveTask = ReceiveMessagesAsync(reader, cts.Token);
            var sendTask = SendMessageAsync(writer, cts.Token);
            
            await Task.WhenAny(receiveTask, sendTask);
            cts.Cancel();
        }

        private static async Task ReceiveMessagesAsync(StreamReader reader, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var message = await reader.ReadLineAsync();
                    if (message == null)
                    {
                        break;
                    }
                    Console.WriteLine(message);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection to server terminated.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error receiving message: " + e.Message);
            }
        }

        private static async Task SendMessageAsync(StreamWriter writer, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var message = Console.ReadLine();
                    if (message == null || message.Equals("q")) break;
                    await writer.WriteLineAsync(message);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection to server terminated.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message: " + e.Message);
            }
        }
        
        private static string ChooseServer()
        {
            Console.WriteLine("Choose a server:");
            for (int i = 0; i < _servers.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {_servers[i]}");
            }

            var choice = Console.ReadLine();
            if (int.TryParse(choice, out int serverChoice) && serverChoice > 0 && serverChoice <= _servers.Count)
            {
                return _servers[serverChoice - 1];
            }

            Console.WriteLine("Invalid choice. Please try again.");
            return ChooseServer();
        }

    }
}