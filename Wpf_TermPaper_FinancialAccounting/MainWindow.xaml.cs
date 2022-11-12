﻿using System;
using System.Collections.Generic;
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
using Wpf_TermPaper_FinancialAccounting.User_ViewModels;

namespace Wpf_TermPaper_FinancialAccounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextHello.Text = "Привіт, " + _CurrentUser.Name;
            DataContext = new MainViewModel();
        }

        private void MainView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void AddIncomeView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AddIncomeViewModel();
        }

        private void AddOutcomeView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AddOutcomeViewModel();
        }

        private void CtgIncomeView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new CtgIncomeViewModel();
        }

        private void CtgOutcomeView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new CtgOutcomeViewModel();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви бажаєте вийти з кабінету?", "Підтвердження", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    AuthWindow authWindow = new AuthWindow();
                    authWindow.Show();
                    this.Close();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
