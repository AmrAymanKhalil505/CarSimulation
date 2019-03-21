using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

    public class AdversialCarController : MonoBehaviour
    {
        
        private void Start()
        {
            
        }
        private void Update()
        {
            transform.Translate(0f,0f,4f*Time.deltaTime);
        }
    }
