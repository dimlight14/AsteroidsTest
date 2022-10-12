using System;
using UnityEngine;

namespace Physics
{
    public class EnemyCollider : IEnemyCollider
    {
        private readonly BoxCollider2D _collider;
        private readonly Action<bool> _onCollisionAction;
        
        public bool IsActive => _isActive;
        private bool _isActive = false;

        public EnemyCollider(BoxCollider2D collider, Action<bool> onCollisionAction)
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

        public void GetWrecked(bool laserHit)
        {
            _onCollisionAction?.Invoke(laserHit);
        }
    }
}