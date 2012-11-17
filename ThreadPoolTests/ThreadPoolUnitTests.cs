using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPoolExample;
using ThreadPool = ThreadPoolExample.ThreadPool;
using Task = ThreadPoolExample.Task;

namespace ThreadPoolTests
{
	[TestClass]
	public class ThreadPoolUnitTests
	{
		[TestMethod]
		public void TasksConsistencyTest()
		{
			int completedTasksCount = 0;
			var pool = new ThreadPool(2);
			var random = new Random();
			int taskCount = random.Next(10);

			for (int i = 0; i < taskCount;i++)
			{
				var task = new Task(() =>
					                    {
											Thread.Sleep(random.Next(100));
						                    completedTasksCount++;
					                    });
				pool.Execute(task, (Priority) random.Next(0, 2));
			}

			pool.Stop();

			Assert.AreEqual(taskCount,completedTasksCount);
		}
	}
}
