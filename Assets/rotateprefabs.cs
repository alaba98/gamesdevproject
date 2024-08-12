using UnityEngine;

public class ElevateAndRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation 
    public float elevationOffset = 0.5f; // How much to elevate the object

    void Start()
    {
        // Elevate the object slightly off the ground
        transform.position += new Vector3(0, elevationOffset, 0);
    }

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); //vector3.up = (0,1,0)
    }
}
