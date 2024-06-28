using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isDragging = false;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (!isDragging) lineRenderer.enabled = false;
        else
        {
            lineRenderer.enabled = true;
            Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(startPos) + new Vector3(0, -1, 0);
            Vector3 worldEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -1, 0);
            lineRenderer.SetPosition(0, worldStartPos);
            lineRenderer.SetPosition(1, worldEndPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            isDragging = false;
        }
        

        if(startPos != Vector3.zero && endPos != Vector3.zero)
        {
            Vector3 direction = new Vector3(startPos.x - endPos.x, 0, startPos.y - endPos.y);

            SphereManager.instance.ApplyForce(direction.normalized, direction.magnitude / 10);
            startPos = endPos = Vector3.zero;
        }
    }

}
