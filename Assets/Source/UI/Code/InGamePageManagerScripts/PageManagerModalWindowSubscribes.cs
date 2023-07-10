using Source.Code.Components.Events;

namespace Source.UI.Code.InGamePageManagerScripts
{
    public partial class PageManager
    {
        private void SubscribeToModalPageEvents()
        {
            _modalUIHandler.RestartGame = RestartGameFromModalWindow;
            _modalUIHandler.BackToMenu = OpenMenu;
        }
        
        private void UnsubscribeToModalPageEvents()
        {
            _modalUIHandler.RestartGame = null;
            _modalUIHandler.BackToMenu = null;
        }
        
        
        private void RestartGameFromModalWindow()
        {
            _sharedData.GameData.Pause = false;
            _modalUIHandler.gameObject.SetActive(false);
            _gameEvents.NewEventSingleton<RestartGameEvent>();
        }
    }
}