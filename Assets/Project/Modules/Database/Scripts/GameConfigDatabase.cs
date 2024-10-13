using UnityEngine;

namespace Game.Database
{
    public class GameConfigDatabase<T> : ScriptableObject
    {
        [field: SerializeField] public T Config { get; private set; }

        public void SetData(T config)
        {
            this.Config = config;
        }
    }
}