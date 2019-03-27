using Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Server.Processor
{
	public sealed class MessageQueue
	{
		public ConcurrentQueue<Message> Messages { get; set; }
		private static object _locker = new object();

		private MessageQueue() { }

		private static MessageQueue _instance;
		public static MessageQueue Instance
		{
			get
			{
				lock (_locker)
				{
					if (_instance == null)
					{
						_instance = new MessageQueue();
						_instance.Messages = new ConcurrentQueue<Message>();
					}

				}

				return _instance;
			}
		}


		public void Queue(Message message)
		{
			lock (_locker)
			{
				Messages.Enqueue(message);
			}
		}


		public Message DeQueue()
		{
			Message returnValue = null;
			lock (_locker)
			{
				Messages.TryDequeue(out returnValue);
			}
			return returnValue;
		}


		public List<Message> DeQueueMany(int maxToDequeue)
		{
			List<Message> returnValue = new List<Message>();

			for (int i = 0; i < maxToDequeue; i++)
			{
				Message testValue = null;
				lock (_locker)
				{
					Messages.TryDequeue(out testValue);
				}

				if (testValue != null)
				{
					returnValue.Add(testValue);
				}
				else
				{
					break;
				}
			}

			return returnValue;
		}




	}
}

