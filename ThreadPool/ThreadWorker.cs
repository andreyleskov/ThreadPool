using System;
using System.Threading;
using Task = ThreadPool.Task;

namespace ThreadPool
{
	public class ThreadWorker
	{
		public bool IsBusy
		{
			get; private set; 
		}
		
		private readonly TaskQueue _taskProvider;

		public ThreadWorker(TaskQueue taskProvider)
		{
			if(taskProvider == null) throw new ArgumentException("TaskProvider");
			_taskProvider = taskProvider;
		}

		private void ResetThreadState()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Normal;	
		}

		public void Run()
		{
			ResetThreadState();
			foreach(Task task in (new Task[2]))//_taskProvider.GetTasksEnumerable())
			{
				try
				{
					if (task == null) continue;
					IsBusy = true;
					task.Execute();
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
			