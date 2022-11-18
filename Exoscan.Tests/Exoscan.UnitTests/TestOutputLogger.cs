﻿using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Exoscan.UnitTests
{

    public sealed class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper output;

        public TestOutputLogger(ITestOutputHelper output)
        {
            this.output = output;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            output.WriteLine($"[ {logLevel} ] {formatter(state, exception)}");
        }
    }
}
