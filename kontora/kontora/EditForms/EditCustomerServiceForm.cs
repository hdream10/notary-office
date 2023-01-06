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

namespace kontora.EditForms
{
    public partial class EditCustomerServiceForm : Form
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

        public EditCustomerServiceForm(DataGridViewRow row)
        {
            InitializeComponent();

            textBox5.Text = row.Cells[0].Value.ToString();

            textBox1.Text = row.Cells[1].Value.ToString();
            numericUpDown1.Value = Convert.ToDecimal(row.Cells[3].Value);
            numericUpDown2.Value = Convert.ToDecimal(row.Cells[3].Value);
            textBox2.Text = row.Cells[4].Value.ToString();
            numericUpDown3.Value = Convert.ToDecimal(row.Cells[5].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) return;

            success = 1;

            try
            {
                string command = string.Format(
                    "Update Customer_Services set " +
                        "[Name] = @Name, [Commission] = @Commission, " +
                        "[Discount] = @Discount, [Descriptions] = @Descriptions, [Cost] = @Cost" +
                        " Where [Service_Code] = @Service_Code");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Service_Code", textBox5.Text);

                    command_group.Parameters.AddWithValue("@Name", textBox1.Text);
                    command_group.Parameters.AddWithValue("@Commission", numericUpDown1.Value);
                    command_group.Parameters.AddWithValue("@Discount", numericUpDown2.Value);
                    command_group.Parameters.AddWithValue("@Descriptions", textBox2.Text);
                    command_group.Parameters.AddWithValue("@Cost", numericUpDown3.Value);
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
