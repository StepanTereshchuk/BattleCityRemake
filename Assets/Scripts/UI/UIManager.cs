using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _topCurtain;
    [SerializeField] private Image _bottomCurtain;
    [SerializeField] private Image _blackCurtain;
    [SerializeField] private TextMeshProUGUI _stageNumberText;
    [SerializeField] private TextMeshProUGUI _playerLivesText;
    [SerializeField] private TextMeshProUGUI _stageNumber;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private Transform _tankReservePanel;
    [SerializeField] private RectTransform _canvasTranform;
    [SerializeField] private GameObject playerControls;

    private GameObject _tankImage;
    private LevelManager _levelManager;
    private SceneTracker _sceneTracker;
    private AudioManager _audioManager;
    private const float _waitBeforeStart = 5f;


    [Inject]
    private void Construct(LevelManager levelManager, SceneTracker sceneTracker, AudioManager audioManager)
    {
        _levelManager = levelManager;
        _sceneTracker = sceneTracker;
        _audioManager = audioManager;
    }

    public void UpdatePlayerLives()
    {
        _playerLivesText.text = _sceneTracker.playerLives.ToString();
    }

    public IEnumerator ShowGameOverText()
    {
        Vector3 _startPosition = _gameOverText.rectTransform.localPosition;
        Vector3 _desiredPosition = new Vector3(_startPosition.x, 0, _startPosition.z);
        float _desiredDuration = _audioManager.GetSoundLenght(SoundKey.Defeat);
        float _elapsedTimed = 0;
        while (_gameOverText.rectTransform.localPosition.y < _desiredPosition.y)
        {
            _elapsedTimed += Time.deltaTime;
            float completed = _elapsedTimed / _desiredDuration;
            _gameOverText.rectTransform.localPosition = Vector3.Lerp(_startPosition, _desiredPosition, completed);
            yield return new WaitForEndOfFrame();
        }
        yield return true;
        StopCoroutine("ShowGameOverText");
    }

    public void RemoveTankReserve()
    {
        int numberOfTanks = _levelManager.smallTanks + _levelManager.fastTanks + _levelManager.bigTanks + _levelManager.armoredTanks;
        _tankImage = _tankReservePanel.transform.GetChild(numberOfTanks).gameObject;
        _tankImage.SetActive(false);
    }

    private void Start()
    {
        ChangeDeviceInput();
        UpdateTankReserve();
        UpdatePlayerLives();
        UpdateStageNumber();
        StartCoroutine(StartStage());
    }

    private void Update()
    {
        UpdatePlayerLives();
        RemoveTankReserve();
    }

    private void ChangeDeviceInput()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            playerControls.SetActive(true);
        }
        else
        {
            playerControls.SetActive(false);
        }
    }

    private void UpdateTankReserve()
    {
        int numberOfTanks = _levelManager.smallTanks + _levelManager.fastTanks + _levelManager.bigTanks + _levelManager.armoredTanks;
        for (int j = 0; j < numberOfTanks; j++)
        {
            _tankImage = _tankReservePanel.transform.GetChild(j).gameObject;
            _tankImage.SetActive(true);
        }
    }

    private void UpdateStageNumber()
    {
        _stageNumberText.text = "STAGE " + _sceneTracker.stageNumber.ToString();
    }

    private IEnumerator RevealTopStage()
    {
        float waitTime = 10f;// replace with sound leght
        float elapsedTime = 0f;
        _stageNumberText.enabled = false;
        while (_topCurtain.rectTransform.anchorMin.y < _topCurtain.rectTransform.anchorMax.y)
        {
            _topCurtain.rectTransform.anchorMin = Vector2.Lerp(_topCurtain.rectTransform.anchorMin, new Vector2(_topCurtain.rectTransform.anchorMin.x, _topCurtain.rectTransform.anchorMax.y), elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RevealBottomStage()
    {
        float waitTime = 10f; // replace with sound leght
        float elapsedTime = 0f;

        while (_bottomCurtain.rectTransform.anchorMax.y > _bottomCurtain.rectTransform.anchorMin.y)
        {
            _bottomCurtain.rectTransform.anchorMax = Vector2.Lerp(_bottomCurtain.rectTransform.anchorMax, new Vector2(_bottomCurtain.rectTransform.anchorMax.x, 0), elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RevealStageNumber()
    {
        while (_blackCurtain.rectTransform.localScale.y > 0)
        {
            _blackCurtain.rectTransform.localScale = new Vector3(_blackCurtain.rectTransform.localScale.x, Mathf.Clamp(_blackCurtain.rectTransform.localScale.y - Time.deltaTime, 0, 1), _blackCurtain.rectTransform.localScale.z);
            yield return null;
        }
        StopCoroutine("RevealStageNumber");
    }

    private IEnumerator StartStage()
    {
        _audioManager.PlaySound(SoundKey.MainTheme);
        StartCoroutine(RevealStageNumber());
        yield return new WaitForSeconds(_waitBeforeStart);
        StartCoroutine(RevealTopStage());
        StartCoroutine(RevealBottomStage());
    }
}
