using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : GenericSingleton<GameManager>
{
    
    public UIManager UI;

    private void Start()
    {
        UI = UIManager.Instance;
        InitialiseEvents();
    }

    private void InitialiseEvents()
    {
        
    }
}
