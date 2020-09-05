using BowieD.Unturned.NPCMaker.Templating.Conditions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Templating
{
    public static class LogicTool
    {
        private static Dictionary<string, int> operations = new Dictionary<string, int>()
        {
            { "(", 0 },
            { ")", 0 },
            { "|", 1 },
            { "||", 1 },
            { "or", 1 },
            { "&", 2 },
            { "&&", 2 },
            { "and", 2 },
            { "!", 3 }
        };
        private static bool resolve(string operation, bool a, bool b)
        {
            switch (operation)
            {
                case "|":
                case "||":
                case "or":
                    return a || b;
                case "&":
                case "&&":
                case "and":
                    return a && b;
                case "!":
                    return !b;
                case "==":
                case "equal":
                case "equals":
                    return a == b;
                case "!=":
                    return a != b;
                default:
                    return false;
            }
        }
        public static bool Evaluate(string expression, IList<ITemplateCondition> conditions, Template template)
        {
            if (conditions.Count == 0)
                return true;

            string finalExpression;

            if (string.IsNullOrEmpty(expression))
                finalExpression = string.Join("&", Enumerable.Range(0, conditions.Count).Select(d => d.ToString()));
            else
                finalExpression = expression;

            Dictionary<int, bool> results = new Dictionary<int, bool>();
            var prefixEntry = GetReversePolish(finalExpression.Replace(" ", ""));
            var stack = new Stack<bool>();

            foreach (var token in prefixEntry)
            {
                int digit1;
                var isDigit = int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out digit1);
                if (isDigit)
                {
                    if (!results.TryGetValue(digit1, out var result))
                    {
                        result = conditions[digit1].IsMet(template);
                        results[digit1] = result;
                    }
                    stack.Push(result);
                }
                else
                {
                    var num2 = stack.Pop();
                    var num1 = stack.Count > 0 ? stack.Pop() : false;
                    stack.Push(resolve(token, num1, num2));
                }
            }
            return stack.Pop();
        }
        private static string[] GetReversePolish(string expression)
        {
            var result = new List<string>();
            var stack = new Stack<string>();
            var tokens = GetTokens(expression);
            foreach (var token in tokens)
            {
                if (!operations.ContainsKey(token))
                {
                    result.Add(token);
                    continue;
                }
                if ("(".Equals(token))
                    stack.Push(token);
                else if (")".Equals(token))
                {
                    var nextInStack = stack.Pop();
                    while (nextInStack != "(")
                    {
                        result.Add(nextInStack);
                        nextInStack = stack.Pop();
                    }
                }
                else
                {
                    while (true)
                    {
                        if (stack.Count == 0 || operations[stack.Peek()] < operations[token])
                        {
                            stack.Push(token);
                            break;
                        }
                        result.Add(stack.Pop());
                    }
                }
            }
            while (stack.Count != 0)
                result.Add(stack.Pop());
            return result.ToArray();
        }
        private static string[] GetTokens(string expression)
        {
            var position = 0;
            var result = new List<string>();
            while (position < expression.Length)
            {
                var token = GetNextToken(expression, position);
                position += token.Length;
                result.Add(token);
            }
            return result.ToArray();
        }
        private static string GetNextToken(string expression, int position)
        {
            var symbol = expression[position];
            if (char.IsDigit(symbol))
            {
                var strBuilder = new StringBuilder();
                while (true)
                {
                    if (position >= expression.Length)
                        return strBuilder.ToString();
                    symbol = expression[position];
                    if (char.IsDigit(symbol) || (symbol == '.' && !strBuilder.ToString().Contains('.')))
                    {
                        strBuilder.Append(symbol);
                        position++;
                    }
                    else
                        return strBuilder.ToString();
                }
            }
            else
            {
                var strBuilder = new StringBuilder();
                while (true)
                {
                    if (position >= expression.Length)
                        return strBuilder.ToString();
                    symbol = expression[position];
                    if (!char.IsDigit(symbol))
                    {
                        strBuilder.Append(symbol);
                        position++;
                    }
                    else
                        return strBuilder.ToString();
                }
            }
        }
    }
}
