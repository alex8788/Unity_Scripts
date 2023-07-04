using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class Globlin : Walks
{
    //* ===== 攻擊 =====

    //* 中心
    Transform atkPoint1;
    Transform atkPoint2;

    //* 範圍
    Vector2 atk1_Box = new Vector2(2.7f, 1.3f);
    float atk2_Radius = 0.6f;


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
                ad = 20f;
                atkDis = 1;
                break;
            case 2:
                atkDis = 2;
                break;
        }
    }

    //* 攻擊命中判定
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


    //* ===== 圖像輔助工具 =====
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(nowPos, faceVec * atkDis);
        //Gizmos.DrawWireSphere(nowPos, 0.3f);
        //Gizmos.DrawWireCube(atkPoint1.position, atk1_Box);
    }
}