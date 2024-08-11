using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class BufferedEvent
{
    private bool ShouldFireEvent = false;

    private UnityEvent OnInvoke = new();

    public void AddListener(
        UnityAction action
        )
    {
        OnInvoke.AddListener( action );

        if( ShouldFireEvent )
        {
            action.Invoke();
        }
    }

    public void RemoveListener(
        UnityAction action
        )
    {
        OnInvoke.RemoveListener( action );
    }

    public void Invoke()
    {
        OnInvoke.Invoke();
        ShouldFireEvent = true;
    }

    public void Reset()
    {
        ShouldFireEvent = false;
    }

    public async UniTask WaitForEventAsync()
    {
        await UniTask.WaitUntil( () => ShouldFireEvent );
    }
}