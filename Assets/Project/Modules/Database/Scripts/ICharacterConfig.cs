namespace Game.Database
{
    public interface ICharacterConfig
    {
        float MaxHealth { get; }
        SpawnTypeExplosion DeathVFX { get; }
        SFXTypeExplosion DeathSFX { get; }
        float MovementSpeed { get; }
        float DashSpeed { get; }
        float DashDuration { get; }
        float DashCooldown { get; }
        float ParryDuration { get; }
        float ParryCooldown { get; }
    }
}