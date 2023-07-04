using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float speed = 1;
    Vector2 nowPos;

    public Transform[] movePoint;
    int point_idx = 0;
    bool isWait;

    Transform PlayerTrans;
    Transform defaltPlayerParent; // 玩家預設父物件


    void Start()
    {
        PlayerTrans = GameObject.FindWithTag("Player").transform;
        defaltPlayerParent = PlayerTrans.parent;
    }


    void Update()
    {
        Move();
    }


    void Move()
    {
        if (!isWait)
        {
            nowPos = transform.position;

            if (Vector2.Distance(nowPos, movePoint[point_idx].position) < 0.01f) // 到達停駐點
            {
                StartCoroutine("Wait");
            }
            else
            {
                transform.position = Vector2.MoveTowards(nowPos, movePoint[point_idx].position, speed * Time.deltaTime);
            }
        }
    }


    IEnumerator Wait()
    {
        isWait = true;
        yield return new WaitForSeconds(3f);
        point_idx = (point_idx == 0)? 1 : 0;
        isWait = false;
    }


    //* 玩家踩上
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            PlayerTrans.parent = this.transform;
        }
    }

    //* 玩家離開
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            PlayerTrans.parent = defaltPlayerParent;
        }
    }

}
