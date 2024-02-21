using Firebase.Auth;
using Firebase;
using System.Collections;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;

public class FireBaseAuthManager : MonoBehaviour
{

    [SerializeField] private UIAuthManager _uiAuthManager;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    private FireBaseManager _fireBaseManager;
    private GoogleAuthService _googleAuthService;
    private FacebookAuthService _facebookAuthService;
    [Inject] private DiContainer _container;
    [Inject]
    private void Construct(FireBaseManager fireBaseManager)
    {
        _fireBaseManager = fireBaseManager;
    }

    public void LogOut()
    {
        _auth.SignOut();
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(_uiAuthManager.emailLoginField.text, _uiAuthManager.passwordLoginField.text));
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(_uiAuthManager.nameRegisterField.text, _uiAuthManager.emailRegisterField.text, _uiAuthManager.passwordRegisterField.text, _uiAuthManager.confirmPasswordRegisterField.text));
    }

    public void LoginWithGoogle()
    {
        Debug.Log("LoginWithGoogle");
        _googleAuthService.LoginWithCredentials += LoginWithCredentials;
        _googleAuthService.LoginWithGoogle();
    }

    public void LoginWithFacebook()
    {
        _facebookAuthService.LoginWithCredentials += LoginWithCredentials;
        _facebookAuthService.LoginWithFacebook();
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    void OnDestroy() // new
    {
        _auth.StateChanged -= AuthStateChanged;
        _auth = null;
    }

    private void LoginWithCredentials(Credential credential)
    {
        StartCoroutine(LoginWithCredentialsAsync(credential));
    }

    private void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void InitializeAuthServices()
    {
        _googleAuthService = new GoogleAuthService();
        _facebookAuthService = new FacebookAuthService();
    }

    private void AutoLogin()
    {
        if (_user != null)
        {
            //_fireBaseManager.RegisterUser(_auth);
            //_fireBaseManager.LoadUserData();
            _uiAuthManager.StartGame();
            //_uiAuthManager.ShowUserProfiler();
        }
        else
        {
            _uiAuthManager.OpenLoginTab();
        }
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        DependencyStatus _dependencyStatus = dependencyTask.Result;

        if (_dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());

            InitializeAuthServices();
        }
        else
        {
            _uiAuthManager.AddStatusTextLogIn("Could not resolve all firebase dependencies: " + _dependencyStatus);
        }
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (_user != null)
        {
            var reloadUser = _user.ReloadAsync();
            yield return new WaitUntil(() => reloadUser.IsCompleted);
            // check button save choice in database
            //AutoLogin();
        }
        else
        {
            _uiAuthManager.OpenLoginTab();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;

            if (!signedIn && _user != null)
            {
                _uiAuthManager.AddStatusTextLogIn("Signed out " + _user.DisplayName);
            }

            _user = _auth.CurrentUser;
            //_fireBaseManager.RegisterUser(_auth);
            if (signedIn)
            {
                _uiAuthManager.AddStatusTextLogIn("Signed in " + _user.DisplayName);
                // Start Game !!!
            }
        }
    }

    private string GetErrorMessage(int errorCode)
    {
        AuthError authError = (AuthError)errorCode;
        switch (authError)
        {
            case AuthError.InvalidEmail:
                return "Email is invalid";
            case AuthError.WrongPassword:
                return "Wrong Password";
            case AuthError.MissingEmail:
                return "Email is missing";
            case AuthError.MissingPassword:
                return "Password is missing";
            default:
                return "Something went wrong";
        }
    }

    private string ShowFirebaseAuthErrorMessage(Task<AuthResult> loginTask)
    {
        FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
        string errorMessage = "ERROR. ";
        errorMessage += GetErrorMessage(firebaseException.ErrorCode);
        return errorMessage;
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            _uiAuthManager.AddStatusTextLogIn(ShowFirebaseAuthErrorMessage(loginTask));
        }
        else
        {
            _user = loginTask.Result.User;
            _uiAuthManager.AddStatusTextLogIn("Wellcome, " + _user.DisplayName + "\nYou Are Successfully Logged In");
            Debug.Log("USER AUTH : " + _user.DisplayName);
            //_fireBaseManager.RegisterUser(_auth);
            _fireBaseManager.LoadUserData();
            _uiAuthManager.StartGame();
        }
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            _uiAuthManager.AddStatusTextRegistration("UserData Name is empty");
        }
        else if (email == "")
        {
            _uiAuthManager.AddStatusTextRegistration("Email field is empty");
        }
        else if (password != confirmPassword)
        {
            _uiAuthManager.AddStatusTextRegistration("Password does not match");
        }
        else
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                _uiAuthManager.AddStatusTextRegistration(ShowFirebaseAuthErrorMessage(registerTask));
            }
            else
            {
                _user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the userData if userData update failed
                    _user.DeleteAsync();
                    _uiAuthManager.AddStatusTextRegistration(ShowFirebaseAuthErrorMessage(registerTask));
                }
                else
                {
                    _uiAuthManager.AddStatusTextRegistration("Registration Sucessful Welcome " + _user.DisplayName);
                    //_fireBaseManager.RegisterUser(_auth);
                    _fireBaseManager.SaveNewUserData();
                    _uiAuthManager.OpenLoginTab();
                }
            }
        }
    }

    private IEnumerator LoginWithCredentialsAsync(Credential credential)
    {
        var loginTask = _auth.SignInAndRetrieveDataWithCredentialAsync(credential);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            _uiAuthManager.AddStatusTextLogIn(loginTask.Exception.Message);
            _uiAuthManager.AddStatusTextLogIn(ShowFirebaseAuthErrorMessage(loginTask));
        }
        else
        {
            _user = loginTask.Result.User;
            _uiAuthManager.AddStatusTextLogIn(loginTask.Result.User.Email);
            _uiAuthManager.AddStatusTextLogIn("\n" + loginTask.Result.User.DisplayName);
            _fireBaseManager.LoadUserData();
            _uiAuthManager.StartGame();
        }
    }
}