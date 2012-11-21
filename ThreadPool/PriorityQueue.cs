using System.Collections.Generic;

namespace ThreadPool
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
					LinkedListNode<KeyValuePair<Priority, T>> newLowNode = _queue.AddLast(newTask);

					if (newLowNode.Previous == null || newLowNode.Previous.Value.Key != Priority.Low)
						_firstLowTask = newLowNode;

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
						if (_firstLowTask == null) _queue.AddLast(newTask);
						else _queue.AddBefore(_firstLowTask, newTask);
					}
					else
					{
						if (_highTaskBlockLength ++ == NormalToHighRatio)
						{
							//block ends								  
							_highTaskBlockLength = 1;
							LinkedListNode<KeyValuePair<Priority,T>> newNode =
								      _queue.AddAfter(_firstUnbalancedNormalTask,newTask);

							if (newNode.Next != null && newNode.Next.Value.Key == Priority.Normal)
								_firstUnbalancedNormalTask = newNode.Next;
							else _firstUnbalancedNormalTask = null;
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

		public bool TryDequeue(out KeyValuePair<Priority,T> item)
		{
			item = new KeyValuePair<Priority, T>(Priority.Low,null);
			LinkedListNode<KeyValuePair<Priority, T>> firstNode = _queue.First;
			if (firstNode != null)
			{
				_queue.RemoveFirst();
				item = firstNode.Value;
				return true;
			}
			return false;
		}
	}
}
