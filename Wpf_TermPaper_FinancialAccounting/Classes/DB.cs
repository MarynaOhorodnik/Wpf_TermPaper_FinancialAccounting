using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using System.Windows;

namespace Wpf_TermPaper_FinancialAccounting
{
    class DB
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=finance_account");

        public void openConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }

        public DataTable SelectTable(string str_command, ArrayList list_str, ArrayList list_var)
        {
            DataTable table = new DataTable();
            int v = 0;

            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand(str_command, this.getConnection());
                for (int i = 0; i < list_str.Count; i++)
                {
                    command.Parameters.Add($"{list_str[v]}", MySqlDbType.VarChar).Value = list_var[v];
                    v += 1;
                }

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            catch
            {
                MessageBox.Show("Виникли проблеми з підключенням бази даних. Перевірте з'єднання.");
            }

            return table;
        }

        public bool EditTable(string str_command, ArrayList list_str, ArrayList list_var)
        {
            bool flag = true;
            int v = 0;

            try
            {
                MySqlCommand command = new MySqlCommand(str_command, this.getConnection());

                for (int i = 0; i < list_str.Count; i++)
                {
                    command.Parameters.Add($"{list_str[v]}", MySqlDbType.VarChar).Value = list_var[v];
                    v += 1;
                }

                this.openConnection();

                command.ExecuteNonQuery();

                this.closeConnection();
            }

            catch
            {
                MessageBox.Show("Виникли проблеми з підключенням бази даних. Перевірте з'єднання.");
                flag = false;
            }

            return flag;
        }
    }
}
