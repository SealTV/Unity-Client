using System.Collections.Generic;
using System.Linq;
using Shared.POCO;
using UnityEngine;

namespace Client
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private int _startWidth;
        [SerializeField] private int _startHeight;

        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _unitsContainer;

        [SerializeField] private int _cellOffset;
        [SerializeField] private CellComponent _cellPrefab;
        [SerializeField] private UnitComponent _unitPrefab;

        [SerializeField] private GameObject _mainUI;
        [SerializeField] private GameObject _levelUI;

        [SerializeField] private UnitSelectionComponent _unitSelectionComponent;

        private readonly List<CellComponent> _cells = new List<CellComponent>();
        private readonly List<UnitComponent> _units = new List<UnitComponent>();

        private readonly Queue<CellComponent> _cellsQueue = new Queue<CellComponent>();
        private readonly Queue<UnitComponent> _unitsQueue = new Queue<UnitComponent>();

        private bool _isPlay;

        #region UI event handlers

        public void Play()
        {
            _isPlay = true;

            Unit[] units = new Unit[3];
            for (int i = 0; i < units.Length; i++)
            {
                units[i] = new Unit
                {
                    Id = i + 1,
                    Position = new Position { X = i + 1, Y = 2 + i }
                };
            }

            Init(_startWidth, _startHeight, units);
            UpdateUI();
        }

        public void ExitLevel()
        {
            _isPlay = false;
            ClereScene();
            UpdateUI();
            _unitSelectionComponent.IsEnable = false;
        }

        public void CloseGame()
        {
            Application.Quit();
        }

        #endregion

        private void Init(int width, int height, Unit[] units)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    CellComponent cell = _cellsQueue.Any() ? _cellsQueue.Dequeue() : Instantiate(_cellPrefab);
                    cell.gameObject.SetActive(true);
                    cell.Transform.SetParent(_cellsContainer);
                    cell.Transform.localPosition = new Vector3(i *  _cellOffset, 0, j * _cellOffset);
                    cell.Init(i, j);
                    cell.OnClick += (x, y) =>
                    {
                        Debug.LogFormat("Click on x: {0}, y: {1}", x, y);
                    };

                    _cells.Add(cell);
                }
            }

            for (int i = 0; i < units.Length; i++)
            {
                UnitComponent unit = _unitsQueue.Any() ? _unitsQueue.Dequeue() : Instantiate(_unitPrefab);

                unit.gameObject.SetActive(true);
                unit.Transform.SetParent(_unitsContainer);
                unit.Transform.localPosition = new Vector3(units[i].Position.X * _cellOffset, 0, units[i].Position.Y * _cellOffset);

                _units.Add(unit);
            }

            _unitSelectionComponent.IsEnable = true;
            _unitSelectionComponent.Init(_units);
        }

        private void ClereScene()
        {
            foreach (var cell in _cells)
            {
                cell.gameObject.SetActive(false);
                _cellsQueue.Enqueue(cell);
            }

            foreach (var unit in _units)
            {
                unit.gameObject.SetActive(false);
                _unitsQueue.Enqueue(unit);
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            _mainUI.SetActive(!_isPlay);
            _levelUI.SetActive(_isPlay);
        }
    }
}
