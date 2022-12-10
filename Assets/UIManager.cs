using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Text scoreText;
    public Text finalScoreText;

    public GameObject GameOverCanvas;
    public GameObject PlayerHUDCanvas;

    private void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        if(scoreText)
            scoreText.text = GameManager.Instance.score.ToString();
    }
}
