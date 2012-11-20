using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ThreadPoolExample;


namespace ThreadPool
{
    public struct PriorityTask
    {
	    public Priority Priority;
	    public Task Task;
    }

	//will block execution on Dequeue()
	public class TaskQueue: IProducerConsumerCollection<PriorityTask>
	{
		private const int MaxHighTasksBeforeNormal = 3;
		private int _priorityTasksRemains = MaxHighTasksBeforeNormal;

		private ConcurrentDictionary<Priority, ConcurrentQueue<Task>> _pendingTasks = new ConcurrentDictionary<Priority, ConcurrentQueue<Task>>();  

		public bool TryAdd(PriorityTask item)
		{
			_pendingTasks[item.Priority].Enqueue(item.Task);
			return true;							  
		}

		public bool TryTake(out PriorityTask item)
		{
			lock (_pendingTasks)
			{
				Task task = null;
				Priority priority;

				if (_priorityTasksRemains <= 0 && _pendingTasks[Priority.High].Count > 0)
				{
					if (_pendingTasks[Priority.Normal].Any()) _priorityTasksRemains--;

					priority = Priority.High;
					_pendingTasks[priority].TryDequeue(out task);
				}
				else
				{
					priority = Priority.Normal;
					if (_pendingTasks[priority].Count > 0)
						_pendingTasks[priority].TryDequeue(out task);
					_priorityTasksRemains = MaxHighTasksBeforeNormal;
				}

				if (task == null)
				{
					priority = Priority.Low;
					_pendingTasks[priority].TryDequeue(out task);
				}

				item = new PriorityTask{Priority = priority,Task=task};
				return task != null;
			}
		}

		#region IProducerConsumerCollection<PriorityTask> Members

		public void CopyTo(PriorityTask[] array, int index)
		{
			throw new NotImplementedException();
		}

		public PriorityTask[] ToArray()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<PriorityTask> Members

		public IEnumerator<PriorityTask> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsSynchronized
		{
			get { throw new NotImplementedException(); }
		}

		public object SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}
