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
            this.Text = "Calculator";
            this.FormBorderStyle = FormBorderStyle.None;

            // Dragging and panel style
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 34;
            topPanel.BackColor = Color.Black;
            this.Controls.Add(topPanel);

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

        }

        private void buttonDEC_Click(object sender, EventArgs e)
        {

        }

        private void operandLabel_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

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
            } catch(Exception ex)
            {
                resultBox.Text = "ERROR: Num too large";
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
    }
}