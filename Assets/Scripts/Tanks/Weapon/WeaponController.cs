using System.Collections;
using UnityEngine;
using Zenject;

public class WeaponController : MonoBehaviour
{
    private GameObject _projectilePrefab;
    private float _projectileSpeed;
    private float _projectileDamage;

    public int level = 1;
    //pool of projectiles
    private Projectile _projectileFirst;
    private Projectile _projectileSecond;
    [Inject] protected DiContainer _container;

    public void UpgradeProjectileSpeed()
    {
        _projectileSpeed *= 2;
        _projectileFirst.IncreaseSpeed(_projectileSpeed);
    }

    public void CanonBallPowerUpgrade()
    {
        if (_projectileFirst != null) _projectileFirst.IncreaseDamage();
        if (_projectileSecond != null) _projectileSecond.IncreaseDamage();
    }

    public void InitializeWithData(SO_GenericTank tankData,AnimationController animationController)
    {
        _projectileSpeed = tankData.ProjectileSpeed;
        _projectileDamage = tankData.ProjectileDamage;
        _projectilePrefab = tankData.ProjectilePrefab;
    }

    private void Start()
    {
        _projectileFirst = _container.InstantiatePrefab(_projectilePrefab, transform.position, transform.rotation, null).GetComponent<Projectile>();
        _projectileFirst.InitializeWithData(_projectileSpeed, _projectileDamage);

    }
    private void ApplyUpgrades()
    {
        Debug.Log("ApplyUpgrades");
        if (level > 1) UpgradeProjectileSpeed();
        if (level > 2) GenerateSecondCanonBall();
        if (level > 3) CanonBallPowerUpgrade();
    }
    public void GenerateSecondCanonBall()
    {
        _projectileSecond = _container.InstantiatePrefab(_projectilePrefab, transform.position, transform.rotation, null).GetComponent<Projectile>();
        _projectileSecond.InitializeWithData(_projectileSpeed, _projectileDamage);
    }

    public void Fire()
    {
        if (_projectileFirst != null && _projectileFirst.gameObject.activeSelf == false)
        {
            _projectileFirst.transform.position = transform.position;
            _projectileFirst.transform.rotation = transform.rotation;
            StartCoroutine(ShowFire());
            _projectileFirst.gameObject.SetActive(true);
        }
        else
        {
            if (_projectileSecond != null)
            {
                if (_projectileSecond.gameObject.activeSelf == false)
                {
                    _projectileSecond.transform.position = transform.position;
                    _projectileSecond.transform.rotation = transform.rotation;
                    StartCoroutine(ShowFire());
                    _projectileSecond.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_projectileFirst != null) _projectileFirst.DestroyProjectile();
        if (_projectileSecond != null) _projectileSecond.DestroyProjectile();
    }

    private IEnumerator ShowFire()
    {
        //_fireStartEffect.SetActive(true);
        //yield return new WaitForSeconds(0.1f);
        //_fireStartEffect.SetActive(false);
        //_fireFinishEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        //_fireFinishEffect.SetActive(false);
    }
}
