using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Processor
{
	public class LongProcess
	{
		public void DoLongProcess(Message message)
		{
			var messageLocal = message;
			System.Threading.Thread.Sleep(5000);
		}
	}
}
