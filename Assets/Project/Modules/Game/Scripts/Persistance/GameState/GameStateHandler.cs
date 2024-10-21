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

        public GameState Load()
        {
            string gameStateJson = PlayerPrefs.GetString(PlayerPrefsKeys.SAVED_GAME_KEY);
            Debug.Log($"GameState - LOAD: {gameStateJson}");

            return this.GameState = JsonConvert.DeserializeObject<GameState>(gameStateJson);
        }

        public void Save()
        {
            string gameStateJson = JsonConvert.SerializeObject(this.GameState);

            Debug.Log($"GameState - SAVE: {gameStateJson}");

            PlayerPrefs.SetString(PlayerPrefsKeys.SAVED_GAME_KEY, gameStateJson);
            PlayerPrefs.Save();
        }

        public void DeleteSavedGame()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.SAVED_GAME_KEY);
            PlayerPrefs.Save();

            Debug.Log("GameState - DELETED");
        }
    }
}