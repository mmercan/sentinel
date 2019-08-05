
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class GuardShould
    {
        [Fact]
        public void ArgumentNotNullThrows()
        {

            object o = null;
            Assert.Throws<ArgumentNullException>(() => { Guard.ArgumentNotNull("null argumemnt", o); });

        }


        [Fact]
        public void ArgumentNotNullOrEmptyThrows()
        {

            string o = null;
            Assert.Throws<ArgumentNullException>(() => { Guard.ArgumentNotNullOrEmpty("null argumemnt", o); });

            string o1 = "";
            Assert.Throws<ArgumentException>(() => { Guard.ArgumentNotNullOrEmpty("empty argumemnt", o1); });

        }

        [Fact]
        public void ArgumentNotNullOrEmptyListThrows()
        {
            IReadOnlyCollection<string> itemsempty = new List<string> { };
            Assert.Throws<ArgumentException>(() => { Guard.ArgumentNotNullOrEmpty<string>("empty", itemsempty); });

            IReadOnlyCollection<string> itemsnull = null;
            Assert.Throws<ArgumentNullException>(() => { Guard.ArgumentNotNullOrEmpty<string>("null", itemsnull); });
        }
    }
}