using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class TallyScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hiScoreText;
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private TextMeshProUGUI _playerScoreText;
    [SerializeField] private TextMeshProUGUI _smallTankScoreText;
    [SerializeField] private TextMeshProUGUI _fastTankScoreText;
    [SerializeField] private TextMeshProUGUI _bigTankScoreText;
    [SerializeField] private TextMeshProUGUI _armoredTankScoreText;
    [SerializeField] private TextMeshProUGUI _smallTanksDestroyed;
    [SerializeField] private TextMeshProUGUI _fastTanksDestroyed;
    [SerializeField] private TextMeshProUGUI _bigTanksDestroyed;
    [SerializeField] private TextMeshProUGUI _armoredTanksDestroyed;
    [SerializeField] private TextMeshProUGUI _totalTanksDestroyed;

    private SceneTracker _sceneTracker;
    private AudioManager _audioManager;
    private float calculationDelay;
    private IEnumerator coroutine1;
    private IEnumerator coroutine2;
    private IEnumerator coroutine3;
    private IEnumerator coroutine4;

    [Inject]
    private void Construct(SceneTracker sceneTracker, AudioManager audioManager)
    {
        _sceneTracker = sceneTracker;
        _audioManager = audioManager;
    }

    private void Start()
    {
        _stageText.text = "STAGE " + _sceneTracker.stageNumber;
        _hiScoreText.text = "" + _sceneTracker.hiScore;
        _totalTanksDestroyed.gameObject.SetActive(false);
        SetCoroutines();
        StartCoroutine(UpdateResults());
    }
    private void SetCoroutines()
    {
        coroutine1 = UpdateConcretTankPoints(_sceneTracker.smallTanksDestroyed, _sceneTracker.smallTankPointsWorth, _smallTankScoreText, _smallTanksDestroyed);
        coroutine2 = UpdateConcretTankPoints(_sceneTracker.fastTanksDestroyed, _sceneTracker.fastTankPointsWorth, _fastTankScoreText, _fastTanksDestroyed);
        coroutine3 = UpdateConcretTankPoints(_sceneTracker.bigTanksDestroyed, _sceneTracker.bigTankPointsWorth, _bigTankScoreText, _bigTanksDestroyed);
        coroutine4 = UpdateConcretTankPoints(_sceneTracker.armoredTanksDestroyed, _sceneTracker.armoredTankPointsWorth, _armoredTankScoreText, _armoredTanksDestroyed);
    }
    // лучше переделать на метод
    // корутину в кортине не реально отслеживать
    private IEnumerator UpdateConcretTankPoints(int tanksDestroyed, int tankWorth, TextMeshProUGUI tanksScoreText, TextMeshProUGUI tanksDestroyedScoreText)
    {
        int tanksScore = 0;
        calculationDelay = _audioManager.GetSoundLenght(SoundKey.ScoreIncrease);
        for (int i = 1; i <= tanksDestroyed; i++)
        {
            tanksScore = tankWorth * i;
            tanksScoreText.text = tanksScore.ToString();
            tanksDestroyedScoreText.text = i.ToString();
            _audioManager.PlaySound(SoundKey.ScoreIncrease);
            yield return new WaitForSeconds(calculationDelay);
        }
        _sceneTracker.playerScore += tanksScore;
    }
    private IEnumerator UpdateTextNumber(int current, int desired, TextMeshProUGUI textTMP)
    {
        int number = current;
        calculationDelay = _audioManager.GetSoundLenght(SoundKey.ScoreIncrease);
        for (int i = current; i <= desired; i++)
        {
            ++i;
            textTMP.text = i.ToString();
            _audioManager.PlaySound(SoundKey.ScoreIncrease);
            yield return new WaitForSeconds(calculationDelay);
        }
    }

    private IEnumerator UpdateResults()
    {
        _sceneTracker.CalculatePlayerScore();
        _playerScoreText.text = _sceneTracker.playerScore.ToString();

        yield return coroutine1;
        yield return new WaitForSeconds(1f);
        yield return coroutine2;
        yield return new WaitForSeconds(1f);
        yield return coroutine3;
        yield return new WaitForSeconds(1f);
        yield return coroutine4;
        yield return new WaitForSeconds(2f);

        _totalTanksDestroyed.text = (_sceneTracker.smallTanksDestroyed + _sceneTracker.fastTanksDestroyed + _sceneTracker.bigTanksDestroyed + _sceneTracker.armoredTanksDestroyed).ToString();
        _totalTanksDestroyed.gameObject.SetActive(true);

        if (_sceneTracker.playerScore > _sceneTracker.hiScore)
        {
            _audioManager.PlaySound(SoundKey.HighScore);
            StartCoroutine(UpdateTextNumber(_sceneTracker.hiScore, _sceneTracker.playerScore, _hiScoreText));
        }

        yield return new WaitForSeconds(_audioManager.GetSoundLenght(SoundKey.HighScore));

        if (_sceneTracker.stageCleared)
        {
            ClearStatistics();
            SceneManager.LoadScene("Stage" + (_sceneTracker.stageNumber + 1));
        }
        else
        {
            ClearStatistics();
            //Load ShowGameOverText Scene
        }
    }
    private void ClearStatistics()
    {
        _sceneTracker.smallTanksDestroyed = 0;
        _sceneTracker.fastTanksDestroyed = 0;
        _sceneTracker.bigTanksDestroyed = 0;
        _sceneTracker.armoredTanksDestroyed = 0;
        _sceneTracker.stageCleared = false;
    }
}
