namespace Game.Database
{
    public interface IPlayerConfig : ICharacterConfig
    {
        int StatsPointsPerLevel { get; }
        float XpToNextLevel { get; }
        float XpToNextLevelFactor { get; }
    }
}
