using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Phy = UnityEngine.Physics2D;

public class Player : Character
{
    [Header("基本參數&組件")]
    BoxCollider2D feet; // 觸地檢測器
    public PlayerInput PlayerInput;


    [Header("移動")]
    float inputX; //? 移動輸入
    public Vector2 inputDir; // 移動輸入向量


    [Header("跳躍")]
    float jumpForce = 7.5f;
    float wallJumpForce = 6f;

    //* 蹬牆跳
    public Transform wallPoint1, wallPoint2;


    [Header("攻擊")]
    Collider2D[] enemyList; // 怪物碰撞體
    LayerMask EnemyMask; // 怪物遮罩

    //* 中心點
    public Transform atkPoint1, atkPoint2, atkPoint3;

    //* 大小
    float atk1_Radius = 0.63f;
    Vector2 atk2_Box = new Vector2(2.3f, 0.9f);
    float atk3_Radius = 0.7f;


    [Header("受傷")]
    CinemachineImpulseSource ShakeSource; // 鏡頭晃動源


    [Header("翻滾")]
    float rollForce = 6f; // 翻滾力道


    [Header("布林值")]
    bool isTouchWall; // 是否觸牆

    bool canAttack3; // 大招解鎖

    bool canDbJump; // 二段跳解鎖
    bool canWallJump; // 蹬牆跳解鎖
    bool isJump; // 是否正在跳躍

    bool isWallSlide; // 是否正在滑牆

    bool canRoll; // 翻滾解鎖
    bool isRoll; // 是否正在翻滾

    bool canBlock; // 格擋解鎖
    bool isBlock; // 是否正在格擋


    //? ----------------------------------------------------------------


    //* Awake
    protected override void Awake()
    {
        base.Awake();

        // input
        PlayerInput = new PlayerInput();
        PlayerInput.Gameplay.Jump.started += Jump; // 跳躍輸入事件監聽

        // reference
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        EnemyMask = LayerMask.GetMask("Enemy");
        ShakeSource = GetComponent<CinemachineImpulseSource>();
        feet = transform.Find("Feet").GetComponent<BoxCollider2D>();
    }

    void OnEnable()
    {
        PlayerInput.Enable();    
    }

    void OnDisable()
    {
        PlayerInput.Disable();   
    }

    //* Start
    void Start()
    {
        hp = maxHp;
        HpBar.SetHpBoard(hp, maxHp);
    }


    //* Update
    void Update()
    {
        inputDir = PlayerInput.Gameplay.Move.ReadValue<Vector2>();
        
        if (hp > 0f)
        {
            inputX = Input.GetAxis("Horizontal");

            TouchCheck();
            SetStatus();
            SetAnime();

            if (canTurn) TurnFace();
            if (canAttack) Attack();
            if (canBlock) Block();
            if (canRoll) Roll();
        }
    }


    //* FixedUpdate
    void FixedUpdate()
    {
        Move();
    }


    //* 接觸檢測
    protected override void TouchCheck()
    {
        // 觸地檢測
        isTouchGround = feet.IsTouchingLayers(GroundMask);

        // 碰牆檢測
        isTouchWall =
        Phy.Raycast(wallPoint1.position, faceVec, 0.1f, GroundMask) &&
        Phy.Raycast(wallPoint2.position, faceVec, 0.1f, GroundMask);
    }


    //* 狀態機
    protected override void SetStatus()
    {
        //* Turn
        if ( (inputX != 0) && (faceR ^ inputX > 0f) ) canTurn = true;
        else canTurn = false;

        //* Jump
        if (isJump && isTouchGround) isJump = false;
        if (!isJump && isTouchGround) canWallJump = true; // 重置蹬牆跳條件

        //* WallSlide
        if (!isHurt && !isAttack && isTouchWall && rb.velocity.y < 0f) isWallSlide = true;
        else isWallSlide = false;

        //* Attack
        if (!isHurt && !isRoll && !isAttack) canAttack = true;
        else canAttack = false;

        if (isAttack && isHurt) isAttack = false;

        if (canAttack3 && isTouchGround) canAttack3 = false;

        //* Roll
        if (!isHurt && !isRoll && isTouchGround && !isAttack) canRoll = true;
        else canRoll = false;

        //* Block
        if (!isHurt && isTouchGround && !isRoll && !isAttack) canBlock = true;
        else canBlock = false;

        if (isHurt) isBlock = false;
    }


    //* 動畫狀態設置
    void SetAnime()
    {
        // 狀態設定
        anim.SetBool("isGrounded", isTouchGround);
        anim.SetFloat("velX", rb.velocity.x);
        anim.SetFloat("velY", rb.velocity.y);

        // Block
        if (isBlock) anim.SetBool("isBlock", true);
        else anim.SetBool("isBlock", false);

        // WallSlide
        if (isWallSlide) anim.SetBool("isWallSlide", true);
        else anim.SetBool("isWallSlide", false);
    }


