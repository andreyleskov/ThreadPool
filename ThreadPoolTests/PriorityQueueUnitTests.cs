using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPoolExample;

namespace ThreadPoolTests
{
	[TestClass]
	public class PriorityQueueUnitTests
	{

		private void MonoTasksTest(Priority priority,int num)
		{
			var queue = new PriorityQueue<object>();
			for(int i=0;i<num;i++)
				queue.Enqueue(priority,i);

			for (int i = 0; i < num; i++)
				Assert.AreEqual(i, queue.Dequeue());
		}

		Random _random = new Random();

		[TestMethod]
		public void MonoPriorityTasksTest()
		{
			foreach(Priority priority in Enum.GetValues(typeof(Priority)))
				MonoTasksTest(priority,_random.Next(10,20));
		}

		[TestMethod]
		public void IncrementPrioritySimpleSequenceTest()
		{
			var queue = new PriorityQueue<object>();
			
			queue.Enqueue(Priority.Low, 1);
			queue.Enqueue(Priority.Low, 2);
			queue.Enqueue(Priority.Normal, 3);
			queue.Enqueue(Priority.Normal, 4);
			queue.Enqueue(Priority.High, 5);
			queue.Enqueue(Priority.High, 6);
			queue.Enqueue(Priority.High, 7);

			Assert.AreEqual(5,queue.Dequeue());
			Assert.AreEqual(6,queue.Dequeue());
			Assert.AreEqual(7,queue.Dequeue());
			Assert.AreEqual(3,queue.Dequeue());
			Assert.AreEqual(4,queue.Dequeue());
			Assert.AreEqual(1,queue.Dequeue());
			Assert.AreEqual(2,queue.Dequeue());
		}

		[TestMethod]
		public void DecrementPrioritySimpleSequenceTest()
		{
			var queue = new PriorityQueue<object>();

			queue.Enqueue(Priority.High, 1);
			queue.Enqueue(Priority.High, 2);
			queue.Enqueue(Priority.High, 3);
			queue.Enqueue(Priority.Normal, 4);
			queue.Enqueue(Priority.Normal, 5);
			queue.Enqueue(Priority.Low, 6);
			queue.Enqueue(Priority.Low, 7);

			Assert.AreEqual(1,queue.Dequeue());
			Assert.AreEqual(2,queue.Dequeue());
			Assert.AreEqual(3,queue.Dequeue());
			Assert.AreEqual(4,queue.Dequeue());
			Assert.AreEqual(5,queue.Dequeue());
			Assert.AreEqual(6,queue.Dequeue());
			Assert.AreEqual(7,queue.Dequeue());
		}
	}
}
