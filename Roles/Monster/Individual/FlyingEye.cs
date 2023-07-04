using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class FlyingEye : Flights
{
    //* ===== 攻擊 =====
    // 中心點
    Transform atkPoint1, atkPoint2;

    // 大小
    float atk1_Radius = 0.4f, atk2_Radius = 0.7f;


    //? ----------------------------------------------------------------


    //* Awake
    protected override void Awake()
    {
        base.Awake();
        
        atkPoint1 = transform.GetChild(0);
        atkPoint2 = transform.GetChild(1);
    }


    //* Start
    void Start()
    {
        hp = maxHp;
        SetMoveBounder();
        SetAttackMode();
        SetRandomNextPos();
    }


    //* Update
    void Update()
    {
        nowPos = transform.position;
        GetPlayerStatus();
        CheckPlayerDis();
        CheckPlayerFace();
        SetStatus();
        
        if (canTurn) TurnFace();
        if (canMove) Move();
        if (canAttack) Attack();
    }


    //* ========== 攻擊 Attack ==========

    //* 設定攻擊模式
    protected override void SetAttackMode()
    {
        base.SetAttackMode();

        switch (atkMode)
        {
            case 1:
                ad = 10f;
                atkDis = 0.8f;
                break;
            case 2:
                atkDis = 1f;
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
                playerColl = Phy.OverlapCircle(atkPoint1.position, atk1_Radius, PlayerMask);
                break;
            case 2:
                playerColl = Phy.OverlapCircle(atkPoint2.position, atk2_Radius, PlayerMask);
                break;
        }

        return (playerColl != null);
    }

}
