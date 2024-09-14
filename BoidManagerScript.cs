using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int numBoids = 1000;
    [SerializeField]
    Vector3 range = new Vector3(250, 250, 250);

    public List<GameObject> boidPool;

    void Start()
    {
        boidPool = new List<GameObject>();
        for (int i = 0; i < numBoids; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            boidPool.Add(obj);
        }

        for (int i = 0; i < numBoids; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-range.x / 2, range.x / 2),
                Random.Range(-range.y / 2, range.y / 2),
                Random.Range(-range.z / 2, range.z / 2)
            );

            GameObject boid = GetPooledObject();
            if (boid != null)
            {
                boid.transform.position = position;
                boid.SetActive(true);
            }
        }
    }

    GameObject GetPooledObject()
    {
        for (int i = 0; i < boidPool.Count; i++)
        {
            if (!boidPool[i].activeInHierarchy)
            {
                return boidPool[i];
            }
        }
        return null;
    }
}
