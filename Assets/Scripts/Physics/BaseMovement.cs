using UnityEngine;

namespace Physics
{
    public class BaseMovement : IBaseMovement
    {
        private readonly GameObject _movingObject;
        private Bounds _screenSize;
        private Vector2 _direction;
        private float _speed;

        public BaseMovement(Vector2 screenSize, GameObject movingObject)
        {
            _screenSize = new Bounds(Vector3.zero, screenSize);;
            _movingObject = movingObject;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        
        public void MoveForward(float time)
        {
            _movingObject.transform.Translate(_direction * _speed * time);
            CheckBorders();
        }

        private void CheckBorders()
        {
            if (_movingObject.transform.position.x > _screenSize.max.x)
            {
                _movingObject.transform.position -= new Vector3(_screenSize.size.x, 0, 0);
            }

            if (_movingObject.transform.position.x < _screenSize.min.x)
            {
                _movingObject.transform.position += new Vector3(_screenSize.size.x, 0, 0);
            }

            if (_movingObject.transform.position.y < _screenSize.min.y)
            {
                _movingObject.transform.position += new Vector3(0, _screenSize.size.y, 0);
            }
            if (_movingObject.transform.position.y > _screenSize.max.y)
            {
                _movingObject.transform.position -= new Vector3(0, _screenSize.size.y, 0);
            }
        }
    }
}