﻿using System;
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
    public partial class EditDealForm : Form
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

        public EditDealForm(DataGridViewRow row)
        {
            InitializeComponent();

            textBox5.Text = row.Cells[0].Value.ToString();

            textBox2.Text = row.Cells[2].Value.ToString();

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

            int client_index = 0;

            for(int i = 0; i < columnData.Count; i++)
            {
                if(columnData[i] == row.Cells[1].Value.ToString())
                {
                    client_index = i;
                    break;
                }
            }

            comboBox1.SelectedIndex = client_index;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            success = 1;

            try
            {
                string command = string.Format(
                    "Update Deal set " +
                        "[Client_Code] = @Client_Code, [Descriptions] = @Descriptions" +
                        " Where [Deal_Code] = @Deal_Code");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    int client_code = columnCode[comboBox1.SelectedIndex];

                    command_group.Parameters.AddWithValue("@Deal_Code", Convert.ToInt32(textBox5.Text));

                    command_group.Parameters.AddWithValue("@Client_code", client_code);

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
