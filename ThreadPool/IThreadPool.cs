namespace ThreadPool
{
	interface IThreadPool
	{
		bool HasPendingTask { get; }
		ThreadWorker[] GetBusyWorkers();
		ThreadWorker GetFreeWorker();
	}
}
