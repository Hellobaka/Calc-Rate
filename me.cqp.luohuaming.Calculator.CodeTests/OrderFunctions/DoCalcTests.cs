using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace me.cqp.luohuaming.Calculator.Code.OrderFunctions.Tests
{
    [TestClass()]
    public class DoCalcTests
    {
        [TestMethod()]
        public void GetCalcResultTest()
        {
            string pattern = "(500/50)*65598.548*(6-5.652)/58";
            System.Console.WriteLine(DoCalc.GetCalcResult(pattern));
        }
    }
}