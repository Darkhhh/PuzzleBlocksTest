using System.Collections;
using System.Collections.Generic;
using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Localization;
using Source.UI.InGame;
using TMPro;
using UnityEngine;

namespace Source.UI.Code
{
    public class InGameUIHandler : PageHandler, IGameUIHandler
    {
        #region Const Strings

        private const string PageTag = "xml-game";

        private const string ScoreTextTag = "xml-score";

        #endregion
        
        
        #region Serialize Fields

        [SerializeField] private PauseUIHandler pauseUIHandler;
        
        [SerializeField] private TextMeshProUGUI scoreValueText;
        
        [SerializeField] private TextMeshProUGUI coinsValueText;

        [SerializeField] private TextMeshProUGUI scoreText;

        #endregion

        
        
        #region Private Fields

        private EventsBus _events;

        #endregion
        


        #region IGameUIHandler

        public void Init(EventsBus events)
        {
            SetNewScore(0);
            SetNewCoinsAmount(0);
            _events = events;
        }

        public void SetNewScore(int score)
        {
            scoreValueText.text = score.ToString();
        }

        public void SetNewCoinsAmount(int coinsAmount)
        {
            coinsValueText.text = coinsAmount.ToString();
        }

        #endregion


        
        #region ButtonClickHandlers

        public void OnSwapButtonClick()
        {
            _events.NewEventSingleton<SwapFiguresAndPowerUpsEvent>();
        }

        public void OnPauseButtonClick()
        {
            pauseUIHandler.gameObject.SetActive(true);
            pauseUIHandler.Prepare(GetLocalizationHandler(), GetCurrentLanguage());
            pauseUIHandler.OnPageOpen();
        }

        #endregion

        
        public override void OnPageOpen()
        {
            
        }

        public override void OnPageClose()
        {
            
        }

        public override void Prepare(ILocalizationHandler localizationHandler, Language currentLanguage)
        {
            base.Prepare(localizationHandler, currentLanguage);
            StartCoroutine(SetAllTexts(localizationHandler));
        }
        
        private IEnumerator SetAllTexts(ILocalizationHandler localizationHandler)
        {
            while (!localizationHandler.IsLoaded())
            {
                yield return null;
            }

            var strings = new Dictionary<string, (string val, int fontSize)>();
            localizationHandler.GetPageStrings(PageTag, ref strings);
        
        
            var data = strings[ScoreTextTag];
            scoreText.text = data.val;
            scoreText.fontSize = data.fontSize;
        }
    }
}
