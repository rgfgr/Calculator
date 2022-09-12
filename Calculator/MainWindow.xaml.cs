using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int parenMis = 0;
        private bool canWrite = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (canWrite)
            {
                string text = ((Button)sender).Content.ToString();
                tb.Text += text == "Pow" ? "^" : text;
                if (text == "(")
                {
                    parenMis++;
                }
                else if (text == ")")
                {
                    parenMis--;
                }
                parenEnd.IsEnabled = parenMis > 0;
            }
        }

        private void Result_click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < parenMis && canWrite; i++)
            {
                tb.Text += ")";
            }
            canWrite = false;
            try
            {
                tbout.Text = Result("(" + tb.Text + ")");
            }
            catch (Exception)
            {
                tb.Text = "Error";
            }
        }

        private string Result(string op)
        {
            while (true)
            {
                bool containMulti = op.Contains("*");
                bool containDivis = op.Contains("/");
                bool containAdd = op.Contains("+");
                bool containSub = op.Contains("-");

                if (op.Substring(1).Contains("("))
                {
                    op = Parens(op, op.Substring(1).IndexOf("(") + 1);
                }
                else if (op.Contains("^"))
                {
                    op = Powers(op, op.IndexOf("^"));
                }
                else if (containMulti || containDivis)
                {
                    int iOp1 = containMulti ? op.IndexOf("*") : op.Length;
                    int iOp2 = containDivis ? op.IndexOf("/") : op.Length;
                    op = iOp1 > iOp2 ? GetSingle(op, iOp2) : GetSingle(op, iOp1);
                }
                else if (containAdd || containSub)
                {
                    int iOp1 = containAdd ? op.IndexOf("+") : op.Length;
                    int iOp2 = containSub ? op.IndexOf("-") : op.Length;
                    op = iOp1 > iOp2 ? GetSingle(op, iOp2) : GetSingle(op, iOp1);
                }
                else
                {
                    return op.Substring(1, op.Length - 2);
                }
                Console.WriteLine(op + ": Result");
            }
        }

        private string Parens(string op, int start)
        {
            int length = 0;
            int parenSt = 0;
            int parenEn = 0;
            int i = start;
            do
            {
                length++;
                if (op[i] == '(')
                {
                    parenSt++;
                }
                else if (op[i] == ')')
                {
                    parenEn++;
                }
                i++;
            } while (parenSt != parenEn);
            Console.WriteLine($"{op.Substring(0, start)}:{op.Substring(start, length)}:{op.Substring(start + length)}: Parens");
            return op.Substring(0, start) + Result(op.Substring(start, length)) + op.Substring(start + length);
        }

        private string Powers(string op, int sign)
        {
            return op;
        }

        private string GetSingle(string text, int sign)
        {
            Console.WriteLine($"{text}:{sign}: GetSingle");
            string opMath = "";
            string opPart1 = text.Substring(0, sign);
            for (int i = opPart1.Length - 1; i >= 0; i--)
            {
                if (opPart1[i] == '*' || opPart1[i] == '/' || opPart1[i] == '+' || opPart1[i] == '-' || opPart1[i] == '(')
                {
                    opMath = opPart1.Substring(i + 1);
                    opPart1 = opPart1.Substring(0, i + 1);
                    Console.WriteLine("yes 1: GetSingle");
                    break;
                }
            }

            string opPart2 = text.Substring(sign);
            for (int i = 1; i < opPart2.Length; i++)
            {
                if (opPart2[i] == '*' || opPart2[i] == '/' || opPart2[i] == '+' || opPart2[i] == '-' || opPart2[i] == ')')
                {
                    opMath += opPart2.Substring(0, i);
                    opPart2 = opPart2.Substring(i);
                    Console.WriteLine("yes 2: GetSingle");
                    break;
                }
            }
            Console.WriteLine($"{opPart1}:{opMath}:{opPart2}: GetSingle");
            Console.WriteLine($"{opPart1}:{DoMath(opMath, opMath.IndexOf(text[sign]))}:{opPart2}: GetSingle");
            return opPart1 + DoMath(opMath, opMath.IndexOf(text[sign])) + opPart2;
        }

        private string DoMath(string math, int sign)
        {
            Console.WriteLine($"{math}:{sign}: DoMath");
            double math1 = Convert.ToDouble(math.Substring(0, sign));
            double math2 = Convert.ToDouble(math.Substring(sign + 1));
            switch (math[sign])
            {
                case '*':
                    math = (math1 * math2).ToString();
                    break;
                case '/':
                    math = (math1 / math2).ToString();
                    break;
                case '+':
                    math = (math1 + math2).ToString();
                    break;
                case '-':
                    math = (math1 - math2).ToString();
                    break;
                //Not used
                default:
                    break;
            }
            Console.WriteLine($"{math}: DoMath");
            return math;
        }

        private void Off_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            canWrite = true;
            tb.Text = "";
            tbout.Text = "";
            parenMis = 0;
            parenEnd.IsEnabled = parenMis > 0;
        }

        private void R_Click(object sender, RoutedEventArgs e)
        {
            if (canWrite && tb.Text.Length > 0)
            {
                if (tb.Text.EndsWith(")"))
                {
                    parenMis++;
                }
                else if (tb.Text.EndsWith("("))
                {
                    parenMis--;
                }
                parenEnd.IsEnabled = parenMis > 0;
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            }
        }
    }
}