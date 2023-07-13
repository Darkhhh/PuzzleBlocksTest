using Source.Code.Views.ManualPowerUp;
using UnityEngine;

namespace Source.UI.Code.Menu.MenuPageManagerScripts
{
    public partial class MenuPageManager
    {
        private void SubscribeToMarketPageEvents()
        {
            _marketHandler.BuyPowerUp = BuyManualPowerUp;

            _marketHandler.ReturnBack = BackToMenuFromMarket;
            _marketHandler.ReturnBack += PlaySoundOnButtonClick;
            
            _marketHandler.ChangeButtonClick += PlaySoundOnButtonClick;
        }
        
        private void UnsubscribeToMarketPageEvents()
        {
            _marketHandler.BuyPowerUp = null;
            _marketHandler.ReturnBack = null;
            _marketHandler.ChangeButtonClick = null;
        }


        private void BuyManualPowerUp(ManualPowerUpType type, int price)
        {
            if (_coinsAmount - price < 0) return;
            
            _coinsAmount -= price;
            _marketHandler.UpdateCoinsAmount(_coinsAmount);
        }

        private void BackToMenuFromMarket()
        {
            _marketHandler.gameObject.SetActive(false);
            _menuHandler.gameObject.SetActive(true);
            _menuHandler.OnPageOpen();
        }
    }
}