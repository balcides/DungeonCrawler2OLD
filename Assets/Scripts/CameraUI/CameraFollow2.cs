﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  RPG.CameraUI
{
    public class CameraFollow2 : MonoBehaviour
    {
        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            transform.position = player.transform.position;
        }
    }
}

