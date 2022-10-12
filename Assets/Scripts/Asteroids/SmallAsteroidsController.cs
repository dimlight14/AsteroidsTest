using Main;
using Physics;
using UnityEngine;

namespace Asteroids
{
    public class SmallAsteroidsController : AsteroidController
    {
        public SmallAsteroidsController(Updater updater, GameObject gameObject, IBaseMovement baseMovement,
            IAsteroidsFactoryService factory, float speed, IBountyComponent bountyComponent) : base(updater, gameObject,
            baseMovement, factory, speed, bountyComponent)
        {
        }

        public override void GetWrecked(bool laserHit)
        {
            _bountyComponent.Claim();
            Despawn();
        }
    }
}