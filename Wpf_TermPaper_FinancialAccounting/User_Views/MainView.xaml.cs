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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                ResultTxtIncome.Text = "Немає результатів";
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
                ResultTxtOutcome.Text = "Немає результатів";
            }
            else
            {
                ResultTxtOutcome.Text = default;
            }
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
                        MessageBox.Show("Успіх!");
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
                        MessageBox.Show("Успіх!");
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
        }
    }
}
