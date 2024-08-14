using UnityEngine;

public abstract class CharacterStateBase : ScriptableObject
{
    //public Animation _entryAnimation;

    public virtual void ProcessInput( Inputs input ) { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}
