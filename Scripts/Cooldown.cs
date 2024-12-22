using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Cooldown 
{
    [SerializeField] public float cooldownTime;
    private float nextAttackTime;

    public bool isCoolingDown => Time.time < nextAttackTime;
    public void startCoolDown() => nextAttackTime = Time.time + cooldownTime;
   
}
