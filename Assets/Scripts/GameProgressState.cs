using System;

[Serializable]
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
        foreach (Status status in System.Enum.GetValues(typeof(Status)))
        {
            json = json.Replace("status\":\"" + status + "\"", "status\":" + (int) status);
        }
        return json;
    }
    
    public enum Status
    {
        DOOR,
        TALK,
        TEST,
        TESTS_ACTIVE,
        DESTROYED,
        MUTATED,
        DEBUGGING
    }
    
    [Serializable]
    public class DialogueCondition
    {
        public int room = 1;
        public int stage = 1;
        public Status status = Status.TALK;
        
        public DialogueCondition() {}

        public DialogueCondition(GameProgressState currentState)
        {
            room = currentState.room;
            stage = currentState.stage;
            status = currentState.status;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                null => false,
                DialogueCondition c => room == c.room && stage == c.stage && status == c.status,
                GameProgressState s => room == s.room && stage == s.stage && status == s.status,
                _ => false
            };
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(room, stage, status);
        }
    }
}
