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

    private TileState _currentState;
    private GameObject _currentBuilding;
    private GameObject _highlightEffectGameObject;

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

    private void HighlightTile()
    {
        if (_highlightEffectGameObject == null) return;
        _highlightEffectGameObject.SetActive(true);
    }

    private void UnhighlightTile()
    {
        if (_highlightEffectGameObject == null) return;
        _highlightEffectGameObject.SetActive(false);
    }
}
