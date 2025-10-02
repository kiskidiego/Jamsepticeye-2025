using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _size = 0.5f;
    protected float _sizeSqr;
    protected float _damage = 10f;

    protected virtual void Start()
    {
        _sizeSqr = _size * _size;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        transform.LookAt(target);
        if (Vector3.SqrMagnitude(transform.position - target.position) < _sizeSqr)
        {
            Hit();
        }
    }

    protected virtual void Hit()
    {
        Hittable hittable = target.GetComponent<Hittable>();
        if (hittable != null)
        {
            hittable.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}