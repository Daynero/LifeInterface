using System;

namespace Core.GameTime
{
    [Serializable]
    public enum TimeType
    {
        GameStart,
        PauseStart,
        PauseFinish,
        GameFinish
    }
}