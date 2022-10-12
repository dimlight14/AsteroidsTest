using UnityEngine;

namespace Player
{
    public class PlayerRotation : IPlayerRotation
    {
        private const float AngularSpeed = 140;

        private readonly GameObject _playerObject;

        public PlayerRotation(GameObject playerObject)
        {
            _playerObject = playerObject;
        }

        public void Reset()
        {
            _playerObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public void UpdateRotation(float time, Vector2 input)
        {
            if (input.x == 0)
            {
                return;
            }

            RotateShip(input.x > 0, time);
        }

        private void RotateShip(bool clockWise, float time)
        {
            _playerObject.transform.Rotate(new Vector3(0, 0, clockWise ? -AngularSpeed * time : AngularSpeed * time));
        }
    }
}