using System;
using System.Windows.Forms;

namespace DLLTest
{
    public class Class1
    {
        public static void ShowMessage(string message, string title = "Message")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void PrintHelloWorld()
        {
            Console.WriteLine("Hello, World!");
        }

        public static int AddTwoValues(int val1, int val2)
        {
            return val1 + val2;
        }
    }
}