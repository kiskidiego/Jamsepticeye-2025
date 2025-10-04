using DG.Tweening;
using UnityEngine;

public class CancelMenu : BaseMenu
{
    [SerializeField] CameraController _cameraController;

    /// <summary>
    /// Closes the current menu and returns to the normal game state. 
    /// </summary>
    public void Cancel()
    {
        GameManager.Instance.ShowHUD();
        _cameraController.ExitState();
    }

    /// <summary>
    /// Opens the cancel menu.
    /// </summary>
    public override void OpenMenu()
    {
        if (_state == MenuState.Open) return;

        _state = MenuState.Open;
        _menuTransform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Closes the cancel menu.
    /// </summary>
    public override void CloseMenu()
    {
        if (_state == MenuState.Closed) return;

        _state = MenuState.Closed;
        _menuTransform.DOLocalMove(_closedPosition, _animationDuration).SetEase(Ease.InBack);
    }
}
