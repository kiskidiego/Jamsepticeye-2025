using DG.Tweening;
using UnityEngine;

/// <summary>
/// Manages the spell unlock menu.
/// </summary>
public class SpellUnlockMenu : BaseMenu
{
    public override void OpenMenu()
    {
        _menuTransform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
        _open = true;
    }

    public override void CloseMenu()
    {
        _menuTransform.DOLocalMove(new Vector3(Screen.width, 0, 0), _animationDuration).SetEase(Ease.OutBack);
        _open = false;
    }
}