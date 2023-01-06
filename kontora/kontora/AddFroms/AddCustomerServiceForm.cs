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
    public partial class AddCustomerServiceForm : Form
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

        public AddCustomerServiceForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) return;

            success = 1;

            try
            {
                string command = string.Format("INSERT INTO Customer_Services" +
                        "(Name, Commission, Discount, Descriptions, Cost)" +
                        "Values(@Name, @Commission, @Discount, @Descriptions, @Cost)");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Name", textBox1.Text);
                    command_group.Parameters.AddWithValue("@Commission", numericUpDown1.Value.ToString());
                    command_group.Parameters.AddWithValue("@Discount", numericUpDown2.Value.ToString());
                    command_group.Parameters.AddWithValue("@Descriptions", textBox2.Text);
                    command_group.Parameters.AddWithValue("@Cost", numericUpDown3.Value.ToString());
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

        private void AddCustomerServiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
