using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private string getScoreURL;
    [SerializeField] private string logoutURL;
    
    [SerializeField] private Dictionary<string, int> userList;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private Transform contentTransform;
    void Start()
    {
        if (userList == null)
        {
            userList = new Dictionary<string, int>();
        }
        else
        {
            userList.Clear();
        }

        StartCoroutine(GetScoreOnLeaderboard(getScoreURL));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator GetScoreOnLeaderboard(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string[] rows = www.downloadHandler.text.Split("\n");

            foreach (string s in rows)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string[] row = s.Split("\t");

                    if (row.Length > 0)
                    {
                        userList.Add(row[0], int.Parse(row[1]));
                    }
                }
                
                    
            }
            
            var sortedDict = from entry in userList orderby entry.Value ascending select entry;
            AddScorePanel();

            
        }
        else
        {
            Debug.Log("FAILED TO CONNECT");
        }
    }

    public void AddScorePanel()
    {
        var myList = userList.ToList();

        myList.Sort((pair1,pair2) => pair2.Value.CompareTo(pair1.Value));

        foreach (KeyValuePair<string, int> s in myList)
        {
            GameObject scorePanelClone = Instantiate(scorePanel, contentTransform);
            
            scorePanelClone.transform.GetChild(0).GetComponent<Text>().text = s.Key;
            scorePanelClone.transform.GetChild(2).GetComponent<Text>().text = s.Value.ToString();
        }

        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LogOutButton()
    {
        StartCoroutine(Logout(logoutURL));
    }
    
    IEnumerator Logout(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        Debug.Log(www.result == UnityWebRequest.Result.Success);

        if (www.result == UnityWebRequest.Result.Success)
        {
            SceneManager.LoadScene("WelcomeScene");
        }
    }
    
}
