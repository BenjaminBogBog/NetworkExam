using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
                        if (contentTransform.transform.childCount <= 7)
                        {
                            GameObject scorePanelClone = Instantiate(scorePanel, contentTransform);
                            userList.Add(row[0], int.Parse(row[1]));

                            scorePanelClone.transform.GetChild(0).GetComponent<Text>().text = row[0];
                            scorePanelClone.transform.GetChild(2).GetComponent<Text>().text = row[1];
                        }
                    }
                }
                
                    
            }

            
        }
        else
        {
            Debug.Log("FAILED TO CONNECT");
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
