using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FileAndDirectoryDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            StorageFiles = new ObservableCollection<StorageFile>();
            DataContext = this;
        }

        public ObservableCollection<StorageFile> StorageFiles { get; set; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //文件选择器
            FileOpenPicker picker  = new FileOpenPicker();

            //设置文件类型
            picker.FileTypeFilter.Add(".txt");

            //打开文件选择器
            var selectedResult = await picker.PickMultipleFilesAsync();
            foreach (var storageFile in selectedResult)
            {
                StorageFiles.Add(storageFile);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();
            picker.FileTypeFilter.Add(".txt");
            var selectedResult = await picker.PickSingleFolderAsync();
            if (selectedResult == null)
            {
                return;
            }

            var files = await selectedResult.GetFilesAsync();
            StorageFiles.Clear();
            foreach (var storageFile in files)
            {
                StorageFiles.Add(storageFile);
            }
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var currentSelectedStorageFile = e.ClickedItem as StorageFile;
            if (currentSelectedStorageFile == null)
            {
                return;
            }

            TbFileName.Text = currentSelectedStorageFile.Name;
            var properties = await currentSelectedStorageFile.GetBasicPropertiesAsync();
            TbFileProperty.Text = (properties.Size / 1024) + "Kb";

            var text = await FileIO.ReadTextAsync(currentSelectedStorageFile);
            TbContent.Text = text;
        }
    }
}
