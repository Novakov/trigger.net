using System;
using System.Threading;

namespace Trigger.NET
{
    internal class Worker
    {
        private readonly Type jobType;
        private readonly IWaitSource waitSource;
        private readonly Thread thread;

        public Worker(Type jobType, IWaitSource waitSource)
        {
            this.jobType = jobType;
            this.waitSource = waitSource;
            this.thread = new Thread(Run)
            {
                IsBackground = true,
                Name = "Worker: " + jobType.FullName
            };
        }

        public void Start()
        {  
            this.thread.Start();
        }

        public void Stop()
        {
            this.thread.Abort();
        }

        private void Run()
        {
            try
            {
                foreach (var wait in this.waitSource.GetWaits())
                {
                    wait.Wait();

                    RunJob();
                }
            }
            catch (ThreadInterruptedException)
            {
            }
        }

        private void RunJob()
        {
            //TODO: use per-run scope to create 
            //TODO: use some kind of factory to create job types
            var jobInstance = (IJob)Activator.CreateInstance(jobType);

            try
            {
                jobInstance.Execute();
            }
            catch (Exception)
            {
                //TODO: error dispatching                
            }
            finally
            {
                var disposable = jobInstance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}