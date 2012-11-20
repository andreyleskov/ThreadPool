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
	public class ThreadPool
	{
		private readonly int _maxThreadNum;
		private volatile bool _isRunning = true;

		private readonly List<ThreadWorker> _threadWorkerList = new List<ThreadWorker>();
		private int _threadCounter;
		public TaskQueue Queue {get;private set;}

		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.

		public ThreadPool(int maxThreadNum)
		{
			_maxThreadNum = maxThreadNum;
		}

		private ThreadWorker InitNewWorkingThread()
		{
			var worker = new ThreadWorker(Queue);
			var thread = new Thread(worker.Run) {Name = "Pool thread #" + ++_threadCounter, IsBackground = true};
			thread.Start();
			return worker;
		}

		private volatile bool _canCreateNewThreads = true;
	    private readonly object _locker = new object(); 

		public bool Execute(Task task, Priority priority)
		{
			if (_isRunning)
			{
			//	Queue.Enqueue(task, priority);

				if (_canCreateNewThreads)
				{
					lock (_locker)
					{
						if (_canCreateNewThreads)
						{
							if (_threadWorkerList.All(w => w.IsBusy) || !_threadWorkerList.Any())
								_threadWorkerList.Add(InitNewWorkingThread());

							_canCreateNewThreads = _threadWorkerList.Count < _maxThreadNum;
						}
					}
				}

			}

			return _isRunning;
		}

		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			_isRunning = false;
		//	Queue.Complete();
		}
	}
}

