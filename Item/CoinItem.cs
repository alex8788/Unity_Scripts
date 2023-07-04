using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public Rigidbody2D rb;
    public float force;


    private void Start()
    {
        rb.AddForce(force * Vector2.up, ForceMode2D.Impulse); //* 彈起效果
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            Destroy(gameObject);
            CoinUI.AddQuantity();
        }
    }
}
