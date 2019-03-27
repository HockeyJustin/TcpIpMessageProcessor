using Shared;
using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
	class Program
	{
		const int PORT_NO = 5000;
		const string SERVER_IP = "127.0.0.1";

		static void Main(string[] args)
		{
			Console.WriteLine("Enter text and hit 'Return', or type 'exit' to close client.");

			string input = "";

			while (1 == 1)
			{
				input = Console.ReadLine();

				if (input != null && String.IsNullOrWhiteSpace(input))
				{
					continue;
				}

				if (input != null && input.ToLower().Trim() == "exit")
				{
					break;
				}

				var message = new Message() { MessageText = input };
				var msg = Newtonsoft.Json.JsonConvert.SerializeObject(message);


				TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
				NetworkStream nwStream = client.GetStream();

				//---data to send to the server---
				byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(msg);

				//---send the text---
				Console.WriteLine("Sending to server : " + message.MessageText);
				nwStream.Write(bytesToSend, 0, bytesToSend.Length);

				//---read back the text---
				byte[] bytesToRead = new byte[client.ReceiveBufferSize];
				int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

				string response = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
				Ack ack = Newtonsoft.Json.JsonConvert.DeserializeObject<Ack>(response);

				Console.WriteLine("Received Back : " + ack.ResponseText);
				//Console.ReadLine();

				client.Close();
				client.Dispose();

			}



		}
	}
}

