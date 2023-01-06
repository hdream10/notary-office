using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace kontora.LoginForm
{
    public partial class LoginForm : Form
    {
        // Включение темного заголовка
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        public bool want_close = true;
        public string username = "";

        public List<string> role;

        public LoginForm()
        {
            InitializeComponent();

            pictureBox1.Image = resizeImage(Properties.Resources.image_1, new Size(200, 200));
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        private void login()
        {
            string address = input_address.Text;
            string login = input_login.Text;
            string password = input_password.Text;

            const string db_name = "CursBD";
            const string instance = "SQLEXPRESS";

            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
            stringBuilder.IntegratedSecurity = false;
            stringBuilder.InitialCatalog = db_name;
            stringBuilder.DataSource = @"" + address + "\\" + instance;
            stringBuilder.UserID = "admin";
            stringBuilder.Password = "adminPSWD";

            Globals.connection = new SqlConnection(stringBuilder.ToString());

            bool success = true;

            role = null;

            try
            {
                Globals.connection.Open();

                var get_login_command = new SqlCommand(
                    "SELECT Role FROM Roles WHERE Roles.Role_Code IN " +
                    "( SELECT Role_Code FROM Logins WHERE Login = @login and Password = @password );",
                    Globals.connection);

                get_login_command.Parameters.AddWithValue("@login", login);
                get_login_command.Parameters.AddWithValue("@password", password);

                using (var dataReader = get_login_command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        role = dataReader["Role"].ToString().Split(',').ToList();
                    }
                }
            }
            catch (Exception err)
            {
                success = false;
                MessageBox.Show("ОШИБКА!\r\n" + err.Message.ToString());
            }

            if(role == null || role.Count == 0)
            {
                success = false;
                MessageBox.Show("ОШИБКА!\r\n" + "Неверные данные для входа");
            }

            if (success)
            {
                want_close = false;
                username = login;

                this.Close();
            }
        }
        private void button_login_Click(object sender, EventArgs e)
        {
            login();
        }

        private void input_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void input_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
