using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class HomeMenu : MonoBehaviour
{
    [SerializeField] private GameObject gamePadControlsUI;
    [SerializeField] private GameObject[] menuTabs;
    [SerializeField] private RectTransform startScreen;
    [SerializeField] private Image[] logoFirstPartImages;
    [SerializeField] private Image[] logoSecondPartImages;
    private EventSystem _currentEventSystem;
    private Sequence _sequenceFirst;
    public AudioManager audioManager { get; private set; }

    private void Awake()
    {
        _currentEventSystem = EventSystem.current;
        audioManager = GetComponentInChildren<AudioManager>();
    }
    private void Start()
    {
        AnimateMenu();
        ChangeDeviceInput();
    }
    private void ChangeDeviceInput()
    {
        //if (SystemInfo.deviceType == DeviceType.Handheld)
        //{
        //    gamePadControlsUI.SetActive(true);
        //}
        //else
        //{
        //    gamePadControlsUI.SetActive(false);
        //}
    }
    private void AnimateMenu()
    {
        _sequenceFirst = DOTween.Sequence();
        MoveStartScreen();
        if (_sequenceFirst != null)
            HighlitingLogoLetters();// need to wait
    }
    private void MoveStartScreen()
    {
        _sequenceFirst.Append(startScreen.DOAnchorMax(Vector2.one, 2f));
        _sequenceFirst.Join(startScreen.DOAnchorMin(Vector2.zero, 2f));
    }
    private void TextAppearing()
    {
        _sequenceFirst = DOTween.Sequence();
        //sequenceSecond = DOTween.Sequence();
        for (int i = 0; i < logoFirstPartImages.Length; i++)
        {
            _sequenceFirst.Append(logoFirstPartImages[i].DOFade(1f, 1f).From(0f));
            _sequenceFirst.Join(logoFirstPartImages[i].transform.DOScale(1f, 1f).From(0f));
            if (i < logoSecondPartImages.Length)
            {
                _sequenceFirst.Join(logoFirstPartImages[i].DOFade(1f, 1f).From(0f));
                _sequenceFirst.Join(logoSecondPartImages[i].transform.DOScale(1f, 1f).From(0f));
            }
        }
    }

    private void HighlitingLogoLetters()
    {
        for (int i = 0; i < logoFirstPartImages.Length; i++)
        {
            _sequenceFirst.Append(logoFirstPartImages[i].transform.DOScale(1.25f, 0.25f));
            _sequenceFirst.Join(logoFirstPartImages[i].DOColor(Color.red, 0.25f));
            _sequenceFirst.Append(logoFirstPartImages[i].transform.DOScale(1f, 0.25f));
            _sequenceFirst.Join(logoFirstPartImages[i].DOColor(Color.white, 0.25f));
        }
        for (int i = 0; i < logoSecondPartImages.Length; i++)
        {
            _sequenceFirst.Append(logoSecondPartImages[i].transform.DOScale(1.25f, 0.25f));
            _sequenceFirst.Join(logoSecondPartImages[i].DOColor(Color.red, 0.25f));
            _sequenceFirst.Append(logoSecondPartImages[i].transform.DOScale(1f, 0.25f));
            _sequenceFirst.Join(logoSecondPartImages[i].DOColor(Color.white, 0.25f));
        }
    }

    //public void FullScreenButton()
    //{
    //    Debug.Log("FullScreen Changed");
    //    Screen.fullScreen = !Screen.fullScreen;
    //}
    public void OpenTab(GameObject tabOpen)
    {
        Debug.Log("OPEN TAB STARTED");
        if (tabOpen != null)
        {
            _currentEventSystem.SetSelectedGameObject(null);
            foreach (var tab in menuTabs)
            {
                if (tab.activeSelf) tab.SetActive(!tab.activeSelf);
            }
            tabOpen.SetActive(true);
            Button btnFirst = tabOpen.GetComponentInChildren<Button>();
            _currentEventSystem.SetSelectedGameObject(btnFirst?.gameObject);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ChangeResolution()
    {
        Debug.Log("Resolution Changed");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}