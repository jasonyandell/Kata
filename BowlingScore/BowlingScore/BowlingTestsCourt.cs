using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore
{
    [TestClass]
    public class BowlingTestsCourt : BowlingTests
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            scorer = new BowlingScorer();
        }
        
    }
}