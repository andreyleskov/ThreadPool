using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadPoolExample;


namespace ThreadPool
{
	//will block execution on Take()
	public class TaskQueue
	{

		//foreach (Priority priority in Enum.GetValues(typeof(Priority)))
		//_pendingTasks[priority] = new ConcurrentQueue<Task>();

		//private readonly ConcurrentDictionary<Priority, ConcurrentQueue<Task>> _pendingTasks = new ConcurrentDictionary<Priority, ConcurrentQueue<Task>>();
		//private const int HightToNormalPriorityBound = 3;
		//private int _priorityTasksRemains = HightToNormalPriorityBound;
		//Task result=null;

		//		if (_priorityTasksRemains <= 0 && !_pendingTasks[Priority.High].IsEmpty)
		//		{
		//			if (_pendingTasks[Priority.Normal].IsEmpty) Interlocked.Decrement(ref _priorityTasksRemains);

		//			_pendingTasks[Priority.High].TryDequeue(out result);
		//			return result;
		//		}

		//		if (!_pendingTasks[Priority.Normal].IsEmpty)
		//		{
		//			_pendingTasks[Priority.Normal].TryDequeue(out result);
		//			return result;
		//		}

		//		if (!_pendingTasks[Priority.Low].IsEmpty)
		//			_pendingTasks[Priority.Low].TryDequeue(out result);

		//		return result;
		public void Complete()
		{
		}

		public IEnumerable<Task> GetTasksEnumerable()
		{
			return null;
		}
		public bool IsEmpty { get; private set; }
	
		public bool TryAdd(Task item,Priority priority)
		{
			throw new NotImplementedException();
		}

		public bool Take(out Task item)
		{
			throw new NotImplementedException();
		}

	
	}
}
