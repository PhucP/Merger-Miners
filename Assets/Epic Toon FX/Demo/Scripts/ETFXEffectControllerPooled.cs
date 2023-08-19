using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
    public class EtfxEffectControllerPooled : MonoBehaviour
    {
        public GameObject[] effects;
        private List<GameObject> _effectsPool;
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

        //Caching components
        private void Awake()
        {
            _effectNameText = GameObject.Find("EffectName").GetComponent<Text>();
            _effectIndexText = GameObject.Find("EffectIndex").GetComponent<Text>();

            _etfxMouseOrbit = Camera.main.GetComponent<EtfxMouseOrbit>();
            _etfxMouseOrbit.etfxEffectControllerPooled = this;

            //Pooling
            _effectsPool = new List<GameObject>();

            for (int i = 0; i < effects.Length; i++)
            {
                GameObject effect = Instantiate(effects[i], transform.position, Quaternion.identity);
                effect.transform.parent = transform;
                _effectsPool.Add(effect);

                effect.SetActive(false);
            }
        }

        private void Start()
        {
            Invoke("InitializeLoop", startDelay);
        }

        private void Update()
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
                _currentEffect.SetActive(false);
            }

            StartCoroutine(EffectLoop());
        }

        private IEnumerator EffectLoop()
        {
            //Pooling effect
            _currentEffect = _effectsPool[_effectIndex];
            _currentEffect.SetActive(true);

            if (disableLights && _currentEffect.GetComponent<Light>())
            {
                _currentEffect.GetComponent<Light>().enabled = false;
            }

            if (disableSound && _currentEffect.GetComponent<AudioSource>())
            {
                _currentEffect.GetComponent<AudioSource>().enabled = false;
            }

            //Update UI
            _effectNameText.text = effects[_effectIndex].name;
            _effectIndexText.text = (_effectIndex + 1) + " of " + effects.Length;

            ParticleSystem particleSystem = _currentEffect.GetComponent<ParticleSystem>();

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

