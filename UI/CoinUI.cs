using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public static int quantity = 0;
    public static Text coinText;


    void Awake()
    {
        coinText = GetComponent<Text>();
    }

    void Start()
    {
        quantity = 0;
        coinText.text = "0";
    }


    public static void AddQuantity()
    {
        quantity += 1;
        coinText.text = quantity.ToString();
    }
}
