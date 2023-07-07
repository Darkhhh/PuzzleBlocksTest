using SevenBoldPencil.EasyEvents;
using Source.Code.Components.Events;
using Source.UI.InGame;
using TMPro;
using UI.InGame;
using UnityEngine;

namespace Source.UI.Code
{
    public class InGameUIHandler : MonoBehaviour, IGameUIHandler
    {
        [SerializeField] private TextMeshProUGUI scoreValueText;
        [SerializeField] private TextMeshProUGUI coinsValueText;

        private EventsBus _events;
        
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

        public void OnSwapButtonClick()
        {
            _events.NewEventSingleton<SwapFiguresAndPowerUpsEvent>();
        }
    }
}
