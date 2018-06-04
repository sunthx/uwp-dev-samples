using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace NavigationDemo.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Page1 : Page
    {

        List<string> temp = new List<string>();

        public Page1()
        {
            this.InitializeComponent();
            Loaded += Page1_Loaded;
        }

        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            for (long i = 0; i < 100; i++)
            {
                temp.Add(Guid.NewGuid().ToString());
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
                Title.Text = e.Parameter.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NaviagetionService.GoBack();
        }
    }
}
