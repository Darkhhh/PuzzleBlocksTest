using System.Collections.Generic;
using Source.Code.Common.Audio;
using Source.Data;
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
        [Inject] private IDataHandler _dataManager;
        
        private Dictionary<string, PageHandler> _pages = new();

        private void Start()
        {
            _dataManager.Load();
            
            _audioManager.Load().SoundOn(_dataManager.GetData().Settings.MusicOn).Play(SoundTag.BackgroundMusic);
            
            _pages = new Dictionary<string, PageHandler>
            {
                { "xml-main-menu", _menuHandler },
                { "xml-market", _marketHandler },
                { "xml-settings", _settingsHandler }
            };
            
            var langToLoad = LocalizationExtensions.GetLanguage(_dataManager.GetData().Settings.Lang);
            
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
            
            _dataManager.Save(_dataManager.GetData());
        }
        
        private void PlaySoundOnButtonClick()
        {
            _audioManager.Play(SoundTag.ButtonClick);
        }
    }
}
