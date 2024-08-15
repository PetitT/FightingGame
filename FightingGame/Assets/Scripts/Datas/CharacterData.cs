using System;
using UnityEngine;

[CreateAssetMenu( fileName = "NewCharacterData", menuName = "Game/CharacterData" )]
public class CharacterData : ScriptableObject
{
    [Serializable]
    public class Movement
    {
        [field: SerializeField] public float MaxSpeed { get; private set; } = 10.0f;
    }

    [field:SerializeField] public Movement MovementData { get; private set; } = new Movement();
}
