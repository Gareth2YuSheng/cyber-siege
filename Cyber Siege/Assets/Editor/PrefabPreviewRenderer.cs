using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabPreviewRenderer : MonoBehaviour
{
    [MenuItem("Tools/Render Prefab Preview")]
    public static void RenderPrefabPreview()
    {
        GameObject prefab = Selection.activeObject as GameObject;
        if (prefab == null)
        {
            Debug.LogError("Please select a prefab in the Project view.");
            return;
        }

        int size = 512;

        // Set up temporary scene elements
        var tempGO = Instantiate(prefab);
        tempGO.transform.position = new Vector3(10000f, 10000f, 0);

        // Calculate bounds
        Bounds bounds = CalculateBounds(tempGO);
        Vector3 center = bounds.center;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        // Set up camera
        var camGO = new GameObject("PreviewCamera");
        var cam = camGO.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // transparent
        cam.orthographic = true;
        cam.orthographicSize = maxSize * 0.6f;
        cam.transform.position = center + Vector3.back * 10f; // Assuming Z-axis faces forward
        cam.transform.LookAt(center);

        // Lighting (optional)
        var lightGO = new GameObject("TempLight");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Set up render texture
        var rt = new RenderTexture(size, size, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;
        RenderTexture.active = rt;

        // Render
        cam.Render();

        // Convert to PNG
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        tex.Apply();

        // Save
        byte[] pngData = tex.EncodeToPNG();
        string assetPath = $"Assets/PrefabImages/{prefab.name}_Preview.png";
        File.WriteAllBytes(assetPath, pngData);
        AssetDatabase.Refresh();

        Debug.Log($"Saved preview to {assetPath}");

        // Cleanup
        DestroyImmediate(tempGO);
        DestroyImmediate(camGO);
        DestroyImmediate(lightGO);
        RenderTexture.active = null;
        cam.targetTexture = null;
    }

    private static Bounds CalculateBounds(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.one);

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);
        return bounds;
    }
}
