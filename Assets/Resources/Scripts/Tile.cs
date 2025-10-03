using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    public enum TileState
    {
        Buildable,
        Occupied,
        Battlefield,
    }
    [SerializeField] private GameObject _highlightEffectGameObject;
    private TileState _currentState;
    private GameObject _currentBuilding;

    /// <summary>
    /// Initializes the tile with a given state and an optional building.
    /// </summary>
    /// <param name="initialState"></param> Initial state of the tile
    /// <param name="initialBuilding"></param> Optional initial building on the tile (trees)
    public void InitializeTile(TileState initialState, GameObject initialBuilding = null)
    {
        _currentState = initialState;
        _currentBuilding = initialBuilding;
    }

    /// <summary>
    /// Attempts to construct a building on this tile. Returns true if successful, false otherwise.
    /// </summary>
    /// <param name="buildingPrefab"></param>
    /// <returns></returns>
    public bool ConstructBuilding(GameObject buildingPrefab)
    {
        if (_currentState != TileState.Buildable) return false;
        // TODO: Add a resource check here
        _currentBuilding = Instantiate(buildingPrefab, transform.position, Quaternion.identity, transform);
        _currentState = TileState.Occupied;
        return true;
    }

    /// <summary>
    /// Demolishes the building on this tile if there is one. Trees just dissapear for a cost and towers return part of their cost.
    /// </summary>
    public void DemolishBuilding()
    {
        if (_currentState != TileState.Occupied) return;
        if (_currentBuilding.TryGetComponent<BaseTower>(out BaseTower tower))
        {
            Destroy(tower.gameObject); // TODO: Add refund logic
        }
        else
        {
            Destroy(_currentBuilding); // TODO: Add cost logic for trees
        }
        _currentBuilding = null;
        _currentState = TileState.Buildable;
    }

    /// <summary>
    /// Plays a spawn animation for the tile.
    /// </summary>
    public void SpawnTileAnimation()
    {
        transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutFlash);
    }

    /// <summary>
    /// Highlights the tile to indicate it is selectable.
    /// </summary>
    public void HighlightTile(CameraController.InteractionMode mode)
    {
        if (mode == CameraController.InteractionMode.Building && _currentState == TileState.Buildable)
        {
            _highlightEffectGameObject.GetComponent<Renderer>().material.color = Color.green;
            _highlightEffectGameObject.SetActive(true);
        }
        else if (mode == CameraController.InteractionMode.Demolishing && _currentState == TileState.Occupied)
        {
            _highlightEffectGameObject.GetComponent<Renderer>().material.color = Color.red;
            _highlightEffectGameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Removes the highlight from the tile.
    /// </summary>
    public void UnhighlightTile()
    {
        _highlightEffectGameObject.SetActive(false);
    }

    /// <summary>
    /// If there is a tower on this tile, interact with it.
    /// </summary>
    public void InteractWithTower()
    {
        if (_currentBuilding == null) return;
        if (_currentBuilding.TryGetComponent<BaseTower>(out BaseTower tower))
        {
            tower.OnInteract();
        }
    }
}
