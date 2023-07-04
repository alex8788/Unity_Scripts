using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public CompositeCollider2D coll;


    //% 跳下平台
    private void OnTriggerStay2D(Collider2D other)
    {
        if ( other.transform.parent.CompareTag("Player") && other is BoxCollider2D ) //? 不明錯誤
        {
            if ( Input.GetButton("JumpDown") )
            {
                coll.isTrigger = true;
            }
        }
    }

    //% 重置
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            coll.isTrigger = false;
        }
    }
}
