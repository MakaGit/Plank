using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plank
{
    public class Player : MonoBehaviour
    {
        private float _playerSpeed = 0.0f;

        public Vector3 target = Vector3.zero;

        private void Start()
        {
            _playerSpeed = SettingsManager.Instance.playerSpeed;       
        }
        private void Update()
        {

        }

        public void MoveToTarget(Vector3 target)
        {
            transform.position = target;
        }
    }
}