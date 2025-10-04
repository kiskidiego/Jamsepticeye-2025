using FMODUnity;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] protected EventReference _menuInteractionSound;
    [SerializeField] protected RectTransform _menuTransform;
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