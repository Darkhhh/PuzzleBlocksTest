using UnityEngine;
using UnityEngine.UIElements;

namespace UI.InGame
{
    public class InGameUserInterfaceHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument doc;

        private Label _scoreLabel;
        private Button _restartButton;
        
        public bool RestartButtonClicked { get; set; }
        
        public void Initialize()
        {
            _scoreLabel = doc.rootVisualElement.Q("ScorePoints") as Label;
            _restartButton = doc.rootVisualElement.Q("RestartButton") as Button;
            if (_restartButton != null) _restartButton.clicked += RestartButtonClick;
            RestartButtonClicked = false;
        }

        public void SetScore(int score)
        {
            _scoreLabel.text = score.ToString();
        }

        private void RestartButtonClick()
        {
            RestartButtonClicked = true;
        }

        public void ResetClickProperty()
        {
            RestartButtonClicked = false;
        }
    }
}
