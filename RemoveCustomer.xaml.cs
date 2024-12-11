using ClassLibraryEME;
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

namespace LibraryAssignmentWPF.UserControls
{
    public partial class RemoveCustomer : UserControl
    {
        public RemoveCustomer()
        {
            InitializeComponent();
            btnEnter.ButtonContent = Resex.btnEnter;
            btnReturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnReturn.ButtonClickEvent += btnInput_Click;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnEnter)
            {
                if (idBox.txtInput.Text != null)
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить {idBox.txtInput.Text} из клуба?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            Customer.RemoveCustomerFromClub(idBox.txtInput.Text);
                            Customer.DeleteFile(idBox.txtInput.Text);
                            MessageBox.Show($"Клиент {idBox.txtInput.Text} был успешно удален из клуба!", "Клиент удален", MessageBoxButton.OK, MessageBoxImage.Information);
                            ReturnToWorkerMenu();
                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        ErrorMessage(ex);
                        Customer.AddCustomerToClub(idBox.txtInput.Text);
                        idBox.txtInput.Focus();
                    }
                    catch (IllegalIdException ex)
                    {
                        ErrorMessage(ex);
                        idBox.txtInput.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите id клиента, которого вы хотите удалить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    idBox.txtInput.Focus();
                }
            }
            else
            {
                ReturnToWorkerMenu();
            }
        }
        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ReturnToWorkerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid RemoveCustomerGrid = (Grid)mainWindow.FindName("RemoveCustomerGrid");
            RemoveCustomerGrid.Visibility = Visibility.Collapsed;

            Grid workerGrid = (Grid)mainWindow.FindName("workerGrid");
            workerGrid.Visibility = Visibility.Visible;

            idBox.txtInput.Text = string.Empty;
        }
    }
}
