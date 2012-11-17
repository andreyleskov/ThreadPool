using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadPoolExample
{
//* До вызова метода stop() задачи ставятся в очередь на выполнение и
//метод boolean execute(Task task, Priority priority) сразу же возвращает true,
//не дожидаясь завершения выполнения задачи;
//а после вызова stop() новые задачи не добавляются в очередь на выполнение, 
//и метод boolean execute(Task task, Priority priority) сразу же возвращает false.	
//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
public class ThreadPool
{
		private int _threadNum;
		private volatile bool _isStoped;
		private Task GetTask()
		{
			return null;
		}
		 
		//* В конструктор этого класса должно передаваться количество потоков, которые будут выполнять задачи.
				   
		public ThreadPool(int threadNum)
		{
			_threadNum = threadNum;
		}
		public bool Execute(Task task,Priority priority)
		{
			return true;
		}
		//* Метод stop() ожидает завершения всех текущих задач (не очищая очередь).
		public void Stop()
		{
			_isStoped = true;
		}
}
}
