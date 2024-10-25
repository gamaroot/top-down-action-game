using System;

namespace Game
{
    public struct CharacterHealthEvents
    {
        public Action OnLoseHealth;
        public Action OnRecoverHealth;
        public Action OnDeath;
        public Action OnSelfDestroy;
    }
}
