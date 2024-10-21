namespace Game
{
    public class GameState
    {
        public int Seed;

        public bool[] EnemiesAlive;

        public float PlayerPositionX;
        public float PlayerPositionY;
        
        public PlayerState PlayerState;

        public GameState()
        {
            EnemiesAlive = new bool[0];

            this.PlayerState = new PlayerState
            {
                Level = 1,
                ExtraStats = new int[4]
            };
        }
    }
}
