using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Calculator_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        string _calculation;
        List<char> _operators = new List<char> { '*', '/', '-', '+' };
        bool _lastActionWasOperator => _operators.Contains(Calculation[^1]);
        bool _hasResult;
        bool _hasError;
        int _parenthesisCount = 0;
        public string Calculation
        {
            get
            {
                return _calculation;
            }
            set
            {
                _calculation = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow()
        {
            Calculation = "";
            DataContext = this;
            InitializeComponent();
        }


        private void btnDecimal_Click(object sender, RoutedEventArgs e)
        {
            if(Calculation.Length > 0)
            {
                var dotIndex = Calculation.LastIndexOf('.');
                var lastOperatorPostion = 0;
                foreach(var op in _operators)
                {
                    if(Calculation.LastIndexOf(op) > lastOperatorPostion)
                    {
                        lastOperatorPostion = Calculation.LastIndexOf(op);
                    }
                }
                if (_lastActionWasOperator)
                {
                    Calculation += "0.";
                }
                else if (dotIndex < lastOperatorPostion)
                {
                    Calculation += ".";
                }
            }
            else
            {
                Calculation += "0.";
            }
            btnParenthesis_OEnabledStateManager();
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if(Calculation.Length > 0)
            {
                Calculator bodmas = new Calculator(Calculation);
                bodmas.Calculate();
                if (bodmas.Result is double.NaN)
                {
                    Calculation = bodmas.Error;
                    _hasError = true;
                }
                else if (bodmas.Result != 0)
                {
                    Calculation = bodmas.Result.ToString();
                    _hasResult = true;
                }
            }
            btnParenthesis_OEnabledStateManager();
        }
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("0");
            btnParenthesis_OEnabledStateManager();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("1");
            btnParenthesis_OEnabledStateManager();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("2");
            btnParenthesis_OEnabledStateManager();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("3");
            btnParenthesis_OEnabledStateManager();
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("4");
            btnParenthesis_OEnabledStateManager();
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("5");
            btnParenthesis_OEnabledStateManager();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("6");
            btnParenthesis_OEnabledStateManager();
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("7");
            btnParenthesis_OEnabledStateManager();
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("8");
            btnParenthesis_OEnabledStateManager();
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            CalculationBuilder("9");
            btnParenthesis_OEnabledStateManager();
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (Calculation.Length > 0 && _lastActionWasOperator)
            {
                Calculation = Calculation.Remove(Calculation.Length - 1);
            }
            if (_hasResult)
            {
                _hasResult = false;
            }
            if (_hasError)
            {
                Calculation = "";
                _hasError = false;
            }
            if (Calculation.Length > 0)
            {
                Calculation += "+";
            }
            btnParenthesis_OEnabledStateManager();
        }
        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            if(Calculation.Length > 0 && _lastActionWasOperator)
            {
                Calculation = Calculation.Remove(Calculation.Length - 1);
            }
            if (_hasResult)
            {
                _hasResult = false;
            }
            if (_hasError)
            {
                Calculation = "";
                _hasError = false;
            }
            if (Calculation.Length > 0)
            {
                Calculation += "*";
            }
            btnParenthesis_OEnabledStateManager();
        }

        private void btnDivide_Click(object sender, RoutedEventArgs e)
        {
            if (Calculation.Length > 0 && _lastActionWasOperator)
            {
                Calculation = Calculation.Remove(Calculation.Length - 1);
            }
            if (_hasResult)
            {
                _hasResult = false;
            }
            if (_hasError)
            {
                Calculation = "";
                _hasError = false;
            }
            if (Calculation.Length > 0)
            {
                Calculation += "/";
            }
            btnParenthesis_OEnabledStateManager();
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Calculation.Length > 0 && _lastActionWasOperator)
            {
                Calculation = Calculation.Remove(Calculation.Length - 1);
            }
            if (_hasResult)
            {
                _hasResult = false;
            }
            if (_hasError)
            {
                Calculation = "";
                _hasError = false;
            }
            if (Calculation.Length > 0)
            {
                Calculation += "-";
            }
            btnParenthesis_OEnabledStateManager();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (_hasResult)
            {
                _hasResult = false;
            }
            _hasError = false;
            Calculation = "";
            btnParenthesis_OEnabledStateManager();
        }

        private void btnClear1_Click(object sender, RoutedEventArgs e)
        {
            if (Calculation.Length > 0)
            {
                if (Calculation[^1] == '(')
                {
                    _parenthesisCount--;
                }
                else if (Calculation[^1] == ')')
                {
                    _parenthesisCount++;
                }
                Calculation = Calculation.Substring(0, Calculation.Length - 1);
            }
            if (_hasResult || _hasError)
            {
                Calculation = "";
                _hasResult = false;
                _hasError = false;
            }
            //check the current state of parenthesis' in the calculation now one has been removed
            if (_parenthesisCount == 0)
            {
                RemoveHighlightClosingParenthesis();
            }
            else if (_parenthesisCount > 0)
            {
                HighlightClosingParenthesis();
            }
            btnParenthesis_OEnabledStateManager();
        }
        private void btnParenthesis_O_Click(object sender, RoutedEventArgs e)
        {
            _parenthesisCount++;
            CalculationBuilder("(");
            HighlightClosingParenthesis();
        }

        private void btnParenthesis_C_Click(object sender, RoutedEventArgs e)
        {
            if (_parenthesisCount > 0)
            {
                _parenthesisCount--;
                CalculationBuilder(")");
            }
            if (_parenthesisCount == 0)
            {
                RemoveHighlightClosingParenthesis();
            }
        }

        private void CalculationBuilder(string newComponent)
        {
            if (_hasResult || _hasError)
            {
                Calculation = "";
                _hasResult = false;
                _hasError = false;
            }
            Calculation += newComponent;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnParenthesis_OEnabledStateManager()
        {
            if (Calculation.Length == 0 || (!char.IsNumber(Calculation[^1]) && Calculation[^1] != '.') || _hasResult || _hasError)
            {
                btnParenthesis_O.IsEnabled = true;
            }
            else
            {
                btnParenthesis_O.IsEnabled = false;
            }
        }
        private void HighlightClosingParenthesis()
        {
            btnEquals.IsEnabled = false;
            btnParenthesis_C.Background = Brushes.Maroon;
            btnParenthesis_C.IsEnabled = true;
        }
        private void RemoveHighlightClosingParenthesis()
        {
            btnEquals.IsEnabled = true;
            btnParenthesis_C.Background = Brushes.Transparent;
            btnParenthesis_C.IsEnabled = false;
        }
    }
}
