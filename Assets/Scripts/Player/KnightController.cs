using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class KnightController : Unit
{
    public static KnightController instance;
    public float stamina;
    public float currentStamina;
    public float staminaRegen;
    public float staminaRate;
    public Animator anim;
    private float horizontal;
    [SerializeField] private bool canRun = true;
    private bool isFacingRight = true;
    [SerializeField] private bool canFlip = true;
    //private bool isShiedlBlocking=false;
    [SerializeField] private bool isAttacking = false;


    public float attackRange;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public LayerMask allyLayer;
    public float HPBuff;


    [SerializeField] private float CurrentAttackCD;
    public float AttackCD;

    [SerializeField] private float CurrentSpellCD;
    public float SpellCD;

    //StatUI
    public GameObject HPBar;
    public GameObject HPAmount;
    /*public GameObject MPBar;
    public GameObject MPAmount;*/
    public GameObject staminaBar;
    public GameObject staminaAmount;
    //public GameObject goldAmount;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = HP;
        currentStamina = stamina;
        //stamiaRegenStart = staminaRegen;
        InvokeRepeating("StatRegen", 1f, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        Knightstat();
        UpdateUI();
        Run();
        if (CurrentAttackCD <= 0)
        {
            if (isAttacking == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isAttacking = true;
                    canFlip = false;
                    canRun = false;
                    KnightAttack();
                    StartCoroutine(WaittingForAttack());
                }
                
            }

        }
        else
        {
            CurrentAttackCD -= Time.deltaTime;
        }


        if (currentStamina > 0)
        {
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Debug.Log("ShieldBlocking");
                    canTakingDamage = false;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    Debug.Log("can take dame");
                    canTakingDamage = true;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    ShieldBlock();
                    canFlip = false;
                }
                else
                {
                    canFlip = true;
                    //staminaRegen = staminaRegen/4 ;
                    anim.SetBool("IsShieldBlock", false);
                }
            }
        }
        if (currentStamina <= 0)
        {
            anim.SetBool("IsShieldBlock", false);
            //staminaRegen = stamiaRegenStart;
            canFlip = true;
            canTakingDamage = true;

        }

        if (CurrentSpellCD <= 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                CastSpell();
                CurrentSpellCD = SpellCD;
            }
        }
        else CurrentSpellCD -= Time.deltaTime;

        if (currentHP <= 0)
        {
            anim.SetTrigger("IsDead");
            this.enabled = false;
            StartCoroutine(LoadDeadScene());
        }

    }

    void Knightstat()
    {
        if (currentHP > HP) currentHP = HP;
        if (currentHP < 0) currentHP = 0;
        if (currentStamina > stamina) currentStamina = stamina;
        if (currentStamina < 0) currentStamina = 0;

    }
    void StatRegen()
    {
        currentHP += HPRegen;

        currentStamina += staminaRegen;
    }

    void Run()//di chuyển
    {
        if (canRun)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            anim.SetFloat("IdleRun", math.abs(horizontal));
            Flip();
        }
        else rb.velocity = new Vector2(horizontal * 0, rb.velocity.y);


    }
    private void KnightAttack()//tấn công
    {
        anim.SetTrigger("IsAttack");
        AudioManager.instance.PlaySFX("Melee1");
        Collider2D target = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
        if (target != null)
        {
            if (target.CompareTag("Unit"))
            {
                target.GetComponent<Unit>().TakeDame(damage);
                // Debug.Log("hit Enemy");
            }
        }
        CurrentAttackCD = AttackCD;
        //canRun = true;  
    }

    private IEnumerator WaittingForAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        canRun = true;
        canFlip = true;
    }

    void ShieldBlock()//chặn đòn
    {
        anim.SetBool("IsShieldBlock", true);

    }

    /*public override void TakeDame(float damage)
    {
        base.TakeDame(damage);
        if (canTakingDamage== false) currentStamina -= damage;
    }*/
    void CastSpell()//Dùng kĩ năng
    {
        anim.SetTrigger("IsCastSpell");
        Collider2D[] ally = Physics2D.OverlapCircleAll(transform.position, 20f, allyLayer);
        for (int i = 0; i < ally.Length; i++)
        {
            ally[i].GetComponent<Unit>().currentHP += HPBuff;
        }

    }

    private void Flip()//Điều khiển hướng mặt 
    {
        if (canFlip)
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                Vector3 a = transform.localScale;
                isFacingRight = !isFacingRight;
                a.x *= -1f;
                transform.localScale = a;
            }
        }
    }

    public void UpdateUI()//hàm hiển thị thanh máu, thể lực
    {
        HPBar.GetComponent<Image>().fillAmount = (int)currentHP / HP;
        HPAmount.GetComponent<TextMeshProUGUI>().text = currentHP.ToString();
        staminaBar.GetComponent<Image>().fillAmount = (int)currentStamina / stamina;
        staminaAmount.GetComponent<TextMeshProUGUI>().text = currentStamina.ToString();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
