using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;

public class GoogleAuthService
{
    public delegate void CredentialsHandler(Credential credential);
    public event CredentialsHandler LoginWithCredentials;
    private string _webClientId = "1002385674498-hdpdt7mf2bupdh969hakku0t4lhfh68o.apps.googleusercontent.com";
    private GoogleSignInConfiguration _configuration;

    public GoogleAuthService()
    {
        _configuration = new GoogleSignInConfiguration
        {
            WebClientId = _webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };
        //OnSignInSilently();
    }

    public void OnSignInSilently() // is it needed ?
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.DefaultInstance.SignInSilently()
              .ContinueWith(OnAuthenticationFinished);
    }

    public void LoginWithGoogle()
    {
        OnSignIn();
    }

    private void OnSignIn()
    {
        //AddStatusText("Calling Google SignIn");
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    //AddStatusText("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    //AddStatusText("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            //AddStatusText("Canceled");
        }
        else
        {
            SendCredentials(task.Result.IdToken);
            //AddStatusText("Welcome: " + task.Result.DisplayName + "!");
        }
    }
    private void SendCredentials(string IdToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(IdToken, null);
        LoginWithCredentials.Invoke(credential);
    }

    //private void SignInWithGoogleOnFirebase(string idToken)
    //{
    //    Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

    //    _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
    //    {

    //        AggregateException ex = task.Exception;
    //        if (ex != null)
    //        {
    //            //if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
    //            //    AddStatusText("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
    //        }
    //        else
    //        {
    //            //AddStatusText("Sign In Successful.");
    //            _user = task.Result;
    //            //if (_fireBaseManager == null)
    //            //{
    //            //    //AddStatusText("_fireBaseManager null");
    //            //}
    //            //else
    //            //{
    //            //    //_fireBaseManager.RegisterUser(_auth);
    //            //    //_fireBaseManager.SaveNewUserData();
    //            //}
    //        }
    //    });
    //}
}