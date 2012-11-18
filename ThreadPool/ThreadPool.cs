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
	    private readonly Dictionary<Priority, Queue<Task>> _pendingTasks= new Dictionary<Priority, Queue<Task>>();
	    private const int HightToNormalPriorityBound = 3;
	    private int _priorityTasksRemains = HightToNormalPriorityBound;
		private readonly Thread _dispatcherThread;
		private readonly List<ThreadWorker> _threadWorkerList = new List<ThreadWorker>();
		private readonly TimeSpan _waitTimeOut = TimeSpan.FromSeconds(1);
		private object _locker = new object();
		private readonly ThreadPoolDispatcher _dispatcher;


		//need to span foreground threads to perfom finally blocks
		private Task GetNextTask()
		{
			lock (_pendingTasks)
			{
				if (_priorityTasksRemains <= 0 && _pendingTasks[Priority.High].Count > 0)
				{
					if (_pendingTasks[Priority.Normal].Any()) _priorityTasksRemains--;

					return _pendingTasks[Priority.High].Dequeue();
				}

				if (_pendingTasks[Priority.Normal].Count > 0)
					return _pendingTasks[Priority.Normal].Dequeue();

				return _pendingTasks[Priority.Low].Count > 0
					       ? _pendingTasks[Priority.Low].Dequeue()
					       : null;
			}

		}
		 
		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
				   
		public ThreadPool(int maxThreadNum)
		{
			_maxThreadNum = maxThreadNum;
			foreach (Priority priority in Enum.GetValues(typeof(Priority)))
				_pendingTasks[priority] = new Queue<Task>();

			_dispatcher = new ThreadPoolDispatcher(IsAnyTasksLeft,GetFreeWorker);
			_dispatcherThread = new Thread(_dispatcher.CallWorkers) {IsBackground = true};
			_dispatcherThread.Start();
		}

		private void TaskDone(){}

		private bool IsAnyTasksLeft()
		{
			return _pendingTasks.Any(pair => pair.Value.Any());
		}

		private ThreadWorker GetFreeWorker()
		{
			ThreadWorker worker = _threadWorkerList.FirstOrDefault(w => !w.IsBusy);
			if (worker == null && _threadWorkerList.Count < _maxThreadNum)
			{
				worker = InitNewWorkingThread();
				_threadWorkerList.Add(worker);
			}

			return worker;
		}

		private ThreadWorker InitNewWorkingThread()
		{
			var worker = new ThreadWorker(GetNextTask, TaskDone);
			var thread = new Thread(worker.Run) { IsBackground = true };
			thread.Start();
			return worker;
		}

		public bool Execute(Task task,Priority priority)
		{
			if(_isRunning)
			{
				_dispatcher.WaitHandler.Set();
				_pendingTasks[priority].Enqueue(task);				
			}
			
			return _isRunning;
		}

		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			_isRunning = false;
			_dispatcherThread.Join(_waitTimeOut);
		}
}
}
