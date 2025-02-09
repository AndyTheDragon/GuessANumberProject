using System.Net;
using System.Net.Sockets;
using System.Text;
using GuessANumberGame;

namespace GameServer;

internal class Program
{
    private static int _port = 7700; // Configurable port number
    private static string _exitCommand = "q"; // Configurable exit command
    private static Game game;
    
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting the GuessANumberGame Server!");
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Console.WriteLine($"Server is running on port {_port}");
            game = new Game(1, 100);
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("{0:u} Client connected", DateTime.UtcNow);
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            string clientName = await GetClientNameAsync(reader, writer);            
            
            await writer.WriteLineAsync($"Welcome {clientName}  to Guess A Number Game! Try and guess the number between 1 and 100.");
            int userGuess;
            do
            {
                userGuess = await GetUserGuessAsync(reader, writer);
                if (userGuess.Equals(Int32.MinValue)) break;
                string response;
                lock (game)
                {
                    response = game.Guess(userGuess).ToString();
                }

                await writer.WriteLineAsync(response);
            } while (game.IsRunning);

        }
        catch (IOException e)
        {
            Console.WriteLine("{0:u} Client disconnected unexpectedly.", DateTime.UtcNow);
            Console.WriteLine($"{0:u} Unexpected error: {e.Message}", DateTime.UtcNow);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");

        }
        finally
        {
            client.Close();
            Console.WriteLine("{0:u} Client disconnected", DateTime.UtcNow);
        }
        
    }
    
    private static async Task<int> GetUserGuessAsync(StreamReader reader, StreamWriter writer)
    {
        try
        {
            await writer.WriteLineAsync("Input a number, please.");
            var input = await reader.ReadLineAsync();
            
            if (input == null) return Int32.MinValue;
            if (input.Trim().ToLower() == _exitCommand) return Int32.MinValue;
            if (int.TryParse(input, out int userGuess)) return userGuess;

            await  writer.WriteLineAsync("Invalid input. Please enter a valid number.");
            return await GetUserGuessAsync(reader, writer);
        }
        catch (Exception e)
        {
            writer.WriteLine("An unexpected error occurred. Disconnecting...");
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }

    }
    
    private static async Task<string> GetClientNameAsync(StreamReader reader, StreamWriter writer)
    {
        await writer.WriteLineAsync("Please enter your name:");
        var input = await reader.ReadLineAsync();
        return input ?? GetClientNameAsync(reader, writer).Result ;
    }
}

