using System;

namespace ThreadPool
{
	//* Интерфейс Task должен содержать один метод: void execute(), который вызывается в произвольном потоке.
	public class Task
	{
		public void Execute()
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
