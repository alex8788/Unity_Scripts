using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    static Image hpBar;
    static Text hpText;


    void Awake()
    {
        hpBar = transform.Find("Hp_bar").GetComponent<Image>();
        hpText = transform.Find("Hp_txt").GetComponent<Text>();
    }


    //* 更改顯示血量
    public static void SetHpBoard(float nowHp, float maxHp)
    {
        hpBar.fillAmount = nowHp / maxHp;
        hpText.text = nowHp.ToString() + " / " + maxHp.ToString();
    }
}
