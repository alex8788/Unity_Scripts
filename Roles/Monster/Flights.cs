using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phy = UnityEngine.Physics2D;

public class Flights : Enemy
{
    //* ===== 移動 =====
    
    protected Vector2 movePos_L, movePos_R; // 移動範圍


    //? ----------------------------------------------------------------


    //* 設置移動邊界
    protected void SetMoveBounder()
    {
        nowPos = transform.position;
        movePos_L = new Vector2(nowPos.x - 3f, nowPos.y - 2f);
        movePos_R = new Vector2(nowPos.x + 3f, nowPos.y + 2f);
    }

    //* 檢測玩家距離 over
    protected override void CheckPlayerDis()
    {
        hasTraceTarget = Vector2.Distance(nowPos, playerPos) < detectDis;
        hasAtkTarget = Vector2.Distance(nowPos, playerPos) < atkDis;
    }


    //* ========== 飛行類移動 Move ==========

    //* 更新目標位置
    protected void SetRandomNextPos()
    {
        isWait = false;
        nextPos = new Vector2(Random.Range(movePos_L.x, movePos_R.x), Random.Range(movePos_L.y, movePos_R.y));
    }

    //* 移動 over
    protected override void Move()
    {
        // 抵達目標位置
        if (Vector2.Equals(nowPos, nextPos))
        {
            if (isWait) // 待機中
            {
                if (canTrace) // 待機偵查到玩家
                {
                    nextPos = playerPos;
                    CancelInvoke("SetRandomNextPos");
                }
            }
            else // 進入待機狀態
            {
                isWait = true;
                Invoke("SetRandomNextPos", 2f);
            }
        }
        // 移動
        else
        {
            // 追擊判斷
            if (canTrace)
            {
                isTrace = true;
                nextPos = playerPos;
            }
            // 執行移動
            transform.position = Vector2.MoveTowards(nowPos, nextPos, speed * Time.deltaTime);
        }
    }


    //* 轉向
    protected override void TurnFace()
    {
        if (nowPos != nextPos && canTurn)
        {
            if ( faceR ^ (nextPos.x > nowPos.x) )
                base.TurnFace();
        }
    }


    //* ========== 受傷 Hurt (over) ==========
    public override void Hurt(float damage, Vector2 hurtDir) //! 死亡仍然會重複受傷 & 攻擊
    {
        base.Hurt(damage, hurtDir);
        if (isWait) CancelInvoke("SetRandomNextPos");
    }
}