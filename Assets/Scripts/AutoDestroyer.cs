using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    public float destroyDelay = 1f;

    private void Start()
    {
        LeanPool.Despawn(gameObject, destroyDelay);
    }
}

