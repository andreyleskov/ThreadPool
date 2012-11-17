using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolExample
{
	//* Интерфейс Task должен содержать один метод: void execute(), который вызывается в произвольном потоке.
	public class Task
	{
		void Execute()
		{
			_payload.Invoke();
		}

		private readonly Action _payload;

		public Task(Action payload)
		{
			_payload = payload;
		}
	}
}
