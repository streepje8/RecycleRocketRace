using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RocketNaming : MonoBehaviour
{
    [HideInInspector] public string rocketName;
    public Stream rocketNameStream;
    private readonly string[] randomNames = { "Round Boy", "Loafer Doafey", "Dwayne", "OwO", "UwU", "x3", "" };

    public void GameStart()
    {
        if (String.IsNullOrEmpty(rocketName))
        {
            AssignRandomName();
        }
    }

    private void AssignRandomName()
    {
        rocketName = randomNames[Random.Range(0, randomNames.Length)];
        rocketNameStream.StreamData(rocketName);
    }
    
    public void UpdateRocketName(string newName)
    {
        rocketName = newName;
        rocketNameStream.StreamData(rocketName);
    }
}
