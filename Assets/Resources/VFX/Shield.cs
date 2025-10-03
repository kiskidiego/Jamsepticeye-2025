using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield : MonoBehaviour
{
    [SerializeField] float dissolveSpeed = 0.6f;
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        StartCoroutine(ActivateShield());
    }

    public IEnumerator ActivateShield()
    {
        _renderer.material.SetFloat("_DissolveFactor", 0f);
        float start = 0f;
        float target = 1f;
        float lerp = 0f;
        while(lerp < 1)
        {
            _renderer.material.SetFloat("_DissolveFactor", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * dissolveSpeed;
            yield return null;
        }
        _renderer.material.SetFloat("_DissolveFactor", target);
    }

    public IEnumerator DissolveShield()
    {
        _renderer.material.SetFloat("_DissolveFactor", 1f);
        float start = 1f;
        float target = 0f;
        float lerp = 0f;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_DissolveFactor", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * dissolveSpeed;
            yield return null;
        }
        _renderer.material.SetFloat("_DissolveFactor", target);
    }
}
