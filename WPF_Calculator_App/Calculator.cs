using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Calculator_App
{
    public class Calculator
    {
        public double Result { get; private set; } = 0;
        public string Error { get; set; }
        ICollection<char> _operators;
        string _calc;
        ICollection<string> _batches;
        public Calculator(string calc)
        {
            _calc = calc;
            _operators = new List<char> { '/', '*', '+', '-' };
            _batches = !_calc.Contains('(') ? new List<string> { _calc } : GetBatches();
        }

        public void Calculate()
        {
            try
            {
                var batchesList = _batches.ToList();
                var operationsComlete = 0;
                while(batchesList.Count > 0)
                {
                    for (int i = 0; i < batchesList.Count; i++)
                    {
                        var batch = batchesList[i];
                        if (!batch.Contains('('))
                        {
                            //var batchAt = _calc.IndexOf(batchesList[i]);

                            if (batch.Contains('/'))
                            {
                                batch = Divide(batch);
                                operationsComlete++;
                            }
                            if (batch.Contains('*'))
                            {
                                batch = Multiply(batch);    
                                operationsComlete++;
                            }
                            if (batch.Contains('+'))
                            {
                                batch = Add(batch);
                                operationsComlete++;
                            }
                            if (batch.Contains('-'))
                            {
                                batch = Subtract(batch);
                                operationsComlete++;
                            }
                            if (_calc.Contains($"({batchesList[i]})"))
                            {
                                _calc = _calc.Replace($"({batchesList[i]})", batch);
                            }
                            else
                            {
                                _calc = _calc.Replace($"{batchesList[i]}", batch);
                            }
                            for (int b = 0; b <  batchesList.Count; b++)
                            {
                                if (batchesList[b].Contains($"({batchesList[i]})"))
                                {
                                    batchesList[b] = batchesList[b].Replace($"({batchesList[i]})", batch);
                                }
                            }
                            batchesList.RemoveAt(i);
                        }
                    }

                }
                if (operationsComlete > 0)
                {
                    Result = double.Parse(_calc);
                }


                
            }
            catch (DivideByZeroException)
            {
                Result = double.NaN;
                Error = "Cannot divide by zero";
            }
        }

        string Divide(string batch)
        {
            var currentOperator = '/';
            int i = 0;
            while (i < batch.Length)
            {
                if (i > 0 && batch[i] == currentOperator)
                {
                    var result = GetNumbersAndIndexesForCalculation(i, batch);
                    if (result.num2 == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero");
                    }
                    if (result.endIndex == batch.Length - 1)
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 / result.num2).ToString();
                    }
                    else
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 / result.num2).ToString() + batch.Substring(result.endIndex + 1);
                    }
                }
                else
                {
                    i++;
                }
            }
            return batch;
        }

        string Multiply(string batch)
        {
            var currentOperator = '*';
            int i = 0;
            while (i < batch.Length)
            {
                if (i > 0 && batch[i] == currentOperator)
                {
                    var result = GetNumbersAndIndexesForCalculation(i, batch);

                    if (result.endIndex == batch.Length - 1)
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 * result.num2).ToString();    
                    }
                    else
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 * result.num2).ToString() + batch.Substring(result.endIndex + 1);
                    }
                }
                else
                {
                    i++;
                }
            }
            return batch;
        }

        string Add(string batch)
        {
            var currentOperator = '+';
            int i = 0;
            while (i < batch.Length)
            {
                if (i > 0 && batch[i] == currentOperator)
                {
                    var result = GetNumbersAndIndexesForCalculation(i, batch);

                    if (result.endIndex == batch.Length - 1)
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 + result.num2).ToString();
                    }
                    else
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 + result.num2).ToString() + batch.Substring(result.endIndex + 1);
                    }
                }
                else
                {
                    i++;
                }
            }
            return batch;
        }
        string Subtract(string batch)
        {
            var currentOperator = '-';
            int i = 0;
            while (i < batch.Length)
            {
                if (i > 0 && batch[i] == currentOperator)
                {
                    var result = GetNumbersAndIndexesForCalculation(i, batch);

                    if (result.endIndex == batch.Length - 1)
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 - result.num2).ToString();
                    }
                    else
                    {
                        batch = batch.Substring(0, result.startIndex) + (result.num1 - result.num2).ToString() + batch.Substring(result.endIndex + 1);
                    }
                }
                else
                {
                    i++;
                }
            }
            return batch;
        }

        ICollection<string> GetBatches()
        {
            var batches = new List<string>();
            batches.Add(_calc);
            var extraParenthesis = 0;
            if (_calc.Where(x => x == '(').Count() == _calc.Where(x => x == ')').Count())
            {
                for (int i = 0; i < _calc.Length; i++)
                {
                    if (_calc[i] == '(')
                    {
                        for (int x = i + 1; x < _calc.Length; x++)
                        {
                            if(_calc[x] == '(')
                            {
                                extraParenthesis++;
                            }
                            else if(_calc[x] == ')')
                            {
                                if (extraParenthesis == 0)
                                {
                                    batches.Add(_calc.Substring(i + 1, x - (i + 1)));
                                    break;
                                }
                                extraParenthesis--;
                            }
                        }
                    }
                }
            }
            
            return batches;
        }

        (double num1, double num2, int startIndex, int endIndex) GetNumbersAndIndexesForCalculation(int operatorIndex, string batch)
        {
            int startIndex = 0;
            int endIndex = batch.Length - 1;
            for (int i = 0; i < batch.Length; i++)
            {
                var previousIndex = i > 0 ? i - 1 : 0;
                if (_operators.Contains(batch[i]) && !_operators.Contains(batch[previousIndex]) && i < operatorIndex)
                {
                    startIndex = i + 1;
                }
                else if (_operators.Contains(batch[i]) && !_operators.Contains(batch[previousIndex]) && i > operatorIndex)
                {
                    endIndex = i - 1;
                    break;
                }
            }

            var num1 = double.Parse(batch.Substring(startIndex, operatorIndex - startIndex));
            var num2 = double.Parse(batch.Substring(operatorIndex + 1, (endIndex) - operatorIndex));

            return (num1, num2, startIndex, endIndex);
        }
    }
}
