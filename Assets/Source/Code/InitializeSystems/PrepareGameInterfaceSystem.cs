using Leopotam.EcsLite;
using Source.Localization;
using Source.UI.Code;

namespace Source.Code.InitializeSystems
{
    public class PrepareGameInterfaceSystem : IEcsInitSystem
    {
        private readonly PageHandler _handler;
        private readonly ILocalizationHandler _localizationHandler;

        public PrepareGameInterfaceSystem(PageHandler handler, ILocalizationHandler localizationHandler)
        {
            _handler = handler;
            _localizationHandler = localizationHandler;
        }

        public void Init(IEcsSystems systems)
        {
            _localizationHandler.Init();
            _handler.Prepare(_localizationHandler, Language.English);
        }
    }
}