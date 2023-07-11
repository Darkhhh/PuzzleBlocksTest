using Source.Localization;
using UnityEngine;

namespace Source.UI.Code.InGamePageManagerScripts
{
    partial class PageManager
    {
        private void SubscribeToSettingsPageEvents()
        {
            _settingsUIHandler.ChangeLanguage = ChangeLanguage;
            
            _settingsUIHandler.ChangeMusicStatus = ChangeMusicStatus;
            
            _settingsUIHandler.ReturnBack = ReturnFromSettings;
            _settingsUIHandler.ReturnBack += PlaySoundOnButtonClick;
        }

        private void UnsubscribeToSettingsPageEvents()
        {
            _settingsUIHandler.ChangeLanguage = null;
            _settingsUIHandler.ChangeMusicStatus = null;
            _settingsUIHandler.ReturnBack = null;
        }
        
        
        private void ChangeLanguage(Language newLang)
        {
            Debug.Log($"Changing To {newLang.ToString()}");
            _localizationHandler.Load(newLang, () =>
            {
                _inGameUIHandler.UpdateTexts();
                _pauseUIHandler.UpdateTexts();
                _settingsUIHandler.UpdateTexts();
            });
        }

        private void ChangeMusicStatus(bool playing)
        {
            var val = playing ? "On" : "Off";
            Debug.Log($"Music {val}");
        }

        private void ReturnFromSettings()
        {
            _settingsUIHandler.gameObject.SetActive(false);
            _pauseUIHandler.OnPageOpen();
        }
    }
}