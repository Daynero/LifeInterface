using System;

namespace Screens
{
    public abstract class ScreenPresenter
    {
        public abstract void ShowScreen(object extraData = null);
        public abstract void CloseScreen();

        public Action OnCloseAction;
    }
}