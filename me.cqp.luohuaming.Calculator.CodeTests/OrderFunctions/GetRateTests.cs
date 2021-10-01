using Microsoft.VisualStudio.TestTools.UnitTesting;
using me.cqp.luohuaming.Calculator.Code.OrderFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.luohuaming.Calculator.Code.OrderFunctions.Tests
{
    [TestClass()]
    public class GetRateTests
    {
        [TestMethod()]
        public void GetRateResultTest()
        {
            string order = "7589日元";
            Console.WriteLine(GetRate.GetRateResult(order));
        }
    }
}