using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Loader : MonoBehaviour
{
    private async void Start()
    {
        Addressables.InternalIdTransformFunc += OnInternalIdTransfrom;
        await Addressables.LoadSceneAsync("Default");
    }
    
    private string OnInternalIdTransfrom(IResourceLocation arg)
    {
        return arg.InternalId;
    }

    private void OnDestroy()
    {
        Addressables.InternalIdTransformFunc -= OnInternalIdTransfrom;
    }
}