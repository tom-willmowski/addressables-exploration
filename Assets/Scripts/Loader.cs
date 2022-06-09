using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

public class Loader : MonoBehaviour
{
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        Addressables.InternalIdTransformFunc += OnInternalIdTransfrom;
        await Addressables.InstantiateAsync("Cube");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadScene("Dynamic").Forget();
        }
        if (Input.GetMouseButtonDown(1))
        {
            LoadScene("Static").Forget();
        }
    }

    private async UniTask LoadScene(string key)
    {
        await Addressables.LoadSceneAsync(key);
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