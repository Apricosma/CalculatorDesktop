using System.Data;
using System.Drawing.Text;
using System.Linq.Expressions;

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
            storedOperator = pressedButton.Text;
            if (storedResult == "")
            {
                operationBox.Text += resultBox.Text += " " + pressedButton.Text + " ";
            } else
            {
                operationBox.Text = storedResult + " " + storedOperator;
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

            DataTable table = new DataTable();
            var result = table.Compute(expression, "");
            resultBox.Text = result.ToString();
            storedResult = result.ToString();
            operationBox.Text = expression;
        }
    }
}