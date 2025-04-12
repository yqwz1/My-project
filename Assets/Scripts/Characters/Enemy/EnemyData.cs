using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]   
public class EnemyData : ScriptableObject
{
        public string enemyName;
        public int maxHealth;
        public int Damage;
        
}
