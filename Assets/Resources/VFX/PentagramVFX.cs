using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
[ExecuteAlways]
public class PentagramVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Transform child;

    ParticleSystem.Particle[] particles;
    private bool waitingToActivate = false;

    void Awake()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[1];

        // El hijo empieza desactivado siempre
        child.gameObject.SetActive(false);
    }

    void Update()
    {
        // Detecta cuando el sistema empieza a jugar
        if (ps.isPlaying && !waitingToActivate && !child.gameObject.activeSelf)
        {
            waitingToActivate = true;
            StartCoroutine(ActivateChildDelayed());
        }

        // Cuando el sistema muere, desactivar el hijo inmediatamente
        if (!ps.IsAlive() && child.gameObject.activeSelf)
        {
            child.gameObject.SetActive(false);
        }

        // Sincronizar tamaño solo si está vivo
        if (ps.IsAlive() && child.gameObject.activeSelf)
        {
            int count = ps.GetParticles(particles);
            if (count > 0)
            {
                float particleSize = particles[0].GetCurrentSize(ps);
                child.localScale = Vector3.one * particleSize;
            }
        }
    }

    private IEnumerator ActivateChildDelayed()
    {
        yield return new WaitForSeconds(1f); // delay de 1 segundo

        // Solo activar si el sistema sigue activo
        if (ps.isPlaying)
        {
            child.gameObject.SetActive(true);
        }

        waitingToActivate = false;
    }
}
