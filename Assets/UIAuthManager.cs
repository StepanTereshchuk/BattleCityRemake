using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAuthManager : MonoBehaviour
{
    [SerializeField] private GameObject loginTab;
    [SerializeField] private GameObject registrationTab;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    // Debugging
    [Space]
    [Header("Debugging")]
    public TextMeshProUGUI statusTextLogin;
    public TextMeshProUGUI statusTextRegistration;
    private List<string> _messagesRegistraion = new List<string>();
    private List<string> _messagesSignIN = new List<string>();


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenRegistrationTab()
    {
        loginTab.SetActive(false);
        registrationTab.SetActive(true);
    }

    public void OpenLoginTab()
    {
        registrationTab.SetActive(false);
        loginTab.SetActive(true);
    }

    public void AddStatusTextRegistration(string newStatusText)
    {
        ManageMessagesCount(statusTextRegistration, newStatusText, _messagesRegistraion);
    }

    public void AddStatusTextLogIn(string newStatusText)
    {
        ManageMessagesCount(statusTextLogin, newStatusText, _messagesSignIN);
    }

    private void ManageMessagesCount(TextMeshProUGUI statusText, string newStatusText, List<string> messages)
    {
        if (messages.Count == 5)
        {
            messages.RemoveAt(0);
        }
        messages.Add(newStatusText);
        string txt = "";
        foreach (string s in messages)
        {
            txt += "\n" + s;
        }
        statusText.text = txt;
    }
}