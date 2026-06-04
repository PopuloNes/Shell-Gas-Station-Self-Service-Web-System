using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gsst.Interfaces;
using gsst.Model;
using gsst.Model.User;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Gsstwpfmock.ViewModel
{
    public partial class PaymentViewModel : ObservableObject
    {
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IBonusService _bonusService;
        
        [ObservableProperty]
        private double _totalAmount;

        [ObservableProperty]
        private double _amountToPay;

        [ObservableProperty]
        private User _clientUser;

        [ObservableProperty]
        private BonusCard _clientBonusCard;

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> _savedPaymentMethods = new();

        [ObservableProperty]
        private PaymentMethod _selectedPaymentMethod;

        [ObservableProperty]
        private bool _isAddingNewMethod;

        [ObservableProperty]
        private string _newMethodType = "Visa/Mastercard";

        [ObservableProperty]
        private string _cardNumber;

        [ObservableProperty]
        private string _cardExpiry;

        [ObservableProperty]
        private string _cardCVC;

        [ObservableProperty]
        private string _blikCode;

        [ObservableProperty]
        private string _cryptoAddress;

        [ObservableProperty]
        private int _paymentMethodTabIndex;

        [ObservableProperty]
        private bool _saveNewMethod = true;

        [ObservableProperty]
        private bool _useBonuses;

        [ObservableProperty]
        private int _bonusesToSpend;

        // Login / Register properties
        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _fullName;
        [ObservableProperty]
        private bool _isRegisterMode;
        
        public Action<bool, string> CloseAction { get; set; }

        public PaymentViewModel(IPaymentService paymentService, IAuthService authService, IUserService userService, IBonusService bonusService, double totalAmount, BonusCard bonusCard)
        {
            _paymentService = paymentService;
            _authService = authService;
            _userService = userService;
            _bonusService = bonusService;
            
            TotalAmount = totalAmount;
            AmountToPay = totalAmount;
            
            if (bonusCard != null)
            {
                ClientBonusCard = bonusCard;
                if (bonusCard.UserId.HasValue)
                {
                    ClientUser = _userService.GetUserById(bonusCard.UserId.Value);
                    LoadPaymentMethods();
                }
            }
        }

        partial void OnUseBonusesChanged(bool value)
        {
            RecalculateAmount();
        }

        partial void OnBonusesToSpendChanged(int value)
        {
            RecalculateAmount();
        }

        private void RecalculateAmount()
        {
            if (UseBonuses && ClientBonusCard != null)
            {
                if (BonusesToSpend > ClientBonusCard.BonusBalance)
                {
                    BonusesToSpend = (int)ClientBonusCard.BonusBalance;
                }
                
                AmountToPay = TotalAmount - BonusesToSpend;
                if (AmountToPay < 0) AmountToPay = 0;
            }
            else
            {
                AmountToPay = TotalAmount;
            }
        }

        [RelayCommand]
        public void Login()
        {
            try
            {
                ClientUser = _authService.Login(Username, Password);
                MessageBox.Show("Logged in successfully!");
                LoadPaymentMethods();
                
                // Fetch BonusCard for user if not already set
                var userCards = _bonusService.GetAllBonusCards().Where(b => b.UserId == ClientUser.Id).ToList();
                if (userCards.Any())
                {
                    ClientBonusCard = userCards.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void Register()
        {
            try
            {
                ClientUser = _userService.CreateUser(FullName, Username, Password, UserRoles.Client);
                MessageBox.Show("Registered successfully!");
                IsRegisterMode = false;
                LoadPaymentMethods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPaymentMethods()
        {
            if (ClientUser != null)
            {
                var methods = _paymentService.GetPaymentMethodsForUser(ClientUser.Id);
                SavedPaymentMethods = new ObservableCollection<PaymentMethod>(methods);
                SelectedPaymentMethod = SavedPaymentMethods.FirstOrDefault(m => m.IsDefault) ?? SavedPaymentMethods.FirstOrDefault();
                
                // If there are saved methods, show them by default. Otherwise show the "Add New" form.
                if (SavedPaymentMethods.Any())
                {
                    IsAddingNewMethod = false;
                }
                else
                {
                    IsAddingNewMethod = true;
                }
            }
        }

        [RelayCommand]
        public void ToggleAddNewMethod()
        {
            IsAddingNewMethod = !IsAddingNewMethod;
            if (IsAddingNewMethod)
            {
                SelectedPaymentMethod = null;
            }
            else
            {
                SelectedPaymentMethod = SavedPaymentMethods.FirstOrDefault();
            }
        }

        [RelayCommand]
        public void DeletePaymentMethod(PaymentMethod method)
        {
            if (method == null) return;
            
            var res = MessageBox.Show($"Are you sure you want to delete {method.Type}: {method.Details}?", "Delete Payment Method", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                _paymentService.RemovePaymentMethod(method.Id);
                LoadPaymentMethods();
            }
        }

        [RelayCommand]
        public void ConfirmPayment()
        {
            string finalPaymentDetails = "";
            
            if (IsAddingNewMethod)
            {
                string details = "";
                string type = "";

                if (PaymentMethodTabIndex == 0) // Card
                {
                    if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 16)
                    {
                        MessageBox.Show("Please enter a valid Card Number.");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(CardExpiry) || !CardExpiry.Contains("/"))
                    {
                        MessageBox.Show("Please enter a valid Expiry Date (MM/YY).");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(CardCVC) || CardCVC.Length < 3)
                    {
                        MessageBox.Show("Please enter a valid CVC.");
                        return;
                    }
                    
                    type = "Visa/Mastercard";
                    string maskedCard = "**** **** **** " + CardNumber.Substring(CardNumber.Length - 4);
                    details = $"{maskedCard} (Exp: {CardExpiry})";
                }
                else if (PaymentMethodTabIndex == 1) // Blik
                {
                    if (string.IsNullOrWhiteSpace(BlikCode) || BlikCode.Length != 6)
                    {
                        MessageBox.Show("Please enter a 6-digit Blik Code.");
                        return;
                    }
                    type = "Blik";
                    details = $"Code: {BlikCode}";
                }
                else if (PaymentMethodTabIndex == 2) // Crypto
                {
                    if (string.IsNullOrWhiteSpace(CryptoAddress))
                    {
                        MessageBox.Show("Please enter your Crypto Wallet Address.");
                        return;
                    }
                    type = "Crypto";
                    details = $"Wallet: {CryptoAddress}";
                }

                if (SaveNewMethod && ClientUser != null)
                {
                    var saved = _paymentService.AddPaymentMethod(ClientUser.Id, type, details, !SavedPaymentMethods.Any());
                    finalPaymentDetails = $"{saved.Type}: {saved.Details}";
                }
                else
                {
                    finalPaymentDetails = $"{type}: {details}";
                }
            }
            else if (SelectedPaymentMethod != null)
            {
                finalPaymentDetails = $"{SelectedPaymentMethod.Type}: {SelectedPaymentMethod.Details}";
            }
            else
            {
                MessageBox.Show("Please select or add a payment method.");
                return;
            }

            CloseAction?.Invoke(true, finalPaymentDetails);
        }
    }
}
