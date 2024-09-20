using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // singleton instance
    public static Player Instance { get; private set; }
   
    public float AttackSpeed  = .5f;
    public float AttackDamage = 20f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
