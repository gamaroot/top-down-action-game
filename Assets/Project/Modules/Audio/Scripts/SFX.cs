using UnityEngine;

namespace Game
{
    public class SFX : MonoBehaviour
    {
        [Header("Gameplay")]
        [SerializeField] private GameObject _prefabShoot;
        [SerializeField] private GameObject _prefabReload;
        [SerializeField] private GameObject _prefabsEmptyMagazine;

        [Header("UI")]
        [SerializeField] private GameObject _prefabButtonClick;

        private static Pool[] _pools;

        private void Awake()
        {
            Transform baseTransform = base.transform;
            _pools = new Pool[]
            {
                new(baseTransform, _prefabShoot),
                new(baseTransform, _prefabReload),
                new(baseTransform, _prefabsEmptyMagazine),
                new(baseTransform, _prefabButtonClick),
            };
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