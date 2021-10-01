using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.Calculator.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.Calculator.PublicInfos;
using me.cqp.luohuaming.Calculator.Tool.Http;
using me.cqp.luohuaming.Calculator.Code.Model;
using Newtonsoft.Json;

namespace me.cqp.luohuaming.Calculator.Code.OrderFunctions
{
    public class GetRate : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "#汇率";

        public bool Judge(string destStr) => destStr.StartsWith(GetOrderStr());

        public FunctionResult Progress(CQGroupMessageEventArgs e)
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
            sendText.MsgToSend.Add(GetRateResult(e.Message.Text.Replace(GetOrderStr(), "").Trim()));
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
            sendText.MsgToSend.Add(GetRateResult(e.Message.Text.Replace(GetOrderStr(), "").Trim()));
            result.SendObject.Add(sendText);
            return result;
        }
        public static string GetRateResult(string order)
        {
            if (string.IsNullOrWhiteSpace(MainSave.RateKey))
                return $"请在 https://app.exchangerate-api.com/dashboard 申请APIKey，并参照文档填写在配置文件内";
            string baseURL = $"https://v6.exchangerate-api.com/v6/{MainSave.RateKey}/latest/";
            int count = 0;
            string type = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < order.Length; i++)
            {
                if (char.IsDigit(order[i]))
                    sb.Append(order[i]);
            }
            if (sb.Length == 0)
            {
                count = 1;
            }
            else
                count = int.Parse(sb.ToString());
            type = order.Replace(" ", "").Replace(count.ToString(), "");
            type = PatternChange(type);
            baseURL += type;
            using (var http = new HttpWebClient())
            {
                string data = http.DownloadString(baseURL);
                Rate json = JsonConvert.DeserializeObject<Rate>(data);
                if (json.result != "success")
                {
                    MainSave.CQLog.Info("请求错误", $"result: {json.result} 请检查请求的货币类型");
                    return $"无法获取汇率，请检查请求的货币类型文本";
                }
                StringBuilder result = new StringBuilder();
                result.AppendLine($"货币类型：{json.base_code}");
                List<string> target = new List<string> { "CNY", "USD", "EUR", "JPY", "HKD" };
                foreach(var item in target)
                {
                    if (json.base_code != item)
                    {
                        float rate = (float)json.conversion_rates.GetType().GetProperty(item).GetValue(json.conversion_rates);
                        result.AppendLine($"{PatternChange(item)}: {rate * count:f2}");
                    }
                }
                result.AppendLine(CommonHelper.TimeStamp2Time(json.time_last_update_unix).ToString("G"));
                return result.ToString();
            }

        }
        public static string PatternChange(string pattern)
        {
            pattern = pattern.ToUpper();
            switch (pattern)
            {
                case "RMB":
                case "人民币":
                case "元":
                    return "CNY";
                case "日元":
                case "円":
                    return "JPY";
                case "欧元":
                    return "EUR";
                case "美元":
                case "刀":
                case "dollar":
                case "doller":
                    return "USD";
                case "港币":
                    return "HKD";
                case "英镑":
                case "£":
                    return "GBP";
                case "GBP":
                    return "英镑";
                case "EUR":
                    return "欧元";
                case "HKD":
                    return "港币";
                case "USD":
                    return "美元";
                case "JPY":
                    return "日元";
                case "CNY":
                    return "人民币";
                default:
                    return pattern;
            }
        }
    }
}
