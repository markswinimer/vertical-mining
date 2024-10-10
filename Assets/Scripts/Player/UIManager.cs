using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public AmmoBar ammoBar;

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

    public void Start()
    {
        ammoBar.SetMaxAmmo(Player.Instance.MaxAmmo);
        ammoBar.SetAmmo(Player.Instance.Ammo);
    }
}
