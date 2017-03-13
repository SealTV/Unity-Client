using System;
using UnityEngine;

namespace Client
{
    public sealed class  CellComponent : MonoBehaviour
    {
        public event Action<int, int> OnClick = delegate {};

        private int _x;
        private int _y;
        private Transform _transform;

        public Transform Transform
        {
            get { return _transform ?? (_transform = transform); }
        }

        public void Init(int x, int y)
        {
            _x = x;
            _y = y;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(1))
                OnClick(_x, _y);
        }
    }
}
