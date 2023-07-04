using UnityEngine;
using UnityEngine.Tilemaps;

public class Spike : MonoBehaviour
{
    public float damage;
    public TilemapCollider2D coll;
    public Player PlayerScript;
    bool isUseColl = true;


    //% 玩家踩進地刺
    void OnTriggerStay2D(Collider2D other)
    {
        if (isUseColl)
        {
            if (other.CompareTag("Player") && other is CapsuleCollider2D)
            {
                PlayerScript.Hurt(damage);
                coll.enabled = false;
                isUseColl = false;
                Invoke("ResetCollider", 1.5f);
            }
        }
    }

    //% 重置碰撞體
    void ResetCollider()
    {
        isUseColl = true;
        coll.enabled = true;
    }
}
