using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Game.Core.Tests
{
    [TestFixture]
    public class LcgTests
    {
        [Test]
        public void LCGTest()
        {
            var lcg = new LinearCongruentGenerator(17, 4294967296, 1103515245, 12345);
            var results = new[] {0, 24107, 16552, 12125, 9427, 13152, 21440, 3383, 6873, 16117};

            foreach(var result in results)
                Assert.That(lcg.NextInt(), Is.EqualTo(result));
        }
    }
}
