using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class Tower : Unit
{
    // Start is called before the first frame update
    public Transform shootPoint;
    public GameObject ammo;
    public LayerMask enemyLayer;
    public Transform enemyCheck;
    public float range;
    [SerializeField] private float CurrentShootCD;
    public float ShootCD;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private float test;
    public static Tower instance;
    public SpriteRenderer normalSprite;
    public SpriteRenderer destroyedSprite;

    public Slider slider;
    public TextMeshProUGUI HPamount;

    public bool isCastleTower = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = HP;

    }

    // Update is called once per frame
    void Update()
    {
        stat();
        if (IsEnemyInRange())
        {
            getEnemyInfor();//Tìm kiếm kẻ địch trong tầm bắn
            //rb.velocity = new Vector2(0, rb.velocity.y);
            test = -Vector3.Angle(Vector3.right, enemyTransform.position - shootPoint.position);
            if (CurrentShootCD <= 0)
            {
                AudioManager.instance.PlaySFX("ShootArrow");
                //anim.SetTrigger("isShootting");
                GameObject bullet = Instantiate(ammo, shootPoint.position, transform.rotation);
                bullet.transform.rotation = Quaternion.Euler(0, 0, 
                    -Vector3.Angle(Vector3.right, enemyTransform.position - shootPoint.position));
                CurrentShootCD = ShootCD;
            }
            else { CurrentShootCD -= Time.deltaTime; }

        }
        if (currentHP <= 0 && isCastleTower)
        {
            normalSprite.sprite = destroyedSprite.sprite;
            Destroy(gameObject);
            //Invoke("IncomeGold", 0.5f);
        }
        //else rb.velocity = new Vector2(speed, rb.velocity.y);
        TowerHPBar();

    }
    private bool IsEnemyInRange()
    {
        return Physics2D.OverlapCircle(enemyCheck.position, range, enemyLayer);

    }

    public void getEnemyInfor()
    {
        Collider2D target = Physics2D.OverlapCircle(shootPoint.position, range, enemyLayer);
        if (target != null)
        {
            if (target.CompareTag("Unit"))
            {
                enemyTransform = target.transform;
            }
        }
    }


    void TowerHPBar()
    {
        if (!isCastleTower)
        {
            slider.maxValue = HP;
            slider.value = currentHP;
            HPamount.text = currentHP.ToString();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyCheck.position, range);
    }
}
