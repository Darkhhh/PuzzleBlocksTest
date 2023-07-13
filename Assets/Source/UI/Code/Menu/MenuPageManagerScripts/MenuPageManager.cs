using System;
using System.Collections.Generic;
using Source.Code.Common.Audio;
using Source.Localization;
using Source.UI.Code.Menu.Pages;
using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.UI.Code.Menu.MenuPageManagerScripts
{
    public partial class MenuPageManager : MonoBehaviour
    {
        [Inject] private MenuUIHandler _menuHandler;
        [Inject] private MarketUIHandler _marketHandler;
        [Inject] private SettingsUIHandler _settingsHandler;

        [Inject] private AudioManager _audioManager;
        [Inject] private ILocalizationHandler _localizationHandler;
        
        private Dictionary<string, PageHandler> _pages = new();


        private int _coinsAmount = 35000;

        private void Start()
        {
            _audioManager.LoadAndPlay(SoundTag.BackgroundMusic);
            
            _pages = new Dictionary<string, PageHandler>
            {
                { "xml-main-menu", _menuHandler },
                { "xml-market", _marketHandler },
                { "xml-settings", _settingsHandler }
            };
            
            var langToLoad = Language.Russian;
            
            _localizationHandler.Load(langToLoad, () =>
            {
                foreach (var item in _pages)
                {
                    item.Value.Init(item.Key);
                }
                
                _menuHandler.OnPageOpen();
            });
            
            SubscribeToMenuPageEvents();
            SubscribeToMarketPageEvents();
            SubscribeToSettingsPageEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeToMenuPageEvents();
            UnsubscribeToMarketPageEvents();
            UnsubscribeToSettingsPageEvents();
        }
        
        private void PlaySoundOnButtonClick()
        {
            _audioManager.Play(SoundTag.ButtonClick);
        }
    }
}
