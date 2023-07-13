using System;
using TMPro;
using UnityEngine;

namespace Source.UI.Code.Menu.Pages
{
    public class MenuUIHandler : PageHandler
    {
        #region Const Strings

        private const string PlayTag = "xml-play-button";
        private const string MarketTag = "xml-market-button";
        private const string SettingsButton = "xml-settings-button";

        #endregion


        #region Serialize Fields

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI playText;
        [SerializeField] private TextMeshProUGUI marketText;
        [SerializeField] private TextMeshProUGUI settingsText;

        #endregion

        public Action PlayButtonClick, MarketButtonClick, SettingsButtonClick;
        
        public override void OnPageOpen() => UpdateTexts();

        public override void OnPageClose() { }

        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[PlayTag];
                playText.text = data.val;
                playText.fontSize = data.fontSize;
                
                data = PageStrings[MarketTag];
                marketText.text = data.val;
                marketText.fontSize = data.fontSize;
                
                data = PageStrings[SettingsButton];
                settingsText.text = data.val;
                settingsText.fontSize = data.fontSize;
            }));
        }


        #region Button Click Handlers

        public void OnPlayButtonClick() => PlayButtonClick?.Invoke();
        public void OnMarketButtonClick() => MarketButtonClick?.Invoke();
        public void OnSettingsButtonClick() => SettingsButtonClick?.Invoke();

        #endregion
    }
}
