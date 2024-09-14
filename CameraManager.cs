using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    float speed = 500.0f;

    [SerializeField]
    float ZoomScale = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        camOrbit();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * speed * Time.deltaTime, 0);
    }

    void camOrbit()
    {
        if(Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0)
        {
            float verticalIInput = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * speed * Time.deltaTime;

            transform.Rotate(Vector3.right,-verticalIInput);
            transform.Rotate(Vector3.forward, -horizontalInput, Space.World);

        }
    }
}
