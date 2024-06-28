using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            // spawn a dot in the UI at the start position
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
        }
        if(startPos != Vector3.zero && endPos != Vector3.zero)
        {
            Vector3 direction = new Vector3(startPos.x - endPos.x, 0, startPos.y - endPos.y);

            SphereManager.instance.ApplyForce(direction.normalized, direction.magnitude / 10);
            startPos = endPos = Vector3.zero;
        }
    }

}
