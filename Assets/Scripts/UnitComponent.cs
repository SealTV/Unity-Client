using System;
using UnityEngine;

namespace Client
{
    public class UnitComponent : MonoBehaviour
    {
        public event Action<UnitComponent> OnClick = delegate {};

        [SerializeField]
        private GameObject _selectionCircle;

        [SerializeField] private bool _isMove;
        [SerializeField] private Vector3 _target;
        [SerializeField] private float _speed;

        private bool _isSelected;
        private Transform _transform;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                _selectionCircle.SetActive(_isSelected);
            }
        }

        public Transform Transform
        {
            get { return _transform ?? (_transform = transform); }
        }

        private void Start()
        {
            IsSelected = false;
        }

        private void MoveToTarget(Vector3 target)
        {
            _target = target;
            Transform.localRotation = Quaternion.LookRotation(_target - Transform.localPosition);
            _isMove = true;
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                MoveToTarget(_target);
            }

            Debug.DrawRay(Transform.position, Transform.forward * 10, Color.green);
            if(!_isMove)
                return;

            Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, _target, _speed * Time.deltaTime);

            if (Transform.localPosition == _target)
            {
                _isMove = false;
            }
        }


        private void OnMouseUpAsButton()
        {
            OnClick(this);
        }
    }
}