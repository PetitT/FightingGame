using UnityEngine;

[CreateAssetMenu( fileName = "NewStateDescription", menuName = "Game/CharacterStateDescription" )]
public class CharacterStateDescription : ScriptableObject
{
    [SerializeField] private CharacterStateBase _statePrefab;

    public CharacterStateBase GetCharacterState(
        Transform _origin,
        int start_tick = 0
        )
    {
        CharacterStateBase new_state = Instantiate( _statePrefab, _origin );
        new_state.Initialize( _origin, start_tick );
        return new_state;
    }
}
