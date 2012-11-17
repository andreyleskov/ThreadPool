using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		private int _threadNum;
		private volatile bool _isRunning = true;
	    private readonly Dictionary<Priority, Queue<Task>> _pendingTasks= new Dictionary<Priority, Queue<Task>>();
	    private const int HightToNormalPriorityBound = 3;
	    private int _priorityTasksRemains = HightToNormalPriorityBound;

		private Task GetNextTask()
		{
			lock(_pendingTasks)
			//{
			//	if (_priorityTasksRemains <= 0 && _pendingTasks[Priority.High].Count > 0)
			//	{
			//		if (_pendingTasks[Priority.Normal].Any()) _priorityTasksRemains--;

			//		return _pendingTasks[Priority.High].Dequeue();
			//	}

				if (_pendingTasks[Priority.High].Count > 0)
					return _pendingTasks[Priority.High].Dequeue();

				if (_pendingTasks[Priority.Normal].Count > 0)
					return _pendingTasks[Priority.Normal].Dequeue();

				return _pendingTasks[Priority.Low].Count > 0 ? 
						  _pendingTasks[Priority.Low].Dequeue() : null;

			   
		
		}
		 
		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
				   
		public ThreadPool(int threadNum)
		{
			_threadNum = threadNum;
			foreach (Priority priority in Enum.GetValues(typeof(Priority)))
				_pendingTasks[priority] = new Queue<Task>();
		}

		private void Run()
		{
			Task task = GetNextTask();
			task.Execute();
		}

		public bool Execute(Task task,Priority priority)
		{
			if(_isRunning)
			{
			   _pendingTasks[priority].Enqueue(task);
				Run();
			}
			return _isRunning;
		}

		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			_isRunning = false;
		}
}
}
