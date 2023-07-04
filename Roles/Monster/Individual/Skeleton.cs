using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class Skeleton : Walks
{
    //* ===== 攻擊 =====

    //* 中心
    Transform atkPoint1;
    Transform atkPoint2;

    //* 範圍
    Vector2 atk1_Box = new Vector2(2.3f, 1.5f);
    float atk2_Radius= 1.5f;


    //* ===== Bool =====
    bool isBlock; // 是否格擋中


    //? ----------------------------------------------------------------


    //* Awake
    protected override void Awake()
    {
        base.Awake();

        GetCheckPoint();
        atkPoint1 = transform.Find("AtkPoint1");
        atkPoint2 = transform.Find("AtkPoint2");
    }


    //* Start
    void Start()
    {
        hp = maxHp;
        SetAttackMode();
    }


    //* 狀態機
    protected override void SetStatus()
    {
        base.SetStatus();

        if (isBlock && isHurt) isBlock = false;
    }

    //* Update
    void Update()
    {
        nowPos = transform.position;
        TouchCheck();
        GetPlayerStatus();

        CheckPlayerDis();
        CheckPlayerFace();
        SetStatus();

        if (canAttack) Attack();
    }


    //* FixedUpdate
    void FixedUpdate()
    {
        if (canMove) Move();
        else anim.SetBool("isMove", false);
    }


    //* ========== 攻擊 Attack =========

    //* 設定攻擊模式 over
    protected override void SetAttackMode()
    {
        base.SetAttackMode();

        switch (atkMode)
        {
            case 1:
                ad = 60f;
                atkDis = 1.8f;
                break;
            case 2:
                ad = 100f;
                atkDis = 1.8f;
                atkDelay = 0;
                break;
        }
    }

    //* 攻擊命中判定 over
    protected override bool CheckAttack()
    {
        Collider2D playerColl = null; // 檢測玩家碰撞體

        switch (atkMode)
        {
            case 1:
                playerColl = Phy.OverlapBox(atkPoint1.position, atk1_Box, 0, PlayerMask);
                break;
            case 2:
                playerColl = Phy.OverlapCircle(atkPoint2.position, atk2_Radius, PlayerMask);
                break;
        }

        return (playerColl != null);
    }

    //* 攻擊 over
    protected override void Attack()
    {
        switch (atkMode)
        {
            case 1:
                base.Attack();
                break;
            case 2:
                StartCoroutine("AttackMode2");
                break;
        }
    }

    //* 攻擊模式2
    IEnumerator AttackMode2()
    {
        isAttack = true;
        var block_cor = StartCoroutine("Block");
        yield return block_cor; // 等待格擋完畢
        
        // 玩家還在攻擊範圍內
        if (hasAtkTarget)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1f);
        }
        isAttack = false;
    }

    //* 格擋
    IEnumerator Block()
    {
        isAttack = true;
        isBlock = true;
        anim.SetBool("isBlock", true);
        yield return new WaitForSeconds(1f);
        isBlock = false;
        anim.SetBool("isBlock", false);
    }


    //* ========== 受傷 Hurt (over) =========
    public override void Hurt(float damage, Vector2 hurtDir)
    {
        if (!isBlock)
        {
            base.Hurt(damage, hurtDir);
            if (atkMode == 2) StopCoroutine("AttackMode2");
        }
    }


    //* ===== 圖像輔助工具 =====
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(nowPos, faceVec * atkDis);
        // Gizmos.DrawWireSphere(nowPos, 1f);
        // Gizmos.DrawWireCube();
    }

}