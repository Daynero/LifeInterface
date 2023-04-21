using Screens;
using Screens.GameScreen;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        private GameTime.GameTime _gameTime;
        private ScreenNavigationSystem _screenNavigationSystem;
        private GameScreenPresenter _gameScreenPresenter;

        [Inject]
        public void Construct(GameTime.GameTime gameTime,
            ScreenNavigationSystem screenNavigationSystem, GameScreenPresenter gameScreenPresenter)
        {
            _gameTime = gameTime;
            _screenNavigationSystem = screenNavigationSystem;
            _gameScreenPresenter = gameScreenPresenter;
        }
    }
}