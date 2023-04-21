using System;
using UniRx;
using Unity.VisualScripting;

namespace Screens.GameScreen
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public GameScreenPresenter(GameScreenView view,
            ScreenNavigationSystem screenNavigationSystem)
        {
            _view = view;
            _screenNavigationSystem = screenNavigationSystem;
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}