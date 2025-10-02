using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [SerializeField] float _explosionRadius = 5f;
    [SerializeField] float _arcHeight = 5f;
    float a, b, c; // Variables for parabolic equation
    Vector3 startPosition;
    Vector3 targetPosition;
    public bool isAlly;
    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        targetPosition = target.position;
        // Calculate coefficients for parabolic trajectory
        Vector2 end = new Vector2(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)), targetPosition.y);
        Vector2 middle = new Vector2(end.x / 2, Mathf.Max(transform.position.y, targetPosition.y) + _arcHeight);
        CalculateParabolaCoefficients(transform.position.y, middle, end);
    }

    void CalculateParabolaCoefficients(float startY, Vector2 middle, Vector2 end)
    {
        float denom = middle.x * middle.x * end.x - end.x * end.x * middle.x;

        if (Mathf.Abs(denom) < Mathf.Epsilon)
        {
            throw new System.Exception("Points are collinear; cannot compute unique parabola.");
        }

        a = ((middle.y - startY) * end.x - (end.y - startY) * middle.x) / denom;
        b = ((end.y - startY) * middle.x * middle.x - (middle.y - startY) * end.x * end.x) / denom;
        c = startY;
    }

    protected override void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        // Move towards target horizontally
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), _speed * Time.deltaTime);

        // Calculate new height using parabolic equation
        float distanceToStart = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(startPosition.x, 0, startPosition.z));
        float newY = a * distanceToStart * distanceToStart + b * distanceToStart + c;
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