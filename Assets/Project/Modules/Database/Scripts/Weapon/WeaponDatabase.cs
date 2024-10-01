using UnityEngine;

namespace Game.Database
{
    [CreateAssetMenu(fileName = "Weapon Database", menuName = "Game/Weapon Database", order = 1)]
    public class WeaponDatabase : ScriptableObject
    {
        [field: SerializeField] public WeaponConfig[] Weapons { get; private set; }
    }
}