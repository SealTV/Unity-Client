using System;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(Animator))]
    public class UnitComponent : MonoBehaviour
    {
        private const string MoveAnimationKey = "IsMove";

        public event Action<UnitComponent> OnClick = delegate { };

        [SerializeField] private GameObject _selectionCircle;

        [SerializeField] private bool _isMove;
        [SerializeField] private Vector3 _target;
        [SerializeField] private float _speed;

        private bool _isSelected;
        private Transform _transform;

        private Animator _animator;

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

        private Animator Animator
        {
            get { return _animator ?? (_animator = GetComponent<Animator>()); }
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
            Animator.SetBool(MoveAnimationKey, _isMove);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                MoveToTarget(_target);
            }

            Debug.DrawRay(Transform.position, Transform.forward * 10, Color.green);
            if (!_isMove)
                return;


            Transform.localPosition = Vector3.MoveTowards(Transform.localPosition, _target, _speed * Time.deltaTime);

            if (Transform.localPosition == _target)
            {
                _isMove = false;
                Animator.SetBool(MoveAnimationKey, _isMove);
            }
        }

        private void OnMouseUpAsButton()
        {
            OnClick(this);
        }
    }
}