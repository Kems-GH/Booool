using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereManager : MonoBehaviour
{
    public static SphereManager instance { get; private set; }
    private Sphere m_Sphere;
    public GameObject spherePrefab;
    private Vector3 spawnPosition = new Vector3(0, 0, -3.2f);

    private void Start()
    {
        m_Sphere = Instantiate(spherePrefab, spawnPosition, spherePrefab.transform.rotation).GetComponent<Sphere>();
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void ApplyForce(Vector3 direction, float force)
    {
        if (m_Sphere == null) return;

        // max force is 20
        force = Mathf.Clamp(force, 0, 20);

        m_Sphere.Move(direction, force);
        m_Sphere = null;

        StartCoroutine(SpawnSpherer());
    }

    private IEnumerator SpawnSpherer()
    {
        //int level = Random.Range(1, 4);
        yield return new WaitForSeconds(1.5f);
        // Liberate the zone before spawning a new one
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);
        // Move all the spheres in the zone
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Sphere"))
            {
                collider.GetComponent<Sphere>().Move(Vector3.forward, 10);
            }
        }

        m_Sphere = Instantiate(spherePrefab, spawnPosition, spherePrefab.transform.rotation).GetComponent<Sphere>();
        //m_Sphere.SetLevel(level);
        CalculeSurface();
    }

    public int CalculeSurface()
    {
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        float surface = 0;
        foreach (var sphere in spheres)
        {
            surface += (Mathf.PI * Mathf.Pow(sphere.scale / 2, 2));
        }
        Debug.Log(surface);
        return (int)surface;
    }
}
