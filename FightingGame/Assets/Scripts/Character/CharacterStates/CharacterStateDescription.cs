using UnityEngine;

[CreateAssetMenu(fileName ="NewStateDescription", menuName = "Game/CharacterStateDescription")]
public class CharacterStateDescription : ScriptableObject
{
    [SerializeField] private CharacterStateBase _statePrefab;

    public CharacterStateBase GetCharacterState()
    {
        CharacterStateBase new_state = Instantiate(_statePrefab);
        new_state.Initialize();
        return new_state;
    }
}
