﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    [SerializeField] float maxHealthPoints = 100f;

    float currentHealthPoints = 100f;

    //Note: not sure why a get is used here, I'm not well versed but later want to look that up
    //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }
}
