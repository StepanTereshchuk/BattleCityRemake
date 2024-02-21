using System.Collections;
using UnityEngine;
using Zenject;

public abstract class PowerUp : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float _destroyDelay = 5f;
    private float _blinkRepeatRate = 0.1f;
    private float _blinkDelay = 0f;
    protected AudioManager audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }


    protected virtual void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        InvokeRepeating("Blink", _blinkDelay, _blinkRepeatRate);
        Destroy(gameObject, _destroyDelay);
    }
    private void Blink()
    {
        _sprite.enabled = !_sprite.enabled;
    }
    //private IEnumerator DestroyPowerUp()
    //{
    //    yield return new WaitForSeconds(_destroyDelay);
    //    Destroy(gameObject);
    //}
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        audioManager.PlaySound(SoundKey.BonusTaken);
    }
}
