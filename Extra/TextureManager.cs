using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.DevIl;
using Tao.OpenGl;

namespace FastLoopExample
{
    public struct Texture
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture(int id, int width, int heiht)
            : this()
        {
            ID = id; Width = width; Height = Height;
        }
    }

    //需要附加 Tao.DevI1  ,并且把DevI1.dll,ILU.dll,ILUT.dll放入指定文件夹
    public  class TextureManager : IDisposable    //IDisposable确保类在销毁时会从内存中释放纹理        
    {
        Dictionary<string , Texture> _textureDatabase = new Dictionary<string,Texture>();

        public Texture Get(string textureId)
        {
            return _textureDatabase[textureId];
        }
        public void Dispose()
        {
            foreach (Texture t in _textureDatabase.Values)
            {
                Gl.glDeleteTextures(1, new int[] { t.ID });    //texture是置于一个数组中的
            }
            _textureDatabase.Clear();
        }
        public void Dispose(string textureId)
        {
            Gl.glDeleteTextures(1,new int[]{_textureDatabase[textureId].ID});
            _textureDatabase.Remove(textureId);
        }

        public bool IsHaveKey(string ID)
        {
            return _textureDatabase.ContainsKey(ID);
        }

        public void LoadTexture(string textureId, string path) //载入纹理
        {
            int devilId = 0;
            Il.ilGenImages(1, out  devilId);
            Il.ilBindImage(devilId);        //作为使用的纹理
            if (!Il.ilLoadImage(path))
            {
                System.Diagnostics.Debug.Assert(false, "Could not open file,[" + path + "].");
            }
            //The files we'll be using need to be flipped before passing to OpenGL
            Ilu.iluFlipImage();
            int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            int openGLId = Ilut.ilutGLBindTexImage();
            System.Diagnostics.Debug.Assert(openGLId != 0);
            Il.ilDeleteImages(1, ref devilId);
            _textureDatabase.Add(textureId, new Texture(openGLId, width, height));
        }

        public void LoadTexture(string textureId, int ImageType, IntPtr imageData, int size = 0)
        {
            int devilId = 0;
            Il.ilGenImages(1, out  devilId);
            Il.ilBindImage(devilId);        //作为使用的纹理

            if (!Il.ilLoadL(ImageType, imageData,size))
            {
                System.Diagnostics.Debug.Assert(false, "给予的图像数据存在问题，请核对是否给予正确的图像数组和格式，以及大小");
            }
            //The files we'll be using need to be flipped before passing to OpenGL
            Ilu.iluFlipImage();
            int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            int openGLId = Ilut.ilutGLBindTexImage();
            System.Diagnostics.Debug.Assert(openGLId != 0);
            Il.ilDeleteImages(1, ref devilId);
            _textureDatabase.Add(textureId, new Texture(openGLId, width, height));
        }

        public void LoadTexture(string textureId, int ImageType, byte[] imageData, int size = 0)
        {
            if (imageData == null)
            {
                throw new Exception(textureId + "提供的数据为无效数据");
            }

            int devilId = 0;
            Il.ilGenImages(1, out  devilId);
            Il.ilBindImage(devilId);        //作为使用的纹理

            if (!Il.ilLoadL(ImageType,imageData,size))
            {
                System.Diagnostics.Debug.Assert(false, "给予的图像数据存在问题，请核对是否给予正确的图像数组和格式，以及大小");
            }
            //The files we'll be using need to be flipped before passing to OpenGL
            Ilu.iluFlipImage();
            int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            int openGLId = Ilut.ilutGLBindTexImage();
            System.Diagnostics.Debug.Assert(openGLId != 0);
            Il.ilDeleteImages(1, ref devilId);
            try
            {
                _textureDatabase.Add(textureId, new Texture(openGLId, width, height));
            }
            catch (Exception e)
            {
                string s = e.ToString();
            }
        }


    }
}
