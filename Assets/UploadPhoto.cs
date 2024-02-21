using Firebase.Extensions;
using Firebase.Storage;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UploadPhoto : MonoBehaviour
{
    [SerializeField] private RawImage _userIcon;
    private FirebaseStorage _storage;
    private StorageReference _storageReference;
    private const string storageUrl = "gs://battlecity-a8cde.appspot.com";

    private void Start()
    {
        _storage = FirebaseStorage.DefaultInstance;
        _storageReference = _storage.GetReferenceFromUrl(storageUrl);
    }

    public void OnbuttonClick()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);

            var newMetaData = new MetadataChange();
            newMetaData.ContentType = "image/jpeg";

            StorageReference uploadReference = _storageReference.Child("uploads/profileIcons/newFile.png");
            uploadReference.PutBytesAsync(bytes,newMetaData).ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception);
                }
               
            });
        }
    }
}