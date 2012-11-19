using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ThreadPool;

namespace ThreadPoolExample
{
//* До вызова метода stop() задачи ставятся в очередь на выполнение и
//метод boolean execute(Task task, Priority priority) сразу же возвращает true,
//не дожидаясь завершения выполнения задачи;
//а после вызова stop() новые задачи не добавляются в очередь на выполнение, 
//и метод boolean execute(Task task, Priority priority) сразу же возвращает false.	
//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
public class ThreadPool: IThreadPool
{
		private readonly int _maxThreadNum;
		private volatile bool _isRunning = true;
	   
		private readonly Thread _dispatcherThread;
		private readonly List<ThreadWorker> _threadWorkerList = new List<ThreadWorker>();
		private readonly ThreadPoolDispatcher _dispatcher;
		private int _threadCounter;
		private TaskQueue _queue = new TaskQueue();

		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
				   
		public ThreadPool(int maxThreadNum)
		{
			_maxThreadNum = maxThreadNum;
	
			_dispatcher = new ThreadPoolDispatcher(this);
			_dispatcherThread = new Thread(_dispatcher.CallWorkers) {Name ="Dispatcher thread", IsBackground = true};
			_dispatcherThread.Start();
		}


		private ThreadWorker InitNewWorkingThread()
		{
			var worker = new ThreadWorker(_queue);
			var thread = new Thread(worker.Run) {Name = "Pool thread #" + ++_threadCounter, IsBackground = true };
			thread.Start();
			return worker;
		}

		public bool Execute(Task task,Priority priority)
		{
			if(_isRunning)
			{
				_queue.TryAdd(task, priority);
			}
			return _isRunning;
		}

		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			_isRunning = false;
			ThreadWorker[] activeWorkers = GetBusyWorkers();
			_queue.Complete();
			if (activeWorkers == null || activeWorkers.Length == 0) return;

			foreach (ThreadWorker worker in activeWorkers)
				worker.Stop();
		}

		#region IThreadPool Members

		public ThreadWorker[] GetBusyWorkers()
		{
			return _threadWorkerList.Where(w => w.IsBusy).ToArray();
		}

		public ThreadWorker GetFreeWorker()
		{
			ThreadWorker worker = _threadWorkerList.FirstOrDefault(w => !w.IsBusy);
			if (worker == null && _threadWorkerList.Count < _maxThreadNum)
			{
				worker = InitNewWorkingThread();
				_threadWorkerList.Add(worker);
			}

			return worker;
		}

		#endregion
}
}
