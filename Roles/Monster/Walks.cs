using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class Walks : Enemy
{
    //* ===== 移動 =====
    protected Transform edgePoint; // 邊緣點
    protected Transform wallPoint; // 牆點

    //* 與玩家距離
    protected float disX, disY;


    //* ===== Bool =====
    protected bool isWall;
    protected bool isEdge;


    //? ----------------------------------------------------------------


    //* 取得檢測點
    protected void GetCheckPoint()
    {
        edgePoint = transform.Find("EdgePoint");
        wallPoint = transform.Find("WallPoint");
    }


    //* 狀態機 over
    protected override void SetStatus()
    {
        base.SetStatus();
        
        // Move
        if (!isTouchGround) canMove = false;
    }


    //* 行走類接觸檢測
    protected override void TouchCheck()
    {
        base.TouchCheck();

        // 觸地檢測
        isTouchGround = Phy.Raycast(transform.position + Vector3.down * 0.8f, Vector2.down, 0.03f, GroundMask);
        // 碰牆檢測
        isWall = Phy.Raycast(wallPoint.position, faceVec, 0.15f, GroundMask);
        // 邊緣檢測
        isEdge = ! Phy.Raycast(edgePoint.position, Vector2.down, 0.8f, GroundMask);
    }


    //* ========== 行走類移動 Move =========

    //* 檢測與玩家距離 over
    protected override void CheckPlayerDis()
    {
        // 與玩家相對距離
        disX = Mathf.Abs(nowPos.x - playerPos.x);
        disY = Mathf.Abs(nowPos.y - playerPos.y);

        hasTraceTarget = (disX <= detectDis) && (disY < 1f);
        hasAtkTarget = (disX <= atkDis) && (disY <= 0.8f);
    }

    //* 移動 over
    protected override void Move()
    {
        // 待機
        if (isWall || isEdge)
        {
            // 進入追擊狀態
            if (canTrace)
            {
                CancelInvoke("TurnFace");
                if (isBackToPlayer) TurnFace();
            }
            // 待機結束
            else
            {
                if (!isWait)
                {
                    isWait = true;
                    Invoke("TurnFace", 2f);
                }
            }
            anim.SetBool("isMove", false);
        }
        // 移動
        else
        {
            // 設置目標位置
            if (canTrace)
            {
                isTrace = true;
                nextPos = new Vector2(playerPos.x, nowPos.y); // 追擊移動
                if (isBackToPlayer) TurnFace(); // 跟隨轉向
            }
            // 待機移動
            else
            {
                nextPos = nowPos + faceVec * 1;
            }
            // 執行移動
            rb.MovePosition(Vector2.MoveTowards(nowPos, nextPos, speed * Time.fixedDeltaTime));
            anim.SetBool("isMove", true);
        }
    }


    //* 轉向
    protected override void TurnFace()
    {
        if (isWait) isWait = false;
        if (canTurn) base.TurnFace();
    }

}