using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRobberForward : SpawnRobber
{
    public bool forward;
    public float speed;
    public float max = 35;
    bool col;
    private void Start()
    {
        x = 7;
        z = 7;
        SpawnRubbers();
        GameManager.Instance.SetMaxIndex(x * z);
    }
    private void FixedUpdate()
    {
        if (forward)
        {
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);
        }
        if (transform.position.z > max || transform.position.z < 0)
        {
            forward = !forward;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Move") && !col)
        {
            collision.transform.GetComponent<SpawnRobberForward>().Toggle();
            col = true;
            StartCoroutine(SetFalse());
        }
    }
    public void Toggle()
    {
        forward = !forward;
    }
    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(0.2f);
        col = false;
    }
}
