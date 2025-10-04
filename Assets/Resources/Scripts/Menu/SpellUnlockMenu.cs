using DG.Tweening;
using UnityEngine;

/// <summary>
/// Manages the spell unlock menu.
/// </summary>
public class SpellUnlockMenu : BaseMenu
{
    public override void OpenMenu()
    {
        if (_state == MenuState.Opened) return;

        _menuTransform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
        _state = MenuState.Opened;
    }

    public override void CloseMenu()
    {
        if (_state == MenuState.Closed) return;

        _menuTransform.DOLocalMove(_closedPosition, _animationDuration).SetEase(Ease.OutBack);
        _state = MenuState.Closed;
    }
}