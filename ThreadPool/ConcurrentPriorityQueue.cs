//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: ConcurrentPriorityQueue.cs
//
//--------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using ThreadPool;

namespace ThreadPoolExample
{
    /// <summary>Provides a thread-safe priority queue data structure.</summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class ConcurrentPriorityQueue<T> : IProducerConsumerCollection<KeyValuePair<Priority,T>> where T:class 
    {
        private readonly object _syncLock = new object();
        private readonly PriorityQueue<T> _queue = new PriorityQueue<T>();


		#region IProducerConsumerCollection<KeyValuePair<Priority,T>> Members

		public void CopyTo(KeyValuePair<Priority, T>[] array, int index)
		{
			throw new NotImplementedException();
		}

		public KeyValuePair<Priority, T>[] ToArray()
		{
			throw new NotImplementedException();
		}

		public bool TryAdd(KeyValuePair<Priority, T> item)
		{
			throw new NotImplementedException();
		}

		public bool TryTake(out KeyValuePair<Priority, T> item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<KeyValuePair<Priority,T>> Members

		public IEnumerator<KeyValuePair<Priority, T>> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
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