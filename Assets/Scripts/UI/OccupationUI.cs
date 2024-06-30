using UnityEngine;
using UnityEngine.UI;

public class OccupationUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    private Image _fill;
    private SphereManager _sphereManager;
    private float _surface = 0;
    void Start()
    {
        _sphereManager = SphereManager.Instance;
        _fill = _slider.fillRect.GetComponent<Image>();
        _slider.value = 0;
        _sphereManager.OnSurfaceChange += UpdateSurface;
    }

    private void UpdateSurface(float surface)
    {
        this._surface = surface;
    }

    void Update()
    {
        if (_surface > _slider.value)
        {
            _slider.value += 0.05f;
            _text.text = $"{(int)(_slider.value)}%";
        }
        else if (_surface < _slider.value)
        {
            _slider.value -= 0.01f;
            _text.text = $"{(int)(_slider.value)}%";
        }
        _fill.color = SphereColor.Colors[(int)(_slider.value * 0.11f)];

    }
    private void OnDestroy()
    {
        _sphereManager.OnSurfaceChange -= UpdateSurface;
    }
}
