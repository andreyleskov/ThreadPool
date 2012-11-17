﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolExample
{
	public enum Priority
	{
		Low,
		Normal,
		High
	}


	//* Тип Priority — это перечисление из трёх приоритетов: HIGH, NORMAL, LOW. 
	//При этом во время выбора следующего задания из очереди действуют такие правила:
	//на три задачи с приоритетом HIGH выполняется одна задача с приоритетом NORMAL,
	//задачи с приоритетом LOW не выполняются, пока в очереди есть хоть одна задача с другим приоритетом.

}