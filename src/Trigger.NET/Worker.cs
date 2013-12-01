using System;
using System.Threading;

namespace Trigger.NET
{
    internal class Worker
    {
        private readonly Type jobType;
        private readonly JobSetup setup;
        private readonly Func<IContainer> containerFactory;
        private readonly Thread thread;
        private readonly ILogger logger;

        public Worker(Type jobType, JobSetup setup, Func<IContainer> containerFactory, ILoggerFactory loggerFactory)
        {
            this.jobType = jobType;
            this.setup = setup;
            this.containerFactory = containerFactory;
            this.thread = new Thread(Run)
            {
                IsBackground = true,
                Name = "Worker: " + jobType.FullName
            };

            this.logger = loggerFactory.GetLogger(jobType, setup.JobId);
        }

        public void Start()
        {
            this.thread.Start();

            this.logger.Log(LogSeverity.Debug, "{0} - started", this.thread.Name);
        }

        public void Stop()
        {
            this.thread.Abort();

            this.logger.Log(LogSeverity.Debug, "{0} - stopped", this.thread.Name);
        }

        private void Run()
        {
            try
            {
                foreach (var wait in this.setup.WaitSource.GetWaits())
                {
                    wait.Wait();

                    RunJob();
                }
            }
            catch (ThreadInterruptedException)
            {
                // NOP!
            }
        }

        private void RunJob()
        {
            //TODO: More uniform log messages

            try
            {
                using (var container = this.containerFactory())
                {
                    logger.Log(LogSeverity.Debug, "Instantiating job: {0}", jobType.FullName);

                    var jobInstance = (IJob)container.Resolve(jobType);

                    try
                    {
                        logger.Log(LogSeverity.Debug, "Executing job ({0})...", jobType.FullName);

                        jobInstance.Execute();

                        logger.Log(LogSeverity.Debug, "Executing job ({0})...Done!", jobType.FullName);
                    }
                    catch (ThreadInterruptedException)
                    {
                        logger.Log(LogSeverity.Info, "Job ({0}) execution interrupted by user code!", jobType.FullName);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        logger.Log(LogSeverity.Error, "Unhandled exception during job ({0}) execution!", jobType.FullName);
                        logger.Log(LogSeverity.Error, ex);
                    }
                    finally
                    {
                        var disposable = jobInstance as IDisposable;
                        if (disposable != null)
                        {
                            logger.Log(LogSeverity.Debug, "Disposing job ({0})...", jobType.FullName);

                            disposable.Dispose();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Log(LogSeverity.Critical, ex);
            }
        }
    }
}