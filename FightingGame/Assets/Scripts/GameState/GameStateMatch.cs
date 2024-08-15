public class GameStateMatch : GameStateBase
{
    public class StateMachineData
    {
        public CharacterStateDescription Description = null;
        public int Tick = 0;
    }

    public StateMachineData _stateMachineData = null;
}
