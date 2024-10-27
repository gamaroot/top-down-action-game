using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    public class GameStateHandler
    {
        public bool HasSavedGame = PlayerPrefs.HasKey(PlayerPrefsKeys.SAVED_GAME_KEY);

        public GameState GameState;

        public GameStateHandler()
        {
            if (this.HasSavedGame)
                this.Load();
            else
                this.GameState = new();
        }

        public void OnGameStart()
        {
            this.GameState.PlayerState.Init();
        }

        public void Save()
        {
            string gameStateJson = JsonConvert.SerializeObject(this.GameState);

            Debug.Log($"GameState - SAVE: {gameStateJson}");

            PlayerPrefs.SetString(PlayerPrefsKeys.SAVED_GAME_KEY, gameStateJson);
            PlayerPrefs.Save();
        }

        private void Load()
        {
            string gameStateJson = PlayerPrefs.GetString(PlayerPrefsKeys.SAVED_GAME_KEY);
            Debug.Log($"GameState - LOAD: {gameStateJson}");

            this.GameState = JsonConvert.DeserializeObject<GameState>(gameStateJson);
        }
    }
}