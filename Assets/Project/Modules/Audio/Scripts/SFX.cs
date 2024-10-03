using UnityEngine;

namespace Game
{
    public class SFX : MonoBehaviour
    {
        [SerializeField] private GameObject[] _uiSfxPrefabs;
        [SerializeField] private GameObject[] _projectileSfxPrefabs;
        [SerializeField] private GameObject[] _explosionSfxPrefabs;
        [SerializeField] private GameObject[] _otherSfxPrefabs;

        private readonly static Pool[][] _pools = new Pool[4][];

        private void Awake()
        {
            Transform baseTransform = base.transform;
            this.CreatePool(0, baseTransform, this._uiSfxPrefabs);
            this.CreatePool(1, baseTransform, this._projectileSfxPrefabs);
            this.CreatePool(2, baseTransform, this._explosionSfxPrefabs);
            this.CreatePool(3, baseTransform, this._otherSfxPrefabs);
        }

        public static AudioSource PlayUI(SFXTypeUI type)
        {
            return Play(0, (int)type);
        }

        public static AudioSource PlayProjectile(SFXTypeProjectile type)
        {
            return Play(1, (int)type);
        }

        public static AudioSource PlayExplosion(SFXTypeExplosion type)
        {
            return Play(2, (int)type);
        }

        public static AudioSource PlayOther(SFXTypeOther type)
        {
            return Play(3, (int)type);
        }

        private static AudioSource Play(int order, int type)
        {
            float volume = GamePreferences.SoundVolume;
            if (volume == 0)
                return null;

            AudioSource audioSource = _pools[order][type].BorrowObject<AudioSource>();
            audioSource.volume = volume;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();

            return audioSource;
        }

        private void CreatePool(int order, Transform baseTransform, GameObject[] prefabs)
        {
            _pools[order] = new Pool[prefabs.Length];
            for (int index = 0; index < prefabs.Length; index++)
            {
                _pools[order][index] = new(baseTransform, prefabs[index]);
            }
        }
    }
}