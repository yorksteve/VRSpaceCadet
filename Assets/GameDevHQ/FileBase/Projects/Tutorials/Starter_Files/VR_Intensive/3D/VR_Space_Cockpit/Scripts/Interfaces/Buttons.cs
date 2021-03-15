using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Interfaces
{
    public abstract class Buttons : MonoBehaviour
    {
        [SerializeField] protected int _sfxSound;
        protected WaitForSeconds _delayYield;

        protected Renderer _rend;
        protected OVRInput.Controller _grabber;
        protected bool _interactable;
        protected bool _on;

        protected Animator _anim;

        private bool _buttonsActive;

        public static Action<int> onSendButtonSFX;


        private void OnEnable()
        {
            PanelManager.onPowerStatus += PowerStatus;
        }

        protected virtual void Start()
        {
            _anim = transform.GetComponent<Animator>();
            _rend = GetComponent<Renderer>();
            if (_anim == null)
            {
                Debug.LogError(transform.name + " animator is NULL");
            }
            if (_rend == null)
            {
                Debug.LogError(transform.name + " renderer is NULL");
            }

            _delayYield = new WaitForSeconds(.5f);

            _buttonsActive = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnLogic();
            if (_sfxSound >= 0)
            {
                if (onSendButtonSFX != null)
                {
                    onSendButtonSFX(_sfxSound);
                }
            }
            
        }

        protected virtual void OnLogic()
        {
            if (_on == false)
            {
                _anim.SetTrigger("Pressed");
                _on = true;
                StartCoroutine(InteractionRoutine());
            }
        }

        IEnumerator InteractionRoutine()
        {
            yield return _delayYield;
            OffLogic();
        }

        protected virtual void OffLogic()
        {
            _on = false;
        }

        private void PowerStatus()
        {
            if (_buttonsActive == true)
            {
                _rend.materials[0].color = Color.black;
                _buttonsActive = false;
            }
            else
            {
                _rend.materials[0].color = Color.red;
            }
        }

        private void OnDisable()
        {
            PanelManager.onPowerStatus -= PowerStatus;
        }
    }
}


