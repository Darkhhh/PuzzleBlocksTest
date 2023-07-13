using System;
using System.Collections.Generic;
using Source.UI.Code.Menu.Pages.Market;

namespace Source.Localization
{
    public interface ILocalizationHandler
    {
        public void Init();
        
        public bool IsLoaded();

        public void GetPageStrings(string pageTag, ref Dictionary<string, (string val, int fontSize)> strings);

        public void Load(Language language, Action callbackOnCompleted = null);

        public Language GetCurrentLanguage();

        public void GetMarketItems(ref Dictionary<string, MarketItemInfo> strings);
    }
}