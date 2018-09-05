using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.IO;
using Xunit.Sdk;
using Microsoft.Extensions.Logging;

namespace Mercan.Common.Tests.LinqExtensions
{


    public class MakeConsoleWork : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly TextWriter _originalOut;
        private readonly TextWriter _textWriter;

        public MakeConsoleWork(ITestOutputHelper output)
        {
            _output = output;
            _originalOut = Console.Out;
            _textWriter = new StringWriter();
            Console.SetOut(_textWriter);
        }

        public void Dispose()
        {
            _output.WriteLine(_textWriter.ToString());
            Console.SetOut(_originalOut);
        }
    }

    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
            => new XunitLogger(_testOutputHelper, categoryName);

        public void Dispose()
        { }
    }

    public class XunitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;

        public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();
            public void Dispose()
            { }
        }
    }

    public class TestDistinctBy : MakeConsoleWork
    {
        private readonly ITestOutputHelper output;
        private ILogger<TestDistinctBy> logger;

        public TestDistinctBy(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            this.output = testOutputHelper;
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            this.logger = loggerFactory.CreateLogger<TestDistinctBy>();
        }

        [Fact]
        public void TestDistinctBy_init()
        {
            output.WriteLine("Blahhh");
            
            logger.LogCritical("Foo bar baz");
            Console.WriteLine("Balhhhhh");

            var list = new List<testlist>
            {
                new testlist{ Id = 1, Property = "value1" },
                new testlist{ Id = 2, Property = "value2" },
                new testlist{ Id = 3, Property = "value1" }
            };
            
            var newlist = list.DistinctBy(p=>p.Property);
            Assert.NotEmpty(newlist);
            


        }
    }

    public class testlist
    {
        public int Id { get; set; }
        public string Property { get; set; }
    }
}

