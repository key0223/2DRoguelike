using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T> (string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public GameObject InstantiateDropItem(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/DropItemPrefabs/{path}");

        if(prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        return Object.Instantiate(prefab, parent);
    }
    public GameObject InstantiateTempItem(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/UIPrefabs/{path}");

        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        return Object.Instantiate(prefab, parent);
    }

    public Sprite GetSprite<T> (string path)
    {
        Sprite sprite = Load<Sprite>($"Sprites/{path}");

        if(sprite == null)
        {
            Debug.Log($"Failed to load sprite : {path}");
            return null;
        }
        return Resources.Load<Sprite>($"Sprites/{path}");
    }
    
    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;
    }
}
