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

    public void Initialize()
    {
        _currentTick = 0;
        _animation.Initialize();
    }

    public virtual void ProcessInput(
        Inputs input
        )
    {
        UpdateAnimation();
        IncrementTick();
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
