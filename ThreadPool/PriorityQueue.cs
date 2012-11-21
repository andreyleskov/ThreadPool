using System.Collections.Generic;

namespace ThreadPool
{
	public class PriorityQueue<T>:LinkedList<KeyValuePair<Priority, T>> where T:class
	{
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
					LinkedListNode<KeyValuePair<Priority, T>> newLowNode = this.AddLast(newTask);

					if (newLowNode.Previous == null || newLowNode.Previous.Value.Key != Priority.Low)
						_firstLowTask = newLowNode;

					break;
				//Normal tasks placed before low ones
				case Priority.Normal:

					if (_firstLowTask != null)
					{
						var newNode = this.AddBefore(_firstLowTask, newTask);
						_firstUnbalancedNormalTask = _firstUnbalancedNormalTask ?? newNode;
					}
					else
						_firstUnbalancedNormalTask = this.AddLast(newTask);

					break;

				//Hight tasks placed before firstUnbalancedNormal, but by blocks with length no more then NormalToHighRatio
				//If block exceeds its max length, task placed after first unbalanced normal task and block length become 1
				case Priority.High:

					if (_firstUnbalancedNormalTask == null)
					{
						if (_firstLowTask == null) this.AddLast(newTask);
						else this.AddBefore(_firstLowTask, newTask);
					}
					else
					{
						if (_highTaskBlockLength ++ == NormalToHighRatio)
						{
							//block ends								  
							_highTaskBlockLength = 1;
							LinkedListNode<KeyValuePair<Priority,T>> newNode =
								      this.AddAfter(_firstUnbalancedNormalTask,newTask);

							if (newNode.Next != null && newNode.Next.Value.Key == Priority.Normal)
								_firstUnbalancedNormalTask = newNode.Next;
							else _firstUnbalancedNormalTask = null;
						}
						else
						{
							//block continues
							this.AddBefore(_firstUnbalancedNormalTask,newTask);
						}
					}
					break;
			}
		}

		public KeyValuePair<Priority,T>  Dequeue()
		{
			LinkedListNode<KeyValuePair<Priority, T>> firstNode = this.First;
			if (firstNode != null)
			{
				this.RemoveFirst();
				return firstNode.Value;
			}

			return new KeyValuePair<Priority, T>(Priority.Low, null);
		}
	}
}
