using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;

namespace CalculatorAPP
{
    public sealed partial class ItemsPage : CalculatorAPP.Common.LayoutAwarePage
    { 
        double sum = 0;
        bool iscontinue = true;    //是否連續輸入數字
        bool islastoperator = false;   //上一個是否運算子
        double firstnum = 0;
        string _operator = "=";      
        double Msum = 0;
        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

        public ItemsPage()
        {
            this.InitializeComponent();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;
        }

        void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = "Calculator";
            args.Request.Data.SetText("這是一個計算機APP");
        }
                
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            
        }

        //數字鍵按鈕
        private void Num0Btn_Click(object sender, RoutedEventArgs e)
        {
            string num = (sender as Button).Tag.ToString();
            if (ScreenNumberTextBlock.Text == "0" || !iscontinue)
            {
                ScreenNumberTextBlock.Text = num;
                iscontinue = true;
                islastoperator = false;
            }
            else
            {
                ScreenNumberTextBlock.Text += num;
                CheckScreenTextLength();
            }
        }

        //小數點按鈕
        private void DotBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ScreenNumberTextBlock.Text.EndsWith(".") && !ScreenNumberTextBlock.Text.Contains("."))
            {
                ScreenNumberTextBlock.Text += (sender as Button).Tag.ToString();
            }  
        }

        //運算按鈕
        private void DivideBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_operator != "=" && !islastoperator)
            {   //有運算子需要計算                
                Calculate();
                ScreenNumberTextBlock.Text = firstnum.ToString();
                CheckScreenTextLength();
            }
            else
            {   //不用立即計算
                firstnum = double.Parse(ScreenNumberTextBlock.Text);
            }
            _operator = (sender as Button).Tag.ToString();
            iscontinue = false;
            islastoperator = true;
        }

        private async void Calculate()
        {
            switch (_operator)
            {
                case "+":
                    firstnum = firstnum + double.Parse(ScreenNumberTextBlock.Text);
                    break;
                case "-":
                    firstnum = firstnum - double.Parse(ScreenNumberTextBlock.Text);
                    break;
                case "*":
                    firstnum = firstnum * double.Parse(ScreenNumberTextBlock.Text);
                    break;
                case "/":
                    if (ScreenNumberTextBlock.Text == "0")
                    {
                        MessageDialog msg = new MessageDialog("無法除0");
                        await msg.ShowAsync();
                        return;
                    }
                    firstnum = firstnum / double.Parse(ScreenNumberTextBlock.Text);
                    break;
            }
        }

        //清除按鈕
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ScreenNumberTextBlock.Text = "0";
            sum = 0;
            iscontinue = true;
            _operator = "=";
        }

        //正負號按鈕
        private void PlusMinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ScreenNumberTextBlock.Text.StartsWith("-"))
            {       //負變正
                ScreenNumberTextBlock.Text = ScreenNumberTextBlock.Text.Remove(0, 1);
            }
            else
            {       //正變負
                ScreenNumberTextBlock.Text = ScreenNumberTextBlock.Text.Insert(0, "-");
            }
        }

        //M+按鈕
        private void MAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Msum += double.Parse(ScreenNumberTextBlock.Text);
            MTextBlock.Visibility = Visibility.Visible;
            iscontinue = false;
        }

        //M-按鈕
        private void MMinusBtn_Click(object sender, RoutedEventArgs e)
        {
            Msum -= double.Parse(ScreenNumberTextBlock.Text);
            MTextBlock.Visibility = Visibility.Visible;
            iscontinue = false;
        }

        //MR按鈕
        private void MRBtn_Click(object sender, RoutedEventArgs e)
        {
            ScreenNumberTextBlock.Text = Msum.ToString();
            MTextBlock.Visibility = Visibility.Visible;
            iscontinue = false;
        }

        //倒退按鈕
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ScreenNumberTextBlock.Text.Length > 1)
            {
                ScreenNumberTextBlock.Text = ScreenNumberTextBlock.Text.Remove(ScreenNumberTextBlock.Text.Length - 1);
                if (ScreenNumberTextBlock.Text == "-")
                {       //剩單一負號
                    ScreenNumberTextBlock.Text = "0";
                }
            }
            else
            {
                ScreenNumberTextBlock.Text = "0";
            }
        }

        void CheckScreenTextLength()
        {
            if (ScreenNumberTextBlock.Text.Length > 10)
            {
                ScreenNumberTextBlock.Text = ScreenNumberTextBlock.Text.Substring(0, 10);
            }
        }

        //MC按鈕
        private void MCBtn_Click(object sender, RoutedEventArgs e)
        {
            Msum = 0;
            MTextBlock.Visibility = Visibility.Collapsed;
            iscontinue = false;
        }

        //等於按鈕
        private void EqualBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_operator != "=")
            {   //有運算子需要計算
                Calculate();
                sum = firstnum;
                ScreenNumberTextBlock.Text = sum.ToString();
                CheckScreenTextLength();
            }
            iscontinue = false;
            _operator = "=";    
        }
    }
}
