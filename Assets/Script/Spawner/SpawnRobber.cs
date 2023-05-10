using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRobber : MonoBehaviour
{
    public GameObject rubberPrefaps;
    public int x;
    public int z;
    private void Start()
    {
        x = 10;
        z = 10;
        SpawnRubbers();
        GameManager.Instance.SetMaxIndex(x * z);
    }
    public virtual void SpawnRubbers()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                GameObject goj = Instantiate(rubberPrefaps, new Vector3(transform.position.x + i, 0,transform.position.z + j), Quaternion.identity);
                goj.transform.SetParent(transform);
            }
        }
    }

}
