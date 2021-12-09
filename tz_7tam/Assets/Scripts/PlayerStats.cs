using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField] int bombRange;
    public int BombRange => bombRange;
    [SerializeField] int maxBombs;
    public int MaxBombs => maxBombs;
}
