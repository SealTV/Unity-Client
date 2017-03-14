using System.Collections.Generic;
using System.Linq;
using Client.Network;
using Client.Network.PackageHandlers;
using Shared.DataPackages.Client;
using Shared.DataPackages.Server;
using Shared.POCO;
using UnityEngine;

namespace Client
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _unitsContainer;

        [SerializeField] private int _cellOffset;
        [SerializeField] private CellComponent _cellPrefab;
        [SerializeField] private UnitComponent _unitPrefab;

        [SerializeField] private GameObject _mainUI;
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private GameObject _levelUI;

        [SerializeField] private UnitSelectionComponent _unitSelectionComponent;
        [SerializeField] private NetworkClient _networkClient;

        private readonly List<CellComponent> _cells = new List<CellComponent>();
        private readonly List<UnitComponent> _units = new List<UnitComponent>();

        private readonly Queue<CellComponent> _cellsQueue = new Queue<CellComponent>();
        private readonly Queue<UnitComponent> _unitsQueue = new Queue<UnitComponent>();

        private readonly Queue<ServerPackage> _packages = new Queue<ServerPackage>();

        public NetworkClient Client { get { return _networkClient; } }
        public List<UnitComponent> Units { get { return _units; } }

        #region UI event handlers

        public void Play()
        {
            _networkClient.Connect();
            _gameState = GameState.Loading;
            UpdateUI();
        }

        public void ExitLevel()
        {
            ClereScene();
            _unitSelectionComponent.IsEnable = false;
            _networkClient.Disconnect();

            _gameState = GameState.MainMenu;
            UpdateUI();
        }

        public void CloseGame()
        {
            Application.Quit();
        }

        #endregion

        public void SetPackage(ServerPackage package)
        {
            _packages.Enqueue(package);
        }

        public void Init(int width, int height, Unit[] units)
        {
            var containerPosition = new Vector3((1 - width) * _cellOffset / 2f, 0, (1 - height) * _cellOffset / 2f);
            _cellsContainer.localPosition = containerPosition;
            _unitsContainer.localPosition = containerPosition;

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
                        if (Units.Any(u => u.IsSelected))
                        {
                            Position position = new Position(x, y);
                            var selectedUnits = Units.Where(u => u.IsSelected).Select(u => u.Unit).ToArray();
                            foreach (var selectedUnit in selectedUnits)
                            {
                                selectedUnit.TargetPosition = position;
                            }

                            Client.SendPackage(new SetTargetsPackage
                            {
                                Units = selectedUnits
                            });
                        }
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
                unit.Init(units[i]);
                _units.Add(unit);
            }

            _unitSelectionComponent.IsEnable = true;
            _unitSelectionComponent.Init(_units);

            _gameState = GameState.Play;
            UpdateUI();
        }

        private void ClereScene()
        {
            foreach (var cell in _cells)
            {
                cell.gameObject.SetActive(false);
                _cellsQueue.Enqueue(cell);
            }
            _cells.Clear();

            foreach (var unit in _units)
            {
                unit.Reset();
                unit.gameObject.SetActive(false);
                _unitsQueue.Enqueue(unit);
            }

            _units.Clear();
        }

        private void Start()
        {
            _gameState = GameState.MainMenu;
            UpdateUI();
        }

        private void Update()
        {
            if (_packages.Any())
            {
                var package = _packages.Dequeue();
                var packageHandler = PackageHandler.GetPackageHandler(this, package);
                if (packageHandler != null)
                {
                    packageHandler.HandlePackage();
                }
                else
                {
                    Debug.LogWarning("Package handler are not found for package " + package.Type);
                }
            }
        }

        private void UpdateUI()
        {
            _mainUI.SetActive(_gameState == GameState.MainMenu);
            _loadingUI.SetActive(_gameState == GameState.Loading);
            _levelUI.SetActive(_gameState == GameState.Play);
        }
    }
}
