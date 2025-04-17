using System;
using System.Globalization;
using pjpproject;
namespace pjpproject
{
public class Interpreter{
        private Stack<object> stack = new();
        private Dictionary<string, object> variables = new();
        private Dictionary<int, int> labels = new();
        private string[]? lines;
        private void ReadFile(string fileName){
            lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("label"))
                {
                    int labelNum = int.Parse(lines[i].Split()[1]);
                    labels[labelNum] = i;
                }
            }
        }

        public void InterpretFile(string fileName){
            ReadFile(fileName);
            int pc = 0;
            while (pc < lines.Length)
            {
                var line = lines[pc].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("label"))
                {
                    pc++;
                    continue;
                }

                var parts = line.Split(' ', 3);
                string op = parts[0];

                switch (op)
                {
                    case "push":
                        var type = parts[1];
                        var valueStr = parts[2];
                        object value = type switch
                        {
                            "I" => int.Parse(valueStr),
                            "F" => float.Parse(valueStr, System.Globalization.CultureInfo.InvariantCulture),
                            "S" => valueStr.Trim('"'),
                            "B" => bool.Parse(valueStr),
                            _ => throw new Exception("Unknown type")
                        };
                        stack.Push(value);
                        break;

                    case "pop":
                        stack.Pop();
                        break;

                    case "add":
                        BinaryOp(stack, parts[1], (a, b) => a + b); break;

                    case "sub":
                        BinaryOp(stack, parts[1], (a, b) => a - b); break;

                    case "mul":
                        BinaryOp(stack, parts[1], (a, b) => a * b); break;

                    case "div":
                        BinaryOp(stack, parts[1], (a, b) => a / b); break;

                    case "mod":
                        {
                            int b = (int)stack.Pop();
                            int a = (int)stack.Pop();
                            stack.Push(a % b);
                            break;
                        }

                    case "uminus":
                        {
                            var t = parts[1];
                            if (t == "I")
                                stack.Push(-(int)stack.Pop());
                            else if (t == "F")
                                stack.Push(-(float)stack.Pop());
                            break;
                        }

                    case "concat":
                        {
                            var b = stack.Pop().ToString();
                            var a = stack.Pop().ToString();
                            stack.Push(a + b);
                            break;
                        }

                   case "and":
                        {
                            var right = (bool)stack.Pop();
                            var left = (bool)stack.Pop();
                            stack.Push(left && right);
                            break;
                        }

                    case "or":
                        {
                            var right = (bool)stack.Pop();
                            var left = (bool)stack.Pop();
                            stack.Push(left || right);
                            break;
                        }

                    case "not":
                        stack.Push(!(bool)stack.Pop());
                        break;

                    case "eq":
                        {
                            var right = stack.Pop();
                            var left = stack.Pop();
                            stack.Push(Equals(left, right));
                            break;
                        }

                    case "lt":
                        stack.Push(Compare(stack.Pop(), stack.Pop(), "<")); break;

                    case "gt":
                        stack.Push(Compare(stack.Pop(), stack.Pop(), ">")); break;

                    case "itof":
                        stack.Push(Convert.ToSingle(stack.Pop()));
                        break;

                    case "load":
                        stack.Push(variables[parts[1]]);
                        break;

                    case "save":
                        variables[parts[1]] = stack.Pop();
                        break;

                    case "print":
                        int n = int.Parse(parts[1]);
                        var items = new List<object>();
                        for (int i = 0; i < n; i++)
                            items.Insert(0, stack.Pop());
                        Console.WriteLine(string.Join("", items));
                        if (stack.Count > 0)
                        {
                            Console.WriteLine($"[WARNING] Stack not empty after print: {stack.Count} items remain.");
                        }
                        break;

                    case "jmp":
                        pc = labels[int.Parse(parts[1])];
                        continue;

                    case "fjmp":
                        var cond = (bool)stack.Pop();
                        if (!cond)
                        {
                            pc = labels[int.Parse(parts[1])];
                            continue;
                        }
                        break;

                    case "read":
                        Console.Write("> ");
                        string input = Console.ReadLine()!;
                        object readVal = parts[1] switch
                        {
                            "I" => int.Parse(input),
                            "F" => float.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                            "B" => bool.Parse(input),
                            "S" => input,
                            _ => throw new Exception("Unknown read type")
                        };
                        stack.Push(readVal);
                        break;

                    default:
                        throw new Exception($"Unknown instruction: {line}");
                }
                pc++;
            }
        }

        private int BinaryOp(Stack<object> stack, string type, Func<dynamic, dynamic, dynamic> op)
        {
            var b = stack.Pop();
            var a = stack.Pop();
            stack.Push(op(a, b));
            return 0;
        }

        private bool Compare(object b, object a, string op)
        {
            if (a is int && b is int)
            {
                return op == "<" ? (int)a < (int)b : (int)a > (int)b;
            }
            else if (a is float || b is float)
            {
                float fa = Convert.ToSingle(a);
                float fb = Convert.ToSingle(b);
                return op == "<" ? fa < fb : fa > fb;
            }
            throw new Exception("Unsupported compare");
        }

    }
}








