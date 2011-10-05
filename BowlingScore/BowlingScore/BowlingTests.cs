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

        [TestMethod]
        public void can_create_the_class()
        {
            var scorer = new BowlingScorer();
        }

        [TestMethod]
        public void can_score_strike()
        {
            var scorer = new BowlingScorer();
            Assert.AreEqual(10, scorer.Score("X"));
        }


    }
}
