using Source.Localization;

namespace Source.UI.Code
{
    public interface IPageHandler
    {
        public void Prepare(ILocalizationHandler localizationHandler, Language currentLanguage);

        public void OnPageOpen();

        public void OnPageClose();

        public void ChangeLanguage(Language newLanguage);
    }
}