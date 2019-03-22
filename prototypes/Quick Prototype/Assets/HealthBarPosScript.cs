using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPosScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float height;

    private GameObject c;
    public Transform par;
    public Transform grandpar;
    void Start()
    {
        c = GameObject.Find("Main Camera");
        par = transform.parent;
        grandpar = transform.parent.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(c.transform);
        par.position = grandpar.position + Vector3.up * height;
    }

}
