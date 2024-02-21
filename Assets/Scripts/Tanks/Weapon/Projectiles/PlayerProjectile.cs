using UnityEngine;
using Zenject;

public class PlayerProjectile : Projectile
{
    private AudioManager _audioManager;
    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _audioManager.PlaySound(SoundKey.BulletLaunch);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) // for tilemap
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Brick") || (destroySteel && collision.gameObject.CompareTag("Steel")))
        {
            if (collision.gameObject.CompareTag("Steel") && destroySteel)
                _audioManager.PlaySound(SoundKey.BlockDestroyed);
            else if (collision.gameObject.CompareTag("Steel") && !destroySteel)
                _audioManager.PlaySound(SoundKey.BlockNotDestroyed);
            else if (collision.gameObject.CompareTag("Brick"))
                _audioManager.PlaySound(SoundKey.BlockDestroyed);
        }
        else
            _audioManager.PlaySound(SoundKey.BlockNotDestroyed);
    }
}