using System;
using System.Linq;

namespace Source.Localization
{
    public static class LocalizationExtensions
    {
        public static Language GetLanguage(string lang)
        {
            foreach (var language in GetAllLanguages())
            {
                if (language.ToString() == lang) return language;
            }

            throw new Exception("Incorrect input string");
        }

        public static Language[] GetAllLanguages()
        {
            return Enum.GetValues(typeof(Language)).Cast<Language>().ToArray();
        }
    }
}