using Controllers;
using Screens;
using Screens.DailyBonusPopup;
using Screens.GamePausedPopup;
using Screens.GameScreen;
using Utils;
using Zenject;

namespace Core.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindControllers();
            BindViews();
            BindScreens();
        }
        
        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<GameController>().FromComponentsInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AnimationsController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScreenNavigationSystem>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameTime.GameTime>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainThreadDispatcher>().FromComponentsInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LivesController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoinsController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DailyBonusController>().FromNew().AsSingle().NonLazy();
        }
        
        private void BindViews()
        {
        }
        
        private void BindScreens()
        {
            Container.BindViewAndPresenter<GameScreenView, GameScreenPresenter>();
            Container.BindViewAndPresenter<LivesPopupView, LivesPopupPresenter>();
            Container.BindViewAndPresenter<DailyBonusPopupView, DailyBonusPopupPresenter>();
        }
    }
}