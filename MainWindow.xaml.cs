using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace postfix_infix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string infix_pattern = @"^([a-zA-Z0-9()]+([-+/*^][a-zA-Z0-9()]+)*)$";
        string postfix_pattern = @"^([a-zA-Z0-9]+([-+/*^]*)*)*$";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUser.Text;

            if (Regex.IsMatch(userInput, infix_pattern))
            {
                txtResult.Text = in_to_postfix(userInput);
            }
            else if (Regex.IsMatch(userInput, postfix_pattern))
            {
                txtResult.Text = post_to_infix(userInput);
            }
            else
            {
                txtResult.Text = "Wrong input";
            }
        }

        private string post_to_infix(string postfix)
        {
            Stack<string> stack = new Stack<string>();

            foreach (char token in postfix)
            {
                if (char.IsLetterOrDigit(token))
                {
                    stack.Push(token.ToString());
                }
                else
                {
                    string operand2 = stack.Pop();
                    string operand1 = stack.Pop();
                    string expression = $"({operand1} {token} {operand2})";
                    stack.Push(expression);
                }
            }

            return stack.Pop();
        }

        private string in_to_postfix(string infix)
        {
            StringBuilder output = new StringBuilder();
            Stack<char> stack = new Stack<char>();
            Dictionary<char, int> precedence = new Dictionary<char, int>
            {
                {'+', 1},
                {'-', 1},
                {'*', 2},
                {'/', 2},
                {'^', 3}
            };

            foreach (char token in infix)
            {
                if (char.IsLetterOrDigit(token))
                {
                    output.Append(token);
                }
                else if (token == '(')
                {
                    stack.Push(token);
                }
                else if (token == ')')
                {
                    while (stack.Peek() != '(')
                    {
                        output.Append(stack.Pop());
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && precedence.ContainsKey(stack.Peek()) && precedence[stack.Peek()] >= precedence[token])
                    {
                        output.Append(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                output.Append(stack.Pop());
            }

            return output.ToString();
        }
    }
}
