using Scripts.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private GameObject _coffee;
        [SerializeField] private GameObject _coffeeContainer;
        [SerializeField] private List<GameObject> _coffeePool;
        [SerializeField] private int _coffeeCount;
        [SerializeField] private Transform _startPoint;

        private WaitForSeconds _coffeeYield = new WaitForSeconds(2f);
        private WaitForSeconds _delay = new WaitForSeconds(.25f);


        public static Action<int> onBrewCoffeeSFX;
        public static Action onCoffeeBrewed;


        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            CoffeeButton.onCoffeeButtonClicked += ReceiveEvent;
        }

        //private void Start()
        //{
        //    CreatePool();
        //}

        public void ReceiveEvent()
        {
            StartCoroutine(BrewCoffeeRoutine());
        }

        //public GameObject SetCoffee()
        //{
        //    GameObject coffee = Instantiate(_coffee, _startPoint.position, Quaternion.identity);
        //    coffee.transform.parent = _coffeeContainer.transform;
        //    coffee.SetActive(false);
        //    _coffeePool.Add(coffee);

        //    return coffee;
        //}

        //List<GameObject> CreatePool()
        //{
        //    for (int i = 0; i < _coffeeCount; i++)
        //    {
        //        GameObject coffee = SetCoffee();
        //    }

        //    return _coffeePool;
        //}

        //public void BrewCoffee()
        //{
        //    foreach (var drop in _coffeePool)
        //    {
        //        drop.transform.position = _startPoint.position;
        //        drop.SetActive(true);
        //        onCoffeeBrewed();
        //    }
        //}

        //public void DisableCoffee(GameObject coffee)
        //{
        //    coffee.SetActive(false);
        //}

        IEnumerator BrewCoffeeRoutine()
        {
            onBrewCoffeeSFX?.Invoke(6);
            yield return _coffeeYield;
            //BrewCoffee();

            var i = _coffeeCount;
            while (i > 0)
            {
                Instantiate(_coffee, _startPoint.position, Quaternion.identity);
                i--;
                yield return _delay;
            }
        }

        private void OnDisable()
        {
            CoffeeButton.onCoffeeButtonClicked -= ReceiveEvent;
        }
    }
}

