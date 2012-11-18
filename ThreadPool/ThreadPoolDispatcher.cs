using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
	class ThreadPoolDispatcher
	{
		private readonly Func<bool> _isAnyTasksLeft;

		public EventWaitHandle WaitHandler = new AutoResetEvent(false);
		//private readonly ICollection<ThreadWorker> _threadWorkerList;
		private readonly Func<ThreadWorker> _workerProvider;
		private bool _closing = false;

		
		public void WaitAll()
		{
			_closing = true;
			WaitHandler.WaitOne();
		}

		public ThreadPoolDispatcher(Func<bool> isAnyTasksLeft,Func<ThreadWorker> workerProvider)
		{
			//_threadWorkerList = threadWorkerList;
			if(isAnyTasksLeft == null) throw new ArgumentException("isAnyTasksLeft");
			if (workerProvider == null) throw new ArgumentException("workerProvider");
			_isAnyTasksLeft = isAnyTasksLeft;
			_workerProvider = workerProvider;
		}

		public void CallWorkers()
		{
			//dont use task completed callbacks due to assumption that threadpool performs many short tasks 
			while (true)
			{
				if (!_isAnyTasksLeft.Invoke())
				{
					if (_closing)
					{
						WaitHandler.Set();
						break;
					}
					
					WaitHandler.WaitOne();
				}

				ThreadWorker worker = _workerProvider.Invoke();
				if (worker != null)
					//resume worker 
					worker.WaitHandler.Set();
			}
		}
	}
}
