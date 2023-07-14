using System;
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
            if (_dataManager.GetData().GameData.coinsAmount - price < 0) return;
            
            _dataManager.GetData().GameData.coinsAmount -= price;
            _marketHandler.UpdateCoinsAmount(_dataManager.GetData().GameData.coinsAmount);

            switch (type)
            {
                case ManualPowerUpType.CanonBall:
                    _dataManager.GetData().GameData.canonBallAmount++;
                    break;
                case ManualPowerUpType.Broomstick:
                    _dataManager.GetData().GameData.broomstickAmount++;
                    break;
                case ManualPowerUpType.Dynamite:
                    _dataManager.GetData().GameData.dynamiteAmount++;
                    break;
                case ManualPowerUpType.LargeDynamite:
                    _dataManager.GetData().GameData.largeDynamiteAmount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void BackToMenuFromMarket()
        {
            _marketHandler.gameObject.SetActive(false);
            _menuHandler.gameObject.SetActive(true);
            _menuHandler.OnPageOpen();
        }
    }
}