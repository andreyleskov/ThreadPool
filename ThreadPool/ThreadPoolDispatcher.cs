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
		private readonly Func<bool> _isPendingTasksLeft;

		public readonly EventWaitHandle WaitHandler = new AutoResetEvent(false);
		private readonly EventWaitHandle _stopWaitHandler = new AutoResetEvent(false);
		private readonly Func<ThreadWorker> _workerProvider;
		private bool _closing = false;
		
		private readonly Func<ThreadWorker[]> _busyWorkers;


		public void WaitAll()
		{
			_closing = true;
			_stopWaitHandler.WaitOne();
		}

		public ThreadPoolDispatcher(Func<bool> isPendingTasksLeft, Func<ThreadWorker[]> busyWorkers, Func<ThreadWorker> workerProvider)
		{
			//_threadWorkerList = threadWorkerList;
			if(isPendingTasksLeft == null) throw new ArgumentException("isPendingTasksLeft");
			if (busyWorkers == null) throw new ArgumentException("busyWorkers");
			if (workerProvider == null) throw new ArgumentException("workerProvider");
			_isPendingTasksLeft = isPendingTasksLeft;
			_workerProvider = workerProvider;
			_busyWorkers = busyWorkers;
		}

		public void CallWorkers()
		{
			//dont use task completed callbacks due to assumption that threadpool performs many short tasks 
			while (true)
			{
				if (!_isPendingTasksLeft.Invoke())
				{
					if (_closing)
					{
						WaitAllWorkersStopped();
						_stopWaitHandler.Set();
						break;

					}
					WaitHandler.WaitOne();
				}

				ThreadWorker worker = _workerProvider.Invoke();
				if (worker != null)
					//resume worker 
					worker.Continue();
			}
		}

		public void WaitAllWorkersStopped()
		{
			ThreadWorker[] activeWorkers = _busyWorkers.Invoke();
			if (activeWorkers == null || activeWorkers.Length == 0) return;

			foreach(ThreadWorker worker in activeWorkers)
				worker.Stop();
		}
	}
}
