using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : TankSpawner
{
    public override void ActivateSpawnedTank()
    {
        base.ActivateSpawnedTank();
        if (EnemyAI.freezing == true)
            tank.GetComponent<EnemyAI>().ToFreezeTank();
    }

    public override void StartSpawning()
    {
        List<int> tankToSpawn = new List<int>();
        tankToSpawn.Clear();
        if (levelManager.smallTanks > 0) tankToSpawn.Add((int)tankType.smallTank);
        if (levelManager.fastTanks > 0) tankToSpawn.Add((int)tankType.fastTank);
        if (levelManager.bigTanks > 0) tankToSpawn.Add((int)tankType.bigTank);
        if (levelManager.armoredTanks > 0) tankToSpawn.Add((int)tankType.armoredTank);

        if (tankToSpawn.Count > 0)
        {
            int tankID = tankToSpawn[Random.Range(0, tankToSpawn.Count)]; // PROBLEMS ????
            tank = _container.InstantiatePrefab(tanks[tankID], transform.position, tanks[tankID].transform.rotation, null);
            if (Random.value <= levelManager.bonusCrateRate)
            {
                tank.GetComponent<BonusTank>().MakeBonusTank();
            }

            if (tankID == (int)tankType.smallTank) levelManager.smallTanks--;
            else if (tankID == (int)tankType.fastTank) levelManager.fastTanks--;
            else if (tankID == (int)tankType.bigTank) levelManager.bigTanks--;
            else if (tankID == (int)tankType.armoredTank) levelManager.armoredTanks--;

            uiManager.RemoveTankReserve();
            gamePlayManager.IncreaseEnemyStash(tank.GetComponent<EnemyAI>());
        }
    }

    protected override void Initialize()
    {
        tanks = new GameObject[4] { smallTank, fastTank, bigTank, armoredTank };
    }
}
