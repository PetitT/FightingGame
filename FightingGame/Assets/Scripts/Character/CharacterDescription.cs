using UnityEngine;

[CreateAssetMenu( fileName = "NewCharacterDescription", menuName = "Game/CharacterDescription" )]
public class CharacterDescription : ScriptableObject
{
    [SerializeField] private Character _characterPrefab;

    public Character GetCharacter(
        ETeam team
        )
    {
        Character new_character = Instantiate( _characterPrefab );
        new_character.Initialize( team );

        return new_character;
    }
}
