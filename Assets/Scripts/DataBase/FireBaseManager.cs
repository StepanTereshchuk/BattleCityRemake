using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class FireBaseManager : MonoBehaviour
{
    public delegate void DataLoaded();
    public event DataLoaded Notify;
    public delegate void ErrorMessage(string message);
    public event ErrorMessage NotifyErrorMessage;
    public UserData userData { get; private set; }
    public LeaderBoardData leaderBoard { get; private set; }
    //public Texture2D userIconTexture { get; private set; }

    private FirebaseAuth _auth;
    private FirebaseUser _user;
    private FirebaseStorage _storage;
    private StorageReference _storageReference;
    private DatabaseReference _dbReference;
    private const string storageUrl = "gs://battlecity-a8cde.appspot.com";

    void Start()
    {
        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;
        _user = _auth.CurrentUser;
        _storage = FirebaseStorage.DefaultInstance;
        _storageReference = _storage.GetReferenceFromUrl(storageUrl);
    }

    public void UpdateUserName(string userName)
    {
        StartCoroutine(UpdateUserNameAsync(userName));
    }

    public void LoadUserData()
    {
        Debug.Log("LOAD USER DATA STARTED");
        _auth = FirebaseAuth.DefaultInstance;
        if (_auth != null && _auth.CurrentUser != null)
        {
            Debug.Log("_auth.CurrentUser =  "+ _auth.CurrentUser.DisplayName);
            StartCoroutine(LoadUserDataAsync());
            //StartCoroutine(GetUserIconAsync());
        }
        else
        {
            NotifyErrorMessage?.Invoke("SOMETHING NULL");
            Debug.Log("SOMETHING NULL");
        }
    }

    public void SaveNewUserData()
    {
        StartCoroutine(CreateNewUserDataAsync());
    }

    public void ChangeUserIcon(byte[] bytes)
    {
        StartCoroutine(ChangeUserIconAsync(bytes));
    }

    public void LoadLeaderBoardData()
    {
        StartCoroutine(LoadLeaderBoard());
    }

    public bool CheckUserInformation()
    {
            return userData != null;
    }

    private IEnumerator LoadLeaderBoard()
    {
        var DbTask = _dbReference.Child("users").OrderByChild("highScore").GetValueAsync();
        yield return new WaitUntil(() => DbTask.IsCompleted);
        if (!DbTask.Result.Exists)
        {
            Debug.Log("NO LEADER BOARD DATA");
        }
        else
        {
            DataSnapshot dataSnapshot = DbTask.Result;
            string json;
            leaderBoard = new LeaderBoardData();
            foreach (DataSnapshot data in dataSnapshot.Children)
            {
                json = data.GetRawJsonValue();
                leaderBoard.users.Add(JsonUtility.FromJson<UserData>(json));
            }
            yield return StartCoroutine(GetAllUsersIcons());
            Notify?.Invoke();
        }
    }

    private IEnumerator UpdateUserNameAsync(string userName)
    {
        userData.name = userName;
        string json = JsonUtility.ToJson(userData);
        var DbTask = _dbReference.Child("users").Child(_auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => DbTask.IsCompleted);

        if (DbTask.Exception != null)
        {
            Debug.Log(DbTask.Exception);
        }
    }

    // ON STACK FOUND THAT SOMETHING MIGHT BE WRONG WITH PACKAGES FOR MOBILE
    // NEED TO USE LOGCAT TO CATCH ERROR CODE
    private IEnumerator LoadUserDataAsync() // NEED TO SHOW ON UI WHAT IS GOING ON ON MOBILE DEVICE
    {
        Debug.Log("LoadUserDataAsync STARTED");
        //var reloadUser = _user.ReloadAsync();
        var reloadUser = _auth.CurrentUser.ReloadAsync();
        yield return new WaitUntil(() => reloadUser.IsCompleted);
        Debug.Log("ReloadAsync COMPLETED");

        var DbTask = _dbReference.Child("users").Child(_auth.CurrentUser.UserId).GetValueAsync().ContinueWithOnMainThread(DbTask =>
        {
            if (DbTask.IsFaulted)
            {
                Debug.Log("DB TASK FAULTED");
            }
            if (!DbTask.Result.Exists)
            {
                Debug.Log("SAVE NEW USER DATA STARTED");
                SaveNewUserData();
            }
            else
            {
                Debug.Log("GETTING USER DATA");
                DataSnapshot dataSnapshot = DbTask.Result;
                string json = dataSnapshot.GetRawJsonValue();
                userData = JsonUtility.FromJson<UserData>(json);
                Debug.Log("USER ID :  " + userData.userId);
                /*yield return */StartCoroutine(GetUserIconAsync());
            }
        });
       
        //yield return new WaitUntil(() => DbTask.IsCompleted);
        //Debug.Log("DB TASK IsCompleted");
      
    }

    private IEnumerator CreateNewUserDataAsync()
    {
        userData = new UserData(_auth.CurrentUser);
        string json = JsonUtility.ToJson(userData);
        var DbTask = _dbReference.Child("users").Child(_auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => DbTask.IsCompleted);

        if (DbTask.Exception != null)
        {
            Debug.Log(DbTask.Exception);
        }
    }

    private IEnumerator ChangeUserIconAsync(byte[] bytes)
    {
        StorageReference uploadReference = _storageReference.Child($"uploads/profileIcons/{_auth.CurrentUser.UserId}");

        MetadataChange newMetaData = new MetadataChange();
        newMetaData.ContentType = "image/jpeg";

        var DbTask = uploadReference.PutBytesAsync(bytes, newMetaData);
        yield return new WaitUntil(() => DbTask.IsCompleted);

        if (DbTask.IsFaulted || DbTask.IsCanceled)
        {
            Debug.Log(DbTask.Exception);
        }
    }

    private IEnumerator GetUserIconAsync()
    {
        Debug.Log("GET USER ICON STARTED");
        StorageReference imgReference = _storageReference.Child($"uploads/profileIcons/{_auth.CurrentUser.UserId}");
        var DbTask = imgReference.GetDownloadUrlAsync();
        yield return new WaitUntil(() => DbTask.IsCompleted);
        if (!DbTask.IsFaulted && !DbTask.IsCanceled)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(DbTask.Result.ToString());

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
            {
                userData.SetTextureNativeSize();
                userData.userIconTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }else
                userData.userIconTexture = null; // default icon 
        }
        else
        {
            Debug.Log("ERROR  "+ DbTask.Exception);
            userData.userIconTexture = null;
        }
        Notify?.Invoke();
    }

    private IEnumerator GetAllUsersIcons()
    {
        if(leaderBoard.users.Count > 0)
        {
            for(int i = 0; i < leaderBoard.users.Count; i++)
            {
                StorageReference imgReference = _storageReference.Child($"uploads/profileIcons/{leaderBoard.users[i].userId}");
                var DbTask = imgReference.GetDownloadUrlAsync();
                yield return new WaitUntil(() => DbTask.IsCompleted);
                if (!DbTask.IsFaulted && !DbTask.IsCanceled)
                {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(DbTask.Result.ToString());

                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
                    {
                        leaderBoard.users[i].SetTextureNativeSize();
                        leaderBoard.users[i].userIconTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    }
                    else
                        leaderBoard.users[i].userIconTexture = null; // default icon 
                }
                else
                {
                    Debug.Log("ERROR  " + DbTask.Exception);
                    leaderBoard.users[i].userIconTexture = null;
                }
            }
        }
    }

}