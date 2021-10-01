using System;
using me.cqp.luohuaming.Calculator.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.Calculator.PublicInfos;
using me.cqp.luohuaming.Calculator.Sdk.Cqp;

namespace me.cqp.luohuaming.Calculator.Code.OrderFunctions
{
    public class DoCalc : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;
        
        public string GetOrderStr() => "#计算";

        public bool Judge(string destStr) => destStr.StartsWith(GetOrderStr());

        public FunctionResult Progress(CQGroupMessageEventArgs e)//群聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromGroup,
            };
            sendText.MsgToSend.Add(CQApi.CQCode_At(e.FromQQ).ToString() + GetCalcResult(e.Message.Text.Replace(GetOrderStr(), "").Trim()));
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromQQ,
            };
            sendText.MsgToSend.Add(GetCalcResult(e.Message.Text.Replace(GetOrderStr(), "").Trim()));
            result.SendObject.Add(sendText);
            return result;
        }
        public static string GetCalcResult(string pattern)
        {
            try
            {
                return Calculator.CalcRPN(Calculator.ParseToRPN(pattern)).ToString();
            }
            catch (Exception e)
            {
                MainSave.CQLog.Info("计算出错", e.Message+"\n"+e.StackTrace);
                MainSave.CQLog.Info("无法计算", "模式无法匹配，可向作者申请增加模式");
                return "无法格式化算式，请按照更正规的格式书写算式";
            }
        }
    }
}
