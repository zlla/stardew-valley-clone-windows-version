using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;

namespace StardewValleyClone
{
    public class Support
    {
        static public List<Texture2D> ImportFolder(string path, GraphicsDevice graphicsDevice)
        {
            List<Texture2D> surfaceList = new();

            foreach (string fullPathImage in Directory.GetFiles(path))
            {
                Texture2D myTexture2D = Texture2D.FromFile(graphicsDevice, fullPathImage);
                surfaceList.Add(myTexture2D);
            }

            return surfaceList;
        }

        static public Dictionary<string, Texture2D> ImportFolderDict(string path, GraphicsDevice graphicsDevice)
        {
            Dictionary<string, Texture2D> surfaceDict = new();

            foreach (string fullPathImage in Directory.GetFiles(path))
            {
                Texture2D myTexture2D = Texture2D.FromFile(graphicsDevice, fullPathImage);
                string tmp = (fullPathImage.Split(new string[] { "\\" }, StringSplitOptions.None)[7].Split(new string[] { "." }, StringSplitOptions.None)[0]);
                tmp = tmp.Split(new string[] { "\\" }, StringSplitOptions.None)[0];
                surfaceDict.Add(tmp, myTexture2D);
            }

            return surfaceDict;
        }
    }
}

