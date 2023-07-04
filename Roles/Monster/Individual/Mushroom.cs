using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class Mushroom : Walks
{
    //* ===== 攻擊 =====
    Transform atkPoint; // 中心
    float atkRadius = 0.5f; // 範圍


    //? ----------------------------------------------------------------


    //* Awake
    protected override void Awake()
    {
        base.Awake();

        GetCheckPoint();
        atkPoint = transform.Find("AtkPoint");
    }


    //* Start
    void Start()
    {
        hp = maxHp;
        SetAttackMode();
    }


    //* Update
    void Update()
    {
        nowPos = transform.position;
        TouchCheck();
        GetPlayerStatus();

        SetStatus();
        CheckPlayerDis();
        CheckPlayerFace();

        if (canAttack) Attack();
    }


    //* FixedUpdate
    void FixedUpdate()
    {
        if (canMove) Move();
        else anim.SetBool("isMove", false);
    }


    //* =================== 攻擊 Attack ===================

    //* 設定攻擊模式
    protected override void SetAttackMode()
    {
        base.SetAttackMode();

        switch (atkMode)
        {
            case 1:
                ad = 30f;
                atkDis = 1;
                break;
            case 2:
                ad = 60f;
                anim.ResetTrigger("Attack");
                break;
        }
    }

    //* 攻擊命中判定
    protected override bool CheckAttack()
    {
        Collider2D playerColl = null; // 檢測玩家碰撞體
        playerColl = Phy.OverlapCircle(atkPoint.position, atkRadius, PlayerMask);
        return (playerColl != null);
    }


    //* ===== 圖像輔助工具 =====
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(nowPos, faceVec * atkDis);
        // Gizmos.DrawWireSphere(transform.position, atkRadius);
    }

}