using System;

namespace СПО_ЛР__1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string line = Convert.ToString(Console.ReadLine()).Trim();

                while (line != "")
                {
                    (string, string) token = getNextToken(line);
                    Console.WriteLine("(" + token.Item1 + "," + token.Item2 + ")");
                    line = line.Trim();
                    line = line.Substring(token.Item2.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
            }
        }

        static public (string, string) getNextToken(string line)
        {
                int state = 0; // начальное состояние ДКА
                int lastAccepting = -1; // последнее принимающее состояние
                int lastPos = -1;

                line = line.Trim();

                for (int pos = 0; state >= 0; pos++)
                {
                    if (state != -1 && state != 3 && state != 4)
                    { // -1 - тупиковое сотояние ДКА
                      // сохраняем последнее принимающее состояние
                        lastAccepting = state;
                        // и позицию, на котором оно достигнуто
                        lastPos = pos;
                    }

                    if (pos < line.Length)
                    {
                        // символ с номером pos во входной строке
                        char symb = line[pos];
                        // получаем новое состояние по таблице переходов
                        state = transitionTable(symb, state);
                    }
                    else
                    {
                        state = -1;
                    }
                }

                if (lastAccepting >= 0 && lastPos != 0)
                {
                    // убрать из строки первых lastPos символов и сохранить их в tokenStr
                    string tokenStr = line.Substring(0, lastPos);
                    // возвращаем идентификатор токена, соответствующий lastAccepting и остаток строки
                    return (acceptingStates(lastAccepting), tokenStr);
                }
                else
                {
                    throw new Exception("Обнаружен неожиданный символ '" + line[lastPos] + "'");
                }
        }

        static public int transitionTable(char symb, int state) // таблица переходов в новые состояния
        {
            switch (state)
            {
                case 0:
                    if ((symb >= 'a' && symb <= 'z') || symb == '_')
                    {
                        state = 7;
                    }
                    else if (symb == '(')
                    {
                        state = 8;
                    }
                    else if (symb == ')')
                    {
                        state = 9;
                    }
                    else if (symb == ',')
                    {
                        state = 10;
                    }
                    else if (symb == '+' || symb == '*' || symb == '/' || symb == '^' || symb == '-')
                    {
                        state = 6;
                    }
                    else if (symb >= '0' && symb <= '9')
                    {
                        state = 1;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 1:
                    if (symb >= '0' && symb <= '9')
                    {
                        state = 1;
                    }
                    else if (symb == '.')
                    {
                        state = 2;
                    }
                    else if (symb == 'e' || symb == 'E')
                    {
                        state = 3;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 2:
                    if (symb >= '0' && symb <= '9')
                    {
                        state = 2;
                    }
                    else if (symb == 'e' || symb == 'E')
                    {
                        state = 3;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 3:
                    if (symb == '+' || symb == '-')
                    {
                        state = 4;
                    }
                    else if (symb >= '0' && symb <= '9')
                    {
                        state = 5;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 4:
                    if (symb >= '0' && symb <= '9')
                    {
                        state = 5;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 5:
                    if (symb >= '0' && symb <= '9')
                    {
                        state = 5;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                case 7:
                    if ((symb >= 'a' && symb <= 'z') || symb == '_' || (symb >= '0' && symb <= '9'))
                    {
                        state = 7;
                    }
                    else
                    {
                        state = -1;
                    }
                    break;

                default:
                    state = -1;
                    break;
            }

            return state;
        }

        static public string acceptingStates(int state) // таблица идентификаторов токенов
        {
            string result = string.Empty;

            if (state == 1 || state == 2 || state == 5)
            {
                result = "Number";
            }
            else if (state == 6)
            {
                result = "Operator";
            }
            else if (state == 7)
            {
                result = "Identifier";
            }
            else if (state == 8)
            {
                result = "LParen";
            }
            else if (state == 9)
            {
                result = "RParen";
            }
            else if (state == 10)
            {
                result = "Comma";
            }

            return result;
        }
    }
}