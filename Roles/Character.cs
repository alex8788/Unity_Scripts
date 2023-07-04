using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody2D rb; // 鋼體
    protected Animator anim; // 動畫控制器
    protected LayerMask GroundMask; // 地板遮罩


    //* ===== 角色訊息 =====
    public float hp; // 血量
    [SerializeField] protected float maxHp; // 最大血量
    protected float ad; // 攻擊力


    //* ===== 移動 =====
    [SerializeField] protected float speed; // 移動速度
    [SerializeField] protected bool faceR; // 面朝向
    protected Vector2 faceVec = Vector2.right; // 方向向量


    //* ===== 受傷 =====
    protected float damage; // 傷害
    protected Vector2 hurtDir; // 受擊方向
    float recoilForce = 10f; // 擊退力道
    protected GameObject BloodEffect; // 血滴粒子特效
    protected GameObject FloatDamage; // 浮動傷害


    //* ===== 攻擊 =====
    protected float atkMode = 1; // 攻擊模式


    //* ===== Bool =====
    protected bool isTouchGround;

    protected bool canMove;
    protected bool canTurn;

    protected bool isHurt;

    protected bool canAttack;
    protected bool isAttack;


    //? ----------------------------------------------------------------


    //* CharacterAwake
    protected virtual void Awake()
    {
        GroundMask = LayerMask.GetMask("Ground");
        BloodEffect = Resources.Load<GameObject>("Prefabs/Effect/BloodEffect");
        FloatDamage = Resources.Load<GameObject>("Prefabs/Effect/FloatDamage_Base");
    }


    //* 接觸檢測
    protected virtual void TouchCheck()
    {

    }


    //* 狀態機 vir
    protected virtual void SetStatus()
    {

    }


    //* ========== 角色移動 Move ==========

    //* 轉向 vir
    protected virtual void TurnFace()
    {
        faceR = !faceR;
        faceVec *= -1;
        transform.Rotate(0f, 180f, 0f);
    }


    //* ========== 角色受傷 Hurt ==========
    public virtual void Hurt(float damage, Vector2 hurtDir) //* 有擊退受傷
    {
    
    }

    public virtual void Hurt(float damage) //* 無擊退受傷
    {
    
    }

    //* 受傷結束 vir
    protected virtual void EndHurt()
    {
        isHurt = false;
    }

    //* 受傷效果
    protected void HurtEffect(float damage, Vector2 hurtDir) //* 有擊退
    {
        hp = (hp > damage)? hp - damage : 0; // 扣血
        anim.SetTrigger("Hurt");

        // 擊退
        rb.AddForce(hurtDir * recoilForce, ForceMode2D.Impulse);
        // 濺血特效
        Instantiate(BloodEffect, transform.position, transform.rotation);
        // 浮動傷害
        GameObject fd = Instantiate(FloatDamage, transform.position, Quaternion.identity);
        fd.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
    }

    protected void HurtEffect(float damage) //* 無擊退
    {
        hp = (hp > damage)? hp - damage : 0; // 扣血
        anim.SetTrigger("Hurt");

        // 濺血特效
        Instantiate(BloodEffect, transform.position, transform.rotation);
        // 浮動傷害
        GameObject fd = Instantiate(FloatDamage, transform.position, Quaternion.identity) as GameObject;
        fd.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
    }


    //* ========== 角色攻擊 Attack ==========

    //* 傳送傷害 vir
    protected virtual void SendDamage()
    {
    
    }

}
