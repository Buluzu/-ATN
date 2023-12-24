using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Castle : Unit
{
    public Slider slider;
    public TextMeshProUGUI HPamount;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        stat();
        CastleHPBar();
        if (currentHP <= 0) StartCoroutine(LoadDeadScene());
    }

    void CastleHPBar()
    {
        slider.maxValue = HP;
        slider.value = currentHP;
        HPamount.text = currentHP.ToString();

    }


}
