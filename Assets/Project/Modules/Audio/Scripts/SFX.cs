using UnityEngine;

namespace Game
{
    public class SFX : MonoBehaviour
    {
        [SerializeField] private GameObject _deathSfxPrefab;
        [SerializeField] private GameObject _kamikazeSfxPrefab;
        [SerializeField] private GameObject[] _uiSfxPrefabs;
        [SerializeField] private GameObject[] _bulletSfxPrefabs;
        [SerializeField] private GameObject[] _bullectImpactSfxPrefabs;
        [SerializeField] private GameObject[] _pickupSfxPrefabs;

        private readonly static Pool[][] _pools = new Pool[6][];

        private void Awake()
        {
            Transform baseTransform = base.transform;
            this.CreatePool(0, baseTransform, this._deathSfxPrefab);
            this.CreatePool(1, baseTransform, this._kamikazeSfxPrefab);
            this.CreatePool(2, baseTransform, this._uiSfxPrefabs);
            this.CreatePool(3, baseTransform, this._bulletSfxPrefabs);
            this.CreatePool(4, baseTransform, this._bullectImpactSfxPrefabs);
            this.CreatePool(5, baseTransform, this._pickupSfxPrefabs);
        }

        public static void PlayDeath()
        {
            Play(0, 0);
        }

        public static void PlayKamikaze()
        {
            Play(1, 0);
        }

        public static void PlayUI(SFXTypeUI type)
        {
            Play(2, (int)type);
        }

        public static void PlayBullet(WeaponType type)
        {
            Play(3, (int)type);
        }

        public static void PlayBulletImpact(WeaponType type)
        {
            Play(4, (int)type);
        }

        public static void PlayPickup(SpawnTypePickup type)
        {
            Play(5, (int)type);
        }

        private static void Play(int order, int type)
        {
            float volume = GamePreferences.SoundVolume;
            if (volume == 0)
                return;

            AudioSource audioSource = _pools[order][type].BorrowObject<AudioSource>();
            audioSource.volume = volume;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
        }

        private void CreatePool(int order, Transform baseTransform, GameObject[] prefabs)
        {
            _pools[order] = new Pool[prefabs.Length];
            for (int index = 0; index < prefabs.Length; index++)
            {
                _pools[order][index] = new(baseTransform, prefabs[index]);
            }
        }

        private void CreatePool(int order, Transform baseTransform, GameObject prefab)
        {
            _pools[order] = new Pool[1];
            _pools[order][0] = new(baseTransform, prefab);
        }
    }
}