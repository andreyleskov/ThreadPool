using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPoolExample;

namespace ThreadPoolTests
{
	[TestClass]
	public class ThreadPoolUnitTests
	{
		
		public void TasksConsistencyTest(int threadCount,int taskCount,Func<Action> taskPayloadProvider)
		{
			var pool = new ThreadPoolExample.ThreadPool(threadCount);
			var random = new Random();
			int completedTasksCount = 0;

			for (int i = 0; i < taskCount;i++)
			{
				pool.Execute(new Task(()=>{
						                      completedTasksCount++;
						                      taskPayloadProvider.Invoke().Invoke();
					                      }),
						     (Priority) random.Next(0, 2));
			}

			pool.Stop();
		
			Assert.AreEqual(taskCount,completedTasksCount);
		}
		private Random _random =new Random();


		private long Measure(Action act)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			act.Invoke();
			stopWatch.Stop();
			return stopWatch.ElapsedMilliseconds;
		}

		[TestMethod]
		public void TestManyShortTasks()
		{
			TasksConsistencyTest(20,
								 _random.Next(1000,5000),
								 ()=> 
									 ()=>Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(0,10)))
								);
		}

		[TestMethod]
		public void TestFewLongTasks()
		{
			TasksConsistencyTest(3,
								 _random.Next(10, 20),
								 () => 
									  () =>Thread.Sleep(TimeSpan.FromMilliseconds(_random.Next(300, 1000)))
								);
		}

		[TestMethod]
		public void TasksPriorityTest()
		{
			var pool = new ThreadPoolExample.ThreadPool(2);
			var random = new Random();
			var priorityChecker = new PrioritySequenceChecker();

			for (int i = 0; i < 100; i++)
			{
				var priority = (Priority)random.Next(0, 2);
				priorityChecker.BuildSequence(priority);
				
				var task = new Task(() =>
					                    {
											Thread.Sleep(random.Next(10,20));
						                    Assert.AreEqual(true, priorityChecker.CheckSequence(priority));
					                    });

				pool.Execute(task, priority);
			}

			pool.Stop();
		}

		class PrioritySequenceChecker
		{
			private readonly Dictionary<Priority,int> _tasksCount = new Dictionary<Priority, int>();
			private const int MaxHighTasks = 3;
			private int _priorityTasksCompleted = MaxHighTasks;

			public PrioritySequenceChecker()
			{
				foreach(Priority priority in Enum.GetValues(typeof(Priority)))
					_tasksCount[priority] = 0;
			}		

			public void BuildSequence(Priority priority)
			{
				_tasksCount[priority]++;
			}

			public bool CheckSequence(Priority priority)
			{
				_tasksCount[priority]--;

				if(priority == Priority.High && _tasksCount[Priority.Normal] > 0)
					_priorityTasksCompleted --;

				if (priority == Priority.Normal && _priorityTasksCompleted == 0)
					_priorityTasksCompleted = MaxHighTasks;

				//fail if cheking sequence is different from building (only in terms of element count of one priority)
			    if(_tasksCount[priority] < 0 ) return false;

				//fail if perfoming low task having normal or hight pending
				if(priority == Priority.Low && 
					 (_tasksCount[Priority.High] > 0 || _tasksCount[Priority.Normal] > 0))
					return false;

				//fail if perfoming normal task before perfoming MaxHigtTask high Tasks
				if (priority == Priority.Normal && _tasksCount[Priority.High] > 0 && _priorityTasksCompleted > 0)
					return false;
		   
				//fail if perfoming too much hight tasks
				if (_priorityTasksCompleted < 0) return false;

				return true;
			}
		}
	}
}
											