using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviourScript : MonoBehaviour
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
    float avoidFactor = 1f;
    [SerializeField]
    float alignFactor = 1f;
    [SerializeField]
    float cohesionFactor = 1f;
    [SerializeField]
    float turnFactor = 1f;

    float speed;

    Vector3 avoidVector = new Vector3(0, 0, 0);
    Vector3 alignVector= new Vector3(0, 0, 0);
    Vector3 cohesionVector = new Vector3(0, 0, 0);

    public float vx;
    public float vy;
    public float vz;


    [SerializeField]
    Vector3 velocity;
    Vector3 direction = new Vector3(1, 0, 0);
    Vector3 origin = new Vector3(0, 0, 0);
    Vector3 movementVector;
    GameObject boidManager;

    List<GameObject> boidPool;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Random.insideUnitSphere.normalized;
        boidManager = GameObject.Find("BoidManager");
        boidPool = boidManager.GetComponent<BoidManagerScript>().boidPool;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sepration();
        Alignment();
        Cohesion();
        Turn();


        speed = Mathf.Sqrt(velocity.x *velocity.y + velocity.y * velocity.z + velocity.x * velocity.z);

        if (speed > maxSpeed) 
        { 
            velocity = (velocity/speed)* maxSpeed;
        }
        if (speed < minSpeed) {
            velocity = (velocity / speed) * minSpeed;
        }

        if (Vector3.Magnitude(velocity) != 0)
        {
            movementVector = velocity;
            Debug.Log("movement vector is veloity");
        }
        else {
            movementVector = direction;
            Debug.Log("movement vector is direction");
        }


        transform.position += movementVector* Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(movementVector);
    }

    void Sepration()
    {
        float close_dx = 0;
        float close_dy = 0;
        float close_dz = 0;

        foreach (GameObject OtherBoid in boidPool)
        {
            if(Vector3.Distance(transform.position,OtherBoid.transform.position) < maxCloseDistance)
            {
                close_dx += transform.position.x - OtherBoid.transform.position.x;
                close_dy += transform.position.y - OtherBoid.transform.position.y;
                close_dz += transform.position.z - OtherBoid.transform.position.z;

                Debug.DrawLine(transform.position, OtherBoid.transform.position, Color.red);
            }
        }
        
        avoidVector = new Vector3 (close_dx, close_dy, close_dz);
        velocity += avoidVector * avoidFactor;
    }

    void Alignment()
    {
        float xvel_avg =0;
        float yvel_avg =0;
        float zvel_avg=0;
        int neighborBoids = 0;

        foreach (GameObject OtherBoid in boidPool)
        {
            if (Vector3.Distance(transform.position, OtherBoid.transform.position) < maxSeeDistance)
            {
                xvel_avg += OtherBoid.GetComponent<BoidBehaviourScript>().vx;
                yvel_avg += OtherBoid.GetComponent<BoidBehaviourScript>().vy;
                zvel_avg += OtherBoid.GetComponent<BoidBehaviourScript>().vz;
                neighborBoids += 1;

                Debug.DrawLine(transform.position, OtherBoid.transform.position, Color.green);

            }
        }

        if(neighborBoids > 0)
        {
            xvel_avg /= neighborBoids;
            yvel_avg /= neighborBoids;
            zvel_avg /= neighborBoids;
        }

        alignVector = new Vector3(xvel_avg, yvel_avg, zvel_avg);

        velocity += (alignVector - velocity) * alignFactor;
    }

    void Cohesion()
    {
        float xpos_avg = 0;
        float ypos_avg = 0;
        float zpos_avg = 0;

        int neighborBoids = 0;

        foreach (GameObject OtherBoid in boidPool)
        {
            if (Vector3.Distance(transform.position, OtherBoid.transform.position) < maxSeeDistance)
            {
                xpos_avg += OtherBoid.transform.position.x;
                ypos_avg += OtherBoid.transform.position.y;
                zpos_avg += OtherBoid.transform.position.z;

                neighborBoids += 1;
            }
        }

        xpos_avg /= neighborBoids;
        ypos_avg /= neighborBoids;
        zpos_avg /= neighborBoids;

        cohesionVector = new Vector3(xpos_avg, ypos_avg, zpos_avg);

        velocity += (cohesionVector - transform.position) * cohesionFactor;


    }
    void checkDistance()
    {
        if (Vector3.Distance(transform.position, origin) > sphereSize)
        {
            Vector3 flipVector = transform.position - origin;

            transform.position = -flipVector;
        }
    }

    void Turn()
    {
        if (transform.position.x < -sphereSize) {
            velocity.x = velocity.x + turnFactor;
        }
        if (transform.position.x > sphereSize)
        {
            velocity.x = velocity.x - turnFactor;
        }
        if (transform.position.y < -sphereSize)
        {
            velocity.y = velocity.y + turnFactor;
        }
        if (transform.position.y > sphereSize)
        {
            velocity.y = velocity.y - turnFactor;
        }
        if (transform.position.z < -sphereSize)
        {
            velocity.z = velocity.z + turnFactor;
        }
        if (transform.position.z > sphereSize)
        {
            velocity.z = velocity.z - turnFactor;
        }
    }
}
