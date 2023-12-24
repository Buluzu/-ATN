using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPrefab : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance = 0.5f;
    public LayerMask targetLayer;
    [SerializeField] private float damage;

    public float attackRange;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    void Start()
    {
        Invoke("DestroyArrow", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //damage = Tower.instance.damage;


        transform.Translate(new Vector3(1,0,0) *speed*Time.deltaTime);

        RaycastHit2D target = Physics2D.Raycast(transform.position, transform.up, distance,targetLayer);
        if(target.collider != null)
        {  
          if(target.collider.CompareTag("Unit"))
            {
                target.collider.GetComponent<Unit>().TakeDameFormArcher(damage);
                //Debug.Log("hit enemy");
            }
            DestroyArrow();
        }
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

   /* void move()
    {
        Collider2D target = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
        if (target != null)
        {
            if (target.CompareTag("Unit"))
            {
                
            }
        }
    }*/
}
