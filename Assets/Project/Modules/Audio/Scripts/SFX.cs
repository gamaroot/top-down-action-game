using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class SFX : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabsShoot;
        [SerializeField] private GameObject _prefabsReload;
        [SerializeField] private GameObject _prefabsEmptyMagazine;
        [SerializeField] private GameObject _prefabButtonClick;
        [SerializeField] private GameObject _prefabButtonHover;

        private static Pool[] _pools;

        private void Awake()
        {
            Transform baseTransform = base.transform;
            _pools = new Pool[]
            {
                new(baseTransform, _prefabsShoot),
                new(baseTransform, _prefabsReload),
                new(baseTransform, _prefabsEmptyMagazine),
                new(baseTransform, _prefabButtonClick),
                new(baseTransform, _prefabButtonHover)
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