using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Player
{
    public class CoffeeSphere : MonoBehaviour
    {
        private Rigidbody _coffeeRB;

        public static Action<int> onCoffeeDrank;



        private void OnEnable()
        {
            PoolManager.onCoffeeBrewed += Brew;
        }

        private void Start()
        {
            _coffeeRB = GetComponent<Rigidbody>();
        }

        private void Brew()
        {
            Vector3 force = new Vector3(UnityEngine.Random.Range(-.5f, 0f), 1, 0);
            _coffeeRB.AddForce(force);
        }

        private void OnTriggerEnter(Collider other)
        {
            if  (other.CompareTag("Head"))
            {
                onCoffeeDrank?.Invoke(7);
                Destroy(this.gameObject);
            }
        }

        private void OnDisable()
        {
            PoolManager.onCoffeeBrewed -= Brew;
        }
    }
}

