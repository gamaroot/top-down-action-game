using UnityEngine;

namespace Game.Database
{
    public interface IPlayerConfig : ICharacterConfig
    {
        LayerMask InvincibilityLayerMask { get; }
        float SpawnInvincibilityDuration { get; }
        int InitialStatsPoints { get; }
        int StatsPointsPerLevel { get; }
        CharacterStats ExtraStatsPerPoint { get; }
        float GetXpToNextLevel(int level);
        int GetTotalStatsPoints(int level);
        (float, float) GetStatsConfigByIndex(int index);
        float GetMaxPointsForStatsByIndex(int index);
    }
}
