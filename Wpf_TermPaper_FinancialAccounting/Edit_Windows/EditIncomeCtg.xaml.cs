﻿using System.Collections;
using System.Data;
using System.Windows;

namespace Wpf_TermPaper_FinancialAccounting
{
    /// <summary>
    /// Interaction logic for EditIncomeCtg.xaml
    /// </summary>
    public partial class EditIncomeCtg : Window
    {
        private int idCtg;
        private string nameCtg;
        public EditIncomeCtg(int id)
        {
            InitializeComponent();
            idCtg = id;
            nameCtg = FindName(id);
            tbName.Text = nameCtg;
        }

        private string FindName(int i)
        {
            DB db = new DB();

            string str_command = "SELECT * FROM `category_income` WHERE `id` = @id";

            ArrayList list_str = new ArrayList() { "@id" };
            ArrayList list_var = new ArrayList() { i };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            string name = table.Rows[0][1].ToString();
            return name;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(tbName.Text != nameCtg && tbName.Text.Length > 0)
            {
                DB db = new DB();

                string str_command = "UPDATE `category_income` SET `name` = @name WHERE `category_income`.`id` = @id;";
                ArrayList list_str = new ArrayList() { "@name", "@id" };
                ArrayList list_var = new ArrayList() { tbName.Text, idCtg };

                bool flag = db.EditTable(str_command, list_str, list_var);

                if (flag)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Щось пішло не так!");
                }
            }
        }
    }
}
