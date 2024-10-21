using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class CharacterConfig : ICharacterConfig
    {
        [field: SerializeField] public CharacterStats Stats { get; set; }
        [field: SerializeField] public SpawnTypeExplosion DeathVFX { get; set; }
        [field: SerializeField] public SFXTypeExplosion DeathSFX { get; set; }
    }
}