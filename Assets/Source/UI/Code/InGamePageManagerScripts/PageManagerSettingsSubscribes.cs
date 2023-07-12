using Source.Code.Common.Audio;
using Source.Localization;

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
            _localizationHandler.Load(newLang, () =>
            {
                _inGameUIHandler.UpdateTexts();
                _pauseUIHandler.UpdateTexts();
                _settingsUIHandler.UpdateTexts();
            });
        }

        private void ChangeMusicStatus(bool playing)
        {
            _audioManager.SoundOn(playing);
            if (!playing) _audioManager.StopAll();
            else _audioManager.Play(SoundTag.BackgroundMusic);
        }

        private void ReturnFromSettings()
        {
            _settingsUIHandler.gameObject.SetActive(false);
            _pauseUIHandler.OnPageOpen();
        }
    }
}