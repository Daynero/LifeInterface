using Controllers;
using Screens;
using Screens.DailyBonusPopup;
using Screens.GameScreen;
using Screens.LivesPopup;
using Utils;
using Zenject;

namespace Core.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindControllers();
            BindScreens();
        }
        
        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<ScreenNavigationSystem>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LivesController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoinsController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DailyBonusController>().FromNew().AsSingle().NonLazy();
        }

        private void BindScreens()
        {
            Container.BindViewAndPresenter<GameScreenView, GameScreenPresenter>();
            Container.BindViewAndPresenter<LivesPopupView, LivesPopupPresenter>();
            Container.BindViewAndPresenter<DailyBonusPopupView, DailyBonusPopupPresenter>();
        }
    }
}