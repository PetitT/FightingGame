using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceScene : AssetReference
{
    public AssetReferenceScene( string guid ) : base( guid ) { }

    public override bool ValidateAsset( Object obj )
    {
        var path = AssetDatabase.GetAssetPath( obj );
        return obj != null && AssetDatabase.GetMainAssetTypeAtPath( path ) == typeof( SceneAsset );
    }

    public override bool ValidateAsset( string path )
    {
        return AssetDatabase.GetMainAssetTypeAtPath( path ) == typeof( SceneAsset );
    }
}
