using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressState
{
    public static GameProgressState CurrentState;
    
    public int id;
    public int room;
    public string componentName;
    public int stage;
    public Status status;
    
    public static string ReplaceStatusStringWithInt(string json)
    {
        return json.Replace("\"status\":\"TALK\"", "\"status\":0")
                   .Replace("\"status\":\"TEST\"", "\"status\":1")
                   .Replace("\"status\":\"TESTS_ACTIVE\"", "\"status\":2")
                   .Replace("\"status\":\"DESTROYED\"", "\"status\":3")
                   .Replace("\"status\":\"MUTATED\"", "\"status\":4")
                   .Replace("\"status\":\"DEBUGGING\"", "\"status\":5");
    }
    
    public enum Status
    {
        TALK,
        TEST,
        TESTS_ACTIVE,
        DESTROYED,
        MUTATED,
        DEBUGGING
    }
}
