using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected Collider2D coll;
    [SerializeField] protected int point; // 怪物分數

    protected Vector2 nowPos, nextPos; // 位置
    protected GameObject Coin; // 金幣物件


    [Header("玩家")]
    protected GameObject PlayerObj; // 物件
    protected Vector2 playerPos; // 位置
    protected LayerMask PlayerMask; // 遮罩
    protected float playerHp; // 血量


    [Header("攻擊")]
    [SerializeField] protected float atkDelay; // 延遲

    //* 範圍
    [SerializeField] protected float detectDis; // 偵查半徑
    protected float atkDis; // 攻擊半徑


    [Header("布林值")]
    protected bool isWait;

    protected bool canTrace;
    protected bool isTrace;

    protected bool isBackToPlayer;
    protected bool hasTraceTarget;
    protected bool hasAtkTarget;


    //? ----------------------------------------------------------------


    //* EnemyAwake
    protected override void Awake()
    {
        base.Awake();
        hp = maxHp;
        
        PlayerObj = GameObject.FindWithTag("Player");
        PlayerMask = LayerMask.GetMask("Player");
        Coin = Resources.Load<GameObject>("Prefabs/Item/Coin");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    //* 取得玩家狀態
    protected void GetPlayerStatus()
    {
        playerHp = PlayerObj.GetComponent<Player>().hp;
        playerPos = PlayerObj.transform.position;
    }

    //* 檢測與玩家距離 vir
    protected virtual void CheckPlayerDis()
    {
    
    }

    //* 檢測玩家朝向
    protected void CheckPlayerFace()
    {
        isBackToPlayer = (playerPos.x > nowPos.x ^ faceR);
    }


    //* 狀態機 over
    protected override void SetStatus()
    {
        // Wait
        if (isWait)
            if ( isHurt || canTrace )
                isWait = false;

        // Move
        if ( hp > 0 && !isHurt && !isAttack )
            canMove = true;
        else
            canMove = false;

        if ( !isHurt && !isAttack && canMove )
            canTurn = true;
        else
            canTurn = false;

        // Trace
        if ( playerHp > 0 && !isHurt && !isAttack && hasTraceTarget )
            canTrace = true;
        else
            canTrace = false;

        if ( isHurt || isAttack || !hasTraceTarget )
            isTrace = false;

        // Attack
        if ( hp > 0 && playerHp > 0 && !isHurt && !isAttack && hasAtkTarget && !isBackToPlayer )
            canAttack = true;
        else
            canAttack = false;

        if ( isAttack && isHurt )
            isAttack = false;
}


    //% ========== 怪物移動 Move (vir) ==========
    protected virtual void Move()
    {

    }

    //* 執行移動 vir
    protected virtual void DoMove(Vector2 start, Vector2 target)
    {
    
    }


    //% ========== 怪物受傷 Hurt (over) ==========
    public override void Hurt(float damage, Vector2 hurtDir)
    {
        isHurt = true;
        HurtEffect(damage, hurtDir);
        SetAttackMode();

        // 死亡
        if (hp == 0)
        {
            anim.SetTrigger("Dead");
            gameObject.layer = 0; // (避免多重受傷)
            Score.AddScore(point); // 加分
            Instantiate(Coin, transform.position, Quaternion.identity); // 掉落金幣
        }

        // 中斷攻擊
        if (isAttack) StopCoroutine("Attack_base");
    }

    //* 消失
    protected void Die()
    {
        Destroy(gameObject);
    }


    //% ========== 怪物攻擊 Attack ==========

    //* 設定攻擊模式 vir
    protected virtual void SetAttackMode()
    {
        atkMode = (hp < maxHp/2)? 2 : 1; // 切換攻擊模式
        anim.SetFloat("atkMode", atkMode);
    }

    //* 追擊 vir
    protected virtual void Trace()
    {

    }

    //* 攻擊(共同)
    protected IEnumerator Attack_base()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.6f * atkDelay); // 攻擊延遲1

        if (!hasAtkTarget || isBackToPlayer) // 玩家不在攻擊範圍
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f * atkDelay); // 攻擊延遲2
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1.2f);
        }

        isAttack = false;
    }

    //* 攻擊(個別) vir
    protected virtual void Attack()
    {
        StartCoroutine("Attack_base");
    }
    
    //* 攻擊命中判定 vir
    protected virtual bool CheckAttack()
    {
        // 依血量切換攻擊模式
        return true;
    }

    //* 傳送傷害 over
    protected override void SendDamage()
    {
        bool isHit = CheckAttack(); // 是否擊中玩家

        // 攻擊成立
        if (isHit)
        {
            // 判斷相對位置
            bool isPlayerR = playerPos.x > nowPos.x;
            Vector2 atkDir = isPlayerR? Vector2.right : Vector2.left;

            PlayerObj.GetComponent<Player>().Hurt(ad, atkDir);
        }
    }

}
