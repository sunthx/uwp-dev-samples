using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SharpDX.DirectWrite;
using FontFamily = Windows.UI.Xaml.Media.FontFamily;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace EmotionDemo
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly string _appleFontFile = "ms-appx:///Assets/Font/AppleColorEmoji.ttc";
        private readonly string _googleFontFile = "ms-appx:///Assets/Font/NotoColorEmoji.ttf";
        private readonly string _microsoftFontFile = "ms-appx:///Assets/Font/seguiemj.ttf";

        public MainPage()
        {
            InitializeComponent();

            CustomFontFilePath = new List<string> { _microsoftFontFile, _appleFontFile, _googleFontFile };
            FontFamilyNames = new List<string> { "Segoe UI Emoji", "Apple Color Emoji", "Noto Color Emoji" };
            FactoryDWrite = new Factory();
        }

        public List<string> CustomFontFilePath { set; get; }
        public ResourceFontLoader CurrentResourceFontLoader { get; set; }
        public FontCollection CurrentFontCollection { get; set; }
        public Factory FactoryDWrite { get; private set; }
        public List<Stream> CustomFontStreams { get; set; }
        public List<string> FontFamilyNames { get; set; }

        public Dictionary<string, List<string>> Characters { set; get; }


        private async void LoadCustomFontFileButtonClick(object sender, RoutedEventArgs e)
        {
            MicrosoftFontCharacters.ItemsSource = await LoadCustomFontCharacters(FontFamilyNames[0], CustomFontFilePath[0]);
            AppleFontCharacters.ItemsSource = await LoadCustomFontCharacters(FontFamilyNames[1], CustomFontFilePath[1]);
            GoogleFontCharacters.ItemsSource = await LoadCustomFontCharacters(FontFamilyNames[2], CustomFontFilePath[2]);
        }

        private async Task<List<string>> LoadCustomFontCharacters(string fontfamilyname, string fontFilePath)
        {
            var uri = new Uri(fontFilePath);
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);

            CurrentResourceFontLoader = new ResourceFontLoader(FactoryDWrite, await storageFile.OpenStreamForReadAsync());
            CurrentFontCollection = new FontCollection(FactoryDWrite, CurrentResourceFontLoader,
                CurrentResourceFontLoader.Key);

            var character = new List<string>();

            CurrentFontCollection.FindFamilyName(fontfamilyname, out var familyIndex);
            if (familyIndex == -1)
                return character;

            using (var fontFamily = CurrentFontCollection.GetFontFamily(familyIndex))
            {
                var font = fontFamily.GetFont(0);
                var count = 65536 * 4 - 1;
                for (var i = 0; i < count; i++)
                    if (font.HasCharacter(i))
                        character.Add(char.ConvertFromUtf32(i));
            }

            return character;
        }
    }
}