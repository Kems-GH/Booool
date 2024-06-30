using UnityEngine;

public class Sphere : MonoBehaviour
{
    public const int MAX_LEVEL = 11;
    public const float WALLS_LIMIT = 5f;
    public const float SIZE_SCALE_FACTOR = 0.35f;
    public bool canMove { get; private set; } = false;
    public float scale { get; private set; } = 0.35f;
    public int level { get; private set; } = 1;

    private Vector3 _direction = Vector3.zero;
    private float _force = 0f;
    private float _slowDownFactor = 0.9f;
    private Renderer _render;

    [SerializeField] private GameObject _visual;


    private void Start()
    {
        _render = GetComponentInChildren<Renderer>();
        UpdateColor();
        scale = level * SIZE_SCALE_FACTOR;
        transform.localScale = Vector3.one * scale;
    }

    public void Move(Vector3 direction, float force)
    {
        this._direction = direction.normalized;
        this._force = force;
        canMove = true;
    }

    private void UpdateColor()
    {
        _render.material.color = SphereColor.Colors[level - 1];
    }

    private void FixedUpdate()
    {
        if (transform.localScale.y < scale)
        {
            transform.localScale += 0.1f * Vector3.one;
        }
        CheckCollisionWithWalls();
        CheckCollision();

        if (!canMove) return;
        Move();
        RotateVisual();
    }

    private void Move()
    {
        if (_force <= 0.1f) return;
        transform.position += new Vector3(_direction.x, 0, _direction.z) * _force * Time.fixedDeltaTime;

        _force -= _force * _slowDownFactor * Time.fixedDeltaTime;
    }

    private void RotateVisual()
    {
        if (_force <= 1f) return;
        transform.rotation = Quaternion.LookRotation(_direction);
        _visual.transform.Rotate(Vector3.right, _force);
    }

    private void CheckCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, scale / 2);
        if (colliders == null || colliders.Length == 0) return;
        HandleCollision(NearesCollider(colliders));
    }

    private Collider NearesCollider(Collider[] colliders)
    {
        Collider nearestCollider = null;
        float minDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;
            if (collider.CompareTag("Ground")) continue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCollider = collider;
            }
        }
        return nearestCollider;
    }

    private void HandleCollision(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Sphere")) return;

        Sphere otherSphere = collider.gameObject.GetComponent<Sphere>();
        if (otherSphere == null) return;

        if (!otherSphere.canMove)
        {
            ReflectSphere(otherSphere);
            return;
        }
        if (!canMove) return;

        // Prevent spheres from sticking together
        AdjustPositionsAfterCollision(otherSphere);

        if (!IsSameLevel(otherSphere))
        {
            if (HasMoreForce(otherSphere))
            {
                otherSphere._direction = _direction;
                otherSphere._force = this._force / otherSphere.level;
                ReflectSphere(otherSphere);
            }
            return;
        }

        if (HasMoreForce(otherSphere))
        {
            if (level == MAX_LEVEL)
            {
                Destroy(otherSphere.gameObject);
                Destroy(gameObject);
            }
            LevelUp();
            Destroy(otherSphere.gameObject);
            return;
        }
    }

    private void ReflectSphere(Sphere otherSphere)
    {
        Vector3 collisionNormal = (otherSphere.transform.position - transform.position).normalized;
        _direction = Vector3.Reflect(_direction, collisionNormal).normalized;
    }

    private void AdjustPositionsAfterCollision(Sphere otherSphere)
    {
        Vector3 collisionNormal = (otherSphere.transform.position - transform.position).normalized;
        float overlap = (scale / 2 + otherSphere.scale / 2) - Vector3.Distance(transform.position, otherSphere.transform.position);
        transform.position -= collisionNormal * overlap / 2;
        otherSphere.transform.position += collisionNormal * overlap / 2;
    }

    private void CheckCollisionWithWalls()
    {
        Vector3 position = transform.position;
        float radius = transform.localScale.x / 2;

        // Assuming walls at x = -5, x = 5, y = -5, y = 5
        if (position.x - radius < -WALLS_LIMIT || position.x + radius > WALLS_LIMIT)
        {
            _direction.x = -_direction.x;
            transform.position = new Vector3(Mathf.Clamp(position.x, -WALLS_LIMIT + radius, WALLS_LIMIT - radius), 0, position.z);
        }

        if (position.z - radius < -WALLS_LIMIT || position.z + radius > WALLS_LIMIT)
        {
            _direction.z = -_direction.z;
            transform.position = new Vector3(position.x, 0, Mathf.Clamp(position.z, -WALLS_LIMIT + radius, WALLS_LIMIT - radius));
        }
    }

    private bool IsSameLevel(Sphere otherSphere)
    {
        return this.level == otherSphere.level;
    }

    private bool HasMoreForce(Sphere otherSphere)
    {
        return this._force > otherSphere._force;
    }

    private void LevelUp()
    {
        ScoreManager.Instance.AddScore(level * 10);
        level++;
        scale += SIZE_SCALE_FACTOR;
        GameManager.Instance.PlaySound();
        UpdateColor();
    }

    public void SetLevel(int level)
    {
        this.level = level;
        scale = level * SIZE_SCALE_FACTOR;
    }
}
