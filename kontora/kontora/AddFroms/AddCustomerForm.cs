using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace kontora.AddForms
{
    public partial class AddCustomerForm : Form
    {
        // Включение темного заголовка
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        public int success = 2;
        public string message = "";

        public AddCustomerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 ||
                textBox2.Text.Length == 0 ||
                textBox3.Text.Length == 0 ||
                textBox4.Text.Length == 0) return;

            success = 1;

            try
            {
                string command = string.Format("INSERT INTO Customers" +
                        "(Name, Addres, Phone, Type_of_activity)" +
                        "Values(@Name, @Addres, @Phone, @Type_of_activity)");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Name", textBox1.Text);
                    command_group.Parameters.AddWithValue("@Addres", textBox2.Text);
                    command_group.Parameters.AddWithValue("@Phone", textBox3.Text);
                    command_group.Parameters.AddWithValue("@Type_of_activity", textBox4.Text);
                    command_group.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                success = 0;
                message = ex.Message;
            }
            Close();
        }
    }
}
