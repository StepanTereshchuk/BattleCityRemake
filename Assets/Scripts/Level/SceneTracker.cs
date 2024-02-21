using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    [SerializeField] private int _smallTankPoints = 100;
    [SerializeField] private int _fastTankPoints = 200;
    [SerializeField] private int _bigTankPoints = 300;
    [SerializeField] private int _armoredTankPoints = 400;
    [SerializeField] private int _hiScore = 1;
    public int smallTankPointsWorth { get { return _smallTankPoints; } }
    public int fastTankPointsWorth { get { return _fastTankPoints; } }
    public int bigTankPointsWorth { get { return _bigTankPoints; } }
    public int armoredTankPointsWorth { get { return _armoredTankPoints; } }
    public int hiScore { get { return _hiScore; } }

    public int smallTanksDestroyed; 
    public int fastTanksDestroyed;
    public int bigTanksDestroyed;
    public int armoredTanksDestroyed;
    public int stageNumber;
    public int playerLives = 3;
    public int playerScore = 0;
    public int playerLevel = 1;
    public bool stageCleared = false;

    public void CalculatePlayerScore()
    {
        playerScore += smallTanksDestroyed * _smallTankPoints + fastTanksDestroyed * _fastTankPoints + bigTanksDestroyed * _bigTankPoints + armoredTanksDestroyed * _armoredTankPoints;
    }

}
