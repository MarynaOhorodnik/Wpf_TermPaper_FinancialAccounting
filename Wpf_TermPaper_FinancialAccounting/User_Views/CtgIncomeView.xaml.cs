﻿using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf_TermPaper_FinancialAccounting.Classes;

namespace Wpf_TermPaper_FinancialAccounting.User_Views
{
    /// <summary>
    /// Interaction logic for CtgIncomeView.xaml
    /// </summary>
    public partial class CtgIncomeView : UserControl
    {
        public CtgIncomeView()
        {
            InitializeComponent();

            ReloadTable();
        }

        private void ReloadTable()
        {
            DB db = new DB();

            string str_command = "SELECT * FROM `category_income` WHERE `is_delete` = 0 AND `user_id` = @user_id";

            ArrayList list_str = new ArrayList() { "@user_id" };
            ArrayList list_var = new ArrayList() { _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            listIncomeCtg.ItemsSource = table.DefaultView;

            if (table.Rows.Count == 0)
            {
                ResultTxt.Text = "Немає даних";
            }
            else
            {
                ResultTxt.Text = default;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = tbNameCtg.Text;

            if(name.Length < 1)
            {
                tbNameCtg.Background = Brushes.MistyRose;
            }
            else if (CheckNameCtg(name))
            {
                tbNameCtg.Background = Brushes.MistyRose;
                tbNameCtg.ToolTip = "Така категорія вже існує";
            }
            else
            {
                tbNameCtg.Background = Brushes.Transparent;
                tbNameCtg.ToolTip = default;


                DB db = new DB();

                string str_command = "INSERT INTO `category_income` (`name`, `user_id`) VALUES (@name, @user_id);";
                ArrayList list_str = new ArrayList() { "@name", "@user_id" };
                ArrayList list_var = new ArrayList() { name, _CurrentUser.Id };

                bool flag = db.EditTable(str_command, list_str, list_var);

                if (flag)
                {
                    tbNameCtg.Text = "";
                    ReloadTable();
                }
                else
                {
                    MessageBox.Show("Щось пішло не так!");
                }
            }
        }

        private bool CheckNameCtg(string nm)
        {
            DB db = new DB();

            string str_command = "SELECT * FROM `category_income` WHERE `name` = @name AND `is_delete` = 0 AND `user_id` = @id";

            ArrayList list_str = new ArrayList() { "@name", "@id" };
            ArrayList list_var = new ArrayList() { nm, _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private bool CheckCtg(int id)
        {
            DB db = new DB();

            string str_command = "SELECT * FROM `income` WHERE `category_id` = @id AND `is_delete` = 0 AND `user_id` = @user_id";

            ArrayList list_str = new ArrayList() { "@id", "@user_id" };
            ArrayList list_var = new ArrayList() { id, _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)(sender)).Tag);
            EditIncomeCtg edit = new EditIncomeCtg(id);
            edit.Show();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити дану категорію?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    int id = Convert.ToInt32(((Button)(sender)).Tag);
                    if (CheckCtg(id))
                    {
                        MessageBox.Show("Ви не можете видалити дану категорію. Спочатку видаліть надходження, що пов'язані з цією категорією.");
                    }
                    else
                    {
                        DB db = new DB();

                        string str_command = "UPDATE `category_income` SET `is_delete` = '1' WHERE `category_income`.`id` = @id;";
                        ArrayList list_str = new ArrayList() { "@id" };
                        ArrayList list_var = new ArrayList() { id };

                        bool flag = db.EditTable(str_command, list_str, list_var);

                        if (!flag)
                        {
                            MessageBox.Show("Щось пішло не так!");
                        }

                        ReloadTable();
                    }

                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadTable();
        }
    }
}
