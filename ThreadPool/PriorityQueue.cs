using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadPoolExample;

namespace ThreadPoolExample
{
	public class PriorityQueue<T> where T:class
	{
		private readonly LinkedList<KeyValuePair<Priority, T>> _queue = new LinkedList<KeyValuePair<Priority, T>>();
		private LinkedListNode<KeyValuePair<Priority, T>> _firstLowTask;
		private LinkedListNode<KeyValuePair<Priority, T>> _firstUnbalancedNormalTask;
		private const int NormalToHighRatio = 3;
		private int _highTaskBlockLength;

		public void Enqueue(Priority priority, T task)
		{
			var newTask = new KeyValuePair<Priority, T>(priority, task); 
			switch (priority)
			{
				//Low tasks always placed to end
				case Priority.Low:

					if (_queue.Last == null  || (_queue.Last != null && _queue.Last.Value.Key != Priority.Low))
						_firstLowTask =  _queue.AddLast(newTask);
					else
						_queue.AddLast(newTask);

					break;
				//Normal tasks placed before low ones
				case Priority.Normal:

					if (_firstLowTask != null)
					{
						var newNode = _queue.AddBefore(_firstLowTask, newTask);
						_firstUnbalancedNormalTask = _firstUnbalancedNormalTask ?? newNode;
					}
					else
						_firstUnbalancedNormalTask = _queue.AddLast(newTask);

					break;

				//Hight tasks placed before firstUnbalancedNormal, but by blocks with length no more then NormalToHighRatio
				//If block exceeds its max length, task placed after first unbalanced normal task and block length become 1
				case Priority.High:

					if (_firstUnbalancedNormalTask == null)
					{
						var newNode = _firstLowTask == null ? _queue.AddLast(newTask) : _queue.AddBefore(_firstLowTask, newTask);
					}
					else
					{
						if (_highTaskBlockLength++ == NormalToHighRatio)
						{
							//block ends
							_highTaskBlockLength = 0;
							_queue.AddAfter(_firstUnbalancedNormalTask,newTask);

							_firstUnbalancedNormalTask = _firstUnbalancedNormalTask.Next != null
									                            ? _firstUnbalancedNormalTask.Next.Next
									                            : null;
						}
						else
						{
							//block continues
							_queue.AddBefore(_firstUnbalancedNormalTask,newTask);
						}
					}
					break;
			}
		}

		public T Dequeue()
		{
			var firstNode = _queue.First;
			_queue.RemoveFirst();
			return firstNode.Value.Value;
		}
	}
}
