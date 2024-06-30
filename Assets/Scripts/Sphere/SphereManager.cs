using System;
using System.Collections;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public static SphereManager Instance { get; private set; }

    public event Action<float> OnSurfaceChange;

    private Sphere _sphere;
    [SerializeField] private GameObject _spherePrefab;
    private Vector3 _spawnPosition = new Vector3(0, 0, -3.2f);

    public float surface { get; private set; } = 0;
    private void Start()
    {
        _sphere = Instantiate(_spherePrefab, _spawnPosition, _spherePrefab.transform.rotation).GetComponent<Sphere>();
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ApplyForce(Vector3 direction, float force)
    {
        if (_sphere == null) return;
        if (GameManager.Instance.isGamePause) return;

        ScoreManager.Instance.ResetCombot();

        // max force is 20
        force = Mathf.Clamp(force, 0, 20);

        _sphere.Move(direction, force);
        _sphere = null;

        StartCoroutine(SpawnSpherer());
    }

    private IEnumerator SpawnSpherer()
    {
        int level = UnityEngine.Random.Range(1, 6);
        yield return new WaitForSeconds(1.5f);
        // Liberate the zone before spawning a new one
        Collider[] colliders = Physics.OverlapSphere(_spawnPosition, Sphere.SIZE_SCALE_FACTOR * level + 2);
        // Move all the spheres in the zone
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Sphere"))
            {
                collider.GetComponent<Sphere>().Move(Vector3.forward, 10);
            }
        }

        _sphere = Instantiate(_spherePrefab, _spawnPosition, _spherePrefab.transform.rotation).GetComponent<Sphere>();
        _sphere.SetLevel(level);
    }
    private void Update()
    {
        CalculeSurface();
    }
    public void CalculeSurface()
    {
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        surface = 0;

        foreach (var sphere in spheres)
        {
            if (sphere != null)
            {
                float radius = sphere.scale / 2;
                surface += Mathf.PI * Mathf.Pow(radius, 2);
            }
        }

        OnSurfaceChange?.Invoke(surface);

        if (surface >= 75)
        {
            GameManager.Instance.GameOver();
        }
    }
}
