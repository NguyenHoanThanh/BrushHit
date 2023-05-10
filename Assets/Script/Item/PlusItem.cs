using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusItem : MonoBehaviour
{
    private float rotSpeed = 100;
    private float speed = 10;
    private bool isMove;
    private Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.fixedDeltaTime);
        if (isMove)
        {
            MoveUp();
        }
    }
    private void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.EarnStarSound();
            GameManager.Instance.GetPlus();
            col.enabled = false;
            Destroy(gameObject, 1);
            isMove = true;
        }
    }
    public void MoveWithPath(Vector3 pos2)
    {
        StartCoroutine(Move(pos2));
    }
    IEnumerator Move(Vector3 pos2)
    {
        while(Vector3.Distance(transform.position, pos2) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2, speed * Time.fixedDeltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(1);
        col.enabled = true;
    }
}
