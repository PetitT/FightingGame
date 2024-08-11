using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( InputTester ) )]
public class InputTesterEditor : Editor
{
    private InputTester _inputTester = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _inputTester ??= target as InputTester;

        GUI.enabled = !_inputTester.HasSentP1InputThisFrame;

        if( GUILayout.Button( "Send P1 input" ) )
        {
            _inputTester.SendP1Input();
        }

        GUI.enabled = true;

        if( GUILayout.Button( "Send P2 input" ) )
        {
            _inputTester.SendP2Input();
        }

        GUI.enabled = _inputTester.HasSentP1InputThisFrame;

        if( GUILayout.Button( "Update state" ) )
        {
            _inputTester.UpdateInputs();
        }

        GUI.enabled = true;
    }
}
