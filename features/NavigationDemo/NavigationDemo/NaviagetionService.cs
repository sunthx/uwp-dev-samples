using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using NavigationDemo.Pages;

namespace NavigationDemo
{
    public static class NaviagetionService
    {
        static NaviagetionService()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += NaviagetionService_BackRequested;
        }

        private static void NaviagetionService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
            e.Handled = true;
        }

        public static Frame NavFrame { get; set; }

        public static void NavToPage1()
        {
            NavFrame.Navigate(typeof(Page1), $"Page1 : Navigated At {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            ResetBackButtonState();
        }

        public static void NavToPage2()
        {
            NavFrame.Navigate(typeof(Page2), $"Page2 : Navigated At {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            ResetBackButtonState();
        }

        public static void GoBack()
        {
            if(NavFrame.CanGoBack)
                NavFrame.GoBack();

            ResetBackButtonState();
        }

        public static void ResetBackButtonState()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
