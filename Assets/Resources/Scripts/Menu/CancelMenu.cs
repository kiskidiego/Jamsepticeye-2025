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
        if (_open) return;

        _open = true;
        _menuTransform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Closes the cancel menu.
    /// </summary>
    public override void CloseMenu()
    {
        if (!_open) return;

        _open = false;
        _menuTransform.DOLocalMove(new Vector3(0, Screen.height, 0), _animationDuration).SetEase(Ease.InBack);
    }
}
