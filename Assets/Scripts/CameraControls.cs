using UnityEngine;

namespace Client
{
    public class CameraControls : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 0.1f;
        [SerializeField] private float _rotationSpeed = 4f;
        [SerializeField] private float _smoothness = 0.85f;

        [SerializeField] private Vector3 _minPositon;
        [SerializeField] private Vector3 _maxPosition;

        private Vector3 _targetPosition;

        [SerializeField] private Quaternion _targetRotation;
        float _targetRotationX;
        float _targetRotationY;

        private void Start()
        {
            _targetPosition = transform.localPosition;
            _targetRotation = transform.rotation;

            _targetRotationX = transform.localRotation.eulerAngles.x;
            _targetRotationY = transform.localRotation.eulerAngles.y;
        }

        private void Update()
        {
            if( Input.GetKey( KeyCode.W ) )
                _targetPosition += transform.forward * _movementSpeed;
            if( Input.GetKey( KeyCode.A ) )
                _targetPosition -= transform.right * _movementSpeed;
            if( Input.GetKey( KeyCode.S ) )
                _targetPosition -= transform.forward * _movementSpeed;
            if( Input.GetKey( KeyCode.D ) )
                _targetPosition += transform.right * _movementSpeed;
            if( Input.GetKey( KeyCode.Q ) )
                _targetPosition -= transform.up * _movementSpeed;
            if( Input.GetKey( KeyCode.E ) )
                _targetPosition += transform.up * _movementSpeed;

            if( Input.GetMouseButton( 2 ) )
            {
                Cursor.visible = false;
                _targetRotationY += Input.GetAxis( "Mouse X" ) * _rotationSpeed;
                _targetRotationX -= Input.GetAxis( "Mouse Y" ) * _rotationSpeed;
                _targetRotation = Quaternion.Euler( _targetRotationX, _targetRotationY, 0.0f );
            }
            else
                Cursor.visible = true;

            _targetPosition = new Vector3(
                Mathf.Clamp(_targetPosition.x, _minPositon.x, _maxPosition.x),
                Mathf.Clamp(_targetPosition.y, _minPositon.y, _maxPosition.y),
                Mathf.Clamp(_targetPosition.z, _minPositon.z, _maxPosition.z));
            transform.localPosition = Vector3.Lerp( transform.localPosition, _targetPosition, ( 1.0f - _smoothness ) );
            transform.rotation = Quaternion.Lerp( transform.rotation, _targetRotation, ( 1.0f - _smoothness ) );
        }
    }
}
