using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Wpf_TermPaper_FinancialAccounting.Charts;
using Wpf_TermPaper_FinancialAccounting.Classes;

namespace Wpf_TermPaper_FinancialAccounting.User_Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            ReloadTableIncome();
            ReloadTableOutcome();
            IncomePieChart();
            OutcomePieChart();
            Total();
        }

        private void ReloadTableIncome()
        {
            DB db = new DB();

            string str_command = "SELECT INC.id, INC.total, INC.category_id, DATE_FORMAT(INC.date, '%d.%m.%Y') AS date , INC.comment, INC.is_delete, INC.user_id, CTG.name FROM income AS INC, category_income AS CTG WHERE INC.category_id = CTG.id AND INC.is_delete = '0' AND INC.user_id = @id ORDER BY INC.date";

            ArrayList list_str = new ArrayList() { "@id" };
            ArrayList list_var = new ArrayList() { _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            objectsListIncome.ItemsSource = table.DefaultView;

            if (table.Rows.Count == 0)
            {
                ResultTxtIncome.Text = "Немає даних";
            }
            else
            {
                ResultTxtIncome.Text = default;
            }
        }

        private void ReloadTableOutcome()
        {
            DB db = new DB();

            string str_command = "SELECT OUTC.id, OUTC.total, OUTC.category_id, DATE_FORMAT(OUTC.date, '%d.%m.%Y') AS date , OUTC.comment, OUTC.is_delete, OUTC.user_id, CTG.name FROM outcome AS OUTC, category_outcome AS CTG WHERE OUTC.category_id = CTG.id AND OUTC.is_delete = '0' AND OUTC.user_id = @id ORDER BY OUTC.date";

            ArrayList list_str = new ArrayList() { "@id" };
            ArrayList list_var = new ArrayList() { _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str, list_var);

            objectsListOutcome.ItemsSource = table.DefaultView;

            if (table.Rows.Count == 0)
            {
                ResultTxtOutcome.Text = "Немає даних";
            }
            else
            {
                ResultTxtOutcome.Text = default;
            }
        }

        private void IncomePieChart()
        {
            DB db = new DB();

            string str_command = "SELECT SUM(INC.total) AS sum, INC.category_id, INC.is_delete, INC.user_id, CTG.name FROM income AS INC, category_income AS CTG WHERE INC.category_id = CTG.id AND INC.is_delete = '0' AND INC.user_id = @id GROUP BY INC.category_id";

            ArrayList list_str1 = new ArrayList() { "@id" };
            ArrayList list_var1 = new ArrayList() { _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str1, list_var1);
            
            ArrayList list_title = new ArrayList();
            ArrayList list_var = new ArrayList();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                list_title.Add(table.Rows[i][4].ToString());
                list_var.Add(Convert.ToDouble(table.Rows[i][0]));
            }

            IncomeControl.DataContext = new IncomePieChart(list_title, list_var);
        }

        private void OutcomePieChart()
        {
            DB db = new DB();

            string str_command = "SELECT SUM(OUTC.total) AS sum, OUTC.category_id, OUTC.is_delete, OUTC.user_id, CTG.name FROM outcome AS OUTC, category_outcome AS CTG WHERE OUTC.category_id = CTG.id AND OUTC.is_delete = '0' AND OUTC.user_id = @id GROUP BY OUTC.category_id";

            ArrayList list_str1 = new ArrayList() { "@id" };
            ArrayList list_var1 = new ArrayList() { _CurrentUser.Id };

            DataTable table = db.SelectTable(str_command, list_str1, list_var1);

            ArrayList list_title = new ArrayList();
            ArrayList list_var = new ArrayList();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                list_title.Add(table.Rows[i][4].ToString());
                list_var.Add(Convert.ToDouble(table.Rows[i][0]));
            }

            OutcomeControl.DataContext = new OutcomePieChart(list_title, list_var);
        }

        private void Total()
        {
            DB db = new DB();

            string str_command1 = "SELECT is_delete, user_id, SUM(total) AS sum FROM income WHERE is_delete = '0' AND user_id = @id GROUP BY is_delete";
            string str_command2 = "SELECT is_delete, user_id, SUM(total) AS sum FROM outcome WHERE is_delete = '0' AND user_id = @id GROUP BY is_delete";

            ArrayList list_str = new ArrayList() { "@id" };
            ArrayList list_var = new ArrayList() { _CurrentUser.Id };

            DataTable table1 = db.SelectTable(str_command1, list_str, list_var);
            DataTable table2 = db.SelectTable(str_command2, list_str, list_var);

            double income;
            double outcome;
            double outcome1;

            if(table1.Rows.Count > 0)
            {
                income = Convert.ToDouble(table1.Rows[0][2]);
            }
            else
            {
                income = 0;
            }

            if(table2.Rows.Count > 0)
            {
                outcome = Convert.ToDouble(table2.Rows[0][2]);
                outcome1 = outcome + (-2 * outcome);
            }
            else
            {
                outcome = outcome1 = 0;
            }

            double total = income + outcome;

            TotalControl.DataContext = new TotalBarChart(income, outcome1);

            btIncome.Content = "Надходження: " + income.ToString();
            btOutcome.Content = "Витрати: " + outcome.ToString();
            btTotal.Content = "Баланс: " + total.ToString();
        }

        private void IncomeEditButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)(sender)).Tag);
            EditIncome edit = new EditIncome(id);
            edit.Show();
        }

        private void IncomeDelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    int id = Convert.ToInt32(((Button)(sender)).Tag);
                    DB db = new DB();

                    string str_command = "UPDATE `income` SET `is_delete` = '1' WHERE `income`.`id` = @id;";
                    ArrayList list_str = new ArrayList() { "@id" };
                    ArrayList list_var = new ArrayList() { id };

                    bool flag = db.EditTable(str_command, list_str, list_var);

                    if (flag)
                    {
                        ReloadTableIncome();
                    }
                    else
                    {
                        MessageBox.Show("Щось пішло не так!");
                    }

                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void OutcomeEditButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)(sender)).Tag);
            EditOutcome edit = new EditOutcome(id);
            edit.Show();
        }

        private void OutcomeDelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    int id = Convert.ToInt32(((Button)(sender)).Tag);
                    DB db = new DB();

                    string str_command = "UPDATE `outcome` SET `is_delete` = '1' WHERE `outcome`.`id` = @id;";
                    ArrayList list_str = new ArrayList() { "@id" };
                    ArrayList list_var = new ArrayList() { id };

                    bool flag = db.EditTable(str_command, list_str, list_var);

                    if (flag)
                    {
                        ReloadTableOutcome();
                    }
                    else
                    {
                        MessageBox.Show("Щось пішло не так!");
                    }

                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadTableIncome();
            ReloadTableOutcome();

            IncomePieChart();
            OutcomePieChart();
            Total();
        }
    }
}
