using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance = 0.5f;
    public LayerMask targetLayer;
    [SerializeField] private float damage;


    void Start()
    {
        Invoke("DestroyArrow", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
        RaycastHit2D target = Physics2D.Raycast(transform.position, transform.up, distance, targetLayer);
        if (target.collider != null)
        {
            if (target.collider.CompareTag("Player")|| target.collider.CompareTag("Unit"))
            {
                target.collider.GetComponent<Unit>().TakeDameFormArcher(damage);
            }
            DestroyArrow();
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
