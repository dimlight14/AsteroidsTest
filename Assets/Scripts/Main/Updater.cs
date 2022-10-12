using System;
using UnityEngine;

namespace Main
{
    public class Updater : MonoBehaviour
    {
        public event Action<float> OnUpdate;
        public event Action<float> OnFixedUpdate;

        private bool _isActive = false;

        private void Update()
        {
            if (!_isActive) return;
            OnUpdate?.Invoke(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            OnFixedUpdate?.Invoke(Time.fixedDeltaTime);
        }

        public void SetActive(bool value)
        {
            _isActive = value;
        }
    }
}