using SevenBoldPencil.EasyEvents;

namespace Source.UI.InGame
{
    public interface IGameUIHandler
    {
        public void Init(EventsBus events);

        public void SetNewScore(int score);

        public void SetNewCoinsAmount(int coinsAmount);
    }
}