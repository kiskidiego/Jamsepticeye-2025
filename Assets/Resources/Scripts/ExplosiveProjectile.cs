using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] float _explosionRadius = 5f;
    [SerializeField] float _arcHeight = 5f;
    float a, b, c; // Variables for parabolic equation
    Vector3 targetPosition;
    public bool isAlly;
    protected override void Start()
    {
        base.Start();
        targetPosition = target.position;
        // Calculate coefficients for parabolic trajectory
        float heightDifference = targetPosition.y - transform.position.y;
        float horizontalDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z));
        float peakHeight = Mathf.Max(transform.position.y, targetPosition.y) + _arcHeight;
        c = transform.position.y;
        a = (targetPosition.y -c - b * horizontalDistance) / horizontalDistance * horizontalDistance;
        
    }

    protected override void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // Move towards target horizontally
        Vector3 horizontalDirection = (new Vector3(targetPosition.x, 0, targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        transform.position += horizontalDirection * _speed * Time.deltaTime;

        // Calculate new height using parabolic equation
        float horizontalDistanceTravelled = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(transform.position.x - horizontalDirection.x * _speed * Time.deltaTime, 0, transform.position.z - horizontalDirection.z * _speed * Time.deltaTime));
        float newY = a * horizontalDistanceTravelled * horizontalDistanceTravelled + b * horizontalDistanceTravelled + c;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        transform.LookAt(target);
        if (Vector3.SqrMagnitude(transform.position - targetPosition) < _sizeSqr)
        {
            Hit();
        }
    }

    protected override void Hit()
    {
        if (isAlly)
        {
            Hittable[] hittables = GameManager.Instance.GetAllEnemiesInRange(transform.position, _explosionRadius);
            foreach (Hittable hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
        else
        {
            Hittable[] hittables = GameManager.Instance.GetAllAlliesInRange(transform.position, _explosionRadius);
            foreach (Hittable hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
    }
}