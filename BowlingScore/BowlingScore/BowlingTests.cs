using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore
{
    [TestClass]
    public abstract class BowlingTests
    {
        protected IBowlingScorer scorer;

        [TestInitialize]
        public abstract void TestInitialize();

        [TestMethod]
        public void can_score_strike()
        {
            Assert.AreEqual(10, scorer.Score("X"));
        }

        [TestMethod]
        public void can_score_frame_with_gutter()
        {
            Assert.AreEqual(5, scorer.Score("5-"));
        }

        [TestMethod]
        public void can_score_spare_as_a_single_frame()
        {
            Assert.AreEqual(10, scorer.Score("5/"));
        }

        [TestMethod]
        public void can_score_mundane_frame()
        {
            Assert.AreEqual(7, scorer.Score("25"));
        }

        [TestMethod]
        public void can_score_spare_mundane()
        {
            Assert.AreEqual(12 + 7, scorer.Score("5/25"));
        }

        [TestMethod]
        public void can_score_strike_mundane_mundane()
        {
            Assert.AreEqual((10 + 2 + 5) + 7 + 7, scorer.Score("X2525"));
        }

        [TestMethod]
        public void can_score_strike_spare_mundane()
        {
            Assert.AreEqual((10 + (5 + 5)) + (10 + 2) + 7, scorer.Score("X5/25"));
        }

        [TestMethod]
        public void WebSample1()
        {
            Assert.AreEqual(300, scorer.Score("XXXXXXXXXXXX"));
        }

        [TestMethod]
        public void WebSample2()
        {
            Assert.AreEqual(90, scorer.Score("9-9-9-9-9-9-9-9-9-9-"));
        }

        [TestMethod]
        public void WebSample3()
        {
            Assert.AreEqual(150, scorer.Score("5/5/5/5/5/5/5/5/5/5/5"));
        }

        [TestMethod]
        public void WebSample4()
        {
            Assert.AreEqual(133, scorer.Score("14456/5/X017/6/X2/6"));
        }
    

    }
}
