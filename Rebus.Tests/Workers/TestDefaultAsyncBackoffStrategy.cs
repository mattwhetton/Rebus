﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebus.Tests.Contracts;
using Rebus.Workers.ThreadPoolBased;

namespace Rebus.Tests.Workers
{
    [TestFixture]
    public class TestDefaultAsyncBackoffStrategy : FixtureBase
    {
        [Test]
        public async Task BacksOffAsItShould()
        {
            var backoffStrategy = new DefaultAsyncBackoffStrategy(new[]
            {
                TimeSpan.FromMilliseconds(100), 
                TimeSpan.FromMilliseconds(500), 
                TimeSpan.FromMilliseconds(1000), 
            });

            Printt("Starting");

            var stopwatch = Stopwatch.StartNew();
            var previousElapsed = TimeSpan.Zero;

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                await backoffStrategy.Wait();

                var waitTime = stopwatch.Elapsed - previousElapsed;
                Printt($"Waited {waitTime}");
                previousElapsed = stopwatch.Elapsed;
            }

            Printt("Done :)");
        }
    }
}