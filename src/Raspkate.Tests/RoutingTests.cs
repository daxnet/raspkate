using NUnit.Framework;
using Raspkate.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Tests
{
    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void ParseStaticRouteTest()
        {
            var route = RouteParser.Parse("api/customers/all");
            Assert.AreEqual(3, route.Count);
            Assert.IsInstanceOf<LiteralRouteItem>(route[0]);
            Assert.IsInstanceOf<LiteralRouteItem>(route[1]);
            Assert.IsInstanceOf<LiteralRouteItem>(route[2]);
        }

        [Test]
        public void ParseParameterRouteTest()
        {
            var route = RouteParser.Parse("api/customers/{id}");
            Assert.AreEqual(3, route.Count);
            Assert.IsInstanceOf<LiteralRouteItem>(route[0]);
            Assert.IsInstanceOf<LiteralRouteItem>(route[1]);
            Assert.IsInstanceOf<ParameterRouteItem>(route[2]);
        }

        [Test]
        public void ParseParameterRouteItemAtMiddlePositionTest()
        {
            var route = RouteParser.Parse("api/customers/{id}/orders");
            Assert.AreEqual(4, route.Count);
            Assert.IsInstanceOf<LiteralRouteItem>(route[0]);
            Assert.IsInstanceOf<LiteralRouteItem>(route[1]);
            Assert.IsInstanceOf<ParameterRouteItem>(route[2]);
            Assert.IsInstanceOf<LiteralRouteItem>(route[3]);
        }
    }
}
