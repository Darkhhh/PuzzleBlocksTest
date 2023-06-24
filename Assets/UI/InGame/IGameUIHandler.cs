namespace UI.InGame
{
    public interface IGameUIHandler
    {
        public void Init();

        public void SetNewScore(int score);

        public void SetNewCoinsAmount(int coinsAmount);
    }
}