using UnityEngine;
using Zenject;

public class Eagle : MonoBehaviour
{
    private GamePlayManager _gamePlayManager;
    private AudioManager _audioManager;

    [Inject]
    public void Construct(GamePlayManager gamePlayManager,AudioManager audioManager)
    {
        _gamePlayManager = gamePlayManager;
        _audioManager = audioManager;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile") || collision.gameObject.CompareTag("PlayerProjectile"))
        {
            GetComponent<Animator>().enabled = true;
            _audioManager.PlaySound(SoundKey.BaseDestroyed);
            _gamePlayManager.GameOver();
        }
    }
}
