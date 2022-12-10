using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

    [SerializeField] private float speed;
    [SerializeField] private float minPipeHeight;
    [SerializeField] private float maxPipeHeight;
    [SerializeField] private GameObject Scroller;
    [HideInInspector] public GameObject player;
    [SerializeField] private Transform pipeDump;
    [SerializeField] private GameObject pipePrefab;

    [SerializeField] private string postURL;

    private bool GameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        score = 0;
        GameOver = false;
        
        UIManager.Instance.GameOverCanvas.SetActive(false);
        
        Invoke("StartGame", 1.5f);
    }

    private void Update()
    {
        //Scroll
        Scroller.transform.position -= Vector3.right * speed * Time.deltaTime;

        if (GameOver)
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
    }

    void StartGame()
    {
        SpawnNewPipe();
    }

    public void SpawnNewPipe()
    {
        GameObject pipeClone = Instantiate(pipePrefab, new Vector3(player.transform.position.x + 12f, Random.Range(minPipeHeight, maxPipeHeight), 0), Quaternion.identity);
        
        pipeClone.transform.SetParent(pipeDump);
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

        StartCoroutine(PostScoreOnLeaderbaord(postURL));
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
            Debug.Log("POSTED ENTRY");
        }
        else
        {
            Debug.Log("FAILED TO CONNECT");
        }
    }
}
