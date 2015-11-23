namespace Trigger.NET.Tests
{
    using System;
    using System.Linq;
    using System.Threading;
    using FluentAPI;
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class CoreTests
    {
        private Scheduler scheduler;

        [SetUp]
        public void SetUp()
        {
            this.scheduler = new Scheduler();
        }

        [Test]
        public void ShouldInvokeJobBasedOnWaitSource()
        {
            // arrange
            var jobExecuted = new ManualResetEvent(false);

            var trigger = new ManualTrigger();

            scheduler.AddJob<SetOnExecute>(cfg => cfg.UseWaitSource(trigger).WithParameter(jobExecuted));

            // act
            trigger.TriggerNow();

            // assert
            Assert.That(jobExecuted.WaitOne(TimeSpan.FromSeconds(0.1)), Is.True, "Job should be executed");
        }

        [Test]
        public void ShouldNotTriggerJobAfterRemovingFromScheduler()
        {
            // arrange
            var jobExecuted = new ManualResetEvent(false);

            var trigger = new ManualTrigger();

            var jobId = scheduler.AddJob<SetOnExecute>(cfg => cfg.UseWaitSource(trigger).WithParameter(jobExecuted));

            // act
            scheduler.RemoveJob(jobId);
            trigger.TriggerNow();

            // assert
            Assert.That(jobExecuted.WaitOne(0), Is.False, "Job should not be executed");
        }

        [Test]
        public void ShouldInterruptJobOnRemoval()
        {
            // arrange
            var mark1 = new ManualResetEvent(false);
            var mark2 = new ManualResetEvent(false);

            var proceed = new ManualResetEvent(false);

            var trigger = new ManualTrigger();

            var jobId = scheduler.AddAction(trigger, ctx =>
            {
                mark1.Set();

                proceed.WaitOne();

                mark2.Set();
            });

            trigger.TriggerNow();
            mark1.WaitOne();

            // act
            scheduler.RemoveJob(jobId);
            proceed.Set();

            // assert
            Assert.That(mark2.WaitOne(TimeSpan.FromSeconds(0.1)), Is.False, "Job should be interrupted");
        }

        [Test]
        public void FailOfJobShouldNotPreventNextExecution()
        {
            // arrange
            var mark1 = new ManualResetEvent(false);
            var trigger = new ManualTrigger();

            scheduler.AddAction(trigger, ctx =>
            {
                mark1.Set();

                throw new Exception("Exception in job");
            });

            trigger.TriggerNow();
            mark1.WaitOne();
            mark1.Reset();

            // act
            trigger.TriggerNow();

            // assert
            Assert.That(mark1.WaitOne(TimeSpan.FromSeconds(0.1)), Is.True, "Job should be executed");
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(20)]
        public void ShouldStopAllJobsOnDispose(int jobs)
        {
            // arrange
            var factory = new Func<ManualTrigger, ManualResetEvent[]>((trig) =>
            {
                var mark1 = new ManualResetEvent(false);
                var mark2 = new ManualResetEvent(false);

                var proceed = new ManualResetEvent(false);
                Action<JobContext> actionJob = ctx =>
                {
                    mark1.Set();

                    proceed.WaitOne();

                    mark2.Set();
                };
                this.scheduler.AddAction(trig, actionJob);
                return new[] { mark1, proceed, mark2 };
            });

            var triggers = (new int[jobs]).Select((j) => new ManualTrigger());
            var actionMarks = triggers.Select((trigger) =>
             {
                 var marks = factory(trigger);
                 trigger.TriggerNow();
                 marks[0].WaitOne();
                 return new[] { marks[1], marks[2] };
             }).ToArray();

            // act
            this.scheduler.Dispose();
            var marksNotSet = actionMarks.Select((events) =>
            {
                events[0].Set();
                return events[1] as WaitHandle;
            }).ToArray();

            // assert
            Assert.That(
                WaitHandle.WaitAll(marksNotSet, TimeSpan.FromSeconds(0.1)),
                Is.False,
                string.Format("{0} Jobs should be interrupted", jobs));
        }

        [Test]
        [ExpectedException("System.ObjectDisposedException")]
        public void ShouldNotAllowNewJobsWhenDisposed()
        {
            // arrange

            // act
            this.scheduler.Dispose();
            this.scheduler.AddJob<SetOnExecute>(cfg => cfg.UseWaitSource(new ManualTrigger()).WithParameter(new ManualResetEvent(false)));
            
            // assert
        }
    }
}
