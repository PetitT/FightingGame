using UnityEngine;

[CreateAssetMenu( fileName = "Character State Data", menuName = "Game/CharacterStateData" )]
public class CharacterStateData : ScriptableObject
{
    [field: SerializeField] public CharacterStateDescription IdleState { get; private set; }
    [field: SerializeField] public CharacterStateDescription WalkState { get; private set; }
}
