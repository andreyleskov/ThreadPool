using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadPool
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

		private readonly List<ThreadWorker<Priority, Task>> _threadWorkerList = new List<ThreadWorker<Priority, Task>>();
		private int _threadCounter;
		private readonly BlockingCollection<KeyValuePair<Priority, Task>> _queue 
			= new BlockingCollection<KeyValuePair<Priority, Task>>(new ConcurrentPriorityQueue<Task>());

		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.

		public ThreadPool(int maxThreadNum)
		{
			_maxThreadNum = maxThreadNum;
		}

		private ThreadWorker<Priority, Task> InitNewWorkingThread()
		{
			var worker = new ThreadWorker<Priority,Task>(_queue);
			var thread = new Thread(worker.Run) {Name = "Pool thread #" + ++_threadCounter, IsBackground = true};
			thread.Start();
			return worker;
		}

		private volatile bool _canCreateNewThreads = true;
	    private readonly object _locker = new object(); 

		public bool Execute(Task task, Priority priority)
		{
			lock(_locker)
			{
				if (_isRunning)
				{
					_queue.TryAdd(new KeyValuePair<Priority, Task>(priority, task));

					if ((_queue.Count > 1 || _queue.Count ==0 ) && _canCreateNewThreads)
					{
							if (_canCreateNewThreads)
							{
								if (_threadWorkerList.All(w => w.IsBusy) || !_threadWorkerList.Any())
									_threadWorkerList.Add(InitNewWorkingThread());

								_canCreateNewThreads = _threadWorkerList.Count < _maxThreadNum;
							}
					}

				}

				return _isRunning;
			}
		}

		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			lock(_locker)
			{
				_isRunning = false;
				_queue.CompleteAdding();
				foreach (ThreadWorker<Priority, Task> worker in _threadWorkerList)
					if(worker.Thread!=null)
				         worker.Thread.Join();
			}
		}
	}
}

