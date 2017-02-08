using System.Collections;
using Tiles.Tools;
using UnityEngine;
using UnityEngine.Networking;

public class Initialize : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        var coords = new double[] { -84.72, 11.17, -5.62, 61.60 };
        var parent = Tilebelt.BboxToTile(coords);
        StartCoroutine(requestTile(parent));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator requestTile(Tile t)
    {
        var url = tile2url(t);
        var handler = new DownloadHandlerBuffer();
        var http = new UnityWebRequest(url);
        http.downloadHandler = handler;
        yield return http.Send();

        if (!http.isError)
        {
            var texture = new Texture2D(10, 5);
            texture.LoadImage(http.downloadHandler.data);
            var tileGO = GameObject.CreatePrimitive(PrimitiveType.Plane);
            var renderer = tileGO.GetComponent<MeshRenderer>();
            renderer.material.mainTexture = texture; 
        }
    }

    private string tile2url(Tile tile)
    {
        return $"https://b.tile.openstreetmap.org/{tile.Z}/{tile.X}/{tile.Y}.png";
    }

}