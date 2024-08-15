using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterStateBase : MonoBehaviour
{
    [SerializeField] private CharacterAnimation _animation = null;
    [SerializeField] private bool _loop = false;

    private UnityEvent _onAnimationOver = new();

    private int _currentTick = 0;
    private int _maxTicks => _animation.MaximumTicks;
    public UnityEvent OnAnimationOver => _onAnimationOver;
    public int CurrentTick => _currentTick;

    public void Initialize(
        int start_tick = 0
        )
    {
        _currentTick = start_tick;
        _animation.Initialize();
    }

    // This method is called every frame
    public void ProcessCommand(
        Command command
        )
    {
        UpdateAnimation();
        IncrementTick();
        ProcessCommandInternal( command );
    }

    protected virtual void ProcessCommandInternal(
         Command command
        )
    {

    }

    private void UpdateAnimation()
    {
        _animation.SetFrame( _currentTick );
    }

    private void IncrementTick()
    {
        _currentTick++;

        if( _currentTick < _maxTicks )
        {
            return;
        }

        if( _loop )
        {
            _currentTick = 0;
        }
        else
        {
            OnAnimationOver.Invoke();
        }
    }
}
