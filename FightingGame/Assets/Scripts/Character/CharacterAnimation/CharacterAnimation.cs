using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private AnimationFrame[] _framesArray;

    [SerializeField, HideInInspector] private int _maximumTicks = 0;
    [SerializeField, HideInInspector] private readonly Dictionary<int, int> _framesAtTicksMap = new();

    int last_displayed_frame = -1;

    public int MaximumTicks => _maximumTicks;

    public void Initialize()
    {
        _framesAtTicksMap.Clear();
        _maximumTicks = 0;

        for( int frame_index = 0; frame_index < _framesArray.Length; frame_index++ )
        {
            _maximumTicks += _framesArray[ frame_index ].FrameCount;

            for( int frame_tick_index = _framesAtTicksMap.Count; frame_tick_index < _maximumTicks; frame_tick_index++ )
            {
                _framesAtTicksMap.Add( frame_tick_index, frame_index );
            }
        }

        HideAll();
    }

    public void SetFrame(
        int current_tick
        )
    {
        if( !_framesAtTicksMap.ContainsKey( current_tick ) )
        {
            Debug.LogError( $"Tick {current_tick} is invalid" );

            return;
        }

        int frame_to_display = _framesAtTicksMap[ current_tick ];

        if( frame_to_display != last_displayed_frame )
        {
            HideAll();

            _framesArray[ frame_to_display ].SetDisplayed( true );
            last_displayed_frame = frame_to_display;
        }
    }

    private void HideAll()
    {
        foreach( AnimationFrame frame in _framesArray )
        {
            frame.SetDisplayed( false );
        }
    }
}
