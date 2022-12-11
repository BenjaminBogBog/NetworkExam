using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

    [SerializeField] private float speed;
    [SerializeField] private float minPipeHeight;
    [SerializeField] private float maxPipeHeight;
    [SerializeField] private GameObject Scroller;
    public GameObject player;
    [SerializeField] private Transform pipeDump;
    [SerializeField] private GameObject pipePrefab;

    [SerializeField] private string postLeaderboardURL;
    [SerializeField] private string postBestTimeURL;

    private bool GameOver;
    private bool StartedGame;

    private bool canRestart;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        GameOver = false;
        
        UIManager.Instance.GameOverCanvas.SetActive(false);
        UIManager.Instance.StartGameCanvas.SetActive(true);
        
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        StartedGame = false;
        canRestart = false;
    }

    private void Update()
    {
        //Scroll
        Scroller.transform.position -= Vector3.right * speed * Time.deltaTime;

        if (GameOver && canRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Go back to main menu
                SceneManager.LoadScene("MainMenu");
            }
        }

        if (!StartedGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UIManager.Instance.StartGameCanvas.SetActive(false);
                player.GetComponent<Rigidbody2D>().isKinematic = false;
                Invoke("SpawnNewPipe", 1.5f);
                StartedGame = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Go back to main menu
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void SpawnNewPipe()
    {
        if (player)
        {
            GameObject pipeClone = Instantiate(pipePrefab, new Vector3(player.transform.position.x + 12f, Random.Range(minPipeHeight, maxPipeHeight), 0), Quaternion.identity);
        
            pipeClone.transform.SetParent(pipeDump);
        }
    }

    public void TriggerGameOver()
    {
        UIManager.Instance.GameOverCanvas.SetActive(true);
        UIManager.Instance.PlayerHUDCanvas.SetActive(false);
        
        if(UIManager.Instance.finalScoreText)
            UIManager.Instance.finalScoreText.text = score.ToString();
        
        GameOver = true;
        speed = 0;
        Destroy(player.gameObject);

        StartCoroutine(PostScoreOnLeaderbaord(postLeaderboardURL));
        StartCoroutine(PostBestTimes(postBestTimeURL));
        canRestart = false;
        Invoke("EnableRestart", 0.5f);
    }

    private void EnableRestart()
    {
        canRestart = true;
    }
    

    IEnumerator PostScoreOnLeaderbaord(string url)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", PlayerPrefs.GetString("Username")));
        formData.Add(new MultipartFormDataSection("score", score.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
            ScoreManager.Instance.RecalcLeaderboard();
        }
        else
        {
            Debug.Log("FAILED TO CONNECT");
        }
    }
    
    IEnumerator PostBestTimes(string url)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", PlayerPrefs.GetString("Username")));
        formData.Add(new MultipartFormDataSection("score", score.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("FAILED TO CONNECT");
        }
    }
}
