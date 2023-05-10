using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow instance;
    public static CameraFollow Instance { get { return instance; } }

    private float speed = 5f;
    private float cameraOffsetX = -5;
   
    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.StarGame())
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position + new Vector3(cameraOffsetX, 0,0), speed * Time.deltaTime);
    }
    public void MoveToZeroPoint()
    {
        transform.position = Vector3.zero;
    }
}
