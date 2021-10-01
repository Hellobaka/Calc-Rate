using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace me.cqp.luohuaming.Calculator.Code.OrderFunctions.Tests
{
    [TestClass()]
    public class DoCalcTests
    {
        [TestMethod()]
        public void GetCalcResultTest()
        {
            string pattern = "80+1.5*2.5-2";
            System.Console.WriteLine(DoCalc.GetCalcResult(pattern));
        }
    }
}