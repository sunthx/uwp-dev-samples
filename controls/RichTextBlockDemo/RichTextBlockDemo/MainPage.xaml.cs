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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace RichTextBlockDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadContnet();
        }

        // #Topic# @AnyOne 这是RichTextBlock的DEMO #Emojis#
        void LoadContnet()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.LineHeight = 40;
            paragraph.LineStackingStrategy = LineStackingStrategy.BaselineToBaseline;

            //link
            InlineUIContainer linkContainer = new InlineUIContainer();
            HyperlinkButton hyperlinkButton = new HyperlinkButton();
            hyperlinkButton.Content = "@敲代码的老年人";
            linkContainer.Child = hyperlinkButton;

            //text
            Run text = new Run();
            text.Text = "富文本格式（Rich Text Format, 一般简称为RTF)，是由微软公司开发的跨平台文档格式。  RTF是Rich TextFormat的缩写，意即多文本格式";

            //images
            InlineUIContainer imagesContainer = new InlineUIContainer();
            Image image = new Image();
            image.Width = 24;
            image.Height = 24;
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/emojis/1.png"));
            imagesContainer.Child = image;

            paragraph.Inlines.Add(linkContainer);
            paragraph.Inlines.Add(text);
            paragraph.Inlines.Add(imagesContainer);

            RichTextBlock2.Blocks.Add(paragraph);
        }
    }
}
