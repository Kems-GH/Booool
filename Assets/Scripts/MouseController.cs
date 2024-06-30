using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isDragging = false;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (!_isDragging)
        {
            _lineRenderer.enabled = false;
        }
        else
        {
            _lineRenderer.enabled = true;
            Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(_startPos) + new Vector3(0, -1, 0);
            Vector3 worldEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -1, 0);
            _lineRenderer.SetPosition(0, worldStartPos);
            _lineRenderer.SetPosition(1, worldEndPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
            _isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            _isDragging = false;
        }

        if (_startPos != Vector3.zero && _endPos != Vector3.zero)
        {
            Vector3 direction = new Vector3(_startPos.x - _endPos.x, 0, _startPos.y - _endPos.y);

            SphereManager.Instance.ApplyForce(direction.normalized, direction.magnitude / 10);
            _startPos = _endPos = Vector3.zero;
        }
    }
}
