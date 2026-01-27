using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace View.Engine.Rendering
{
    public class Texture : IDisposable
    {
        private int handle;
        private bool disposedValue = false;

        public Texture(string filepath)
        {
            handle = GL.GenTexture();
            Bind();

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(filepath), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, image.Data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        ~Texture()
        {
            if(disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteTexture(handle);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
