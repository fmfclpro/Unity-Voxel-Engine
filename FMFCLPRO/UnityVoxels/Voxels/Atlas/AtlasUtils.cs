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

namespace FMFCLPRO.UnityVoxels.Voxels.Atlas
{
    public static class AtlasUtils
    {
        public static TextureAtlas StitchTextures(Texture2D[] sprites)
        {
            TextureAtlas at = new TextureAtlas();

            int sizeX = 16;
            int sizeY = 16;

            Texture2D texture2D = new Texture2D(16 * sizeX, 16 * sizeY, TextureFormat.ARGB32, false);

            int cellSizeX = 16;
            int cellSizeY = 16;

            int throughX = 0;
            int throughY = 0;

            int count = 0;
            int end = sprites.Length / 2;

            for (int i = 0; i < sprites.Length; i++)
            {
                Texture2D texture = sprites[i];

                if (count == end)
                {
                    throughY++;
                    throughX = 0;
                }

                for (int x = 0; x < cellSizeX; x++)
                {
                    for (int y = 0; y < cellSizeY; y++)
                    {
                        Color color = texture.GetPixel(x, y);
                        Vector2Int coordinate = new Vector2Int(x + (throughX * cellSizeX), y + (throughY * cellSizeY));
                        texture2D.SetPixel(coordinate.x, coordinate.y, color);
                    }
                }

                at.tagToUv.Add(texture.name, at.get_texture(throughX, throughY));
                //
                throughX++;
                count++;
            }

            texture2D.filterMode = FilterMode.Point;
            at.Atlas = texture2D;
            at.Atlas.Apply();


            // byte[] bytes =  toSet.EncodeToPNG();
            // var dirPath = @"C:\Users\FMFCL\Desktop\Merge";
            //
            // File.WriteAllBytes(dirPath + @"\Image" + ".png", bytes);
            return at;
        }
    }
}