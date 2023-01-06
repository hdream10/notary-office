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
    public partial class AddDealStructureForm : Form
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

        List<int> columnCode = new List<int>();
        List<String> columnData = new List<String>();

        int deal_code = 0;

        public AddDealStructureForm(int _deal_code)
        {
            InitializeComponent();

            deal_code = _deal_code;

            string query = "SELECT Service_Code, Name FROM Customer_Services";
            using (SqlCommand command = new SqlCommand(query, Globals.connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columnCode.Add(reader.GetInt32(0));
                        columnData.Add(reader.GetString(1));
                    }
                }
            }

            comboBox1.Items.AddRange(columnData.ToArray());

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            success = 1;

            try
            {
                string command = string.Format("INSERT INTO Structure_Deal" +
                        " (Deal_Code, Service_Code, Descriptions)" +
                        " Values(@Deal_Code, @Service_Code, @Descriptions)");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    int service_code = columnCode[comboBox1.SelectedIndex];

                    command_group.Parameters.AddWithValue("@Deal_Code", deal_code);
                    command_group.Parameters.AddWithValue("@Service_Code", service_code);
                    command_group.Parameters.AddWithValue("@Descriptions", textBox2.Text);
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
