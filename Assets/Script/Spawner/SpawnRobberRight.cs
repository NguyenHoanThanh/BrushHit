using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRobberRight : SpawnRobber
{
    public bool right;
    public float speed;
    private float max = 35;

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
        if (right)
        {
            transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);
        }
        if(transform.position.x > max || transform.position.x < 0)
        {
            right = !right;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Move") && !col)
        {
            collision.transform.GetComponent<SpawnRobberRight>().Toggle();
            col = true;
            StartCoroutine(SetFalse());
        }
    }
    public void Toggle()
    {
        right = !right;
    }
    IEnumerator SetFalse()
    {
        yield return new WaitForSeconds(0.2f);
        col = false;
    }
}
