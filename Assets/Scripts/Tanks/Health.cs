using UnityEngine;
using Zenject;

public class Health : MonoBehaviour
{
    private AnimationController _animationController;
    private SceneTracker _sceneTracker;
    private GamePlayManager _gamePlayManager;
    private AudioManager _audioManager;
    private int _actualHealth;
    private int _currentHealth;
    private int invincibleHealthAmount;
    public bool divineIntervention { get; private set; }

    [Inject]
    private void Construct(SceneTracker sceneTracker, GamePlayManager gamePlayManager,AudioManager audioManager)
    {
        _sceneTracker = sceneTracker;
        _gamePlayManager = gamePlayManager;
        _audioManager = audioManager;
    }

    // need to replace with private ^^ callBack
    public void TakeDamage(int damage = 1, bool destroyedByPowerUp = false)
    {
        divineIntervention = destroyedByPowerUp;
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _animationController.Explode();

            if (gameObject.CompareTag("Player"))
            {
                _audioManager.PlaySound(SoundKey.EnemyKilled);
                _audioManager.StopSound(SoundKey.MotorIdle);
                _audioManager.StopSound(SoundKey.MotorMove);
                _sceneTracker.playerLevel = 1;
                _gamePlayManager.SpawnPlayer();
            }
            else
            {
                if (!divineIntervention)
                {
                    if (gameObject.CompareTag("Small")) _sceneTracker.smallTanksDestroyed++;
                    else if (gameObject.CompareTag("Fast")) _sceneTracker.fastTanksDestroyed++;
                    else if (gameObject.CompareTag("Big")) _sceneTracker.bigTanksDestroyed++;
                    else if (gameObject.CompareTag("Armored")) _sceneTracker.armoredTanksDestroyed++;
                }
                if (gameObject.GetComponent<BonusTank>().IsBonusTankCheck())
                {
                    _audioManager.PlaySound(SoundKey.BonusTankShooted);
                    _audioManager.PlaySound(SoundKey.EnemyKilled);
                    _gamePlayManager.GenerateBonusCrate();
                }
                else
                {
                    _audioManager.PlaySound(SoundKey.EnemyKilled);
                }
            }
        }
        else
        {
                _audioManager.PlaySound(SoundKey.ShootAtArmoredTank);
        }
    }
    // used in animations
    private void SetHealth()
    {
        _currentHealth = _actualHealth;
    }

    public void SetInvincible()
    {
        _currentHealth = invincibleHealthAmount;
    }

    public void InitializeWithData<T>(T data,AnimationController animationController) where T : SO_GenericTank
    {
        _actualHealth = data.Health;
        SetHealth();
        invincibleHealthAmount = data.InvincibleHpAmount;
        _animationController = animationController;
    }
    // used in animations
    private void Death()
    {
        _audioManager.StopSound(SoundKey.MotorIdle);
        Destroy(gameObject);
    }
}
