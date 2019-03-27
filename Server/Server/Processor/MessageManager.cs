using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Processor
{
	public class MessageManager
	{

		public MessageManager() { }

		public void RunManager()
		{
			while (1 == 1)
			{

				var nextJobs = MessageQueue.Instance.DeQueueMany(5);

				if (nextJobs == null || !nextJobs.Any())
				{
					System.Threading.Thread.Sleep(1000);
				}
				else
				{
					Parallel.ForEach(nextJobs, new ParallelOptions { MaxDegreeOfParallelism = 4 },
						_ =>
						{
							NotifyStart(_);

							new LongProcess().DoLongProcess(_);

							NotifyEnd(_);
						}
						);
				}
			}
		}


		public void NotifyStart(Message message)
		{
			var messageLocal = message;
			Console.WriteLine($"Start Work {message.MessageText}");
		}

		public void NotifyEnd(Message message)
		{
			var messageLocal = message;
			Console.WriteLine($"End Work {message.MessageText}");
		}


	}
}
