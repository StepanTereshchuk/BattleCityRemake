using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTankData", menuName = "ScriptableObjects/Tanks/PlayerTank", order = 1)]
public class SO_PlayerTank : SO_GenericTank
{
    [SerializeField] private Sprite[] tankUpgrades;

    public Sprite[] Upgrades => tankUpgrades;
}