    //% =================== 移動 Move ===================
    void Move()
    {
        if (isBlock) // 格擋滑步
        {
            if (speed >= 10) speed -= 10f;
        }
        else if (!isRoll && inputX != 0) // 一般移動
        {
            speed = isHurt ? 100f : 250f;
            float deltaX = inputX * speed * Time.fixedDeltaTime; // 位移量
            rb.velocity = new Vector2(deltaX, rb.velocity.y); // 更新速度
        }
    }


    //% =================== 跳躍 Jump ===================
    void Jump(InputAction.CallbackContext obj)
    {
        if (!isHurt)
        {
            //* 一段跳
            if (isTouchGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                isJump = true;
                canDbJump = true;
                anim.SetTrigger("Jump");
            }
            //* 蹬牆跳
            else if (canWallJump && isTouchWall) //?這裡isTouchWall可以刪除
            {
                rb.velocity = Vector2.up * wallJumpForce;

                isJump = true;
                canWallJump = false;
                anim.SetTrigger("Jump");
            }
            //* 二段跳
            else if (canDbJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                isJump = true;
                canDbJump = false;
                canAttack3 = true; // 解鎖大招
                anim.SetTrigger("Jump");
            }
        }
    }


    //% =================== 攻擊 Attack ===================
    void Attack()
    {
        if (Input.GetButtonDown("attack1"))
        {
            isAttack = true;
            atkMode = 1;
            anim.SetTrigger("Attack");
        }
        else if (Input.GetButtonDown("attack2"))
        {
            isAttack = true;
            atkMode = 2;
            anim.SetTrigger("Attack");
        }
        else if (Input.GetButtonDown("attack3") && canAttack3)
        {
            isAttack = true;
            atkMode = 3;
            canAttack3 = false;
            anim.SetTrigger("Attack");
        }

        anim.SetFloat("atkMode", atkMode);
    }

    //* 傳送傷害
    protected override void SendDamage()
    {
        // 取得敵人陣列
        switch (atkMode)
        {
            case 1:
                enemyList = Phy.OverlapCircleAll(atkPoint1.position, atk1_Radius, EnemyMask);
                ad = 30f;
                break;
            case 2:
                enemyList = Phy.OverlapBoxAll(atkPoint2.position, atk2_Box, 0, EnemyMask);
                ad = 20f;
                break;
            case 3:
                enemyList = Phy.OverlapCircleAll(atkPoint3.position, atk3_Radius, EnemyMask);
                ad = 60f;
                break;
        }

        // 遍歷陣列
        if (enemyList != null)
        {
            foreach (Collider2D coll in enemyList)
            {
                // 判斷相對位置
                bool isEnemyRight = coll.transform.position.x > transform.position.x;
                Vector2 atkDir = isEnemyRight? Vector2.right : Vector2.left;

                // 傳送傷害
                coll.gameObject.GetComponent<Enemy>().Hurt(ad, atkDir);
            }
        }

    }

    //* 結束攻擊
    void EndAttack()
    {
        isAttack = false;
    }


    //% =================== 受傷 Hurt ===================
    public override void Hurt(float damage, Vector2 hurtDir) //* 有擊退受傷
    {
        // 防禦成功
        if (isBlock && faceVec != hurtDir)
        {
            anim.SetTrigger("Block");
        }
        // 防禦失敗
        else if (!isRoll)
        {
            isHurt = true;
            HurtEffect(damage, hurtDir);
            HpBar.SetHpBoard(hp, maxHp); // 更新血條顯示
            ShakeSource.GenerateImpulse(); // 鏡頭晃動

            // 死亡
            if (hp == 0)
            {
                anim.SetTrigger("Dead");
                this.enabled = false; // 關閉程式
            }
        }
    }

    public override void Hurt(float damage) //* 無擊退受傷
    {
        isHurt = true;
        HurtEffect(damage);
        HpBar.SetHpBoard(hp, maxHp); // 更新血條顯示

        // 死亡
        if (hp == 0)
        {
            anim.SetTrigger("Dead");
            this.enabled = false; // 關閉程式
        }
    }


    //% =================== 格擋 Block ===================
    void Block()
    {
        if (Input.GetButton("Block")) isBlock = true;
        else isBlock = false;
    }


    //% =================== 翻滾 Roll ===================
    void Roll()
    {
        if (Input.GetButtonDown("Roll"))
        {
            isRoll = true;
            rb.velocity = faceVec * rollForce;
            anim.SetTrigger("Roll");
            Invoke("EndRoll", 0.8f);
        }
    }

    //* 結束翻滾
    void EndRoll()
    {
        isRoll = false;
    }


    //* Gizmos
    void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireSphere(transform.position, 1f);
        //Gizmos.DrawWireCube(atkPoint2.position, new Vector2(2.3f, 0.9f));
        //Gizmos.DrawRay(wallPoint.position, Vector2.down * 0.5f);
    }
}