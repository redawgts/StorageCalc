using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace StorageCalc.Resources
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        public static LocalizedStrings Instance { get; set; } = new LocalizedStrings();

        public void SetCulture(string cultureCode)
        {
            var newCulture = new CultureInfo(cultureCode);
            LocalizeDictionary.Instance.Culture = newCulture;
        }

        public string this[string key]
        {
            get
            {
                var result = LocalizeDictionary.Instance.GetLocalizedObject("StorageCalc", "Strings", key, LocalizeDictionary.Instance.Culture);
                return result as string;
            }
        }
    }
}