using Cinemachine;
using Scripts.Interactables;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        public enum GameScene
        {
            Coffee,
            Bootup,
            TakeOff,
            Warp1,
            Astroid,
            Warp2,
            Repair,
            Warp3,
            FreeFlight
        }

        [SerializeField] private GameObject _fleet;
        [SerializeField] private GameObject _astroidBelt;
        [SerializeField] private Material _earthSkybox;
        [SerializeField] private Material _astroidSkybox;
        [SerializeField] private Material _galaxySkybox;
        [SerializeField] private Transform _startPos;
        [SerializeField] private Transform _astroidStartPos;

        [SerializeField] private GameObject _playerShip;
        [SerializeField] private CinemachineSmoothPath _dollyTrack1;
        [SerializeField] private CinemachineSmoothPath _dollyTrack2;
        [SerializeField] private CinemachineSmoothPath _dollyTrack3;
        [SerializeField] private Light _shipLight;
        [SerializeField] private float _shipSpeed = 7f;

        [SerializeField] private GameObject _astroidParticleSystem;

        [SerializeField] private GameObject _redScreen;
        [SerializeField] private GameObject _normalScreen;
        [SerializeField] private GameObject _chair;
        [SerializeField] private GameObject _player;

        [SerializeField] private GameObject _core;
        [SerializeField] private GameObject _coreBase;
        [SerializeField] private GameObject _brokenCore;
        [SerializeField] private GameObject _replacementCore;
        [SerializeField] private GameObject _sparks;

        private Renderer _coreRend;
        private Color[] _colorArray = { Color.red, Color.white };
        private Color[] _reactorColorArray = { Color.white, Color.black };

        private CinemachineDollyCart _shipPath;
        private Rigidbody _shipRB;

        private WaitForSeconds _coffeeDelay = new WaitForSeconds(15f);
        private WaitForSeconds _bootupDelay = new WaitForSeconds(5f);
        private WaitForSeconds _warningLightDelay = new WaitForSeconds(1f);

        public GameScene _gameScene;
        private bool _startCoffee;
        private bool _sendBootupAudio;
        private bool _turnChair;
        private bool _astroidScene;

        public static Action<int> onSendMusic;
        public static Action<int> onSendVO;
        public static Action onCheckBootupButtonStatus;
        public static Action onUnlockBootup;
        public static Action onUnlockTakeOff;
        public static Action onUnlockWarp;
        public static Action onStartCoffee;
        public static Action onAstroidScene;
        public static Action onRepairScene;
        public static Action onFade;
        public static Action onUnlockChair;
        public static Action onUnlockReactor;
        public static Action onEnableFreeFlight;
        public static Action onEndFinalWarp;



        private void OnEnable()
        {
            CoffeeButton.onCoffeeButtonClicked += Coffee;
            AudioManager.onVOEnd += VOCheck;
            ButtonCounter.onNextAudio += BootupVO;
            TimelineManager.onTakeOffComplete += TakeOffComplete;
            ButtonCounter.onBootupComplete += BootupComplete;
            TimelineManager.onWarpComplete += WarpComplete;
            ChairButton.onChairButtonClicked += RepairScene;
            Reactor.onCoreReplaced += RepairFinished;
            Reactor.onBrokenCoreRemoved += BrokenCoreRemoved;
            ButtonCounter.onReactorReset += EndRepairScene;
            FadeManager.onFading += RotateChair;
        }

        private void Start()
        {
            onFade?.Invoke();
            _shipPath = _playerShip.GetComponent<CinemachineDollyCart>();
            _shipPath.m_Path = null;
            _playerShip.transform.position = _startPos.position;
            _playerShip.transform.rotation = _startPos.rotation;
            _astroidBelt.SetActive(false);
            _shipRB = _playerShip.GetComponent<Rigidbody>();
            _coreRend = _core.GetComponent<Renderer>();
            _astroidScene = false;
            //StartCoroutine(StartYield());
        }

        IEnumerator StartYield()
        {
            yield return new WaitForSeconds(3f);
            onSendVO?.Invoke(0);
        }

        public void ChangeGameScene(GameScene scene)
        {
            _gameScene = scene;

            if (_gameScene.Equals(GameScene.Coffee))
            {
                _startCoffee = true;
                onStartCoffee?.Invoke();
                Coffee();
            }

            if (_gameScene.Equals(GameScene.Bootup))
            {
                onUnlockBootup?.Invoke();
                Bootup();
            }

            if (_gameScene.Equals(GameScene.TakeOff))
            {
                TakeOff();
            }

            if (_gameScene.Equals(GameScene.Warp1))
            {
                InitiateWarp();
            }

            if (_gameScene.Equals(GameScene.Astroid))
            {
                _astroidScene = true;
                AstroidStart();
            }

            if (_gameScene.Equals(GameScene.Warp2))
            {
                InitiateWarp();
            }

            if (_gameScene.Equals(GameScene.Repair))
            {
                _turnChair = true;
                onRepairScene?.Invoke();
            }

            if (_gameScene.Equals(GameScene.Warp3))
            {
                InitiateWarp();
            }

            if (_gameScene.Equals(GameScene.FreeFlight))
            {
                FreeFlight();
            }
        }

        public void VOCheck(int id)
        {
            if (id.Equals(0))
            {
                ChangeGameScene(GameScene.Coffee);
            }

            if (id.Equals(1))
            {
                Bootup();
            }

            if (id.Equals(15))
            {
                ChangeGameScene(GameScene.FreeFlight);
            }
        }

        public void Coffee()
        {
            if (_startCoffee == true)
            {
                _startCoffee = false;
            }
            else
            {
                StartCoroutine(CoffeeSceneRoutine());
            }
        }

        public void Bootup()
        {
            if (_sendBootupAudio == false)
            {
                onSendVO?.Invoke(1);
                _sendBootupAudio = true;
            }
            else
            {
                StartCoroutine(BootupYieldRoutine());
            }
        }

        public void BootupVO()
        {
            onSendVO?.Invoke(2);
        }

        public void BootupComplete()
        {
            ChangeGameScene(GameScene.TakeOff);
        }

        public void TakeOff()
        {
            onSendVO?.Invoke(3);
            onUnlockTakeOff?.Invoke();
        }

        public void TakeOffComplete()
        {
            ChangeGameScene(GameScene.Warp1);
        }

        public void InitiateWarp()
        {
            onUnlockWarp?.Invoke();
        }

        public void Warp()
        {
            if (_gameScene.Equals(GameScene.Warp1))
            {
                //Change scene to astroids
                _fleet.SetActive(false);
                RenderSettings.skybox = _astroidSkybox;
                _shipPath.m_Path = _dollyTrack2;
                _shipPath.m_Position = 0;
                _shipPath.m_Speed = 0;
                //_playerShip.transform.position = _astroidStartPos.position;
                //_playerShip.transform.rotation = _astroidStartPos.rotation;
                onSendVO?.Invoke(16);
            }
            else if (_gameScene.Equals(GameScene.Warp2))
            {
                _astroidBelt.SetActive(false);
                RenderSettings.skybox = _galaxySkybox;
                _shipPath.m_Path = null;
                _redScreen.SetActive(false);
                _normalScreen.SetActive(true);
                _shipLight.color = Color.white;
            }
            else
            {
                _fleet.SetActive(true);
                RenderSettings.skybox = _earthSkybox;
                _shipPath.m_Path = _dollyTrack3;
                _shipPath.m_Position = 0;
                _shipPath.m_Speed = 0;
            }
        }

        public void WarpComplete()
        {
            Debug.Log("Warp Complete");
            if (_gameScene.Equals(GameScene.Warp1))
            {
                _astroidBelt.SetActive(true);
                ChangeGameScene(GameScene.Astroid);
            }
            else if (_gameScene.Equals(GameScene.Warp2))
            {
                ChangeGameScene(GameScene.Repair);
            }
            else
            {
                onEndFinalWarp?.Invoke();
            }
        }

        public void AstroidStart()
        {
            onAstroidScene?.Invoke();
            StartCoroutine(AstroidWarningLightRoutine());
            _shipPath.m_Speed = _shipSpeed;
            _redScreen.SetActive(true);
            _normalScreen.SetActive(false);
        }

        public void AstroidEnd()
        {
            _shipPath.m_Path = null;
            _astroidScene = false;
            ChangeGameScene(GameScene.Warp2);
        }

        public void RepairScene()
        {
            _brokenCore.SetActive(true);
            _core.SetActive(false);
            onFade?.Invoke();
        }

        public void RepairButtons()
        {
            if (_turnChair == true)
            {
                onUnlockChair?.Invoke();
                _turnChair = false;
            }
            else
            {
                onUnlockReactor?.Invoke();
            }
        }

        public void BrokenCoreRemoved()
        {
            onSendVO?.Invoke(12);
            _replacementCore.SetActive(true);
            _brokenCore.layer = 8;
            _sparks.SetActive(false);
        }

        public void RepairFinished()
        {
            onSendVO?.Invoke(13);
            _coreRend.materials[0].SetColor("_EmissionColor", Color.black);
            _core.SetActive(true);
            _replacementCore.SetActive(false);
            _brokenCore.SetActive(false);
            _coreBase.layer = 8;
            RepairButtons();
        }

        public void EndRepairScene()
        {
            StartCoroutine(CoreFlickerRoutine());
        }

        public void RepairSceneOver()
        {
            onFade?.Invoke();
            onSendVO?.Invoke(14);
            ChangeGameScene(GameScene.Warp3);
        }

        public void RotateChair()
        {
            _chair.transform.Rotate(0f, 180f, 0f);
        }

        public void FinalWarpComplete()
        {
            onSendVO?.Invoke(15);
            _shipPath.m_Path = null;
        }

        public void FreeFlight()
        {
            _shipRB.isKinematic = false;
            onEnableFreeFlight?.Invoke();
        }

        public void SetDollyPath()
        {
            _shipPath.m_Path = _dollyTrack1;
        }

        IEnumerator CoffeeSceneRoutine()
        {
            yield return _coffeeDelay;
            ChangeGameScene(GameScene.Bootup);
        }

        IEnumerator BootupYieldRoutine()
        {
            yield return _bootupDelay;
            onCheckBootupButtonStatus?.Invoke();
        }

        IEnumerator AstroidWarningLightRoutine()
        {
            int i = 0;
            while (_astroidScene == true)
            {
                _shipLight.color = _colorArray[i % 2];
                i++;
                yield return _warningLightDelay;
            }
        }

        IEnumerator AstroidParticleSystemRoutine()
        {
            while (_astroidScene == true)
            {
                //_astroidParticleSystem.transform.position.x = _playerShip.transform.position.x;
                _astroidParticleSystem.transform.position = new Vector3(_playerShip.transform.position.x - 100, _astroidParticleSystem.transform.position.y, _astroidParticleSystem.transform.position.z);
                yield return null;
            }
        }

        IEnumerator CoreFlickerRoutine()
        {
            int i = 2;
            int j = 0;
            while (i > 0)
            {
                _coreRend.materials[0].SetColor("_EmissionColor", _reactorColorArray[j % 2]);
                j++;
                yield return new WaitForSeconds(Random.Range(0f, .5f));
                //_coreRend.materials[0].SetColor("_EmissionColor", Color.black);
                i--;
            }
            _coreRend.materials[0].SetColor("_EmissionColor", Color.white);
            RepairSceneOver();
        }

        private void OnDisable()
        {
            CoffeeButton.onCoffeeButtonClicked -= Coffee;
            AudioManager.onVOEnd -= VOCheck;
            ButtonCounter.onNextAudio += BootupVO;
            TimelineManager.onTakeOffComplete -= TakeOffComplete;
            ButtonCounter.onBootupComplete -= BootupComplete;
            TimelineManager.onWarpComplete -= WarpComplete;
            ChairButton.onChairButtonClicked -= RepairScene;
            Reactor.onCoreReplaced -= RepairFinished;
            Reactor.onBrokenCoreRemoved -= BrokenCoreRemoved;
            ButtonCounter.onReactorReset -= EndRepairScene;
            FadeManager.onFading -= RotateChair;
        }
    }
}

