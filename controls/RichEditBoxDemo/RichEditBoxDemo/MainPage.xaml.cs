using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RichEditBoxDemo
{
    public sealed partial class MainPage : Page
    {
        readonly ITextSelection _currentSelection;

        List<string> _patterns = new List<string>
        {
            @"@[0-9a-zA-Z\u4e00-\u9fa5]+(?=\s|:|/)",
            @"#[0-9a-zA-Z\u4e00-\u9fa5]+#",
            @"\b(([\w-]+://?|www[.])[^\s()<>]+(?:\([\w\d]+\)|([^[:punct:]\s]|/)))"
        };


        public MainPage()
        {
            InitializeComponent();

            _currentSelection = ContentRichEditBox.Document.Selection;
            ContentRichEditBox.Document.Selection.Options = SelectionOptions.Active;
            ContentRichEditBox.DisabledFormattingAccelerators = DisabledFormattingAccelerators.All;
            ContentRichEditBox.IsSpellCheckEnabled = false;
            ContentRichEditBox.Paste += ContentRichEditBox_Paste;
            ContentRichEditBox.TextChanged += ContentRichEditBox_TextChanged;

            string defualtValue = "@everyone\r#topic#\r[笑脸]\rhttp://www.baidu.com\r☺\r这是毛线啊....";
            ContentRichEditBox.Document.SetText(TextSetOptions.None,defualtValue);
        }

        private string _textValue = string.Empty;

        //使用正则表达式 -> 高亮的文本
        private void ContentRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            //文本变化时，如果已经失去了焦点，主动获取焦点
            if (ContentRichEditBox.FocusState == FocusState.Unfocused)
            {
                ContentRichEditBox.Focus(FocusState.Programmatic);
            }

            ContentRichEditBox.Document.GetText(TextGetOptions.None,out var content);
            if (!string.IsNullOrEmpty(_textValue) && _textValue == content)
            {
                return;
            }

            _textValue = content;

            //重置所有文本颜色，重新匹配高亮
            ContentRichEditBox.Document.GetRange(0, content.Length).CharacterFormat.ForegroundColor = Colors.Black;
            

            //通过正则表达式查找
            foreach (var pattern in _patterns)
            {
                var regex = new Regex(pattern);
                if (!regex.IsMatch(content))
                    return;

                foreach (Match match in regex.Matches(content))
                {
                    var textRange = ContentRichEditBox.Document.GetRange(match.Index, match.Index + match.Length);
                    textRange.CharacterFormat.ForegroundColor = Colors.Blue;
                }
            }
        }

        //来自剪贴板内容的限制
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

        private void BoldButtonClick(object sender, RoutedEventArgs e)
        {
            _currentSelection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void UrlButtonClick(object sender, RoutedEventArgs e)
        {
            _currentSelection.SetText(TextSetOptions.None, "http://www.");
            _currentSelection.MoveRight(TextRangeUnit.Character, 1, false);
        }
    }
}
