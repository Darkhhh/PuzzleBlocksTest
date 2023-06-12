using PuzzleCore.ECS.Views;

namespace PuzzleCore.ECS.Components
{
    public struct ManualPowerUp
    {
        public ManualPowerUpView View;
        public ManualPowerUpType Type => View.Type;
        public int AvailableAmount;
    }
    
    
    public struct Activated { }
}