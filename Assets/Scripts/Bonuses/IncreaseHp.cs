using UnityEngine;
using Zenject;

public class IncreaseHp : PowerUp
{
    private SceneTracker _sceneTracker;
    private UIManager _uiManager;

    [Inject]
    private void Construct(UIManager uIManager, SceneTracker sceneTracker)
    {
        _uiManager = uIManager;
        _sceneTracker = sceneTracker;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);
        audioManager.PlaySound(SoundKey.AdditionalLife);
        _sceneTracker.playerLives++;
        _uiManager.UpdatePlayerLives();
        Destroy(gameObject);
    }
}
