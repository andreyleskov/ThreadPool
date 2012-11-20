﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPoolExample;

namespace ThreadPoolTests
{
	[TestClass]
	public class PriorityQueueUnitTests
	{
		[TestMethod]
		public void MonoLowPriorityTasksTest()
		{
			var queue = new PriorityQueue<object>();
			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Low, 2);
			queue.Enqueue(Priority.Low, 3);

			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());

		}

		[TestMethod]
		public void MonoHighPriorityTasksTest()
		{
			var queue = new PriorityQueue<object>();
			queue.Enqueue(Priority.High, 1);
			queue.Enqueue(Priority.High, 2);
			queue.Enqueue(Priority.High, 3);

			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());

		}

		[TestMethod]
		public void MonoNormalPriorityTasksTest()
		{
			var queue = new PriorityQueue<object>();
			queue.Enqueue(Priority.Normal, 1);
			queue.Enqueue(Priority.Normal, 2);
			queue.Enqueue(Priority.Normal, 3);

			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());

		}

		[TestMethod]
		public void IncrementPrioritySequenceTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Low, 2);
			queue.Enqueue(Priority.Normal, 3);
			queue.Enqueue(Priority.Normal, 4);
			queue.Enqueue(Priority.High, 5);
			queue.Enqueue(Priority.High, 6);
			queue.Enqueue(Priority.High, 7);

			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(7, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
		}

		[TestMethod]
		public void DecrementPrioritySequenceTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.High, 1);
			queue.Enqueue(Priority.High, 2);
			queue.Enqueue(Priority.High, 3);
			queue.Enqueue(Priority.Normal, 4);
			queue.Enqueue(Priority.Normal, 5);
			queue.Enqueue(Priority.Low, 6);
			queue.Enqueue(Priority.Low, 7);

			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(7, queue.Dequeue());
		}

		[TestMethod]
		public void IncrementPrioritySequenceWithOverflowTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Low, 2);
			queue.Enqueue(Priority.Normal, 3);
			queue.Enqueue(Priority.Normal, 4);
			queue.Enqueue(Priority.High, 5);
			queue.Enqueue(Priority.High, 6);
			queue.Enqueue(Priority.High, 7);
			queue.Enqueue(Priority.High, 8);
			queue.Enqueue(Priority.High, 9);

			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(7, queue.Dequeue());
			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(8, queue.Dequeue());
			Assert.AreEqual(9, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
		}

		[TestMethod]
		public void MixedPrioritySequenceTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.Low,    1);
			queue.Enqueue(Priority.Normal, 2);	
			queue.Enqueue(Priority.High,   3);
			queue.Enqueue(Priority.Low,    4);
			queue.Enqueue(Priority.Normal, 5);
			queue.Enqueue(Priority.High,   6);

			
			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
		}

		[TestMethod]
		public void MixedPrioritySequenceWithOverflowTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Normal, 2);
			queue.Enqueue(Priority.High, 3);
			queue.Enqueue(Priority.Low, 4);
			queue.Enqueue(Priority.High, 5);
			queue.Enqueue(Priority.High, 6);
			queue.Enqueue(Priority.Normal, 7);
			queue.Enqueue(Priority.High, 8);
			queue.Enqueue(Priority.High, 9);
			queue.Enqueue(Priority.High, 10);
			queue.Enqueue(Priority.Low, 12);
			queue.Enqueue(Priority.Normal, 13);
			queue.Enqueue(Priority.Low, 14);
			queue.Enqueue(Priority.Normal, 2);


			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(8, queue.Dequeue());
			Assert.AreEqual(9, queue.Dequeue());
			Assert.AreEqual(10, queue.Dequeue());
			Assert.AreEqual(7, queue.Dequeue());
			Assert.AreEqual(13, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
			Assert.AreEqual(12, queue.Dequeue());
			Assert.AreEqual(14, queue.Dequeue());
		}

		[TestMethod]
		public void MixedPrioritySequenceWithDoubleOverflowTest()
		{
			var queue = new PriorityQueue<object>(); 

			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Normal, 2);
			queue.Enqueue(Priority.High, 3);
			queue.Enqueue(Priority.Low, 4);
			queue.Enqueue(Priority.High, 5);
			queue.Enqueue(Priority.High, 6);
			queue.Enqueue(Priority.Normal, 7);
			queue.Enqueue(Priority.High, 8);
			queue.Enqueue(Priority.High, 9);
			queue.Enqueue(Priority.High, 10);
			queue.Enqueue(Priority.Low, 12);
			queue.Enqueue(Priority.Normal, 13);
			queue.Enqueue(Priority.Low, 14);
			queue.Enqueue(Priority.Normal, 2);


			Assert.AreEqual(3, queue.Dequeue());
			Assert.AreEqual(5, queue.Dequeue());
			Assert.AreEqual(6, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(8, queue.Dequeue());
			Assert.AreEqual(9, queue.Dequeue());
			Assert.AreEqual(10, queue.Dequeue());
			Assert.AreEqual(7, queue.Dequeue());
			Assert.AreEqual(13, queue.Dequeue());
			Assert.AreEqual(2, queue.Dequeue());
			Assert.AreEqual(1, queue.Dequeue());
			Assert.AreEqual(4, queue.Dequeue());
			Assert.AreEqual(12, queue.Dequeue());
			Assert.AreEqual(14, queue.Dequeue());
		}
	}
}
