using System;
using System.Collections.Generic;

namespace Source.Localization
{
    public interface ILocalizationHandler
    {
        public void Init();
        
        public bool IsLoaded();

        public void GetPageStrings(string pageTag, ref Dictionary<string, (string val, int fontSize)> strings);

        public void Load(Language language, Action callbackOnCompleted = null);

        public Language GetCurrentLanguage();
    }
}