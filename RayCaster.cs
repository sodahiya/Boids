using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField] int numRays = 100;
    [SerializeField] float rayLength = 5f;
    [SerializeField] float maxAngle = 45.0f;
    [SerializeField] float turnFraction = 1.618f;
    [SerializeField] LayerMask layerMask;

    private BoidBehaviourScript bbs;
    private Color col;

    void Start()
    {
        bbs = gameObject.GetComponent<BoidBehaviourScript>();
        turnFraction = Mathf.Deg2Rad * turnFraction;
        maxAngle = Mathf.Deg2Rad * maxAngle;
        col = Color.green;
    }

    void FixedUpdate()
    {
        CastRays();
    }

    void CastRays()
    {
        for (int i = 0; i < numRays; i++)
        {
            float azimuthal = 2 * Mathf.PI * i / turnFraction;
            float angle = Mathf.Acos(1 - 2 * (i + 0.5f) / numRays);
            angle = Mathf.Min(angle, maxAngle);

            float x = Mathf.Sin(angle) * Mathf.Cos(azimuthal);
            float y = Mathf.Sin(angle) * Mathf.Sin(azimuthal);
            float z = Mathf.Cos(angle);

            Vector3 direction = new Vector3(x, y, z);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction * rayLength,out hit, layerMask))
            {
                Debug.DrawRay(transform.position, direction * rayLength, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayLength, col);
            }
        }
    }
}
