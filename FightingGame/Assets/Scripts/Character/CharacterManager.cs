using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharacterDescription _characterDescription = null;

    private readonly List<Character> _characters = new();

    public void SpawnAllCharacters()
    {
        SpawnCharacter( ETeam.TeamOne );
        SpawnCharacter( ETeam.TeamTwo );
    }

    private void SpawnCharacter(
        ETeam team
        )
    {
        Character new_character = _characterDescription.GetCharacter( team );
        _characters.Add( new_character );
        GameManager.Instance.InputReceiverManager.RegisterInputReceiver( new_character );
    }
}
