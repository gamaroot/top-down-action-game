using UnityEngine;

namespace Game
{
    public class SFX : MonoBehaviour
    {
        [Header("Gameplay")]
        [SerializeField] private GameObject[] _prefabs;

        private static Pool[] _pools;

        private void Awake()
        {
            Transform baseTransform = base.transform;

            _pools = new Pool[this._prefabs.Length];
            for (int index = 0; index < this._prefabs.Length; index++)
            {
                _pools[index] = new(baseTransform, this._prefabs[index]);
            }
        }

        public static void Play(SFXType type)
        {
            float volume = GamePreferences.SoundVolume;
            if (volume == 0)
                return;

            AudioSource audioSource = _pools[(int)type].BorrowObject<AudioSource>(volume);
            audioSource.volume = volume;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
        }
    }
}