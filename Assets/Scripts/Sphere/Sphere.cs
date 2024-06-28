using UnityEngine;

public class Sphere : MonoBehaviour
{
    public const int MAX_LEVEL = 11;
    public bool canMove { get; private set; } = false;
    private Vector3 direction = Vector3.zero;
    private float force = 0f;
    private float slowDownFactor = 0.9f;
    public float scale { get; private set; } = 0.35f;
    private float sizeScaleFactor = 0.35f;

    private Renderer render;

    [SerializeField] private GameObject visual;

    public int level { get; private set; } = 1;

    private void Start()
    {
        render = GetComponentInChildren<Renderer>();
        UpdateColor();
    }
    public void Move(Vector3 direction, float force)
    {
        this.direction = direction;
        this.force = force;
        canMove = true;
    }
    private void UpdateColor()
    {
        render.material.color = SphereColor.colors[level - 1];
    }

    private void FixedUpdate()
    {
        CheckCollision();
        if (transform.localScale.y < scale)
        {
            transform.localScale += 0.1f * Vector3.one;
        }
        if (!canMove) return;
        if (force <= 0.1f)
        {
            return;
        }

        if (canMove)
        {
            transform.position += new Vector3(direction.x, 0, direction.z) * force * Time.fixedDeltaTime;

            transform.rotation = Quaternion.LookRotation(direction);
            visual.transform.Rotate(Vector3.right, force);

        }
        force -= force * slowDownFactor * Time.fixedDeltaTime;

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position + direction, (scale / 2));
    //}

    private void CheckCollision()
    {
        // Vérifier si la sphère est déjà en collision avec une autre
        Collider[] colliders = Physics.OverlapSphere(transform.position, scale / 2);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Sphere") && collider != GetComponent<Collider>())
            {
                // Empêcher les sphères de rentrer l'une dans l'autre
                Vector3 separationDirection = (transform.position - collider.transform.position).normalized;
                transform.position += separationDirection * Time.fixedDeltaTime;
            }
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scale / 2, direction, 0.1f);
        foreach (var hit in hits)
        {
            HasHit(hit.collider, hit.normal);
        }
    }

    private void HasHit(Collider collider, Vector3 normal)
    {

        if (!collider.gameObject.CompareTag("Sphere"))
        {
            direction = Vector3.Reflect(direction, normal);
            return;
        }

        Sphere otherSphere = collider.gameObject.GetComponent<Sphere>();
        if (otherSphere == null) return;

        if (!otherSphere.canMove)
        {
            direction = Vector3.Reflect(direction, normal);
            return;
        }

        if (!IsSameLevel(otherSphere))
        {
            if(level == MAX_LEVEL)
            {
                Destroy(otherSphere.gameObject);
                Destroy(gameObject);
            }
            if (HasMoreForce(otherSphere))
            {
                otherSphere.direction = direction;
                otherSphere.force = this.force / otherSphere.level;
                direction = Vector3.Reflect(direction, normal);
            }
            return;
        }

        if (HasMoreForce(otherSphere))
        {
            LevelUp();
            Destroy(otherSphere.gameObject);
            return;
        }
    }

    private bool IsSameLevel(Sphere otherSphere)
    {
        return this.level == otherSphere.level;
    }

    private bool HasMoreForce(Sphere otherSphere)
    {
        return this.force > otherSphere.force;
    }
    private void LevelUp()
    {
        ScoreManager.instance.AddScore(level * 10);
        level++;
        scale += sizeScaleFactor;
        GameManager.instance.PlaySound();
        UpdateColor();
    }
    public void SetLevel(int level)
    {
        this.level = level;
        scale = level * sizeScaleFactor;
    }
}
