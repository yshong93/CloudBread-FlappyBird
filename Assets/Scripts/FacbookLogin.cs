using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using AssemblyCSharp;
using System.Text;
using System;
using Assets.Scripts.CloudBread;

public class FacbookLogin : MonoBehaviour {

	private string ServerAddress = "https://cb2-auth-demo.azurewebsites.net/";
    private AzureAuthentication azureAuth;

    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void LoginwithPermissions()
    {
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            FB.API("me?fields=name", HttpMethod.GET, NameCallBack);
            
            // AuzreAuthentication 을 사용하여 인증 토큰 가져 오기
            azureAuth = new AzureAuthentication();
            azureAuth.Login(AzureAuthentication.AuthenticationProvider.Facebook, ServerAddress, aToken.TokenString, Login_success, Login_error);

        }
        else {
            Debug.Log("User cancelled login");
        }
    }

    private void NameCallBack(IGraphResult result)
    {
        string userName = (string)result.ResultDictionary["name"];

        PlayerPrefs.SetString("nickName", userName);
    }

    private void Login_success(string id, WWW www)
    {
        
        string resultJson = www.text;
        AuthData resultData = JsonParser.Read<AuthData>(resultJson);
        //AuthToken = resultData.authenticationToken;
        //UserID = resultData.user.userId;

        AzureMobileAppRequestHelper.AuthToken = resultData.authenticationToken;
        print(resultJson);

        PlayerPrefs.SetString("userId", resultData.user.userId);
        //PlayerPrefs.SetString("userId", )

		Assets.Scripts.CloudBread.CloudBread cb = new Assets.Scripts.CloudBread.CloudBread ();
//		CloudBread.CloudBread cb = new CloudBread.CloudBread();
        cb.CBInsRegMember(Callback_Success);

        
    }

    public void Callback_Success(string id , WWW www)
    {
        //print(JsonParser.WritePretty(obj));
        if (www.error != null) //새로 회원 가입
        {
            print("이미 가입된 회원");
            StartGame();
        }
        else //이미 가입된 회원
        {
			PlayerPrefs.SetInt("bestScore", 0);

            print("새로 가입한 회원");
            StartGame();
        }
    }

    public void Login_error(string id, WWW www)
    {
        print("[Error] : " + www.error);

    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void StartGame()
    {
        //PlayerPrefs.SetString("access_token", "0000");
        SceneManager.LoadScene("mainGame");
    }

    public void LoginButtonClicked()
    {
		if (!FB.IsLoggedIn)
			LoginwithPermissions ();
		else {
			AuthCallback (null);
		}
    }
}
