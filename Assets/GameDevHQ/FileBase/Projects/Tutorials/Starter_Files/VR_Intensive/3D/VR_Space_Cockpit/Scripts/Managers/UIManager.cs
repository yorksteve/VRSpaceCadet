using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using UnityEngine.SceneManagement;


namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public override void Init()
        {
            base.Init();
        }

        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _settingsMenu;


        public void StartButton()
        {
            SceneManager.LoadScene(1);
        }

        public void SettingsButton()
        {
            _mainMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void BackButton()
        {
            _mainMenu.SetActive(true);
            _settingsMenu.SetActive(false);
        }

    }
}

