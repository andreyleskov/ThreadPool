using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Task = ThreadPool.Task;

namespace ThreadPool
{
	public class ThreadWorker<TPriority,T> where T:Task
	{
		public bool IsBusy
		{
			get; private set; 
		}

		private readonly BlockingCollection<KeyValuePair<TPriority, T>> _taskProvider;

		public ThreadWorker(BlockingCollection<KeyValuePair<TPriority,T>> taskProvider)
		{
			if(taskProvider == null) throw new ArgumentException("TaskProvider");
			_taskProvider = taskProvider;
		}

		private void ResetThreadState()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Normal;	
		}

		public Thread Thread { get; private set; }
		public void Run()
		{
			ResetThreadState();
			Thread = Thread.CurrentThread;

			foreach (KeyValuePair<TPriority, T> task in _taskProvider.GetConsumingEnumerable())
			{
				try
				{
					if (task.Value == null) continue;
					IsBusy = true;
					task.Value.Execute();
				}
				finally
				{
					ResetThreadState();
					IsBusy = false;
				}
			}
		}
	}
}
			