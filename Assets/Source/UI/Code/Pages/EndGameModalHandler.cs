using System;
using TMPro;
using UnityEngine;

namespace Source.UI.Code.Pages
{
    public class EndGameModalHandler : PageHandler
    {
        #region Const Strings

        private const string TitleTextTag = "xml-title";
        private const string RestartTextTag = "xml-restart";
        private const string MenuTextTag = "xml-menu";

        #endregion
        
        
        #region Serialize Fields

        [SerializeField] private TextMeshProUGUI titleText;
        
        [SerializeField] private TextMeshProUGUI restartText;

        [SerializeField] private TextMeshProUGUI menuText;

        #endregion
        
        
        public Action RestartGame, BackToMenu;
    
    
        public override void OnPageOpen() => UpdateTexts();

        public override void OnPageClose() { }

        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[TitleTextTag];
                titleText.text = data.val;
                titleText.fontSize = data.fontSize;
                
                data = PageStrings[RestartTextTag];
                restartText.text = data.val;
                restartText.fontSize = data.fontSize;
                
                data = PageStrings[MenuTextTag];
                menuText.text = data.val;
                menuText.fontSize = data.fontSize;
            }));
        }
        
        
        #region ButtonClickHandlers

        public void OnRestartButtonClick() => RestartGame?.Invoke();

        public void OnMenuButtonClick() => BackToMenu?.Invoke();

        #endregion
    }
}
