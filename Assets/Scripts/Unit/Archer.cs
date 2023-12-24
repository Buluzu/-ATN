using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Archer : Unit
{
    public static Archer instance;
    public Transform shootPoint;
    public GameObject ammo;
    public LayerMask enemyLayer;
    public Transform enemyCheck;
    public float range;
    private bool isfacing = true;


    [SerializeField] private float CurrentShootCD;
    public float ShootCD;

    [SerializeField] private Animator anim;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = HP;
        StartCoroutine(GetRandomPos(5f));
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
            if (IsEnemyInRange())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                Shooting();

            }

        }


        if (isAlly == true) Flip();
        if (isAlly == false) FaceToAlly();
        if (isAlly == true) FaceToEnemy();


        if (currentHP <= 0)
        {
            IncomeGold();
            StartCoroutine(Dying(0f));
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
    public void Shooting()
    {
        if (CurrentShootCD <= 0)
        {
            anim.SetTrigger("isShootting");
            AudioManager.instance.PlaySFX("ShootArrow");
            Instantiate(ammo, shootPoint.position, transform.rotation);
            CurrentShootCD = ShootCD;
        }
        else { CurrentShootCD -= Time.deltaTime; }
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

    public IEnumerator Dying(float a)
    {


        //anim.SetBool("IsDead", true);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(a);
        Destroy(gameObject);

        //Debug.Log("bien mat");
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
            randomPos = Random.Range(-20, -15);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyCheck.position, range);
    }






}
