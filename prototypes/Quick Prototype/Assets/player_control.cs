using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_control : MonoBehaviour
{
    public float speed;
    public Vector3 targetPos;
    public bool isMoving;
    public float maxSpeed;
    const int MOUSE = 0;
    // Use this for initialization1
    void Start()
    {

        targetPos = transform.position;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {

        //SetTarggetPosition();

        if (isMoving)
        {
            MoveObject();
        }
        if (Input.GetKeyUp(KeyCode.Alpha4)) isMoving = false;

        transform.rotation = Quaternion.LookRotation(Vector3.forward * -1f, Vector3.up);

    }
    void SetTarggetPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point))
            targetPos = ray.GetPoint(point);

        isMoving = true;
    }
    void MoveObject()
    {
        /*
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (transform.position == targetPos)
            isMoving = false;
        //Debug.DrawLine(transform.position, targetPos, Color.red);
        */

        Rigidbody wallRB = gameObject.GetComponent<Rigidbody>();

        Vector3 delta_velocity;

        delta_velocity = new Vector3(0, 0, 0);

        float side_angle = speed * Input.GetAxis("Mouse X");
        float forward_angle = speed * 1.5f * -Input.GetAxis("Mouse Y");
        Vector3 xadd = Vector3.forward * forward_angle;
        Vector3 zadd = Vector3.right * side_angle;

        //offsetAngle = new Vector3(yangle, xangle, 0);
        //cameraTransform.eulerAngles = offsetAngle;
        //transform.eulerAngles = new Vector3(0, xangle, 0);
        //delta_velocity += /*Mathf.Abs(xadd.z) > maxSpeed ? Vector3.forward * Time.deltaTime * -Input.GetAxis("Mouse X") * maxSpeed :*/ xadd;
        //delta_velocity += /*Mathf.Abs(yadd.x) > maxSpeed ? Vector3.right * Time.deltaTime * -Input.GetAxis("Mouse Y") * maxSpeed : */ zadd;
        //wallRB.velocity += delta_velocity;
        transform.position += xadd + zadd;
        

    }
}
