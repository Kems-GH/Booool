using System;
using System.Collections;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public static SphereManager instance { get; private set; }

    public event Action<float> OnSurfaceChange;

    private Sphere m_Sphere;
    public GameObject spherePrefab;
    private Vector3 spawnPosition = new Vector3(0, 0, -3.2f);

    public float surface { get; private set; } = 0;
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
        if (GameManager.instance.isGamePause) return;

        ScoreManager.instance.ResetCombot();

        // max force is 20
        force = Mathf.Clamp(force, 0, 20);

        m_Sphere.Move(direction, force);
        m_Sphere = null;

        StartCoroutine(SpawnSpherer());
    }

    private IEnumerator SpawnSpherer()
    {
        int level = UnityEngine.Random.Range(1, 6);
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
        m_Sphere.SetLevel(level);
        CalculeSurface();
    }

    public void CalculeSurface()
    {
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        surface = 0;
        foreach (var sphere in spheres)
        {
            surface += (Mathf.PI * Mathf.Pow(sphere.scale / 2, 2));
        }
        OnSurfaceChange?.Invoke(surface);
        if (surface >= 75)
        {
            GameManager.instance.GameOver();
        }
    }
}
