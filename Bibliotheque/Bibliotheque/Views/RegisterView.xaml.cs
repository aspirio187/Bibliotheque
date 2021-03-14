using System;
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

namespace Bibliotheque.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour RegisterView.xaml
    /// </summary>
    public partial class RegisterView : ContentControl
    {
        // TODO : Mettre des Placeholder personnalisés dans les textbox
        public RegisterView()
        {
            InitializeComponent();
        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailBox.Text))
                EmailBox.BorderBrush = Brushes.Red;
        }

        private void EmailConfirmationBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailConfirmationBox.Text))
                EmailConfirmationBox.BorderBrush = Brushes.Red;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
                PasswordBox.BorderBrush = Brushes.Red;
        }

        private void PasswordConfirmationBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordConfirmationBox.Password))
                PasswordConfirmationBox.BorderBrush = Brushes.Red;
        }

        private void FirstNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstNameBox.Text))
                FirstNameBox.BorderBrush = Brushes.Red;
        }

        private void LastNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(LastNameBox.Text))
                LastNameBox.BorderBrush = Brushes.Red;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGender.SelectedIndex < 0)
                cmbGender.BorderBrush = Brushes.Red;
        }

        private void DateBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateBox.SelectedDate >= DateTime.Now)
                DateBox.BorderBrush = Brushes.Red;
        }

        private void StreetBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(StreetBox.Text))
                StreetBox.BorderBrush = Brushes.Red;
        }

        private void ZipCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ZipCodeBox.Text))
                ZipCodeBox.BorderBrush = Brushes.Red;
        }

        private void CityBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CityBox.Text))
                CityBox.BorderBrush = Brushes.Red;
        }

        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PhoneBox.Text))
                PhoneBox.BorderBrush = Brushes.Red;
        }
    }
}
