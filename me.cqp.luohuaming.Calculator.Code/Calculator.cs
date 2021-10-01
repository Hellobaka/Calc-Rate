using System;
using System.Collections.Generic;
using System.Text;

namespace me.cqp.luohuaming.Calculator.Code
{
    public static class Calculator
    {
        static Dictionary<char, int> operators = new Dictionary<char, int>
        {
            {'+', 1 },
            {'-', 1 },
            {'*', 2 },
            {'/', 2 },
            {'(', 10 },
        };
        public static double CalcRPN(string RPN)
        {
            Stack<double> nums = new Stack<double>();
            StringBuilder numSu = new StringBuilder();
            RPN = ReplaceUnleagalChar(RPN);
            bool flag = false;
            for (int i = 0; i < RPN.Length; i++)
            {
                if (char.IsDigit(RPN[i]) || RPN[i] == '.' || (i > 0 && RPN[i - 1] == '('))
                {
                    if (flag)
                    {
                        numSu.Append(RPN[i]);
                        continue;
                    }
                    nums.Push(RPN[i] - '0');
                }
                else
                {
                    if (RPN[i] != '(' && !string.IsNullOrWhiteSpace(numSu.ToString()))
                    {
                        nums.Push(double.Parse(numSu.ToString()));
                        numSu.Clear();
                        flag = false;
                        continue;
                    }
                    if (RPN[i] == '(' || RPN[i] == ')')
                    {
                        flag = true;
                        continue;
                    }
                    double OPb = nums.Pop();
                    double OPa = nums.Pop();
                    switch (RPN[i])
                    {
                        case '+':
                            nums.Push(OPa + OPb);
                            break;
                        case '-':
                            nums.Push(OPa - OPb);
                            break;
                        case '*':
                            nums.Push(OPa * OPb);
                            break;
                        case '/':
                            nums.Push(OPa / OPb);
                            break;
                        default:
                            break;
                    }
                    numSu.Clear();
                }
            }
            return nums.Pop();
        }
        public static string ParseToRPN(string pattern)
        {
            StringBuilder Rexpression = new StringBuilder();

            pattern = ReplaceUnleagalChar(pattern);
            Stack<double> num = new Stack<double>();
            Stack<char> oper = new Stack<char>();
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            for (int i = 0; i < pattern.Length; i++)
            {
                if (char.IsDigit(pattern[i]) || pattern[i] == '.' || i == 0 || (i > 0 && pattern[i - 1] == '('))
                {
                    stringBuilder.Append(pattern[i]);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(stringBuilder.ToString()))
                        num.Push(double.Parse(stringBuilder.ToString()));
                    if (stringBuilder.Length > 1)
                    {
                        Rexpression.Append($"({stringBuilder})");
                    }
                    else
                        Rexpression.Append(stringBuilder.ToString());
                    stringBuilder.Clear();
                    flag = pattern[i] == '(';
                    PushOperators(ref oper, ref Rexpression, pattern[i]);
                }
            }
            if (!string.IsNullOrWhiteSpace(stringBuilder.ToString()))
            {
                if (stringBuilder.Length > 1)
                    Rexpression.Append($"({stringBuilder})");
                else
                    Rexpression.Append(stringBuilder.ToString());
            }
            while (oper.Count > 0)
                Rexpression.Append(oper.Pop());
            Console.WriteLine($"逆波兰表达式：{Rexpression}");
            return Rexpression.ToString();
        }
        public static string ReplaceUnleagalChar(string p)
        {
            return p.Replace("（", "(").Replace("）", ")").Replace("÷", "/").Replace("×", "*")
                .Replace("x", "*").Replace("除以", "/").Replace("除", "/").Replace("乘以", "*").Replace("乘", "*").Replace(" ", "");
        }
        public static void PushOperators(ref Stack<char> opers, ref StringBuilder Rexpression, char oper)
        {
            if (opers.Count == 0)
                opers.Push(oper);
            else if (oper == ')')
            {
                while (opers.Count != 0 && opers.Peek() != '(')
                    Rexpression.Append(opers.Pop());
                opers.Pop();
            }
            else if (operators[oper] >= operators[opers.Peek()])
                opers.Push(oper);
            else
            {
                while (opers.Count != 0 && opers.Peek() != '(' && operators[oper] <= operators[opers.Peek()])
                    Rexpression.Append(opers.Pop());
                opers.Push(oper);
            }
        }
    }
}
