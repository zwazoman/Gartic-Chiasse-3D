using Unity.Services.Core;
using UnityEngine;
using System;

public class InitializerBien : MonoBehaviour
{
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            print("ça marche au top");
        }
        catch(Exception e)
        {
            Debug.LogException(new Exception("hésite pas a niquer ta mère",e));
        }
    }
}
