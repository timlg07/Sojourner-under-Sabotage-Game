using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public void TriggerAlarm(string componentName)
    {
        Debug.Log("Alarm triggered for " + componentName);
    }
}
