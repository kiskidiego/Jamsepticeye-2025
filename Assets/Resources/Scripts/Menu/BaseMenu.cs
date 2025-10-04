using FMODUnity;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] protected EventReference _menuInteractionSound;
    [SerializeField] protected RectTransform _menuTransform;
    [SerializeField] protected float _animationDuration = 0.5f;
    protected bool _open;

    /// <summary>
    /// Opens the menu.
    /// </summary>
    public abstract void OpenMenu();

    /// <summary>
    /// Closes the menu.
    /// </summary>
    public abstract void CloseMenu();
}