using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace RichEditBoxDemo
{
    public sealed partial class MainPage : Page
    {
        readonly ITextSelection _currentSelection;

        public MainPage()
        {
            this.InitializeComponent();

            _currentSelection = ContentRichEditBox.Document.Selection;
            ContentRichEditBox.Document.Selection.Options = SelectionOptions.Active;
            ContentRichEditBox.DisabledFormattingAccelerators = DisabledFormattingAccelerators.All;
            ContentRichEditBox.IsSpellCheckEnabled = false;
            ContentRichEditBox.ClipboardCopyFormat = RichEditClipboardFormat.PlainText;
            ContentRichEditBox.Paste += ContentRichEditBox_Paste;
            ContentRichEditBox.TextChanged += ContentRichEditBox_TextChanged;
            string defualtValue = "@everyone\r#topic#\r[笑脸]\rhttp://www.baidu.com\r☺\r这是毛线啊....";
            ContentRichEditBox.Document.SetText(TextSetOptions.None,defualtValue);
        }

        private void ContentRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (ContentRichEditBox.FocusState == FocusState.Unfocused)
            {
                ContentRichEditBox.Focus(FocusState.Programmatic);
            }
        }

        private void ContentRichEditBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            if (ContentRichEditBox.Document.CanPaste())
            {

                var content = Clipboard.GetContent();

                if (content.Contains("text") ||  content.Contains("rtf"))
                {
                    //0 代表最佳格式，通常为RTF格式。
                    //1 代表 CF_TEXT
                    //2 代表 CF_BITMAP : A handle to a bitmap
                    //8 代表 CF_DIB : A memory object containing a BITMAPINFO structure followed by the bitmap bits.
                    //具体的FormatId介绍：https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168.aspx?f=255&MSPPError=-2147217396
                    ContentRichEditBox.Document.Selection.Paste(1);
                }
                if (content.Contains("bitmap"))
                {
                    ContentRichEditBox.Document.Selection.SetText(TextSetOptions.None,"bitmap");
                }

                e.Handled = true;
            }
        }

        private void TopicButtonClick(object sender, RoutedEventArgs e)
        {
            _currentSelection.SetText(TextSetOptions.None, "##");
            _currentSelection.MoveRight(TextRangeUnit.Character, 1,false);
            _currentSelection.MoveLeft(TextRangeUnit.Character, 1, false);
        }

        private void AtButtonClick(object sender, RoutedEventArgs e)
        {
            ContentRichEditBox.Document.Selection.SetText(TextSetOptions.None, "@");
            _currentSelection.MoveRight(TextRangeUnit.Character, 1, false);
        }

        private void EmojiButtonClick(object sender, RoutedEventArgs e)
        {
            ContentRichEditBox.Document.Selection.SetText(TextSetOptions.None, "[可爱]");
            _currentSelection.MoveRight(TextRangeUnit.Character, 1, false);
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            var document = ContentRichEditBox.Document;
            string content = string.Empty;
            document.GetText(TextGetOptions.None, out content);
        }
    }
}
