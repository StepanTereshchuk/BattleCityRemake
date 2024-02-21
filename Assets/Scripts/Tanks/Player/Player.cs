using UnityEngine;

public class Player : TankAbstract<SO_PlayerTank>
{
    // LevelController Class
    private Sprite[] _tankUpgrades;
    private SpriteRenderer[] _bodySpriteRenderers;
    public int playerLevel { get; private set; }
    private int _upgradeIndex = 1;
    //
    private TankInputController _playerInputSystem;

    public void UpgradeTank()
    {
        if (playerLevel < 4)
        {
            playerLevel++;
            switch (playerLevel)
            {
                case 2:
                    _upgradeIndex = 1;
                    weaponController.UpgradeProjectileSpeed();
                    break;
                case 3:
                    _upgradeIndex = 3;
                    weaponController.GenerateSecondCanonBall();
                    break;
                case 4:
                    _upgradeIndex = 5;
                    weaponController.CanonBallPowerUpgrade();
                    break;
            }
        }

        _bodySpriteRenderers[0].sprite = _tankUpgrades[_upgradeIndex];
        _bodySpriteRenderers[1].sprite = _tankUpgrades[_upgradeIndex - 1];
    }

    protected override void Start()
    {
        base.Start();
        _playerInputSystem = new TankInputController(weaponController, audioManager, this);
        _bodySpriteRenderers = new SpriteRenderer[2];
        for (int i = 0; i < _bodySpriteRenderers.Length; i++)
            _bodySpriteRenderers[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        playerLevel = 1;
        ApplyPlayerLevel();
        audioManager.PlaySound(SoundKey.MotorIdle);
    }
    protected override void InitializeWithData()
    {
        base.InitializeWithData();
        blockingLayer = tankData.BlockingLayerMask;
        speed = tankData.Speed;
        _tankUpgrades = tankData.Upgrades;
    }
    private void FixedUpdate()
    {
        if (!_playerInputSystem.inputVector.Equals(Vector2.zero))
        {
            Move(_playerInputSystem.inputVector);
        }
    }
    private void ApplyPlayerLevel()
    {
        if (sceneTracker.playerLevel > 1)
        {
            _bodySpriteRenderers[0].sprite = _tankUpgrades[0];
            _bodySpriteRenderers[1].sprite = _tankUpgrades[1];
            weaponController.level = 2;
            if (sceneTracker.playerLevel > 2)
            {
                _bodySpriteRenderers[0].sprite = _tankUpgrades[2];
                _bodySpriteRenderers[1].sprite = _tankUpgrades[3];
                weaponController.level = 3;
                if (sceneTracker.playerLevel > 3)
                {
                    _bodySpriteRenderers[0].sprite = _tankUpgrades[4];
                    _bodySpriteRenderers[1].sprite = _tankUpgrades[5];
                    weaponController.level = 4;
                }
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}