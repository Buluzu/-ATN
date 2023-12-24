using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    public float HP;
    public float currentHP;
    public float HPRegen;
    public float damage;
    public float speed;
    public Rigidbody2D rb;
    public float dyingTime;
    public bool canTakingDamage = true;
    public float incomeGold;
    public bool canDropGold = false;

    //Enemy Flip
    public bool isEnemyFacingLeft = true;
    public bool isKnight = false;

    //DefenseMode
    public bool isAlly;
    public bool isDefense = false;
    public float randomPos;


    public void stat()
    {
        if (currentHP < 0) currentHP = 0;
        if (currentHP > HP) currentHP = HP;
    }

    public void TakeDame(float damage)
    {
        if (canTakingDamage) currentHP -= damage;
        if (canTakingDamage == false && isKnight == true) KnightController.instance.currentStamina -= (damage / 2);

    }

    public void TakeDameFormArcher(float damage)
    {
        AudioManager.instance.PlaySFX("ArrowHitted");
        if (canTakingDamage) currentHP -= damage;
        if (canTakingDamage == false && isKnight == true) KnightController.instance.currentStamina -= (damage / 2);
    }

    public void IncomeGold()
    {

        MainUI.instance.gold += incomeGold;
        MainUI.instance.killed++;
    }

    public IEnumerator LoadDeadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("DeadScene");
    }

    /*public void EnemyFlip()
    {
        if(isAlly==false)
        {
            if(Knight.instance.transform.position.x<transform.position.x&&isEnemyFacingLeft|| Knight.instance.transform.position.x > transform.position.x && !isEnemyFacingLeft)
            {

                Vector3 a = transform.localScale;
                isEnemyFacingLeft = !isEnemyFacingLeft;
                a.x *= -1f;
                transform.localScale = a;
            }
        }
    }*/

    /* public void IncomeGold()
     {
     }*/


}
