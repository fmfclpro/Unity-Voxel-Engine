using System;
using System.Collections.Generic;
using System.IO;
using FMFCLPRO.UnityVoxels.Voxels.Shapes;
using UnityEditor;
using UnityEngine;

/*
MIT License

Copyright (c) 2023 Filipe Lopes | FMFCLPRO

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/


public class RegisteredResource<T>
{
    public Dictionary<string, T> tag_to_item_block;

    public RegisteredResource()
    {
        tag_to_item_block = new Dictionary<string, T>();
    }

    public void Register(string tag, T t)
    {
        OnAdd?.Invoke(tag, t);
        tag_to_item_block[tag] = t;
    }

    public event Action<string, T> OnAdd;

    public T Get(string tag)
    {
        return tag_to_item_block[tag];
    }
}
#if UNITY_EDITOR
public class VoxelMeshScreenShotController : MonoBehaviour
{
    public Material Material;
    public Renderer toTake;
    private VoxelCube _voxelCube = new VoxelCube();

    public Texture2D[] blockTextures;
    public List<Texture2D> Texture2Ds;
    public RegisteredResource<Texture2D> registeredItemBlocks = new RegisteredResource<Texture2D>();
    public Texture2D rk;

    private Camera _camera;

    private void Awake()
    {
        registeredItemBlocks.OnAdd += (s, texture2D) => { Texture2Ds.Add(texture2D); };
        _camera = FindObjectOfType<Camera>();


        foreach (var texture2D in blockTextures)
        {
            Set(texture2D);
        }
    }


    private void Set(Texture2D texture2D)
    {
        Material.mainTexture = texture2D;
        TakeScreenShot(Path, texture2D.name);
    }

    private const string Path = @"C:\Unity Repositories\PetraAdventure Repository\PetraAdventure\Assets\UI";

    private void TakeScreenShot(string path, string elated)
    {
        int x = 256;
        int y = 256;

        RenderTexture t = new RenderTexture(x, y, 24);
        _camera.targetTexture = t;
        Texture2D scrn = new Texture2D(x, y, TextureFormat.RGBA32, false);
        _camera.Render();
        RenderTexture.active = t;
        scrn.ReadPixels(new Rect(0, 0, x, y), 0, 0);
        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(t);
        rk = scrn;
        scrn.Apply();

        byte[] bytes = scrn.EncodeToPNG();

        registeredItemBlocks.Register(elated, scrn);
        string kkk = @$"{path}\{elated}.png";
        File.WriteAllBytes(kkk, bytes);
        AssetDatabase.Refresh();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
#endif