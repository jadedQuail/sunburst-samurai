using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    int currentMoney;

    // Function that returns the player's current money
    public int GetMoney()
    {
        return currentMoney;
    }

    // Function that adds to the player's current money
    public void AddMoney(int amountToAdd)
    {
        currentMoney += amountToAdd;
    }

    // Function that takes away from the player's current money
    public void RemoveMoney(int amountToRemove)
    {
        currentMoney -= amountToRemove;
    }
}
