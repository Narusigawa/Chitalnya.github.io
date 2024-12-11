using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using ClassLibraryEME;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class AddCustomer : UserControl
    {
        const int MEMBERSHIPPRICE = 25;
        public AddCustomer()
        {
            InitializeComponent();
            btnEnter.ButtonContent = Resex.btnEnter;
            btnreturn.ButtonContent = Resex.btnReturn;
            btnPayEnter.ButtonContent = Resex.btnPay;
            btnPayReturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnreturn.ButtonClickEvent += btnInput_Click;
            btnPayEnter.ButtonClickEvent += BtnPayEnter_ButtonClickEvent;
            btnPayReturn.ButtonClickEvent += BtnPayReturn_ButtonClickEvent;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия кнопки
        {
            if (sender == btnEnter)
            {
                try
                {
                    if (idBox != null)
                    {
                        Customer.IsIdValid(idBox.txtInput.Text);
                        Customer.DoesIdExistInClub(idBox.txtInput.Text);
                        ProceedToPayMenu();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста введите ID клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (IllegalIdException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    idBox.txtInput.Focus();
                }
            }
            else
            {
                ReturnToWorkerMenuFromStart();
            }
        }
        private void ReturnToWorkerMenuFromStart()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid AddCustomerGrid = (Grid)mainWindow.FindName("AddCustomerGrid");
            AddCustomerGrid.Visibility = Visibility.Collapsed;

            Grid workerGrid = (Grid)mainWindow.FindName("workerGrid");
            workerGrid.Visibility = Visibility.Visible;

            idBox.txtInput.Text = string.Empty;
        }

        private void BtnPayEnter_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            if (payBox.txtInput.Text == MEMBERSHIPPRICE.ToString())
            {
                try
                {
                    Customer.AddCustomerToClub(idBox.txtInput.Text);
                    Customer.SaveCustomer(idBox.txtInput.Text);
                    MessageBox.Show($"Клиент {idBox.txtInput.Text} был успешно добавлен в клуб! ", "Клиент добавлен", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToWorkerMenuEnd();
                }
                catch (DirectoryNotFoundException ex)
                {
                    Customer.RemoveCustomerFromClub(idBox.txtInput.Text);
                    ErrorMessage(ex);
                    payBox.txtInput.Focus();
                }
                catch (IllegalIdException ex)
                {
                    ErrorMessage(ex);
                    payBox.txtInput.Focus();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заплатите указанную сумму!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                payBox.txtInput.Focus();
            }
        }

        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnPayReturn_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            ReturnToIdMenu();
        }

        private void ProceedToPayMenu()
        {
            title.Text = $"Вам нужно заплатить {MEMBERSHIPPRICE:C}";
            btnEnter.Visibility = Visibility.Collapsed;
            btnreturn.Visibility = Visibility.Collapsed;
            viewBox1.Visibility = Visibility.Collapsed;

            btnPayEnter.Visibility = Visibility.Visible;
            btnPayReturn.Visibility = Visibility.Visible;
            viewBox2.Visibility = Visibility.Visible;
        }

        private void ReturnToWorkerMenuEnd()
        {
            ReturnToIdMenu();
            ReturnToWorkerMenuFromStart();
        }

        private void ReturnToIdMenu()
        {
            title.Text = "Пожалуйста, введите номер ID клиента, которого вы хотите добавить:";
            btnEnter.Visibility = Visibility.Visible;
            btnreturn.Visibility = Visibility.Visible;
            viewBox1.Visibility = Visibility.Visible;

            btnPayEnter.Visibility = Visibility.Collapsed;
            btnPayReturn.Visibility = Visibility.Collapsed;
            viewBox2.Visibility = Visibility.Collapsed;
            payBox.txtInput.Text = string.Empty;
        }
    }
}
