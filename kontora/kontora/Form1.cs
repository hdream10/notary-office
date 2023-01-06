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


namespace kontora
{
    public partial class Form1 : Form
    {
        // Включение темного заголовка
        [DllImport("DwmApi")] //System.Runtime.InteropServices
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        protected override void OnHandleCreated(EventArgs e)
        {
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        // Перечисления, относящиеся к текущей выбранной таблицы
        enum TableType
        {
            Customers = 1,
            Customer_Services = 2,
            Deals = 3
        }
        TableType currentTable = TableType.Customers;

        bool can_read_customers = false;
        bool can_write_customers = false;

        bool can_read_customer_services = false;
        bool can_write_customer_services = false;

        bool can_read_deals = false;
        bool can_write_deals = false;

        public Form1()
        {
            InitializeComponent();

            customers_button.Click += new System.EventHandler(tableSelectButtonsClick);
            customer_services_button.Click += new System.EventHandler(tableSelectButtonsClick);
            deals_button.Click += new System.EventHandler(tableSelectButtonsClick);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Для иконки в трее
            notifyIcon1.Icon = Properties.Resources.app;
            notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);

            ShowLogin();
        }

        // Открытие окна логина
        private void ShowLogin()
        {
            notifyIcon1.Visible = false; // окно логина
            LoginForm.LoginForm loginForm = new LoginForm.LoginForm();
            loginForm.ShowDialog();

            if (loginForm.want_close)
                this.Close();
            else
            {
                this.Visible = true;
                notifyIcon1.Visible = true;

                this.Text = "Управление БД нотариальной конторы - " + loginForm.username + " ";

                ShowNotification("Состояние подключения", "Вход выполнен успешно");

                // Обработка прав
                int current_mode_check = 0;
                foreach (var mode in loginForm.role)
                {
                    switch (current_mode_check)
                    {
                        case 0:
                            if (mode.Contains('n'))
                            {
                                can_read_customers = false;
                                can_write_customers = false;
                            }
                            else
                            {
                                if (mode.Contains('r'))
                                {
                                    can_read_customers = true;
                                    can_write_customers = false;
                                }
                                if (mode.Contains('w'))
                                {
                                    can_read_customers = true;
                                    can_write_customers = true;
                                }
                            }
                            break;
                        case 1:
                            if (mode.Contains('n'))
                            {
                                can_read_customer_services = false;
                                can_write_customer_services = false;
                            }
                            else if (mode.Contains('r'))
                            {
                                can_read_customer_services = true;
                                can_write_customer_services = false;
                            }
                            else if (mode.Contains('w'))
                            {
                                can_read_customer_services = true;
                                can_write_customer_services = true;
                            }
                            break;
                        case 2:
                            if (mode.Contains('n'))
                            {
                                can_read_deals = false;
                                can_write_deals = false;
                            }
                            else if (mode.Contains('r'))
                            {
                                can_read_deals = true;
                                can_write_deals = false;
                            }
                            else if (mode.Contains('w'))
                            {
                                can_read_deals = true;
                                can_write_deals = true;
                            }
                            break;
                        default:
                            break;
                    }

                    current_mode_check++;
                }

                dataGridView_customers.Visible = true;
                dataGridView_customer_services.Visible = true;
                tableLayoutPanel_deals.Visible = true;

                dataGridView_customers.Columns.Clear();
                dataGridView_customer_services.Columns.Clear();
                dataGridView_deals.Columns.Clear();

                customers_button.Hide();
                customer_services_button.Hide();
                deals_button.Hide();

                addRowButton.Hide();

                if (!can_read_customers)
                {
                    dataGridView_customers.Hide();
                }
                else
                {
                    dataGridView_customer_services.Hide();
                    tableLayoutPanel_deals.Hide();
                    SelectFromCustomers();
                    customers_button.Show();
                }

                if (!can_read_customer_services)
                {
                    dataGridView_customer_services.Hide();
                }
                else
                {
                    SelectFromCustomerServices();
                    tableLayoutPanel_deals.Hide();
                    customer_services_button.Show();
                }

                if (!can_read_deals)
                {
                    tableLayoutPanel_deals.Hide();
                }
                else
                {
                    SelectFromDeals();
                    deals_button.Show();
                }

                if (can_write_customers || can_write_customer_services || can_write_deals)
                {
                    addRowButton.Show();
                }

            }
        }

