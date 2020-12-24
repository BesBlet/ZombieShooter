using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
  public float destroyDelay = 1f;

  private void Start()
  {
    Destroy(gameObject, destroyDelay);
  }
}
