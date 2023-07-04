using TMPro;
using UI.InGame;
using UnityEngine;

namespace Source.UI.Code
{
    public class InGameUIHandler : MonoBehaviour, IGameUIHandler
    {
        [SerializeField] private TextMeshProUGUI scoreValueText;
        [SerializeField] private TextMeshProUGUI coinsValueText;

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
    }
}
