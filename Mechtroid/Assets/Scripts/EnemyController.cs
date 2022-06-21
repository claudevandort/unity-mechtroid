using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Mech
{
    new private void Start()
    {
        base.Start();
        for(int i = 0; i < 5; i++)
            Invoke("Shoot", 3 * (i+1));
    }
}
