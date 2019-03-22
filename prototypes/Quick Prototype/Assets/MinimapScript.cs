using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimapScript : MonoBehaviour
{
    public Camera cam;
    public Transform playerRB;
    public float zoomScale;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 updatePos = playerRB.position;
        updatePos.y = transform.position.y;
        transform.position = updatePos;
        transform.rotation = Quaternion.Euler(90, playerRB.eulerAngles.y, 0);
    }

    private void Update()
    {
        float newSize = cam.orthographicSize - Input.mouseScrollDelta.y * zoomScale;
        newSize = newSize <= 0 ? 5 : newSize;
        cam.orthographicSize = newSize;
    }

}
