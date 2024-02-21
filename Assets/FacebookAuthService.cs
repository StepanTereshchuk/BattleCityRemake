using Facebook.Unity;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;

public class FacebookAuthService
{
    public delegate void CredentialsHandler(Credential credential);
    public event CredentialsHandler LoginWithCredentials;
    private FirebaseAuth _auth;
    public FacebookAuthService()
    {
        _auth = FirebaseAuth.DefaultInstance;

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void LoginWithFacebook()
    {
        var permissons = new List<string>()
        {
            "gaming_profile","gaming_user_picture"
        };

        FB.LogInWithReadPermissions(permissons, AuthStatusCallback);
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
        else
        {
            Debug.Log("Something went wrong to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameScreenShown)
    {
        if (!isGameScreenShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void AuthStatusCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            AccessToken accessToken = AccessToken.CurrentAccessToken;
            // current access token's User ID : aToken.UserId
            LoginViaFirebaseFacebook(accessToken);

        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void LoginViaFirebaseFacebook(AccessToken accessToken)
    {
        Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken.TokenString);

        _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
        });
    }
}