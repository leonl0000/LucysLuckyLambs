using UnityEngine;

public class LureScript : MonoBehaviour
{
    public hellSceneManager hsm;

    public int index;
    public float lifetime;
    public float age = 0;

    // Start is called before the first frame update
    void Start()
    {
        hsm = FindObjectOfType<hellSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // update age
        age += Time.deltaTime;

        // update transparency
        var color = gameObject.GetComponent<Renderer>().material.color;
        color.a = Mathf.Clamp((lifetime - age) / lifetime, 0, 1);

        // remove if past lifetime
        if (age >= lifetime) {
            hsm.sheepDict.Remove(index);
            Destroy(gameObject);
        }
    }
}
