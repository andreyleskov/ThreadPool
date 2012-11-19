using System;
using System.Threading;
using Task = ThreadPoolExample.Task;

namespace ThreadPool
{
	public class ThreadWorker
	{
		private readonly EventWaitHandle _waitHandler = new AutoResetEvent(false);
		public bool IsBusy
		{
			get; private set; 
		}
		
		private readonly TaskQueue _taskProvider;
		private Thread _runThread;

		private bool _isStopped;
		public void Stop()
		{
			_isStopped = true;
			Continue();
			_runThread.Join();
		}

		public void Pause()
		{
			_waitHandler.WaitOne();
		}

		public void Continue()
		{
			_waitHandler.Set();
		}

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
			_runThread = Thread.CurrentThread;
			ResetThreadState();
			while (!_isStopped)
			{
				Task task;
				_taskProvider.Take(out task);
				IsBusy = true;
				try
				{
					if(task != null)
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
			