using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace ThreadPool
{
    [DebuggerDisplay("Count={Count}")]
    public class ConcurrentPriorityQueue<T>:PriorityQueue<T>, IProducerConsumerCollection<KeyValuePair<Priority,T>> where T:class 
    {
        private readonly object _syncLock = new object();

		public KeyValuePair<Priority, T>[] ToArray()
		{
			var array = new KeyValuePair<Priority, T>[Count];
			CopyTo(array,0);
			return array;
		}


		public bool TryAdd(KeyValuePair<Priority, T> item)
		{
			lock (_syncLock)
			{
				Enqueue(item.Key, item.Value);
				return true;
			}
		}

		public bool TryTake(out KeyValuePair<Priority, T> item)
		{
			lock (_syncLock)
			{
				return (item = Dequeue()).Value != null;
			}
		}

		
	}
}