using System.IO;
using Newtonsoft.Json;
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

[System.Serializable]
public struct BlockUvTags
{
    public string up;
    public string down;
    public string right;
    public string left;
    public string forward;
    public string backward;
}

namespace FMFCLPRO.UnityVoxels.Resources
{
    [CreateAssetMenu(menuName = "BlockTextures/New Block Texture", fileName = "BlockTextureData", order = 0)]
    public class VoxelTextureData : ScriptableObject
    {
        [Header("tag")] public string tag;
        [Header("Replace all sides")] public string allSides;

        [Header("Specific Sides")] public string up;
        public string down;
        public string right;
        public string left;
        public string forward;
        public string backward;

        [Header("Button")] public bool create;

        private void OnValidate()
        {
            if (allSides != null)
            {
                if (allSides.Length > 0)
                {
                    up = allSides;
                    down = allSides;
                    right = allSides;
                    left = allSides;
                    forward = allSides;
                    backward = allSides;
                }
            }

            if (create)
            {
                BlockUvTags tags = new BlockUvTags()
                {
                    up = up,
                    down = down,
                    right = right,
                    left = left,
                    backward = backward,
                    forward = forward
                };

                string toCreate = JsonConvert.SerializeObject(tags, Formatting.Indented);

                // TODO replace path 
                string path = "";
                var a = File.CreateText($@"{path}\{tag}.json");
                a.Write(toCreate);
                a.Dispose();
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
                create = !create;
            }
        }
    }
}