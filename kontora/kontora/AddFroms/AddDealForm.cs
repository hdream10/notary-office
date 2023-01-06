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
    public partial class AddDealForm : Form
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

        public int new_deal_code = -1;

        public AddDealForm()
        {
            InitializeComponent();

            string query = "SELECT Client_Code, Name FROM Customers";
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
                string command = string.Format("INSERT INTO Deal" +
                        " (Client_Code, Descriptions)" +
                        " Values(@Client_Code, @Descriptions)");

                int client_code = columnCode[comboBox1.SelectedIndex];

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Client_Code", client_code);
                    command_group.Parameters.AddWithValue("@Descriptions", textBox2.Text);
                    command_group.ExecuteNonQuery();
                }

                // сразу же узнаем код новой добавленной сделки

                var get_deal_code_command = new SqlCommand(
                    "SELECT TOP 1 Deal_Code FROM Deal WHERE" +
                    " Client_Code = @Client_Code and Descriptions = @Descriptions " +
                    " ORDER BY Deal_Code DESC",
                    Globals.connection);

                get_deal_code_command.Parameters.AddWithValue("@Client_Code", client_code);
                get_deal_code_command.Parameters.AddWithValue("@Descriptions", textBox2.Text);

                using (var dataReader = get_deal_code_command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        new_deal_code = Convert.ToInt32(dataReader["Deal_Code"]);
                    }
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
