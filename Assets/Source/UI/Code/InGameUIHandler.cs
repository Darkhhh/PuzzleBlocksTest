using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.Localization;
using Source.UI.InGame;
using TMPro;
using UI.InGame;
using UnityEngine;

namespace Source.UI.Code
{
    public class InGameUIHandler : PageHandler, IGameUIHandler
    {
        [SerializeField] private PauseUIHandler pauseUIHandler;
        
        [SerializeField] private TextMeshProUGUI scoreValueText;
        [SerializeField] private TextMeshProUGUI coinsValueText;

        private EventsBus _events;
        //private ILocalizationHandler _localizationHandler;
        //private Language _currentLanguage;


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
            //pauseUIHandler.Prepare(_localizationHandler, _currentLanguage);
            pauseUIHandler.OnPageOpen();
        }

        #endregion
        

        

        private void UpdateTexts()
        {
            
        }

        public override void OnPageOpen()
        {
            
        }

        public override void OnPageClose()
        {
            
        }
    }
}
