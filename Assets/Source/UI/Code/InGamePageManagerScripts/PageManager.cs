using System;
using System.Collections.Generic;
using SevenBoldPencil.EasyEvents;
using Source.Code.Common.Audio;
using Source.Code.SharedData;
using Source.Localization;
using Source.UI.Code.Pages;
using UnityEngine;
using Zenject;

namespace Source.UI.Code.InGamePageManagerScripts
{
    public partial class PageManager : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;
        
        [Inject] private InGameUIHandler _inGameUIHandler;
        [Inject] private PauseUIHandler _pauseUIHandler;
        [Inject] private SettingsUIHandler _settingsUIHandler;
        [Inject] private EndGameModalHandler _modalUIHandler;

        private ILocalizationHandler _localizationHandler;
        
        private EventsBus _gameEvents;

        private SystemsSharedData _sharedData;

        private Dictionary<string, PageHandler> _pages = new();

        public void Init(SystemsSharedData sharedData, ILocalizationHandler handler, Language langToLoad)
        {
            _sharedData = sharedData;
            _gameEvents = sharedData.EventsBus;
            _localizationHandler = handler;
            
            _pages = new Dictionary<string, PageHandler>
            {
                { "xml-game", _inGameUIHandler },
                { "xml-pause", _pauseUIHandler },
                { "xml-settings", _settingsUIHandler },
                { "xml-endgame", _modalUIHandler }
            };
            
            _localizationHandler.Load(langToLoad, () =>
            {
                foreach (var item in _pages)
                {
                    item.Value.Init(item.Key);
                }
                
                _inGameUIHandler.OnPageOpen();
            });

            SubscribeToGamePageEvents();
            SubscribeToSettingsPageEvents();
            SubscribeToPausePageEvents();
            SubscribeToModalPageEvents();
        }

        
        private void OnDestroy()
        {
            UnsubscribeToSettingsPageEvents();
            UnsubscribeToPausePageEvents();
            UnsubscribeToGamePageEvents();
            UnsubscribeToModalPageEvents();
        }


        public void OpenPage(string pageTag)
        {
            if (_pages[pageTag].gameObject.activeSelf) throw new Exception("Trying to open already opened page");
            _pages[pageTag].gameObject.SetActive(true);
            _pages[pageTag].OnPageOpen();
        }
        
        public void ClosePage(string pageTag)
        {
            _pages[pageTag].OnPageClose();
            _pages[pageTag].gameObject.SetActive(false);
        }


        private void PlaySoundOnButtonClick()
        {
            _audioManager.Play(SoundTag.ButtonClick);
        }
    }
}