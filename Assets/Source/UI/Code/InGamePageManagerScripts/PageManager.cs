using SevenBoldPencil.EasyEvents;
using Source.Localization;
using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.UI.Code.InGamePageManagerScripts
{
    public partial class PageManager : MonoBehaviour
    {
        [Inject] private InGameUIHandler _inGameUIHandler;
        [Inject] private PauseUIHandler _pauseUIHandler;
        [Inject] private SettingsUIHandler _settingsUIHandler;

        private ILocalizationHandler _localizationHandler;
        private EventsBus _gameEvents;

        public void Init(EventsBus gameEvents, ILocalizationHandler handler, Language langToLoad)
        {
            _gameEvents = gameEvents;
            _localizationHandler = handler;
            _localizationHandler.Load(langToLoad, () =>
            {
                _inGameUIHandler.Init("xml-game");
                _pauseUIHandler.Init("xml-pause");
                _settingsUIHandler.Init("xml-settings");
                
                _inGameUIHandler.OnPageOpen();
            });

            SubscribeToGamePageEvents();
            SubscribeToSettingsPageEvents();
            SubscribeToPausePageEvents();
        }

        
        private void OnDestroy()
        {
            UnsubscribeToSettingsPageEvents();
            UnsubscribeToPausePageEvents();
            UnsubscribeToGamePageEvents();
        }
    }
}