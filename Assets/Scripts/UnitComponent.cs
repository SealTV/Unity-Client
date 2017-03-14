using System;
using Shared.POCO;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(Animator))]
    public class UnitComponent : MonoBehaviour
    {
        private const string MoveAnimationKey = "IsMove";

        public event Action<UnitComponent> OnClick = delegate { };

        [SerializeField] private GameObject _selectionCircle;

        [SerializeField] private int _id;
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

        private Unit _unit;

        public Unit Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                MoveToTarget(new Vector3(value.PositionF.X, 0, value.PositionF.Y));
            }
        }

        private void Start()
        {
            IsSelected = false;
        }

        private void MoveToTarget(Vector3 target)
        {
            _target = target;
            _isMove = true;
            Animator.SetBool(MoveAnimationKey, _isMove);
        }

        private void Update()
        {
            Animator.SetBool(MoveAnimationKey, Unit.State == States.Move);
            if (!_isMove)
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

        public void Init(Unit unit)
        {
            _id = unit.Id;
            _unit = unit;
        }

        public void Reset()
        {
            _isMove = false;
            Animator.SetBool(MoveAnimationKey, _isMove);
        }
    }
}