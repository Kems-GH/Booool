using UnityEngine;
using UnityEngine.UI;

public class OccupationUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    private Image fill;
    private SphereManager sphereManager;
    private float surface = 0;
    void Start()
    {
        sphereManager = SphereManager.instance;
        fill = slider.fillRect.GetComponent<Image>();
        slider.value = 0;
        sphereManager.OnSurfaceChange += UpdateSurface;
    }

    private void UpdateSurface(float surface)
    {
        this.surface = surface;
    }
    
    void Update()
    {
        if(surface > slider.value)
        {
            slider.value += 0.05f;
            text.text = $"{(int)(slider.value)}%";
        }
        else if(surface < slider.value)
        {
            slider.value -= 0.01f;
            text.text = $"{(int)(slider.value)}%";
        }
        fill.color = SphereColor.colors[(int)(slider.value * 0.11f)];

    }
    private void OnDestroy()
    {
        sphereManager.OnSurfaceChange -= UpdateSurface;
    }
}
