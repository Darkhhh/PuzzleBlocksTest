using System;
using Source.Code.Views.ManualPowerUp;
using UnityEngine;

namespace Source.UI.Code.Menu.Pages.Market
{
    [Serializable]
    public class MarketItem
    {
        public ManualPowerUpType type;
        
        public Sprite sprite;

        public string xmlTag;

        public int price;

        [HideInInspector] public MarketItemInfo Info;
    }
}