using UnityEngine;

namespace Player
{
    public class PlayerMovement : IPlayerMovement, IPlayerSpeedProvider
    {
        private float _maxSpeed = 9;
        private float _acceleration = 15f;
        private const float SlowDeceleration = 6f;
    
        private GameObject _playerObject;
        private Bounds _screenSize;
    
        private Vector3 _currentDirection;
        private float _currentSpeed = 0f;
        private float _currentAcceleration;
        private Vector3 _newDirection;

        public PlayerMovement(GameObject playerObject, Vector2 screenSize)
        {
            _playerObject = playerObject;
            _screenSize = new Bounds(Vector3.zero, screenSize);
            _currentDirection = playerObject.transform.up;
        }
    
        public void SetMaxSpeedAndAcceleration(float maxSpeed, float acceleration)
        {
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;
        }

        public void UpdateMovement(float time, Vector2 inputValue)
        {
            if (inputValue.y > 0)
            {
                CalculateNewDirection();
                var inSameDirection = Vector3.Dot(_newDirection, _currentDirection.normalized);
                if (inSameDirection > 0.7f || Mathf.Approximately(_currentSpeed,0))
                {
                    Accelerate(time);
                }
                else if(inSameDirection < 0.3f)
                {
                    Decelerate(time);
                }
            }
            else
            {
                DecelerateSlowly(time);
            }
        
            MoveForward(time);
            CheckBorders();
        }

        public float GetSpeed()
        {
            return _currentSpeed;
        }
        private void CalculateNewDirection()
        {
            _newDirection = _playerObject.transform.up;
        }

        private void CheckBorders()
        {
            if (_playerObject.transform.position.x > _screenSize.max.x)
            {
                _playerObject.transform.position -= new Vector3(_screenSize.size.x, 0, 0);
            }

            if (_playerObject.transform.position.x < _screenSize.min.x)
            {
                _playerObject.transform.position += new Vector3(_screenSize.size.x, 0, 0);
            }

            if (_playerObject.transform.position.y < _screenSize.min.y)
            {
                _playerObject.transform.position += new Vector3(0, _screenSize.size.y, 0);
            }
            if (_playerObject.transform.position.y > _screenSize.max.y)
            {
                _playerObject.transform.position -= new Vector3(0, _screenSize.size.y, 0);
            }
        }

        private void Decelerate(float time)
        {
            if (_currentSpeed > 0)
            {
                _currentSpeed = Mathf.Max(0, _currentSpeed - _acceleration * time);
            }
        }
        private void DecelerateSlowly(float time)
        {
            if (_currentSpeed > 0)
            {
                _currentSpeed = Mathf.Max(0, _currentSpeed - SlowDeceleration * time);
            }
        }

        private void Accelerate(float time)
        {
            if (_currentSpeed >= _maxSpeed)
            {
                return;
            }
        
            _currentAcceleration = _acceleration * time;
            _currentSpeed = Mathf.Min(_currentSpeed + _currentAcceleration, _maxSpeed);
        }
        private void MoveForward(float time)
        {
            if(_currentSpeed <=0) return;

            _currentDirection = _newDirection*_currentAcceleration + _currentDirection;
            _currentDirection.Normalize();
            _currentDirection *= _currentSpeed;
        
            _playerObject.transform.position += _currentDirection * time;
        }

        public void Reset()
        {
            _currentDirection = _playerObject.transform.up;
            _currentSpeed = 0;
        }
    }
}