using System;
using Source.UI.InGame;
using TMPro;
using UnityEngine;

namespace Source.UI.Code.Pages
{
    public class InGameUIHandler : PageHandler, IGameUIHandler
    {
        #region Const Strings

        private const string ScoreTextTag = "xml-score";

        #endregion
        
        
        #region Serialize Fields

        [SerializeField] private TextMeshProUGUI scoreValueText;
        
        [SerializeField] private TextMeshProUGUI coinsValueText;

        [SerializeField] private TextMeshProUGUI scoreText;

        #endregion


        public Action PauseOpened, SwapItems;


        #region IGameUIHandler

        public void Init()
        {
            SetNewScore(0);
            SetNewCoinsAmount(0);
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

        public void OnSwapButtonClick() => SwapItems?.Invoke();

        public void OnPauseButtonClick() => PauseOpened?.Invoke();

        #endregion

        
        public override void OnPageOpen() => UpdateTexts();

        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[ScoreTextTag];
                scoreText.text = data.val;
                scoreText.fontSize = data.fontSize;
            }));
        }

        public override void OnPageClose() { }
    }
}
