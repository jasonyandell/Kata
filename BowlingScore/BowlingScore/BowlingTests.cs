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
            Assert.AreEqual(10 + 2, scorer.Score("5/25"));
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
    }
}
