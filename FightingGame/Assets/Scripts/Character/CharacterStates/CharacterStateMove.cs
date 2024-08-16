using UnityEngine;

public class CharacterStateMove : CharacterStateBase
{
    [SerializeField] private CharacterData _data;

    private float _currentSpeed;
    private float _acceleration;

    float delta => ScenarioManager.Instance.ActiveScenario.ConnexionHandler.DeltaTime;

    protected override void ProcessCommandInternal(
        Command command
        )
    {
        Move( command );
    }

    private void Move(
        Command command
        )
    {
        Vector3 direction = command.MoveDirection == EMoveDirection.Left ? Vector2.left : Vector2.right;
        _parent.transform.position += direction * _data.MovementData.MaxSpeed * delta;
    }
}
