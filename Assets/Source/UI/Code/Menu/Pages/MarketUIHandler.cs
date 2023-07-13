using System;
using System.Collections;
using System.Collections.Generic;
using Source.Code.Views.ManualPowerUp;
using Source.UI.Code.Menu.Pages.Market;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Code.Menu.Pages
{
    public class MarketUIHandler : PageHandler
    {
        #region Const Strings

        private const string TitleTag = "xml-market-title";
        private const string BuyTag = "xml-market-buy";

        #endregion


        #region Serialize Fields

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI buyText;
        [SerializeField] private TextMeshProUGUI coinsAmount;

        [SerializeField] private MarketItem[] items;

        [Space] 
        [Header("Item Info")] 
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemPrice;
        [SerializeField] private Image preview;

        #endregion


        #region Private Fields

        private int _currentItemIndex;

        #endregion
        

        public Action ReturnBack, ChangeButtonClick;

        public Action<ManualPowerUpType, int> BuyPowerUp;
        

        public override void OnPageOpen()
        {
            _currentItemIndex = 0;
            UpdateTexts();
            var itemsInfo = new Dictionary<string, MarketItemInfo>(items.Length);

            StartCoroutine(GetItemsInfo(itemsInfo, () =>
            {
                foreach (var marketItem in items)
                {
                    marketItem.Info = itemsInfo[marketItem.xmlTag];
                }
                SetItem(_currentItemIndex);
            }));
        }

        public override void OnPageClose() { }

        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[TitleTag];
                titleText.text = data.val;
                titleText.fontSize = data.fontSize;
                
                data = PageStrings[BuyTag];
                buyText.text = data.val;
                buyText.fontSize = data.fontSize;
            }));
        }


        public void UpdateCoinsAmount(int coins)
        {
            coinsAmount.text = coins.ToString();
        }


        #region Button Click Handlers

        public void OnReturnButtonClick() => ReturnBack?.Invoke();

        public void OnBuyItemButtonClick()
        {
            var item = items[_currentItemIndex];
            BuyPowerUp?.Invoke(item.type, item.price);
        }

        public void OnNextItemButtonClick() => ChangeItem(1);

        public void OnPreviousItemButtonClick() => ChangeItem(-1);

        #endregion


        #region Private Methods

        private IEnumerator GetItemsInfo(Dictionary<string, MarketItemInfo> itemsInfo, Action callback = null)
        {
            while (!LocalizationHandler.IsLoaded()) yield return null;
            
            LocalizationHandler.GetMarketItems(ref itemsInfo);
            callback?.Invoke();
        }
        
        private void ChangeItem(int direction)
        {
            if (direction < 0)
            {
                if (_currentItemIndex - 1 < 0) _currentItemIndex = 0;
                else _currentItemIndex--;
            }
            else
            {
                if (_currentItemIndex + 1 > items.Length - 1) _currentItemIndex = items.Length - 1;
                else _currentItemIndex++;
            }
            ChangeButtonClick?.Invoke();
            SetItem(_currentItemIndex);
        }

        private void SetItem(int index)
        {
            var item = items[index];

            itemName.text = item.Info.Name.Text;
            itemName.fontSize = item.Info.Name.FontSize;
            
            itemDescription.text = item.Info.Description.Text;
            itemDescription.fontSize = item.Info.Description.FontSize;

            itemPrice.text = item.price.ToString();
            preview.sprite = item.sprite;
        }

        #endregion
    }
}
