using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    private readonly List<SceneInstance> _loadedScenes = new();

    public async UniTask<SceneInstance> LoadSceneAsync(
        AssetReferenceScene scene_reference,
        LoadSceneMode load_scene_mode
        )
    {
        SceneInstance load_result = await Addressables.LoadSceneAsync( scene_reference, load_scene_mode );

        _loadedScenes.Add( load_result );

        return load_result;
    }

    public async UniTask UnloadSceneAsync(
        SceneInstance scene
        )
    {
        await Addressables.UnloadSceneAsync( scene );
        _loadedScenes.Remove( scene );
    }

    public async UniTask UnloadAllLoadedScenes()
    {
        foreach( var scene in _loadedScenes )
        {
            await Addressables.UnloadSceneAsync( scene );
        }

        _loadedScenes.Clear();
    }
}
