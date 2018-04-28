using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UwpThemeDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.RootTheme = ElementTheme.Default;
        }

        // 0 : System
        // 1 : Light
        // 2 : Dark
        // 3 : Custom
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox themeSelector))
            {
               return;
            }

            var selectedIndex = themeSelector.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    App.RootTheme = ElementTheme.Default;
                    break;
                case 1:
                    App.RootTheme = ElementTheme.Light;
                    break;
                case 2:
                    App.RootTheme = ElementTheme.Dark;
                    break;
            }
        }
    }
}
