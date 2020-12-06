using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{

    public int numberToSpawn;
    public GameObject gamebounds;
    public List <GameObject> spawnPool;


    // Start is called before the first frame update
    void Start()
    {
        spawnObjects();
    }

    public void spawnObjects()
    {
        destroyObjects();
        int randomItem = 0;
        GameObject toSpawn;
        MeshCollider c = gamebounds.GetComponent<MeshCollider>();

        float Xloc, Zloc;
        Vector2 pos;

        for (int i = 0; i < numberToSpawn; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];

            Xloc = Random.Range(c.bounds.min.x, c.bounds.max.x);
            Zloc = Random.Range(c.bounds.min.z, c.bounds.max.z);
            pos = new Vector3(Xloc, 2, Zloc);

            Instantiate(toSpawn, pos, toSpawn.transform.rotation);

        }
    }

    private void destroyObjects()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(o);
        }
    }
}
