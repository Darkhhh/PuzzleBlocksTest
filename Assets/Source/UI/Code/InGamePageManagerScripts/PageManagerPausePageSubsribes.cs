using Source.Code.Components.Events;
using UnityEngine;

namespace Source.UI.Code.InGamePageManagerScripts
{
    partial class PageManager
    {
        private void SubscribeToPausePageEvents()
        {
            _pauseUIHandler.SettingsOpened = OpenSettings;
            _pauseUIHandler.GameOpened = OpenGame;
            _pauseUIHandler.MenuOpened = OpenMenu;
            _pauseUIHandler.RestartClicked = RestartGame;
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
            _pauseUIHandler.gameObject.SetActive(false);
            _inGameUIHandler.OnPageOpen();
        }

        private void OpenMenu()
        {
            Debug.Log("Exit To Menu");
        }

        private void RestartGame()
        {
            _pauseUIHandler.gameObject.SetActive(false);
            _gameEvents.NewEventSingleton<RestartGameEvent>();
        }
    }
}