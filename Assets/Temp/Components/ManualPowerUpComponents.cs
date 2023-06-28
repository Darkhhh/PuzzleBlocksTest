using Temp.Views;

namespace Temp.Components
{
    public struct ManualPowerUp
    {
        public ManualPowerUpView View;
        public ManualPowerUpType Type => View.Type;
        public int AvailableAmount;
    }
    
    
    public struct Activated { }
}