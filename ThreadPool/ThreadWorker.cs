using System;
using System.Threading;
using Task = ThreadPoolExample.Task;

namespace ThreadPool
{
	class ThreadWorker
	{
		public EventWaitHandle WaitHandler = new AutoResetEvent(false);
		private readonly object _locker = new object();
		private readonly Action _taskCompletedCallback;
		private bool _isBusy;
		public bool IsBusy 
		{
			get { return _isBusy; }
			private set
			{
				lock (_locker)
				{
					_isBusy = value;
				}
			}
		}
		
		private readonly Func<Task> _taskProvider;


		public ThreadWorker(Func<Task> taskProvider,Action taskCompletedCallback)
		{
			if(taskProvider == null) throw new ArgumentException("TaskProvider");
			if (taskCompletedCallback == null) throw new ArgumentException("TaskCompletedCallback");

			_taskCompletedCallback = taskCompletedCallback;
			_taskProvider = taskProvider;
		}

		private void ResetThreadState()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Normal;	
		}

		public void Run()
		{
			ResetThreadState();
			while (true)
			{
				WaitHandler.WaitOne();
				IsBusy = true;
				Task task = _taskProvider.Invoke();
				if (task == null)
				{
					IsBusy = false;
					continue;
				}
				
				try
				{
					task.Execute();
				}
				finally
				{
					ResetThreadState();
					IsBusy = false;
					_taskCompletedCallback();
				}
			}
		}
	}
}
			