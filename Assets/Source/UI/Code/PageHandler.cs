using Source.Localization;
using UnityEngine;
using Zenject;

namespace Source.UI.Code
{
    public abstract class PageHandler : MonoBehaviour, IPageHandler
    {
        [Inject] private ILocalizationHandler _localizationHandler;
        private Language _currentLanguage;
        
        
        public virtual void Prepare()
        {
            if (!_localizationHandler.IsLoaded())
            {
                _localizationHandler.Load(_currentLanguage);
            }
        }

        public abstract void OnPageOpen();

        public abstract void OnPageClose();

        public virtual void ChangeLanguage(Language newLanguage)
        {
            _localizationHandler.Load(newLanguage);
            _currentLanguage = newLanguage;
        }


        public ILocalizationHandler GetLocalizationHandler() => _localizationHandler;
        public Language GetCurrentLanguage() => _currentLanguage;
    }
}