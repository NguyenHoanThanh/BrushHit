using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    private static StarSpawner instance;
    public static StarSpawner Instance { get { return instance; } }

    [SerializeField] private GameObject starPrefab;
    public bool check;
    private void Awake()
    {
        instance = this;
    }
    public void SpawnStar(Vector3 pos1, Vector3 pos2)
    {
        GameObject goj=Instantiate(starPrefab, pos1, Quaternion.identity);
        goj.transform.GetComponent<PlusItem>().MoveWithPath(pos2);
    }
}
