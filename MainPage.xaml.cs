// Name - Harsh Kamleshbhai Parikh
// Class - MAP526-NSA
// Student Id - 129168217

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UtilityBillCalculator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetRenewableSwitchProperties();
            totalBillLabel.Text = string.Empty;
        }
        private void ProvincePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProvince = provincePicker.SelectedItem as string;

            if (selectedProvince == "BC")
            {
                renewableSwitch.IsToggled = true;
                renewableSwitch.IsEnabled = false;
            }
            else
            {
                renewableSwitch.IsEnabled = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            totalBillLabel.BackgroundColor = Color.Default;
        }
        private void SetRenewableSwitchProperties()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                renewableSwitch.VerticalOptions = LayoutOptions.Center;
                renewableSwitch.HorizontalOptions = LayoutOptions.Start;
            }
            else
            {
                renewableSwitch.VerticalOptions = LayoutOptions.Start;
                renewableSwitch.HorizontalOptions = LayoutOptions.Center;
            }
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            double daytimeUsage;
            double eveningUsage;

            if (!double.TryParse(daytimeUsageEntry.Text, out daytimeUsage) || !double.TryParse(eveningUsageEntry.Text, out eveningUsage))
            {
                DisplayAlert("Error", "You must enter valid usage values", "OK");
                return;
            }

            string province = provincePicker.SelectedItem as string;

            if (string.IsNullOrEmpty(province))
            {
                DisplayAlert("Error", "Please select your province.", "OK");
                return;
            }
            bool isRenewableSource = renewableSwitch.IsToggled;

            double daytimeCharge = daytimeUsage * 0.314;
            double eveningCharge = eveningUsage * 0.186;
            double totalUsageCharge = daytimeCharge + eveningCharge;

            double salesTaxRate = GetSalesTaxRate(province);
            double salesTax = totalUsageCharge * (salesTaxRate / 100);

            double environmentalRebate = 0;
            if (isRenewableSource || province == "BC")
            {
                environmentalRebate = totalUsageCharge * 0.095;
            }

            double totalBillAmount = totalUsageCharge + salesTax - environmentalRebate;
            totalBillLabel.Text = $"            You Must Pay:{totalBillAmount:C}           ";

            string breakdownText =  $"                 Charge Breakdown   " +
                $"\n\nDaytime usage charges: {daytimeCharge:C} " +
                $"\nEvening usage charges: {eveningCharge:C} " +
                $"\nTotal Charges: {totalUsageCharge:C}" +
                $"\nSales Tax ({salesTaxRate}%): {salesTax:C}" +
                $"\nEnvironmental Rebate: -{environmentalRebate:C}";

            calculationBreakdownLabel.Text = breakdownText;
        }


        private double GetSalesTaxRate(string province)
        {
            switch (province)
            {
                case "AB":
                    return 0;
                case "BC":
                    return 12;
                case "ON":
                    return 13;
                case "NL":
                    return 15;
                default:
                    return 0;
            }
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            daytimeUsageEntry.Text = string.Empty;
            eveningUsageEntry.Text = string.Empty;
            provincePicker.SelectedItem = null;
            SetRenewableSwitchProperties();
            totalBillLabel.Text = string.Empty;
            calculationBreakdownLabel.Text = string.Empty;
            renewableSwitch.IsEnabled = false;
            renewableSwitch.IsToggled = false;
        }
    }
}
