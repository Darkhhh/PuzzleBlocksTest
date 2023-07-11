using Source.Code.Components.Events;

namespace Source.UI.Code.InGamePageManagerScripts
{
    partial class PageManager
    {
        private void SubscribeToGamePageEvents()
        {
            _inGameUIHandler.PauseOpened = OnPausePageOpen;
            _inGameUIHandler.PauseOpened += PlaySoundOnButtonClick;
            
            _inGameUIHandler.SwapItems = OnSwapButtonClick;
        }
        
        private void UnsubscribeToGamePageEvents()
        {
            _inGameUIHandler.PauseOpened = null;
            _inGameUIHandler.SwapItems = null;
        }


        private void OnSwapButtonClick()
        {
            _gameEvents.NewEventSingleton<SwapFiguresAndPowerUpsEvent>();
        }

        private void OnPausePageOpen()
        {
            _sharedData.GameData.Pause = true;
            _pauseUIHandler.gameObject.SetActive(true);
            _pauseUIHandler.OnPageOpen();
        }
    }
}