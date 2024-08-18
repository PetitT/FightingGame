using UnityEngine;

public class GameStateMatch : GameStateBase
{
    public class StateMachineData
    {
        public CharacterStateDescription Description = null;
        public int Tick = 0;
        public Vector3 Position = Vector3.zero;
    }

    public StateMachineData _stateMachineData = null;
}
