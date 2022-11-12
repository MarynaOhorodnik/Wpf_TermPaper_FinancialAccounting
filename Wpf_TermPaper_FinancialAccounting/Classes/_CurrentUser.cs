using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_TermPaper_FinancialAccounting.Classes
{
    public static class _CurrentUser
    {
        private static string id;
        private static string name;
        private static string login;
        private static string email;
        private static string pass;

        public static string Id
        {
            get { return id; }
            set { id = value; }
        }

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static string Login
        {
            get { return login; }
            set { login = value; }
        }

        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

        public static string Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        public static void NewUser(string login_user)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @login", db.getConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login_user;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            Id = table.Rows[0][0].ToString();
            Name = table.Rows[0][1].ToString();
            Login = table.Rows[0][2].ToString();
            Email = table.Rows[0][3].ToString();
            Pass = table.Rows[0][4].ToString();
        }

    }
}