        // Функции, относящиеся к иконке в трее
        // Показ уведомления
        private void ShowNotification(string title, string text)
        {
            notifyIcon1.ShowBalloonTip(1000, title, text, ToolTipIcon.Info);
        }
        // Показ ошибки
        private void ShowError(string title, string text)
        {
            notifyIcon1.ShowBalloonTip(1000, title, text, ToolTipIcon.Error);
        }
        // Двойное нажатие на иконку в трее
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
        // Нажатие в трее в меню на кнопку Закрыть
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }
        // Нажатие на иконку приложения в трее
        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                case MouseButtons.Right:
                    notifyIcon1.ContextMenuStrip.Show();
                    break;
                case MouseButtons.None:
                default:
                    break;
            }
        }


        // Функция закрытия соединения и выхода из приложения
        private void ExitApp()
        {
            if (Globals.connection != null && Globals.connection.State == ConnectionState.Open)
            {
                Globals.connection.Close();
            }
            this.Close();
        }


        // Кнопка отключения и открытия окна логина 
        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (Globals.connection != null && Globals.connection.State == ConnectionState.Open)
            {
                Globals.connection.Close();
            }
            ShowLogin();
        }


        // Выход из приложения
        private void button2_Click(object sender, EventArgs e)
        {
            ExitApp();
        }


        // Обработчик нажатия кнопки добавления новой записи
        private void addRowButton_Click(object sender, EventArgs e)
        {
            bool result;
            switch (currentTable)
            {
                case TableType.Customers:
                    result = AddCustomer();
                    if (result)
                        SelectFromCustomers();
                    break;
                case TableType.Customer_Services:
                    result = AddCustomerService();
                    if (result)
                        SelectFromCustomerServices();
                    break;
                case TableType.Deals:
                    result = AddDeal();
                    if (result)
                        SelectFromDeals();
                    break;
            }
        }


        // Обработчик нажатия кнопок выбора таблиц
        private void tableSelectButtonsClick(object sender, EventArgs e)
        {
            dataGridView_customers.Visible = false;
            dataGridView_customer_services.Visible = false;
            tableLayoutPanel_deals.Visible = false;

            customers_button.BackColor = Color.Black;
            customer_services_button.BackColor = Color.Black;
            deals_button.BackColor = Color.Black;

            if (sender == customers_button)
            {
                if (can_read_customers) {
                    dataGridView_customers.Visible = true;
                    currentTable = TableType.Customers;
                    customers_button.BackColor = SystemColors.Highlight;
                }
                if (can_write_customers)
                    addRowButton.Show();
                else
                    addRowButton.Hide();
            }

            if (sender == customer_services_button)
            {
                if (can_read_customer_services)
                {
                    dataGridView_customer_services.Visible = true;
                    currentTable = TableType.Customer_Services;
                    customer_services_button.BackColor = SystemColors.Highlight;
                }
                if (can_write_customer_services)
                    addRowButton.Show();
                else
                    addRowButton.Hide();
            }
            if (sender == deals_button)
            {
                if (can_read_deals)
                {
                    tableLayoutPanel_deals.Visible = true;
                    currentTable = TableType.Deals;
                    deals_button.BackColor = SystemColors.Highlight;
                }
                if (can_write_deals)
                    addRowButton.Show();
                else
                    addRowButton.Hide();
            }
        }

        /// <summary>
        /// Отправляет запрос, и ответ превращает в таблицу в виде DataTable
        /// </summary>
        /// <param name="query">Строка запроса</param>
        /// <returns>Таблица в виде DataTable</returns>
        public DataTable GetTableFromQuery(string query)
        {
            DataTable result = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(@"set dateformat dmy " + query, Globals.connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }

        // ------------------------------------------------------------------------

        // Просмотр, добавление, изменение, удаление данных из таблиц

        // Таблица Customers
        private void SelectFromCustomers()
        {
            dataGridView_customers.Columns.Clear();
            dataGridView_customers.DataSource =
                GetTableFromQuery("SELECT Client_Code, Name, Addres, Phone, Type_of_activity FROM Customers");

            dataGridView_customers.Columns[0].HeaderText = "Код";
            dataGridView_customers.Columns[1].HeaderText = "Название";
            dataGridView_customers.Columns[2].HeaderText = "Адрес";
            dataGridView_customers.Columns[3].HeaderText = "Телефон";
            dataGridView_customers.Columns[4].HeaderText = "Вид деятельности";
        }
        // Добавление
        private bool AddCustomer()
        {
            AddForms.AddCustomerForm addForm = new AddForms.AddCustomerForm();

            addForm.ShowDialog();

            if (addForm.success == 1)
            {
                ShowNotification("Таблица Клиенты", "Запись добавлена успешно");
                return true;
            }
            else if (addForm.success == 0)
            {
                ShowError("Таблица Клиенты", "Ошибка добавления: " + addForm.message);
                return false;
            }
            return true;
        }
        // Изменение
        private bool EditCustomer(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();

            EditForms.EditCustomerForm editForm = new EditForms.EditCustomerForm(row);

            editForm.ShowDialog();

            if (editForm.success == 1)
            {
                ShowNotification("Таблица Клиенты", "Запись " + code + " изменена успешно");
                return true;
            }
            else if (editForm.success == 0)
            {
                ShowError("Таблица Клиенты", "Ошибка изменения: " + editForm.message);
                return false;
            }
            return true;
        }
        // Удаление
        private bool DeleteCustomer(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();
            string name = row.Cells[1].Value.ToString();
            string address = row.Cells[2].Value.ToString();
            string phone = row.Cells[3].Value.ToString();
            string activity = row.Cells[4].Value.ToString();

            try
            {
                string sql = string.Format("DELETE FROM Customers WHERE[Client_code] = @Client_code;");

                using (SqlCommand command_group = new SqlCommand(sql, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Client_code", code);
                    command_group.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ShowError("Таблица Клиенты", "Невозможно удалить запись с кодом " +
                    code + ". " + ex.Message);
                return false;
            }

            ShowNotification("Таблица Клиенты", "Запись успешно удалена:\n" +
                code + ", " +
                name + ", " +
                address + ", " +
                phone + ", " +
                activity);
            return true;
        }
        // Удаление выделенной строки, нажатием кнопки Delete
        private void dataGridView_customers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;

            if (!can_write_customers) return;

            int index = (int)e.Row.Index;

            bool result = DeleteCustomer(dataGridView_customers.Rows[index]);

            SelectFromCustomers();
        }
        // Открытие контексного меню в строке таблицы для удаления или изменения
        private void dataGridView_customers_MouseClick(object sender, MouseEventArgs e)
        {
            if (!can_write_customers) return;
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();

                int currentMouseOverRow = dataGridView_customers.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dataGridView_customers.ClearSelection();

                    dataGridView_customers.Rows[currentMouseOverRow].Selected = true;

                    m.MenuItems.Add(new MenuItem("Изменить"));
                    m.MenuItems.Add(new MenuItem("Удалить"));

                    // Изменение строки
                    m.MenuItems[0].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = EditCustomer(dataGridView_customers.Rows[currentMouseOverRow]);
                        if (result)
                            SelectFromCustomers();
                    });

                    // Удаление строки
                    m.MenuItems[1].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = DeleteCustomer(dataGridView_customers.Rows[currentMouseOverRow]);
                        if (result)
                            SelectFromCustomers();
                    });

                    m.Show(dataGridView_customers, new Point(e.X, e.Y));
                }
            }
        }


        // Таблица Customer_Services
        private void SelectFromCustomerServices()
        {
            dataGridView_customer_services.Columns.Clear();
            dataGridView_customer_services.DataSource =
                GetTableFromQuery("SELECT Service_Code, Name, Commission, Discount, Descriptions, Cost FROM Customer_Services");

            dataGridView_customer_services.Columns[0].HeaderText = "Код";
            dataGridView_customer_services.Columns[1].HeaderText = "Название";
            dataGridView_customer_services.Columns[2].HeaderText = "Комиссия (руб.)";
            dataGridView_customer_services.Columns[3].HeaderText = "Скидка (%)";
            dataGridView_customer_services.Columns[4].HeaderText = "Комментарий";
            dataGridView_customer_services.Columns[5].HeaderText = "Цена (руб.)";
        }
        // Добавление
        private bool AddCustomerService()
        {
            AddForms.AddCustomerServiceForm addForm = new AddForms.AddCustomerServiceForm();

            addForm.ShowDialog();

            if (addForm.success == 1)
            {
                ShowNotification("Таблица Услуги", "Запись добавлена успешно");
                return true;
            }
            else if (addForm.success == 0)
            {
                ShowError("Таблица Услуги", "Ошибка добавления: " + addForm.message);
                return false;
            }
            return true;
        }
        // Изменение
        private bool EditCustomerService(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();

            EditForms.EditCustomerServiceForm editForm = new EditForms.EditCustomerServiceForm(row);

            editForm.ShowDialog();

            if (editForm.success == 1)
            {
                ShowNotification("Таблица Услуги", "Запись " + code + " изменена успешно");
                return true;
            }
            else if (editForm.success == 0)
            {
                ShowError("Таблица Услуги", "Ошибка изменения: " + editForm.message);
                return false;
            }
            return true;
        }
        // Удаление
        private bool DeleteCustomerService(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();
            string name = row.Cells[1].Value.ToString();
            string commission = row.Cells[2].Value.ToString();
            string discount = row.Cells[3].Value.ToString();
            string descriptions = row.Cells[4].Value.ToString();
            string cost = row.Cells[5].Value.ToString();

            try
            {
                string sql = string.Format("DELETE FROM Customer_Services WHERE[Service_Code] = @Service_Code;");

                using (SqlCommand command_group = new SqlCommand(sql, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Service_Code", code);
                    command_group.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ShowError("Таблица Услуги", "Невозможно удалить запись с кодом " +
                    code + ". " + ex.Message);
                return false;
            }

            ShowNotification("Таблица Услуги", "Запись успешно удалена:\n" +
                code + ", " +
                name + ", " +
                commission + ", " +
                discount + ", " +
                descriptions + ", " +
                cost);
            return true;
        }
        // Удаление выделенной строки, нажатием кнопки Delete
        private void dataGridView_customer_services_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;

            if (!can_write_customer_services) return;

            int index = (int)e.Row.Index;

            bool result = DeleteCustomerService(dataGridView_customer_services.Rows[index]);

            SelectFromCustomerServices();
        }
        // Открытие контексного меню в строке таблицы для удаления или изменения
        private void dataGridView_customer_services_MouseClick(object sender, MouseEventArgs e)
        {
            if (!can_write_customer_services) return;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();

                int currentMouseOverRow = dataGridView_customer_services.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dataGridView_customer_services.ClearSelection();

                    dataGridView_customer_services.Rows[currentMouseOverRow].Selected = true;

                    m.MenuItems.Add(new MenuItem("Изменить"));
                    m.MenuItems.Add(new MenuItem("Удалить"));

                    // Изменение строки
                    m.MenuItems[0].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = EditCustomerService(dataGridView_customer_services.Rows[currentMouseOverRow]);
                        if (result)
                            SelectFromCustomerServices();
                    });

                    // Удаление строки
                    m.MenuItems[1].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = DeleteCustomerService(dataGridView_customer_services.Rows[currentMouseOverRow]);
                        if (result)
                            SelectFromCustomerServices();
                    });

                    m.Show(dataGridView_customer_services, new Point(e.X, e.Y));
                }
            }
        }

        // Таблица Deals
        private void SelectFromDeals()
        {
            dataGridView_deals.Columns.Clear();

            dataGridView_deals.DataSource = GetTableFromQuery(
                "SELECT Deal.Deal_Code,Customers.Name,Deal.Descriptions " +
                ", SUM(Customer_Services.Cost) " +
                ", SUM(Customer_Services.Cost+Customer_Services.Commission) " +
                ", SUM((Customer_Services.Cost+Customer_Services.Commission) - " + 
                " ((Customer_Services.Cost+Customer_Services.Commission)*Customer_Services.Discount*0.01)) " +
                " FROM Structure_Deal, Customer_Services, Deal, Customers " +
                " WHERE Structure_Deal.Service_Code = Customer_Services.Service_Code " +
                " and Deal.Deal_Code = Structure_Deal.Deal_Code and Deal.Client_Code = Customers.Client_Code " +
                " GROUP BY Deal.Deal_Code, Structure_Deal.Deal_Code, Deal.Descriptions, Customers.Name");

            dataGridView_deals.Columns[0].HeaderText = "Код";
            dataGridView_deals.Columns[1].HeaderText = "Клиент";
            dataGridView_deals.Columns[2].HeaderText = "Комментарий";
            dataGridView_deals.Columns[3].HeaderText = "Сумма";
            dataGridView_deals.Columns[4].HeaderText = "Потытог с учетом комиссий";
            dataGridView_deals.Columns[5].HeaderText = "Итого со скидками";

        }
        // Добавление
        private bool AddDeal()
        {
            AddForms.AddDealForm addForm = new AddForms.AddDealForm();

            addForm.ShowDialog();

            if (addForm.success == 1)
            {
                ShowNotification("Таблица Сделки", "Запись добавлена успешно");
                AddDealStructure(addForm.new_deal_code);
                return true;
            }
            else if (addForm.success == 0)
            {
                ShowError("Таблица Сделки", "Ошибка добавления: " + addForm.message);
                return false;
            }
            return true;
        }
        // Изменение
        private bool EditDeal(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();

            EditForms.EditDealForm editForm = new EditForms.EditDealForm(row);

            editForm.ShowDialog();

            if (editForm.success == 1)
            {
                ShowNotification("Таблица Услуги", "Запись " + code + " изменена успешно");
                return true;
            }
            else if (editForm.success == 0)
            {
                ShowError("Таблица Услуги", "Ошибка изменения: " + editForm.message);
                return false;
            }
            return true;
        }
        // Удаление сделки, но сначала удаление содержимого сделки
        // Удаление
        private bool DeleteDeal(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();
            string client_name = row.Cells[1].Value.ToString();
            string descriptions = row.Cells[2].Value.ToString();

            try
            {
                string sql_structure_deal = string.Format("DELETE FROM Structure_Deal WHERE[Deal_Code] = @Deal_Code;");

                using (SqlCommand command_group = new SqlCommand(sql_structure_deal, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Deal_Code", code);
                    command_group.ExecuteNonQuery();
                }

                string sql_deal = string.Format("DELETE FROM Deal WHERE[Deal_Code] = @Deal_Code;");

                using (SqlCommand command_group = new SqlCommand(sql_deal, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Deal_Code", code);
                    command_group.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                ShowError("Таблица Сделки", "Невозможно удалить запись с кодом " +
                    code + ". " + ex.Message);
                return false;
            }

            ShowNotification("Таблица Сделки", "Запись успешно удалена:\n" +
                code + ", " +
                client_name + ", " +
                descriptions);
            return true;
        }
        // Нажатие на сделку и отображение содержимого сделки
        private void dataGridView_deals_MouseClick(object sender, MouseEventArgs e)
        {
            int currentMouseOverRow = dataGridView_deals.HitTest(e.X, e.Y).RowIndex;

            if (currentMouseOverRow >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (!can_write_customer_services) return;
                    dataGridView_deals.ClearSelection();

                    dataGridView_deals.Rows[currentMouseOverRow].Selected = true;

                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(new MenuItem("Добавить содержимое сделки"));
                    m.MenuItems.Add(new MenuItem("Изменить"));
                    m.MenuItems.Add(new MenuItem("Удалить"));

                    // Добавление содержимого в сделку
                    m.MenuItems[0].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        int deal_code = Convert.ToInt32(dataGridView_deals.Rows[currentMouseOverRow].Cells[0].Value);
                        AddForms.AddDealStructureForm addForm = new AddForms.AddDealStructureForm(deal_code);

                        addForm.ShowDialog();

                        if (addForm.success == 1)
                        {
                            SelectFromDealStructure(dataGridView_deals.Rows[currentMouseOverRow].Cells[0].Value.ToString());
                            SelectFromDeals();

                            ShowNotification("Таблица Структура Сделки", "Запись в сделку " + deal_code + " добавлена успешно");
                        }
                        else if (addForm.success == 0)
                        {
                            ShowError("Таблица Услуги", "Ошибка добавления услуги в сделку: " + addForm.message);
                        }
                    });

                    // Изменение строки
                    m.MenuItems[1].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = EditDeal(dataGridView_deals.Rows[currentMouseOverRow]);
                        if (result)
                            SelectFromDeals();
                    });

                    // Удаление строки
                    m.MenuItems[2].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = DeleteDeal(dataGridView_deals.Rows[currentMouseOverRow]);
                        if (result)
                        {
                            SelectFromDeals();
                            if(dataGridView_deals.RowCount > 0)
                                SelectFromDealStructure(dataGridView_deals.Rows[0].Cells[0].Value.ToString());
                        }
                    });

                    m.Show(dataGridView_deals, new Point(e.X, e.Y));
                }
                else if (e.Button == MouseButtons.Left)
                {
                    string deal_code_string = dataGridView_deals.Rows[currentMouseOverRow].Cells[0].Value.ToString();

                    SelectFromDealStructure(deal_code_string);
                }
            }
        }


        // Таблица Structure_Deal
        private void SelectFromDealStructure(string deal_code)
        {
            dataGridView_structure_deal.DataSource = GetTableFromQuery(
                "SELECT Structure_Deal.Structure_Deal_Code" +
                ", Customer_Services.Name as 'Service Name'" +
                ", Customer_Services.Commission as 'Service Commission'" +
                ", Customer_Services.Discount as 'Service Discount'" +
                ", Structure_Deal.Descriptions" +
                ", Customer_Services.Cost as 'Service Cost'" +
                " FROM Deal, Customers, Structure_Deal, Customer_Services" +
                " WHERE Deal.Client_Code = Customers.Client_Code" +
                " and Deal.Deal_Code = Structure_Deal.Deal_Code" +
                " and Structure_Deal.Service_Code = Customer_Services.Service_Code" +
                " and Deal.Deal_Code = " + deal_code +
                " ORDER BY Deal.Deal_Code");


            dataGridView_structure_deal.Columns[0].Visible = false;
            dataGridView_structure_deal.Columns[1].HeaderText = "Услуга";
            dataGridView_structure_deal.Columns[2].HeaderText = "Комиссия (руб.)";
            dataGridView_structure_deal.Columns[3].HeaderText = "Скидка (%)";
            dataGridView_structure_deal.Columns[4].HeaderText = "Описание";
            dataGridView_structure_deal.Columns[5].HeaderText = "Цена (руб.)";
        }
        // Добавление
        private bool AddDealStructure(int deal_code)
        {
            AddForms.AddDealStructureForm addForm = new AddForms.AddDealStructureForm(deal_code);

            addForm.ShowDialog();

            if (addForm.success == 1)
            {
                ShowNotification("Таблица Структура Сделки", "Запись добавлена успешно");

                return true;
            }
            else if (addForm.success == 0)
            {
                ShowError("Таблица Структура Сделки", "Ошибка добавления: " + addForm.message);
                return false;
            }
            return true;
        }
        // Изменение
        private bool EditDealStructure(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();

            EditForms.EditDealStructureForm editForm = new EditForms.EditDealStructureForm(row);

            editForm.ShowDialog();

            if (editForm.success == 1)
            {
                ShowNotification("Таблица Структура Сделки", "Запись " + code + " изменена успешно");
                return true;
            }
            else if (editForm.success == 0)
            {
                ShowError("Таблица Структура Сделки", "Ошибка изменения: " + editForm.message);
                return false;
            }
            return true;
        }
        // Удаление
        private bool DeleteDealStructure(DataGridViewRow row)
        {
            string code = row.Cells[0].Value.ToString();

            try
            {
                string command = string.Format("DELETE FROM Structure_Deal WHERE[Structure_Deal_Code] = @Structure_Deal_Code;");

                using (SqlCommand command_group = new SqlCommand(command, Globals.connection))
                {
                    command_group.Parameters.AddWithValue("@Structure_Deal_Code", code);
                    command_group.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                ShowError("Таблица Структура Сделки", "Невозможно удалить запись с кодом " +
                    code + ". " + ex.Message);
                return false;
            }

            ShowNotification("Таблица Структура Сделки", "Запись успешно удалена:\n" +
                code);
            return true;
        }
        // Контексное меню на строке содержимого сделки
        private void dataGridView_structure_deal_MouseClick(object sender, MouseEventArgs e)
        {
            if (!can_write_deals) return;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();

                int currentMouseOverRow = dataGridView_structure_deal.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dataGridView_structure_deal.ClearSelection();

                    dataGridView_structure_deal.Rows[currentMouseOverRow].Selected = true;

                    m.MenuItems.Add(new MenuItem("Изменить"));
                    m.MenuItems.Add(new MenuItem("Удалить"));

                    // Изменение строки
                    m.MenuItems[0].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = EditDealStructure(dataGridView_structure_deal.Rows[currentMouseOverRow]);
                        if (result)
                        {
                            SelectFromDeals();
                            if (dataGridView_deals.RowCount > 0)
                                SelectFromDealStructure(dataGridView_deals.Rows[0].Cells[0].Value.ToString());
                        }
                    });

                    // Удаление строки
                    m.MenuItems[1].Click += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        bool result = DeleteDealStructure(dataGridView_structure_deal.Rows[currentMouseOverRow]);
                        if (result)
                        {
                            SelectFromDeals();
                            if (dataGridView_deals.RowCount > 0)
                                SelectFromDealStructure(dataGridView_deals.Rows[0].Cells[0].Value.ToString());
                        }
                    });

                    m.Show(dataGridView_structure_deal, new Point(e.X, e.Y));
                }
            }
        }

        private void dataGridView_deals_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView_structure_deal_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
