using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum InteractionMode
    {
        Spellcasting,
        Building,
        Demolishing,
        Selecting
    }
    [SerializeField] private float _moveSpeed = 5f;
    private Camera _camera;
    private Vector3 _cameraForward;
    private InteractionMode _currentMode;
    private Tile _currentlyHighlightedTile;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraForward = Vector3.Cross(_camera.transform.right, Vector3.up);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D)) transform.position += _moveSpeed * Time.deltaTime * _camera.transform.right;
        if (Input.GetKey(KeyCode.A)) transform.position += _moveSpeed * Time.deltaTime * -_camera.transform.right;
        if (Input.GetKey(KeyCode.W)) transform.position += _moveSpeed * Time.deltaTime * _cameraForward;
        if (Input.GetKey(KeyCode.S)) transform.position += _moveSpeed * Time.deltaTime * -_cameraForward;
        if (Input.GetKeyDown("1")) _currentMode = InteractionMode.Demolishing;
        else if (Input.GetKeyDown("2")) _currentMode = InteractionMode.Building;
        else if (Input.GetKeyDown("3")) _currentMode = InteractionMode.Spellcasting;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent<Tile>(out Tile tile))
            {
                if (tile != _currentlyHighlightedTile)
                {
                    _currentlyHighlightedTile?.UnhighlightTile();
                    _currentlyHighlightedTile = tile;
                    _currentlyHighlightedTile.HighlightTile(_currentMode);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    switch (_currentMode)
                    {
                        case InteractionMode.Demolishing:
                            tile.DemolishBuilding();
                            break;
                        case InteractionMode.Building:
                            GameObject towerPrefab = Resources.Load<GameObject>("Prefabs/TestTower");
                            tile.ConstructBuilding(towerPrefab);
                            break;
                        case InteractionMode.Selecting:
                            tile.InteractWithTower();
                            break;
                    }
                }
            }
        }
        else
        {
            _currentlyHighlightedTile?.UnhighlightTile();
            _currentlyHighlightedTile = null;
        }
    }
}