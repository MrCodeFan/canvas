using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoAP_Lab1_2
{
    public class CommandLine
    {
        private Stack<Operator> operators = new Stack<Operator>();
        private Stack<Operand> operands = new Stack<Operand>();

        public string command { get; set; }
        private bool flag = true;
        private int intCommand { get; set; }
        public CommandLine()
        {
            intCommand = 0;
            operators = new Stack<Operator>();
            operands = new Stack<Operand>();
        }

        public void ob(string command)
        { 
            this.command = command;
            ob();
        }
        public void ob()
        {
            
            for (int i = 0; i < command.Length; i++)
            {
                char symbol = command[i];
                if ( IsNotOperation(symbol) )
                {
                    if (symbol == '"')
                    {
                        string str = getString(command, i + 1, '"');
                        i += str.Length + 1;
                        // i--;
                        operands.Push(new Operand(str));
                    }
                    else if (!(Char.IsDigit(symbol)))
                    {
                        if (operands.Count == 0)
                        {
                            operands.Push(new Operand(symbol));
                        }
                        else
                        {


                            Operand operand = operands.Pop();
                            // operand.
                            operands.Push(operand);
                            String name = operand.GetType().Name;

                            if (name == "String" || name == "string")
                            {
                                operands.Pop();
                                operand = new Operand(
                                    String.Format("{0}{1}", operand.value, symbol)
                                );
                            }
                            else if (name == "Char" || name == "char")
                            {
                                operand = new Operand(
                                    String.Format("{0}{1}", operand.value, symbol)
                                );
                            }


                            if (symbol != ' ')
                            {
                                this.operands.Push(operand);
                            }
                            continue;
                        }
                    }
                    else if (Char.IsDigit(symbol))
                    {
                        if (Char.IsDigit(command[i + 1]))
                        {
                            if (flag)
                            {
                                this.operands.Push(new Operand(CharToInt(symbol)));
                            }
                            Operand operand = operands.Pop();

                            this.operands.Push(new Operand((int)operand.value * 10 + CharToInt(command[i + 1])));
                            flag = false;
                            continue;
                        }
                        else if ((command[i + 1] == ',' || command[i + 1] == ')' || command[i + 1] == ' ') && !(Char.IsDigit(command[i - 1])))
                        {
                            this.operands.Push(new Operand(CharToInt(symbol)));
                            continue;
                        }
                    }
                    
                }
                else if (symbol == 'O')
                {
                    intCommand = 2;
                    if (this.operators.Count == 0)
                    {
                        this.operators.Push(OperatorContainer.FindOperator(symbol));
                    }
                }
                else if (symbol == 'M')
                {
                    intCommand = 3;
                    if (this.operators.Count == 0)
                    {
                        this.operators.Push(OperatorContainer.FindOperator(symbol));
                    }
                }
                else if (symbol == 'D')
                {
                    intCommand = 4;
                    if (this.operators.Count == 0)
                    {
                        this.operators.Push(OperatorContainer.FindOperator(symbol));
                    }
                }
                else if (symbol == 'C')
                {
                    intCommand = 5;
                    if (this.operators.Count == 0)
                    {
                        this.operators.Push(OperatorContainer.FindOperator(symbol));
                    }
                }
                else if (symbol == '(')
                {
                    this.operators.Push(OperatorContainer.FindOperator(symbol));
                }
                else if (symbol == ')')
                {
                    do
                    {
                        if (operators.Peek().symbolOperator == '(')
                        {
                            operators.Pop();
                            break;
                        }
                        if (operators.Count == 0)
                        {
                            break;
                        }
                    }
                    while (operators.Peek().symbolOperator != '(');
                }

                flag = true;
            }



            

        }

        private bool IsNotOperation(char item)
        {
            if (
                (
                    item == 'O' || item == 'M' || item == 'D' || item == 'C' || 
                    item == ',' || item == '(' || item == ')')
               )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsNumber(char item)
        {
            if (item == '0' || item == '1' || item == '2' || item == '3' || item == '4' || item == '5' || item == '6' || item == '7' || item == '8' || item == '9')
            {
                return true;
            }
            return false;
        }

        private int CharToInt(char ch)
        {
            switch (ch)
            {
                case ('1'):
                    return 1;
                    break;
                case ('2'):
                    return 2;
                    break;
                case ('3'):
                    return 3;
                    break;
                case ('4'):
                    return 4;
                    break;
                case ('5'):
                    return 5;
                    break;
                case ('6'):
                    return 6;
                    break;
                case ('7'):
                    return 7;
                    break;
                case ('8'):
                    return 8;
                    break;
                case ('9'):
                    return 9;
                    break;
                default:
                    return 0;
                    break;
            }
        }


        public Stack<Operator> getOperators()
        {
            return operators;
        }
        public Stack<Operand> getOperands()
        {
            return operands;
        }

        public string getString(string str, int from, char toStr)
        {
            string res = "";
            for (int i = from; i < str.Length; i++)
            {
                if (str[i] != toStr)
                {
                    res += String.Format("{0}", str[i]);
                } else { break; }
            }

            return res;
        }

    }

}
