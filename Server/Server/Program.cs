using Server.Processor;
using Shared;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	class Program
	{
		const int PORT_NO = 5000;
		const string SERVER_IP = "127.0.0.1";

		static void Main(string[] args)
		{
			var messageManager = new MessageManager();
			Task.Run(() => messageManager.RunManager());


			//---listen at the specified IP and port no.---
			IPAddress localAdd = IPAddress.Parse(SERVER_IP);
			TcpListener listener = new TcpListener(localAdd, PORT_NO);
			Console.WriteLine("Listening...");
			listener.Start();
			while (true)
			{
				//---incoming client connected---
				TcpClient client = listener.AcceptTcpClient();

				//---get the incoming data through a network stream---
				NetworkStream nwStream = client.GetStream();
				byte[] buffer = new byte[client.ReceiveBufferSize];

				//---read incoming stream---
				int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

				//---convert the data received into a string---
				string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);


				Message msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(dataReceived);

				Console.WriteLine("Received : " + msg.MessageText);

				// queue the message for handling later.
				MessageQueue.Instance.Queue(msg);

				//---write back the text to the client---
				Ack ack = new Ack() { ResponseText = "I received: " + msg.MessageText };
				var ackSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(ack);

				Console.WriteLine("Sending back: " + ack.ResponseText);
				byte[] bytesToReturn = ASCIIEncoding.ASCII.GetBytes(ackSerialized);
				nwStream.Write(bytesToReturn, 0, bytesToReturn.Length);
			}
			listener.Stop();
			Console.ReadLine();
		}
	}
}
