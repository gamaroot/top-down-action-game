namespace Game.Database
{
    public interface ICharacterConfig
    {
        CharacterStats Stats { get; }
        SpawnTypeExplosion DeathVFX { get; }
        SFXTypeExplosion DeathSFX { get; }
    }
}