using System.Collections.Generic;
using System.IO;
using FMFCLPRO.UnityVoxels.Resources;
using Newtonsoft.Json;
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

public class Initialiser : MonoBehaviour
{
    public static readonly Dictionary<string, VoxelTextureData> TextureDatas =
        new Dictionary<string, VoxelTextureData>();

    private void Awake()
    {
        string path = $@"{Application.dataPath}\BlockDatas";

        foreach (string file in Directory.EnumerateFiles(path, "*.json"))
        {
            string contents = File.ReadAllText(file);

            string fname = Path.GetFileName(file).Replace(".json", "");
            ;

            VoxelTextureData a = JsonConvert.DeserializeObject<VoxelTextureData>(contents);
            if (a != null)
            {
                TextureDatas[fname] = a;
            }
        }
    }
}