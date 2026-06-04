using Gsstwpfmock.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PetrolWPF.View
{
    public partial class PaymentWindow : Window
    {
        public PaymentViewModel ViewModel { get; private set; }
        public string FinalPaymentDetails { get; private set; }

        private bool _isFormatting = false;

        public PaymentWindow(PaymentViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            ViewModel.CloseAction = (success, details) =>
            {
                if (success)
                {
                    FinalPaymentDetails = details;
                    DialogResult = true;
                    Close();
                }
            };

            // Set placeholder texts via tag or style if needed
            CardNumberBox.Tag = "Card number";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PaymentViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void CardNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormatting) return;

            var textBox = sender as TextBox;
            if (textBox == null) return;

            _isFormatting = true;
            
            // Remove all spaces and non-digits
            string raw = new string(textBox.Text.Where(char.IsDigit).ToArray());
            
            // Reinsert spaces every 4 digits
            string formatted = string.Empty;
            for (int i = 0; i < raw.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formatted += " ";
                }
                formatted += raw[i];
            }

            textBox.Text = formatted;
            textBox.CaretIndex = formatted.Length;
            
            _isFormatting = false;
        }

        private void CardExpiryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormatting) return;

            var textBox = sender as TextBox;
            if (textBox == null) return;

            _isFormatting = true;
            
            // Remove non-digits
            string raw = new string(textBox.Text.Where(char.IsDigit).ToArray());
            
            // Format as MM/YY
            string formatted = raw;
            if (raw.Length >= 2)
            {
                formatted = raw.Insert(2, "/");
            }
            if (formatted.Length > 5)
            {
                formatted = formatted.Substring(0, 5);
            }

            textBox.Text = formatted;
            textBox.CaretIndex = formatted.Length;
            
            _isFormatting = false;
        }
    }
}
