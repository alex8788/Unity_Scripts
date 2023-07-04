using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Text scoreText;
    public static int score = 0;


    void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    void Start()
    {
        score = 0;
        scoreText.text = "Score ：0";
    }


    public static void AddScore(int point)
    {
        score += point;
        scoreText.text = "Score ：" + score.ToString(); // 更新文字
    }
}
