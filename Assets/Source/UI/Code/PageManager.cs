using Source.Localization;
using UnityEngine;

namespace Source.UI.Code
{
    public class PageManager : MonoBehaviour
    {
        private PageHandler _inGamePage, _pausePage, _settingsPage;

        private ILocalizationHandler _localizationHandler;

        public void Init(ILocalizationHandler handler, Language langToLoad)
        {
            _localizationHandler = handler;
            _localizationHandler.Load(langToLoad);
        }
    }
}