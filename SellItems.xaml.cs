using System;
using System.IO;
using System.Collections.Generic;
using ClassLibraryEME;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;


namespace LibraryAssignmentWPF.UserControls
{
    public partial class SellItems : UserControl
    {
        List<AbstractItem> itemCart = new List<AbstractItem>();
        List<string> itemsToDisplay = new List<string>();
        bool HasActivatedDiscount;
        public SellItems()
        {
            InitializeComponent();
            AssignButtonNames();
            AssignButtonEvents();
            DisplayItems();
        }

        private void BtnDiscount_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            if (HasActivatedDiscount)
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Customer.ActivateDiscount(clubDiscount.txtInput.Text);
                    HasActivatedDiscount = true;
                    clubDiscount.txtInput.IsReadOnly = true;
                    double totalPrice = GetCartTotalPrice();
                    totalPrice *= 0.9;
                    title.Text = $"Итого к оплате {totalPrice:C}";
                    clubDiscount.tbText.Text = "Скидочная карта активирована";
                    MessageBox.Show($" Cкидочная карта на 10% активирована! \nID клиента: {clubDiscount.txtInput.Text}",
                        "Скидка активирована", MessageBoxButton.OK, MessageBoxImage.Information);
                    clubDiscount.txtInput.Text = string.Empty;
                }
                catch (IllegalIdException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    clubDiscount.txtInput.Focus();
                }
            }
        }

        public void DisplayItems()
        {
            listBox.ItemsSource = null;
            listBox.ItemsSource = DataBase.GetItems();
        }

        private void BtnRemove_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems != null)
            {
                string isbn = itemsToDisplay[listBox.SelectedIndex].Substring(6, 6);
                AbstractItem item = DataBase.FindItem(isbn);
                item.Quantity++;
                itemCart.Remove(item);
                itemsToDisplay.Remove(itemsToDisplay[listBox.SelectedIndex]);
                listBox.ItemsSource = null;
                listBox.ItemsSource = itemsToDisplay;
                double totalPrice = GetCartTotalPrice();
                if (HasActivatedDiscount) totalPrice *= 0.9;
                title.Text = $"Ваша корзина (итоговая стоимость {totalPrice:C}):";
                MessageBox.Show($"{item.Name} была удалена из корзины", "Товар удален", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар прежде чем удалить!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == btnAdd)
            {
                if (listBox.SelectedItem != null)
                {
                    try
                    {
                        AbstractItem item = DataBase.GetItemByIndex(listBox.SelectedIndex);
                        item.SubstractItemFromLibrary();
                        itemCart.Add(item);
                        string itemToDisplay = item.PartialToString();
                        itemsToDisplay.Add(itemToDisplay);
                        listBox.ItemsSource = null;
                        listBox.ItemsSource = DataBase.GetItems();
                        MessageBox.Show($"{item.Name} добавлен в корзину", "Товар добавлен", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (ItemOutOfStockExceptions ex)
                    {
                        DataBase.LogException(ex);
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите товар, прежде чем добавить!", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else if (sender == btnCart)
            {
                if (itemCart.Count == 0)
                {
                    MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    ProceedToCartMenu();
                }
            }
            else
            {
                ReStockItems();
                ReturnToWorkerMenuFromStart();
            } 
        }

        private void Button_ClickCheckOut(object sender, RoutedEventArgs e)
        {
            double totalPrice = GetCartTotalPrice();
            MessageBoxResult result = MessageBox.Show("Вы уверены что хотите оформить заказ?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (totalPrice != 0) ProceedToCheckoutMenu();
                else MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ClickPay(object sender, RoutedEventArgs e)
        {
            double totalPrice = GetCartTotalPrice();
            if (HasActivatedDiscount) totalPrice *= 0.9;
            float totalPriceFloat = (float)totalPrice;
            if (amountToPay.txtInput.Text == totalPriceFloat.ToString())
            {
                try
                {
                    if (HasActivatedDiscount) DataBase.SaveReceipt(itemCart, totalPrice, true);
                    else DataBase.SaveReceipt(itemCart, totalPrice);

                    SaveItemInformation();
                    MessageBox.Show("Спасибо за покупку! \nЧек сохранен в базе данных, и менеджер может получить к ней доступ.",
                        "Покупка", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToWorkerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, оплатите указанную сумму!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                amountToPay.txtInput.Focus();
            }
        }


        private void Button_ClickReturnCheckout(object sender, RoutedEventArgs e)
        {
            ReturnToSecondMenu();
        }

        private void Button_ClickReturnCart(object sender, RoutedEventArgs e)
        {
            ReturnToFirstMenu();
        }

        private void ReturnToWorkerMenu()
        {
            ReturnToWorkerMenuFromStart();

            gridCheckout.Visibility = Visibility.Collapsed;
            amountToPay.txtInput.Clear();

            blueViewBox.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            btnCart.Visibility = Visibility.Visible;
            title.Text = "Товары, доступные для продажи:";

            itemCart.Clear();
            itemsToDisplay.Clear();
            DisplayItems();
        }

        private void ReturnToWorkerMenuFromStart()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid workerGrid = (Grid)mainWindow.FindName("workerGrid");
            workerGrid.Visibility = Visibility.Visible;
            Grid SellItemsGrid = (Grid)mainWindow.FindName("SellItemsGrid");
            SellItemsGrid.Visibility = Visibility.Collapsed;
            RevertDiscoundOptions();
        }

        private void RevertDiscoundOptions()
        {
            HasActivatedDiscount = false;
            clubDiscount.txtInput.IsReadOnly = false;
            clubDiscount.tbText.Text = "ID для скидочной карты";
        }

        private void ReStockItems()
        {
            foreach(var item in itemCart)
            {
                item.Quantity++;
            }
            listBox.ItemsSource = DataBase.GetItems();
            itemCart.Clear();
            itemsToDisplay.Clear();
        }

        private void ProceedToCartMenu()
        {
            btnAdd.Visibility = Visibility.Collapsed;
            btnReturn.Visibility = Visibility.Collapsed;
            btnCart.Visibility = Visibility.Collapsed;

            double totalPrice = GetCartTotalPrice();
            if (HasActivatedDiscount) totalPrice *= 0.9;
            title.Text = $"Ваша корзина (итоговая стоимость{totalPrice:C}):";
            btnReturnCart.Visibility = Visibility.Visible;
            btnCheckout.Visibility = Visibility.Visible;
            btnRemove.Visibility = Visibility.Visible;

            listBox.ItemsSource = itemsToDisplay;
        }

        private void ReturnToFirstMenu()
        {
            btnReturnCart.Visibility = Visibility.Collapsed;
            btnCheckout.Visibility = Visibility.Collapsed;
            btnRemove.Visibility = Visibility.Collapsed;

            btnAdd.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            btnCart.Visibility = Visibility.Visible;
            DisplayItems();
            title.Text = "Товары, доступные для продажи:";
        }

        private void ReturnToSecondMenu()
        {
            gridCheckout.Visibility = Visibility.Collapsed;

            btnReturnCart.Visibility = Visibility.Visible;
            btnCheckout.Visibility = Visibility.Visible;
            btnRemove.Visibility = Visibility.Visible;
            blueViewBox.Visibility = Visibility.Visible;

            double totalPrice = GetCartTotalPrice();
            if (HasActivatedDiscount) totalPrice *= 0.9;
            title.Text = $"Ваша корзина (итоговая стоимость {totalPrice:C}):";
        }
        
        private void ProceedToCheckoutMenu()
        {
            btnReturnCart.Visibility = Visibility.Collapsed;
            btnCheckout.Visibility = Visibility.Collapsed;
            btnRemove.Visibility = Visibility.Collapsed;
            blueViewBox.Visibility = Visibility.Collapsed;

            gridCheckout.Visibility = Visibility.Visible;

            double totalPrice = GetCartTotalPrice();
            if (HasActivatedDiscount) totalPrice *= 0.9;
            title.Text = $"Итого к оплате {totalPrice:C}";
        }

        private void SaveItemInformation()
        {
            foreach (var item in itemCart)
            {
                DataBase.SaveItemInformation(item);
            }
        }

        private double GetCartTotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in itemCart)
            {
                totalPrice += item.Price;
            }
            return totalPrice;
        }

        private void AssignButtonNames()
        {
            btnAdd.ButtonContent = Resex.btnAddCart;
            btnCart.ButtonContent = Resex.btnCart;
            btnReturn.ButtonContent = Resex.btnReturn;
            btnReturnCart.ButtonContent = Resex.btnReturn;
            btnCheckout.ButtonContent = Resex.btnCheckOut;
            btnPay.ButtonContent = Resex.btnPay;
            btnReturnCheckOut.ButtonContent = Resex.btnReturn;
            btnRemove.ButtonContent = Resex.btnRemoveCart;
            btnDiscount.ButtonContent = Resex.btnClubDiscount;
        }

        private void AssignButtonEvents()
        {
            btnAdd.ButtonClickEvent += Button_Click;
            btnCart.ButtonClickEvent += Button_Click;
            btnReturn.ButtonClickEvent += Button_Click;
            btnReturnCart.ButtonClickEvent += Button_ClickReturnCart;
            btnCheckout.ButtonClickEvent += Button_ClickCheckOut;
            btnPay.ButtonClickEvent += Button_ClickPay;
            btnReturnCheckOut.ButtonClickEvent += Button_ClickReturnCheckout;
            btnRemove.ButtonClickEvent += BtnRemove_ButtonClickEvent;
            btnDiscount.ButtonClickEvent += BtnDiscount_ButtonClickEvent;
        }
    }
}
