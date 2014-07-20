namespace Trigger.NET.Tests
{
    using System;
    using System.Threading;
    using FluentAPI;
    using Helpers;
    using Moq;
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
    }
}
