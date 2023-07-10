using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public GameObject PlayerBody;


    //% 跳下平台
    private void OnTriggerStay2D(Collider2D other)
    {
        if ( other.CompareTag("JumpPlatform") && Input.GetButton("JumpDown") )
        {
            PlayerBody.layer = 10;
            Invoke("ResetLayer", 0.3f);
        }
    }

    //% 重置
    private void ResetLayer()
    {
        PlayerBody.layer = 8;
    }
}
