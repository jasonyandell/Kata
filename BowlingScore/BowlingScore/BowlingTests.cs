using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore
{
    [TestClass]
    public class BowlingTests
    {
        private BowlingScorer scorer;

        [TestInitialize]
        public void TestInitialize()
        {
            scorer = new BowlingScorer();
        }

        [TestMethod]
        public void can_score_strike()
        {
            Assert.AreEqual(10, scorer.Score("X"));
        }

        [TestMethod]
        public void can_score_spare()
        {
            Assert.AreEqual(10, scorer.Score("5/"));
        }

        [TestMethod]
        public void can_score_mundane_frame()
        {
            Assert.AreEqual(7, scorer.Score("25"));
        }

    }
}
