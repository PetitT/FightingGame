using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class BufferedEvent<T_Args>
{
    // -- FIELDS

    private bool ShouldFireEvent = false;
    private T_Args EventParameter = default( T_Args );

    // -- EVENTS

    private UnityEvent<T_Args> OnInvoke = new();

    // -- METHODS

    public void SetEventCallback(
        UnityAction<T_Args> action
        )
    {
        OnInvoke.AddListener( action );

        if( ShouldFireEvent )
        {
            action.Invoke( EventParameter );
        }
    }

    public void ClearEventCallback(
        UnityAction<T_Args> action
        )
    {
        OnInvoke.RemoveListener( action );
    }

    public void FireEvent(
        T_Args parameter
        )
    {
        EventParameter = parameter;
        OnInvoke.Invoke( parameter );
        ShouldFireEvent = true;
    }

    public void Reset()
    {
        EventParameter = default( T_Args );
        ShouldFireEvent = false;
    }

    public async UniTask<T_Args> WaitForEventAsync()
    {
        await UniTask.WaitUntil( () => ShouldFireEvent );

        return EventParameter;
    }
}