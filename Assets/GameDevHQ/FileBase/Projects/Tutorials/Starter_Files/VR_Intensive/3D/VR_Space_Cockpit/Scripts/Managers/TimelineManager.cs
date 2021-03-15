using Cinemachine;
using Scripts.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using YorkSDK.Util;


namespace Scripts.Managers
{
    public class TimelineManager : MonoSingleton<TimelineManager>
    {
        [SerializeField] private GameObject _takeOffTimeline;
        [SerializeField] private GameObject _warpTimeline;
        [SerializeField] private GameObject _astroidTimeline;
        [SerializeField] private GameObject _repairTimeline;
        [SerializeField] private GameObject _finalWarpTimeline;

        public static Action onTakeOffComplete;
        public static Action onWarpComplete;


        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            WarpButton.onInitiateWarp += InitiateWarp;
            TakeOffButton.onTakeOff += InitiateTakeOff;
            GameSceneManager.onAstroidScene += AstroidBelt;
            GameSceneManager.onRepairScene += Repair;
            GameSceneManager.onEndFinalWarp += FinalWarp;
        }

        private void Start()
        {
            _takeOffTimeline.SetActive(false);
            _warpTimeline.SetActive(false);
        }

        public void InitiateTakeOff()
        {
            if (_takeOffTimeline.activeInHierarchy == true)
            {
                onTakeOffComplete?.Invoke();
            }
            else
            {
                ActivateTimeline(_takeOffTimeline.name);
            }
        }

        public void InitiateWarp()
        {
            if (_warpTimeline.activeInHierarchy == true)
            {
                onWarpComplete?.Invoke();
                _warpTimeline.SetActive(false);
            }
            else
            {
                _warpTimeline.SetActive(true);
                _warpTimeline.GetComponent<PlayableDirector>().Play();
                ActivateTimeline(_warpTimeline.name);
            }
        }

        public void AstroidBelt()
        {
            Debug.Log("Activate Astroid timeline");
            if (_astroidTimeline.activeInHierarchy == true)
            {
                _astroidTimeline.SetActive(false);
            }
            else
            {
                _astroidTimeline.SetActive(true);
            }
        }

        public void Repair()
        {
            _repairTimeline.SetActive(true);
        }

        public void FinalWarp()
        {
            _finalWarpTimeline.SetActive(true);
            ActivateTimeline(_finalWarpTimeline.name);
        }

        private void ActivateTimeline(string timeline)
        {
            _takeOffTimeline.SetActive(_takeOffTimeline.name.Equals(timeline));
            _warpTimeline.SetActive(_warpTimeline.name.Equals(timeline));
            _astroidTimeline.SetActive(_astroidTimeline.Equals(timeline));
            _repairTimeline.SetActive(_repairTimeline.Equals(timeline));
        }

        private void OnDisable()
        {
            WarpButton.onInitiateWarp -= InitiateWarp;
            TakeOffButton.onTakeOff += InitiateTakeOff;
            GameSceneManager.onAstroidScene -= AstroidBelt;
            GameSceneManager.onRepairScene -= Repair;
            GameSceneManager.onEndFinalWarp -= FinalWarp;
        }

    }
}

