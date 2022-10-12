using System;
using UnityEngine;

namespace Physics
{
    public class SimpleCollider : ISimpleCollider
    {
        private readonly BoxCollider2D _collider;
        private readonly Action _onCollisionAction;
        
        public bool IsActive => _isActive;
        private bool _isActive = false;

        public SimpleCollider(BoxCollider2D collider, Action onCollisionAction)
        {
            _collider = collider;
            _onCollisionAction = onCollisionAction;
        }

        public void SetActive(bool value)
        {
            _isActive = value;
        }
        
        public BoxCollider2D GetCollider()
        {
            return _collider;
        }

        public void OnCollision()
        {
            _onCollisionAction?.Invoke();
        }
    }
}