using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_TermPaper_FinancialAccounting
{
    /// <summary>
    /// Interaction logic for RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text.Trim();
            string login = tbLogin.Text.Trim();
            string email = tbEmail.Text.Trim();
            string pass1 = passbox1.Password.Trim();
            string pass2 = passbox2.Password.Trim();
            bool flag = true;

            if (name.Length < 3)
            {
                editTextBox(tbName, "Мінімальна довжина ім'я 3 символи", true);
                flag = false;
            }
            else
            {
                editTextBox(tbName);
            }
                

            if (login.Length < 5)
            {
                editTextBox(tbLogin, "Мінімальна довжина логіну 5 символів", true);
                flag = false;
            }
            else
            {
                if (checkLogin(login))
                {
                    editTextBox(tbLogin, "Такий логін вже існує", true);
                    flag = false;
                }
                else
                    editTextBox(tbLogin);
            }

            if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                editTextBox(tbEmail, "Некоректне значення", true);
                flag = false;
            }
            else
            {
                editTextBox(tbEmail);
            }

            if (pass1.Length < 5)
            {
                editPassBox(passbox1, "Мінімальна довжина паролю 5 символів", true);
                flag = false;
            }
            else
            {
                editPassBox(passbox1);
                if (pass1 != pass2)
                {
                    editPassBox(passbox2, "Паролі не збігаються", true);
                    flag = false;
                }
                else
                {
                    editPassBox(passbox2);
                }
            }

            if (flag)
            {
                DB db = new DB();

                string str_command = "INSERT INTO `users` (`name`, `login`, `email`, `password`) VALUES (@name, @login, @email, @pass)";
                ArrayList list_str = new ArrayList() { "@name", "@login", "@email", "@pass" };
                ArrayList list_var = new ArrayList() { name, login, email, pass1 };

                bool flag2 = db.EditTable(str_command, list_str, list_var);

                if (flag2)
                {
                    MessageBox.Show("Успіх!");
                }
                else
                {
                    MessageBox.Show("Щось пішло не так!");
                }
            }

        }

        private void editTextBox(TextBox textbox, string str = "", bool mark = false)
        {
            if (mark)
            {
                textbox.ToolTip = str;
                textbox.Background = Brushes.MistyRose;
            }
            else
            {
                textbox.ToolTip = default;
                textbox.Background = Brushes.Transparent;
            }
            
        }
        private void editPassBox(PasswordBox passbox, string str="", bool mark=false)
        {
            if (mark)
            {
                passbox.ToolTip = str;
                passbox.Background = Brushes.MistyRose;
            }
            else
            {
                passbox.ToolTip = default;
                passbox.Background = Brushes.Transparent;
            }
            
        }

        private bool checkLogin(string login)
        {
            DB db = new DB();

            string str_command = "SELECT * FROM `users` WHERE `login` = @login AND `is_delete` = 0";

            ArrayList list_str = new ArrayList() { "@login" };
            ArrayList list_var = new ArrayList() { login };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }
    }
}
