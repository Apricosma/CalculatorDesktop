using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;

namespace CalculatorDesktop
{
    public partial class Form1 : Form
    {
        private bool operatorPressed;
        private string storedOperator = "";
        private string storedOperand = "";
        private string storedResult = "";

        public Form1()
        {
            InitializeComponent();

            // Panel styling
            this.Text = "Calculator";
            this.FormBorderStyle = FormBorderStyle.None;
            
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 34;
            topPanel.BackColor = Color.Black;
            this.Controls.Add(topPanel);

            // Windows API dragging
            [DllImport("user32.dll")]
            static extern bool ReleaseCapture();

            [DllImport("user32.dll")]
            static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

            const int WM_NCLBUTTONDOWN = 0x00A1;
            const int HT_CAPTION = 0x02;

            topPanel.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonEqual;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            operatorPressed = false;
            storedOperator = "";
            storedOperand = "";
            storedResult = "";
            resultBox.Text = "";
            operationBox.Text = "";
        }

        private void buttonBin_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(resultBox.Text, out int binaryNumber) || binaryNumber < 0)
            {
                resultBox.Text = "Error: Incorrect input";
                return;
            }

            string resultBinary = "";

            // limiting output to 16 bit since I do not want to change the display
            // to handle multi-line
            if (binaryNumber > 65536)
            {
                resultBox.Text = "Error: Num too large";
                return;
            }

            if (binaryNumber == 0)
            {
                resultBinary = "0";
            } else
            {
                while (binaryNumber > 0)
                {
                    int remainder = binaryNumber % 2;
                    resultBinary = remainder.ToString() + resultBinary;
                    binaryNumber /= 2;
                }
            }

            resultBox.Text = resultBinary;
        }

        private void buttonDEC_Click(object sender, EventArgs e)
        {
            string bin = resultBox.Text;
            int decimalValue = 0;
            int baseValue = 1;

            // See if the input is valid input by checking if there is anything other than 1 or 0
            if (!bin.All(b => b == '0' || b == '1'))
            {
                resultBox.Text = "Error: Not Binary";
                return;
            }

            for (int i = bin.Length - 1; i >= 0; i--)
            {
                if (bin[i] == '1')
                {
                    decimalValue += baseValue;
                }

                baseValue *= 2;
            }

            resultBox.Text = decimalValue.ToString();
        }

        private void digitInput(object sender, EventArgs e)
        {
            if (operatorPressed)
            {
                resultBox.Text = "";
            }

            Button pressedButton = (Button)sender;
            resultBox.Text += pressedButton.Text;
            

            if (storedOperand == "")
            {
                storedOperand = pressedButton.Text;
            } else if (operatorPressed)
            {
                storedOperand = pressedButton.Text;
            } else
            {
                storedOperand += pressedButton.Text;
            }

            operatorPressed = false;
        }

        private void operandButton_Click(object sender, EventArgs e)
        {

            Button pressedButton = (Button)sender;
            string newOperator = pressedButton.Text;

            // replacing stored operator with new one when an operator is pressed after another
            if (operatorPressed && storedOperator != "" && newOperator != "=")
            {
                operationBox.Text = operationBox.Text.Remove(operationBox.Text.Length - 2, 2) + newOperator + " ";
                storedOperator = newOperator;
            }
            else
            {
                storedOperator = newOperator;
                if (storedResult == "")
                {
                    operationBox.Text += resultBox.Text + " " + newOperator + " ";
                }
                else
                {
                    resultBox.Text = storedResult.ToString();
                    operationBox.Text = storedResult + " " + storedOperator + " ";
                    storedResult = "";
                }
            }

            operatorPressed = true;
        }

        private void equalsButton_Click(object sender, EventArgs e)
        {
            string expression;
            if (storedResult == "")
            {
                expression = operationBox.Text += storedOperand;

            } else
            {
                expression = storedResult + " " + storedOperator + " " + storedOperand;
            }

            try
            {
                DataTable table = new DataTable();
                var result = table.Compute(expression, "");
                resultBox.Text = result.ToString();
                storedResult = result.ToString();
                operationBox.Text = expression;
            } catch(Exception)
            {
                resultBox.Text = "ERROR: Num Too Large";
            }

            
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch(e.KeyChar.ToString())
            {
                case "1":
                    buttonOne.PerformClick();
                    break;
                case "2":
                    buttonTwo.PerformClick();
                    break;
                case "3":
                    buttonThree.PerformClick();
                    break;
                case "4":
                    buttonFour.PerformClick();
                    break;
                case "5":
                    buttonFive.PerformClick();
                    break;
                case "6":
                    buttonSix.PerformClick();
                    break;
                case "7":
                    buttonSeven.PerformClick();
                    break;
                case "8":
                    buttonEight.PerformClick();
                    break;
                case "9":
                    buttonNine.PerformClick();
                    break;
                case "0":
                    buttonZero.PerformClick();
                    break;
                case "-":
                    buttonSubtract.PerformClick();
                    break;
                case "+":
                    buttonAdd.PerformClick();
                    break;
                case "*":
                    buttonMultiply.PerformClick();
                    break;
                case "/":
                    buttonDivide.PerformClick();
                    break;
                default:
                    break;
            }

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                buttonEqual.PerformClick();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEqual.PerformClick();
            }

            if (e.KeyCode == Keys.Delete)
            {
                buttonClear.PerformClick();
            }
        }

        private void buttonLOC_Click(object sender, EventArgs e)
        {

        }
    }
}