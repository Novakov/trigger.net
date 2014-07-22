using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trigger.NET.Tests
{
    using System.Collections;
    using Helpers;
    using NativeWrappers;
    using NUnit.Framework;

    [TestFixture(TypeArgs = new[] { typeof(Win32Native) }, Category = "platform/win")]    
    [Category("platforms")]
    [Category("longrunning")]
    [TestFixture]
    public class NativeSupportTest<TNative>
        where TNative : INative, new()
    {
        private INative native;

        [TestFixtureSetUp]
        public void Before()
        {
            var categories = (ArrayList)TestContext.CurrentContext.Test.Properties["_CATEGORIES"];
            var platforms = categories.OfType<string>()
                .Where(x => x.StartsWith("platform/"))                
                .Select(x => x.Substring("platform/".Length)).ToArray();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && !platforms.Contains("win"))
            {
                Assert.Ignore("Fixture not supported on this platform");
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix && !platforms.Contains("linux"))
            {
                Assert.Ignore("Fixture not supported on this platform");
            }
        }

        [SetUp]
        public void SetUp()
        {
            this.native = new TNative();
        }

        [Test]
        public void AbsoluteTimerShouldExpireOnProperTime()
        {
            // arrange
            var expireAt = DateTimeOffset.Now.AddSeconds(5);

            // act
            var waitHandle = this.native.AbsoluteTimer(expireAt);

            // assert
            waitHandle.WaitOne();
            Assert.That(DateTimeOffset.Now - expireAt, Is.InRange(TimeSpan.FromSeconds(-0.1), TimeSpan.FromSeconds(0.1)));
        }

        [Test]
        public void AbsoluteTimeShouldExpireImmediatelyWhenExpriationTimeIsInPast()
        {
            // arrange
            var expireAt = DateTimeOffset.Now.AddYears(-1);

            // act
            var waitHandle = this.native.AbsoluteTimer(expireAt);

            // assert
            Assert.That(waitHandle.WaitOne(0), Is.True);
        }
    }
}
