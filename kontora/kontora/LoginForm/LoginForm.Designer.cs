
namespace kontora.LoginForm
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.input_login = new System.Windows.Forms.TextBox();
            this.input_password = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_login = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.input_address = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(300, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль:";
            // 
            // input_login
            // 
            this.input_login.BackColor = System.Drawing.Color.Black;
            this.input_login.ForeColor = System.Drawing.Color.White;
            this.input_login.Location = new System.Drawing.Point(363, 47);
            this.input_login.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_login.Name = "input_login";
            this.input_login.Size = new System.Drawing.Size(171, 22);
            this.input_login.TabIndex = 2;
            this.input_login.Text = "admin";
            this.input_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_login_KeyDown);
            // 
            // input_password
            // 
            this.input_password.BackColor = System.Drawing.Color.Black;
            this.input_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.input_password.ForeColor = System.Drawing.Color.White;
            this.input_password.Location = new System.Drawing.Point(363, 79);
            this.input_password.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_password.Name = "input_password";
            this.input_password.Size = new System.Drawing.Size(171, 23);
            this.input_password.TabIndex = 3;
            this.input_password.Text = "12345";
            this.input_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_password_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(16, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(267, 246);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // button_login
            // 
            this.button_login.BackColor = System.Drawing.Color.Black;
            this.button_login.FlatAppearance.BorderSize = 2;
            this.button_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_login.ForeColor = System.Drawing.Color.White;
            this.button_login.Location = new System.Drawing.Point(295, 111);
            this.button_login.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(240, 36);
            this.button_login.TabIndex = 5;
            this.button_login.Text = "Вход";
            this.button_login.UseVisualStyleBackColor = false;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 246);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "2022 (c) Гарашкин Владислав";
            // 
            // input_address
            // 
            this.input_address.BackColor = System.Drawing.Color.Black;
            this.input_address.ForeColor = System.Drawing.Color.White;
            this.input_address.Location = new System.Drawing.Point(363, 15);
            this.input_address.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_address.Name = "input_address";
            this.input_address.Size = new System.Drawing.Size(171, 22);
            this.input_address.TabIndex = 9;
            this.input_address.Text = "localhost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Адрес:";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(544, 273);
            this.Controls.Add(this.input_address);
            this.Controls.Add(this.input_password);
            this.Controls.Add(this.input_login);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Введите логин и пароль";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox input_login;
        private System.Windows.Forms.TextBox input_password;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox input_address;
        private System.Windows.Forms.Label label5;
    }
}