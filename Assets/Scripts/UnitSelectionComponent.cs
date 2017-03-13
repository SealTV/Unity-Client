using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class UnitSelectionComponent : MonoBehaviour
    {
        public event Action<List<UnitComponent>> OnSelectUnits = delegate {};

        [SerializeField] private Color _rectColor;
        [SerializeField] private Color _borderColor;

        private bool _isSelecting = false;
        private Vector3 _mousePosition;
        private List<UnitComponent> _units;

        public bool _isEnable;
        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable = value;
                if (!_isEnable)
                {
                    _isSelecting = false;
                }
            }
        }

        public void Init(List<UnitComponent> units)
        {
            _units = units;
            foreach (var unit in _units)
            {
                unit.OnClick += OnUnitClickHandler;
            }
        }

        private void OnUnitClickHandler(UnitComponent unit)
        {
            unit.IsSelected = true;
            OnSelectUnits(new List<UnitComponent>
            {
                unit
            });
        }

        private void Update()
        {
            if (!IsEnable)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                _isSelecting = true;
                _mousePosition = Input.mousePosition;

                foreach (var selectableObject in _units)
                {
                    selectableObject.IsSelected = false;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var selectedUnits = new List<UnitComponent>();
                foreach (var selectableObject in _units)
                {
                    if (IsWithinSelectionBounds(selectableObject.Transform.position))
                    {
                        selectedUnits.Add(selectableObject);
                    }
                }

                if (selectedUnits.Any())
                    OnSelectUnits(selectedUnits);

                _isSelecting = false;
            }

            // Highlight all objects within the selection box
            if (_isSelecting)
            {
                foreach (var selectableObject in _units)
                {
                    selectableObject.IsSelected = IsWithinSelectionBounds(selectableObject.Transform.position);
                }
            }
        }

        public bool IsWithinSelectionBounds(Vector3 positon)
        {
            if (!_isSelecting)
                return false;

            var viewportBounds = Utils.GetViewportBounds(Camera.main, _mousePosition, Input.mousePosition);
            return viewportBounds.Contains(Camera.main.WorldToViewportPoint(positon));
        }

        private void OnGUI()
        {
            if (_isSelecting)
            {
                // Create a rect from both mouse positions
                var rect = Utils.GetScreenRect(_mousePosition, Input.mousePosition);
                Utils.DrawScreenRect(rect, _rectColor);
                Utils.DrawScreenRectBorder(rect, 2, _borderColor);
            }
        }
    }
}