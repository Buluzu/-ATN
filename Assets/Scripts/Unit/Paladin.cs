using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Unit
{
    public Animator anim;
    public float attackRange;
    public float range;//tam nhin
    public Transform attackPoint;
    public Transform enemyCheck;
    public LayerMask enemyLayer;
    private bool isfacing = true;

    [SerializeField] private float CurrentAttackCD;
    public float AttackCD;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = HP;
        if (isAlly == true) StartCoroutine(GetRandomPos(5f));

    }

    // Update is called once per frame
    void Update()
    {
        stat();

        if (rb.velocity.x == 0)
        {
            anim.SetBool("isIdling", true);
        }
        else anim.SetBool("isIdling", false);
        if (isAlly == true && isDefense == true)
        {
            Defense();

        }
        else rb.velocity = new Vector2(speed, rb.velocity.y);
        if (IsEnemyNear())
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //Debug.Log("co ke dich o gan");
            if (IsEnemyInRange())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (CurrentAttackCD <= 0)
                {
                    if (isAlly == true) AudioManager.instance.PlaySFX("Melee");
                    if (isAlly == false) AudioManager.instance.PlaySFX("Melee1");
                    Attack();
                    CurrentAttackCD = AttackCD;
                }
                else { CurrentAttackCD -= Time.deltaTime; }
            }
        }

        /*else
        {
            anim.SetTrigger("IsRun");
            rb.velocity = new Vector2(speed, rb.velocity.y);

        }*/



        if (currentHP <= 0)
        {
            IncomeGold();
            StartCoroutine(Dying(1));
        }

        if (isAlly == true) Flip();
        if (isAlly == false) FaceToAlly();
        if (isAlly == true) FaceToEnemy();

    }


    void Attack()
    {
        anim.SetTrigger("IsAttack");
        Collider2D target = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
        if (target != null)
        {
            if (target.CompareTag("Unit") || target.CompareTag("Player"))
            {
                target.GetComponent<Unit>().TakeDame(damage);
                //Debug.Log("hit Enemy");
            }
        }
    }


    private bool IsEnemyInRange()
    {
        return Physics2D.OverlapCircle(enemyCheck.position, range, enemyLayer);
    }

    private bool IsEnemyNear()
    {
        return Physics2D.OverlapCircle(enemyCheck.position, range + 4f, enemyLayer);

    }
    public IEnumerator Dying(float a)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("IsDead", true);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(a);
        Destroy(gameObject);

        //Debug.Log("bien mat");
    }

    private void Flip()
    {
        if (isfacing && rb.velocity.x < 0 || !isfacing && rb.velocity.x > 0)
        {
            Vector3 localScale = transform.localScale;
            isfacing = !isfacing;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void FaceToEnemy()
    {
        if (IsEnemyInRange())
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
            if (transform.localScale.x < 0)
            {
                Vector3 a = transform.localScale;
                isfacing = !isfacing;
                a.x *= -1f;
                transform.localScale = a;
            }
        }
    }

    private void FaceToAlly()
    {
        if (IsEnemyInRange())
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
            if (target.transform.position.x > transform.position.x && isEnemyFacingLeft || target.transform.position.x < transform.position.x && !isEnemyFacingLeft)
            {
                Vector3 a = transform.localScale;
                isEnemyFacingLeft = !isEnemyFacingLeft;
                a.x *= -1f;
                transform.localScale = a;
            }

        }
    }

    public void Defense()
    {
        if (transform.position.x < randomPos) rb.velocity = new Vector2(speed, rb.velocity.y);
        if (Mathf.RoundToInt(transform.position.x) == randomPos)
        {
            Debug.Log("dung yen");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (transform.position.x > randomPos)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

    }

    public IEnumerator GetRandomPos(float time)
    {
        //randomPos = Random.Range(basePos.position.x, towerPos.position.x);
        while (true)
        {
            yield return new WaitForSeconds(time);
            randomPos = Random.Range(-13, -15);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(enemyCheck.position, range);
        Gizmos.DrawWireSphere(enemyCheck.position, range + 4);


    }
}
