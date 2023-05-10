using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayItem : MonoBehaviour
{
    private float rotateSpeed = 200f;
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ResetScene(true);
        }
    }
}
