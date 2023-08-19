using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace EpicToonFX
{
    public class EtfxEffectController : MonoBehaviour
    {
        public GameObject[] effects;
        private int _effectIndex = 0;

        [Space(10)]

        [Header("Spawn Settings")]
        public bool disableLights = true;
        public bool disableSound = true;
        public float startDelay = 0.2f;
        public float respawnDelay = 0.5f;
        public bool slideshowMode = false;
        public bool autoRotation = false;
        [Range(0.001f, 0.5f)]
        public float autoRotationSpeed = 0.1f;

        private GameObject _currentEffect;
        private Text _effectNameText;
        private Text _effectIndexText;

        private EtfxMouseOrbit _etfxMouseOrbit;

        private void Awake()
        {
            _effectNameText = GameObject.Find("EffectName").GetComponent<Text>();
            _effectIndexText = GameObject.Find("EffectIndex").GetComponent<Text>();

            _etfxMouseOrbit = Camera.main.GetComponent<EtfxMouseOrbit>();
            _etfxMouseOrbit.etfxEffectController = this;
        }

        void Start()
        {
            _etfxMouseOrbit = Camera.main.GetComponent<EtfxMouseOrbit>();
            _etfxMouseOrbit.etfxEffectController = this;

            Invoke("InitializeLoop", startDelay);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                NextEffect();
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousEffect();
            }
        }

        private void FixedUpdate()
        {
            if (autoRotation)
            {
                _etfxMouseOrbit.SetAutoRotationSpeed(autoRotationSpeed);

                if (!_etfxMouseOrbit.isAutoRotating)
                    _etfxMouseOrbit.InitializeAutoRotation();
            }
        }

        public void InitializeLoop()
        {
            StartCoroutine(EffectLoop());
        }

        public void NextEffect()
        {
            if (_effectIndex < effects.Length - 1)
            {
                _effectIndex++;
            }
            else
            {
                _effectIndex = 0;
            }

            CleanCurrentEffect();
        }

        public void PreviousEffect()
        {
            if (_effectIndex > 0)
            {
                _effectIndex--;
            }
            else
            {
                _effectIndex = effects.Length - 1;
            }

            CleanCurrentEffect();
        }

        private void CleanCurrentEffect()
        {
            StopAllCoroutines();

            if (_currentEffect != null)
            {
                Destroy(_currentEffect);
            }

            StartCoroutine(EffectLoop());
        }

        private IEnumerator EffectLoop()
        {
            //Instantiating effect
            GameObject effect = Instantiate(effects[_effectIndex], transform.position, Quaternion.identity);
            _currentEffect = effect;

            if (disableLights && effect.GetComponent<Light>())
            {
                effect.GetComponent<Light>().enabled = false;
            }

            if (disableSound && effect.GetComponent<AudioSource>())
            {
                effect.GetComponent<AudioSource>().enabled = false;
            }

            //Update GUIText with effect name
            _effectNameText.text = effects[_effectIndex].name;
            _effectIndexText.text = (_effectIndex + 1) + " of " + effects.Length;

            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

            while (true)
            {
                yield return new WaitForSeconds(particleSystem.main.duration + respawnDelay);

                if (!slideshowMode)
                {
                    if (!particleSystem.main.loop)
                    {
                        _currentEffect.SetActive(false);
                        _currentEffect.SetActive(true);
                    }
                }
                else
                {
                    //Double delay for looping effects
                    if (particleSystem.main.loop)
                    {
                        yield return new WaitForSeconds(respawnDelay);
                    }

                    NextEffect();
                }
            }
        }
    }
}