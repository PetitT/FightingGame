using UnityEngine;

[CreateAssetMenu(fileName ="NewGameDatas", menuName = "Game/GameDatas")]
public class GameDatas : ScriptableObject
{
    [field:SerializeField] public int TicksForGameStart { get; private set; } = 100;
    [field:SerializeField] public int InputTickDelay { get; private set; } = 3;
}
