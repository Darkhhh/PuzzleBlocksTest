using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.UI.Code.Menu.MenuPageManagerScripts
{
    public partial class MenuPageManager
    {
        private void SubscribeToMenuPageEvents()
        {
            _menuHandler.PlayButtonClick = OnPlayButtonClick;
            
            _menuHandler.MarketButtonClick = OnMarketButtonClick;
            _menuHandler.MarketButtonClick += PlaySoundOnButtonClick;
            
            _menuHandler.SettingsButtonClick = OnSettingsButtonClick;
            _menuHandler.SettingsButtonClick += PlaySoundOnButtonClick;
        }
        
        private void UnsubscribeToMenuPageEvents()
        {
            _menuHandler.PlayButtonClick = null;
            _menuHandler.MarketButtonClick = null;
            _menuHandler.SettingsButtonClick = null;
        }


        private void OnPlayButtonClick()
        {
            SceneManager.LoadSceneAsync("InGameScene");
        }

        private void OnMarketButtonClick()
        {
            _menuHandler.gameObject.SetActive(false);
            _marketHandler.gameObject.SetActive(true);
            _marketHandler.OnPageOpen();
            _marketHandler.UpdateCoinsAmount(_dataManager.GetData().GameData.coinsAmount);
        }

        private void OnSettingsButtonClick()
        {
            _menuHandler.gameObject.SetActive(false);
            _settingsHandler.gameObject.SetActive(true);
            _settingsHandler.OnPageOpen();
            _settingsHandler.SetMusicValue(_audioManager.IsSoundOn);
        }
    }
}