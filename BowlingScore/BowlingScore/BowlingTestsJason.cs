using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore
{
    [TestClass]
    public class BowlingTestsJason : BowlingTests
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            scorer = new JBowlingScorer();
        }
        
    }
}