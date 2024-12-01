using System;
using UnityEngine;

namespace Game.Database
{
    [Serializable]
    public class CharacterConfig : ICharacterConfig
    {
        [field: SerializeField] public CharacterStats Stats { get; set; }
    }
}