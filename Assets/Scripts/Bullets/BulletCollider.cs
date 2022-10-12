using System;
using Physics;
using UnityEngine;

namespace Bullets
{
    public class BulletCollider :  SimpleCollider, IBulletCollider
    {
        public BulletCollider(BoxCollider2D collider, Action onCollisionAction) : base(collider, onCollisionAction)
        {
        }

        public bool IsLaser { get; private set; }

        public void MarkAsLaser()
        {
            IsLaser = true;
        }
    }
}