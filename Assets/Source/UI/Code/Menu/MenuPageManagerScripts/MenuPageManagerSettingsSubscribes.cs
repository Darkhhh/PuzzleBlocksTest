using Source.Code.Common.Audio;
using Source.Localization;

namespace Source.UI.Code.Menu.MenuPageManagerScripts
{
    public partial class MenuPageManager
    {
        private void SubscribeToSettingsPageEvents()
        {
            _settingsHandler.ChangeLanguage = ChangeLanguage;
            
            _settingsHandler.ChangeMusicStatus = ChangeMusicStatus;
            
            _settingsHandler.ReturnBack = ReturnFromSettings;
            _settingsHandler.ReturnBack += PlaySoundOnButtonClick;
        }
        
        private void UnsubscribeToSettingsPageEvents()
        {
            _settingsHandler.ChangeLanguage = null;
            _settingsHandler.ChangeMusicStatus = null;
            _settingsHandler.ReturnBack = null;
        }
        
        
        private void ChangeLanguage(Language newLang)
        {
            _dataManager.GetData().Settings.Lang = newLang.ToString();
            
            _localizationHandler.Load(newLang, () =>
            {
                _settingsHandler.UpdateTexts();
            });
        }

        private void ChangeMusicStatus(bool playing)
        {
            _dataManager.GetData().Settings.MusicOn = playing;
            
            _audioManager.SoundOn(playing);
            if (!playing) _audioManager.StopAll();
            else _audioManager.Play(SoundTag.BackgroundMusic);
        }

        private void ReturnFromSettings()
        {
            _settingsHandler.gameObject.SetActive(false);
            _menuHandler.gameObject.SetActive(true);
            _menuHandler.OnPageOpen();
        }
    }
}