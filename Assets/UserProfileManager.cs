using Firebase.Auth;
using SimpleFileBrowser;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UserProfileManager : MonoBehaviour
{
    [Space]
    [Header("UserData Info")]
    [SerializeField] private TextMeshProUGUI _errorMessage;

    [SerializeField] private TMP_InputField _userName;
    [SerializeField] private TextMeshProUGUI _email;
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private TextMeshProUGUI _maxStage;
    [SerializeField] private TextMeshProUGUI _tankDestroyed;
    [SerializeField] private Button _btnSave;
    [SerializeField] private Button _btnEdit;
    [SerializeField] private Button _btnUploadIcon;
    [SerializeField] private RawImage _userIcon;
    [SerializeField] private VerticalLayoutGroup[] verticalGroups;
    [SerializeField] private GameObject _loadingIconObject;
    [SerializeField] private EventSystem _currentEventSystem;
    private Image _userNameFieldImage;
    private UserData userData;
    private Navigation customNav;
    private FireBaseManager _fireBaseManager;
    [Inject]
    private void Construct(FireBaseManager fireBaseManager)
    {
        _fireBaseManager = fireBaseManager;
    }

    public void EditProfile()
    {
        _currentEventSystem.SetSelectedGameObject(null);
        _currentEventSystem.SetSelectedGameObject(_btnSave?.gameObject);
        verticalGroups[1].gameObject.SetActive(false);
        customNav.mode = Navigation.Mode.Automatic;
        _userName.navigation = customNav;
        _btnUploadIcon.gameObject.SetActive(true);
        _btnSave.gameObject.SetActive(true);
        _btnEdit.gameObject.SetActive(false);
        _userName.readOnly = false;
        _userNameFieldImage.color = Color.white;
        _userName.textComponent.color = Color.black;
    }

    public void SaveProfile()
    {
        _currentEventSystem.SetSelectedGameObject(null);
        _currentEventSystem.SetSelectedGameObject(_btnEdit?.gameObject);
        verticalGroups[1].gameObject.SetActive(true);

        customNav.mode = Navigation.Mode.None;
        _userName.navigation = customNav;
        _btnUploadIcon.gameObject.SetActive(false);
        _btnSave.gameObject.SetActive(false);
        _btnEdit.gameObject.SetActive(true);
        _userName.readOnly = true;
        _userNameFieldImage.color = Color.black;
        _userName.textComponent.color = Color.white;
        UpdateUserName();
    }

    public void ShowLoadPhotoDialog()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void LogOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        SceneManager.LoadScene(0);
    }

    private void ShowLoadingIcon()
    {
        _errorMessage.text = " -> LOADING STARTED ";
        _userIcon.gameObject.SetActive(false);
        _loadingIconObject.SetActive(true);
        for (int i = 0; i < verticalGroups.Length; i++)
        {
            verticalGroups[i].gameObject.SetActive(false);
        }
    }

    private void HideLoadingIcon()
    {
        _errorMessage.text += " -> HIDE LOADING ICON ";
        _loadingIconObject.SetActive(false);
        _userIcon.gameObject.SetActive(true);

        for (int i = 0; i < verticalGroups.Length; i++)
        {
            verticalGroups[i].gameObject.SetActive(true);
        }
       // _currentEventSystem.SetSelectedGameObject(null);
        _currentEventSystem.SetSelectedGameObject(_btnEdit?.gameObject);
    }

    private void UpdateUserName()
    {
        if (userData != null)
        {
            userData.name = _userName.text;
            _fireBaseManager.UpdateUserName(_userName.text);
        }
    }
    private void Awake()
    {
        _currentEventSystem = EventSystem.current.GetComponent<EventSystem>();
    }
    private void Start()
    {
        customNav = new Navigation();
    
        _userNameFieldImage = _userName.GetComponent<Image>();
        _userName.DeactivateInputField();
        //if (_fireBaseManager)
        //{
        //    _fireBaseManager.Notify += SetProfileData;
        //    _fireBaseManager.NotifyErrorMessage += ShowErrorMessage;
        //}
    }
    private void ShowErrorMessage(string message)
    {
        _errorMessage.text += message;
    }

    private void OnEnable()
    {
        Debug.Log("PROFILE ON ENABLE");
        if (_fireBaseManager.CheckUserInformation())
        {
            Debug.Log("USER DATA EXISTS");
            SetProfileData();
        }
        else
        {
            ShowLoadingIcon();
            Debug.Log("NO USER DATA FOUND");
        }
        //_fireBaseManager.LoadUserData();
    }
    private void OnDestroy()
    {
        _fireBaseManager.Notify -= SetProfileData;
    }

    private void SetProfileData()
    {
        _errorMessage.text += " -> Set Profile Data STARTED";
        Debug.Log("SET PROFILE DATA");
        HideLoadingIcon();
        userData = _fireBaseManager.userData;
        _userName.text = userData.name;
        _email.text = userData.email;
        _highScore.text = userData.highScore.ToString();
        _maxStage.text = userData.maxStage.ToString();
        _tankDestroyed.text = userData.tanksDestroyed.ToString();
        if (userData.userIconTexture && _userIcon)
            _userIcon.texture = userData.userIconTexture;
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            //Debug.Log(FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            _fireBaseManager.ChangeUserIcon(bytes);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            _userIcon.texture = texture;
        }
    }
}