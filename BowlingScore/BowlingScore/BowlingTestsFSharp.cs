using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScore
{
    [TestClass]
    public class BowlingTestsFSharp : BowlingTests
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            scorer = new FSharpBowlingScorer();
        }
        
    }
}