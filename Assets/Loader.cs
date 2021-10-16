using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Loader : MonoBehaviour
{
    private const string remoteUrl = "http://192.168.8.103:8080";
    private const string REMOTE_URL_KEY = "RemoteURL";
    
    // Start is called before the first frame update
    private async void Start()
    {
        await UpdateAssetsCatalog();
        await LoadStatic();
        await LoadDynamic();
    }
    
    private async UniTask UpdateAssetsCatalog()
    {
        AddressablesRuntimeProperties.SetPropertyValue(REMOTE_URL_KEY, remoteUrl);
        Addressables.InternalIdTransformFunc += TransformInternalId;
        var catalogs = await Addressables.CheckForCatalogUpdates();
        if (catalogs.Count == 0)
        {
            return;
        }
        await Addressables.UpdateCatalogs(catalogs);
    }

    private string TransformInternalId(IResourceLocation location)
    {
        Debug.Log(location.InternalId);
        return location.InternalId;
    }

    private async UniTask LoadStatic()
    {
        await Addressables.InstantiateAsync("Cube");
    }

    private async UniTask LoadDynamic()
    {
        await Addressables.InstantiateAsync("Sphere");
    }
}
