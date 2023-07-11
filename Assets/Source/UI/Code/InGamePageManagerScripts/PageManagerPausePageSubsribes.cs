using Source.Code.Components.Events;
using UnityEngine;

namespace Source.UI.Code.InGamePageManagerScripts
{
    partial class PageManager
    {
        private void SubscribeToPausePageEvents()
        {
            _pauseUIHandler.SettingsOpened = OpenSettings;
            _pauseUIHandler.SettingsOpened += PlaySoundOnButtonClick;
            
            _pauseUIHandler.GameOpened = OpenGame;
            _pauseUIHandler.GameOpened += PlaySoundOnButtonClick;
            
            _pauseUIHandler.MenuOpened = OpenMenu;
            _pauseUIHandler.MenuOpened += PlaySoundOnButtonClick;
            
            _pauseUIHandler.RestartClicked = RestartGame;
            _pauseUIHandler.RestartClicked += PlaySoundOnButtonClick;
        }
        
        private void UnsubscribeToPausePageEvents()
        {
            _pauseUIHandler.SettingsOpened = null;
            _pauseUIHandler.GameOpened = null;
            _pauseUIHandler.MenuOpened = null;
            _pauseUIHandler.RestartClicked = null;
        }

        private void OpenSettings()
        {
            _settingsUIHandler.gameObject.SetActive(true);
            _settingsUIHandler.OnPageOpen();
        }

        private void OpenGame()
        {
            _sharedData.GameData.Pause = false;
            _pauseUIHandler.gameObject.SetActive(false);
            _inGameUIHandler.OnPageOpen();
        }

        private void OpenMenu()
        {
            Debug.Log("Exit To Menu");
        }

        private void RestartGame()
        {
            _sharedData.GameData.Pause = false;
            _pauseUIHandler.gameObject.SetActive(false);
            _gameEvents.NewEventSingleton<RestartGameEvent>();
        }
    }
}