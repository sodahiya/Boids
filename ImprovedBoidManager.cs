using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedBoidManager : MonoBehaviour
{
    [SerializeField]
    float sphereSize = 30f;
    [SerializeField]
    float minSpeed = 5f;
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    float maxCloseDistance = 2f;
    [SerializeField]
    float maxSeeDistance = 5f;

    [SerializeField]
    float avoidFactor = 2f;  // Increased for stronger separation
    [SerializeField]
    float alignFactor = 0.5f;  // Decreased for less alignment influence
    [SerializeField]
    float cohesionFactor = 0.05f;  // Decreased for less cohesion influence
    [SerializeField]
    float turnFactor = 0.1f;  // Slightly adjusted for smooth turning

    float speed;

    Vector3 avoidVector = Vector3.zero;
    Vector3 alignVector = Vector3.zero;
    Vector3 cohesionVector = Vector3.zero;

    public float vx;
    public float vy;
    public float vz;

    [SerializeField]
    Vector3 velocity;
    Vector3 direction = Vector3.right;
    Vector3 origin = Vector3.zero;
    Vector3 movementVector;
    GameObject boidManager;

    List<GameObject> boidPool;

    void Start()
    {
        velocity = Random.insideUnitSphere.normalized * minSpeed;
        boidManager = GameObject.Find("BoidManager");
        boidPool = boidManager.GetComponent<BoidManagerScript>().boidPool;
    }

    void FixedUpdate()
    {
        Separation();
        Alignment();
        Cohesion();
        Turn();

        speed = velocity.magnitude;

        if (speed > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        else if (speed < minSpeed)
        {
            velocity = velocity.normalized * minSpeed;
        }

        movementVector = velocity.magnitude != 0 ? velocity : direction;

        transform.position += movementVector * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(movementVector);
    }

    void Separation()
    {
        Vector3 closeOffset = Vector3.zero;

        foreach (GameObject otherBoid in boidPool)
        {
            if (otherBoid == gameObject) continue;

            if (Vector3.Distance(transform.position, otherBoid.transform.position) < maxCloseDistance)
            {
                closeOffset += transform.position - otherBoid.transform.position;
                Debug.DrawLine(transform.position, otherBoid.transform.position, Color.red);
            }
        }

        avoidVector = closeOffset;
        velocity += avoidVector * avoidFactor;
    }

    void Alignment()
    {
        Vector3 velAvg = Vector3.zero;
        int neighborBoids = 0;

        foreach (GameObject otherBoid in boidPool)
        {
            if (otherBoid == gameObject) continue;

            if (Vector3.Distance(transform.position, otherBoid.transform.position) < maxSeeDistance)
            {
                ImprovedBoidManager otherBoidScript = otherBoid.GetComponent<ImprovedBoidManager>();
                velAvg += new Vector3(otherBoidScript.vx, otherBoidScript.vy, otherBoidScript.vz);
                neighborBoids++;
                Debug.DrawLine(transform.position, otherBoid.transform.position, Color.green);
            }
        }

        if (neighborBoids > 0)
        {
            velAvg /= neighborBoids;
        }

        alignVector = velAvg;
        velocity += (alignVector - velocity) * alignFactor;
    }

    void Cohesion()
    {
        Vector3 posAvg = Vector3.zero;
        int neighborBoids = 0;

        foreach (GameObject otherBoid in boidPool)
        {
            if (otherBoid == gameObject) continue;

            if (Vector3.Distance(transform.position, otherBoid.transform.position) < maxSeeDistance)
            {
                posAvg += otherBoid.transform.position;
                neighborBoids++;
            }
        }

        if (neighborBoids > 0)
        {
            posAvg /= neighborBoids;
        }

        cohesionVector = posAvg;
        velocity += (cohesionVector - transform.position) * cohesionFactor;
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, origin) > sphereSize)
        {
            Vector3 flipVector = transform.position - origin;
            transform.position = -flipVector;
        }
    }

    void Turn()
    {
        if (transform.position.x < -sphereSize) velocity.x += turnFactor;
        if (transform.position.x > sphereSize) velocity.x -= turnFactor;
        if (transform.position.y < -sphereSize) velocity.y += turnFactor;
        if (transform.position.y > sphereSize) velocity.y -= turnFactor;
        if (transform.position.z < -sphereSize) velocity.z += turnFactor;
        if (transform.position.z > sphereSize) velocity.z -= turnFactor;
    }
}