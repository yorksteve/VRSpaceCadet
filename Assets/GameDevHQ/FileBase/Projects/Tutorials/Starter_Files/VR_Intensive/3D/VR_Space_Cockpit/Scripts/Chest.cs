using Scripts.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{
    public class Chest : MonoBehaviour
    {
        private Animator _anim;


        private void OnEnable()
        {
            ChestButton.onChestOpened += OpenChest;
        }

        private void Start()
        {
            _anim = GetComponent<Animator>();
            if (_anim != null)
            {
                //_anim.SetTrigger("Open");
                Debug.Log("ANIMATOR SUCCESSFULLY GRABBED");
            }
            else
            {
                Debug.LogError("Animator is NULL");
            }
        }

        private void OpenChest()
        {
            if (_anim != null)
            {
                _anim.SetTrigger("Open");
            }
        }

        private void OnDisable()
        {
            ChestButton.onChestOpened -= OpenChest;
        }
    }
}

