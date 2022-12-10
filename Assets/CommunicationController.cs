using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CommunicationController : MonoBehaviour
{
    public string loginUrl, logoutUrl, registerUrl;
    private string username, password, userFeedback;
    public Canvas loginCanvas, logoutCanvas, errorCanvas;
    public InputField usernameText, passwordText;
    public Text errorText, welcomeText;
    public Button loginButton;
    public Button registerButton;

    private void Start()
    {
        loginButton.interactable = false;
    }

    private void Update()
    {
        loginButton.interactable = (usernameText.text.Length > 0 && passwordText.text.Length > 7);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == usernameText.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(passwordText.gameObject);
            }else if (EventSystem.current.currentSelectedGameObject == passwordText.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(loginButton.gameObject);
            }else if (EventSystem.current.currentSelectedGameObject == loginButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(registerButton.gameObject);
            }else if (EventSystem.current.currentSelectedGameObject == registerButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(usernameText.gameObject);
            }
        }
        
    }

    public void RegisterWeb()
    {
        Application.OpenURL(registerUrl);
    }

    public void LoginWeb()
    {
        StartCoroutine(Login(loginUrl));
    }

    public void LogoutWeb()
    {
        StartCoroutine(Logout(logoutUrl));
    }

    public void ShowError(string feedback)
    {
        loginCanvas.gameObject.SetActive(false);
        logoutCanvas.gameObject.SetActive(false);
        errorCanvas.gameObject.SetActive(true);

        feedback = userFeedback;

    }

    IEnumerator Logout(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        Debug.Log(www.result == UnityWebRequest.Result.Success);

        if (www.result == UnityWebRequest.Result.Success)
        {
            logoutCanvas.gameObject.SetActive(false);
            loginCanvas.gameObject.SetActive(true);
            errorCanvas.gameObject.SetActive(false);
        }
    }

    IEnumerator LoginRequest(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameText.text);
        form.AddField("password", passwordText.text);

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, form))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Established Connection");

                Debug.Log(uwr.downloadHandler.text);
            }
            else
            {
                Debug.Log("YOO WE DIDNt LOG IN BRUH " + uwr.error );
            }
            
            
        }
    }

    IEnumerator Login(string url)
    {
        username = usernameText.text;
        password = passwordText.text;

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("password", password));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Established Connection");

            Debug.Log(www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                switch (www.downloadHandler.text[0])
                {
                    case '0':
                        Debug.Log("DAMNN YOU REMEMBERED YOUR PASSWORD");
                        PlayerPrefs.SetString("Username", username);
                        PlayerPrefs.SetString("Date", DateTime.Now.ToString());
                        PlayerPrefs.Save();
                        //Maybe open game?
                        //For Testing - open the logout canvas
                        logoutCanvas.gameObject.SetActive(true);
                        loginCanvas.gameObject.SetActive(false);
                        errorCanvas.gameObject.SetActive(false);
                        break;
                    case '1':
                        Debug.Log("PASSWORD WRONG");
                        break;
                    case '2' :
                        Debug.Log("Username Wrong");
                        break;
                    case '3':
                        Debug.Log("Something went wrong");
                        break;
                    case '4':
                        Debug.Log("User already logged in lmao?");
                        PlayerPrefs.SetString("Username", username);
                        PlayerPrefs.SetString("Date", DateTime.Now.ToString());
                        PlayerPrefs.Save();
                        //Maybe open game?
                        //For Testing - open the logout canvas
                        logoutCanvas.gameObject.SetActive(true);
                        loginCanvas.gameObject.SetActive(false);
                        errorCanvas.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
}
