using System.Collections.Generic;

namespace Source.Localization
{
    public interface ILocalizationHandler
    {
        public bool IsLoaded();

        public Dictionary<string, (string text, float fontSize)> GetPageStrings();

        public void Load(Language language);
    }
}