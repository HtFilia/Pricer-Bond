﻿using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PricerBond
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bond bond;
        Action action;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void SetParameters(object sender, RoutedEventArgs e)
        {
            // Create Bond from User's Input
            int frequency;
            if ((bool)monthly.IsChecked)
            {
                frequency = 12;
            }
            else if ((bool)quaterly.IsChecked)
            {
                frequency = 3;
            }
            else if ((bool)twiceYear.IsChecked)
            {
                frequency = 2;
            }
            else
            {
                frequency = 1;
            }
            double marketRate = Double.Parse(MarketRate.Text, CultureInfo.InvariantCulture) / 100;
            double annualRate = Double.Parse(AnnualRate.Text, CultureInfo.InvariantCulture) / 100;
            double faceValue = Double.Parse(FaceValue.Text, CultureInfo.InvariantCulture);
            ConventionDate maturity = new ConventionDate(Maturity.Text);

            // Get Action if needed
            string yieldType = FormatString(YieldType.SelectedItem.ToString());
            if (yieldType == "Variable Yield")
            {
                action = new Action("ACCOR", 100, new ConventionDate(), maturity);
            }
            bond = new Bond(faceValue, annualRate, marketRate, frequency, maturity);
        }

        private void ShowPrice(double price)
        {
            MessageBox.Show("Price of the bond : " + Math.Round(price, 2) + "€");
        }

        private void Price(object sender, RoutedEventArgs e)
        {
            SetParameters(sender, e);
            ShowPrice(bond.GetPrice(action));
        }

        private void QuickSimulation(object sender, RoutedEventArgs e)
        {
            double faceValue = 100000;
            double annualRate = 5.5/100;
            double marketRate = 1.2/100;
            ConventionDate maturity = new ConventionDate(2022, 11, 15);
            int frequency = 1;
            bond = new Bond(faceValue, annualRate, marketRate, frequency, maturity);
            
            double price = 0;
            int nbSimulations = 100;
            for (var simul = 0; simul < nbSimulations; simul++)
            {
                action = new Action("ACCOR", 100, new ConventionDate(), maturity);
                price += bond.GetPrice(action);
            }
            ShowPrice(price / nbSimulations);
        }

        private string FormatString(string toFormat)
        {
            return toFormat.Substring(39);
        }
    }
}
