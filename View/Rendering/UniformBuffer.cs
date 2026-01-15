using OpenTK.Graphics.OpenGL4;

namespace Rexar.View.Rendering
{
    public class UniformBuffer : IDisposable
    {
        private string name;
        private int blockSize;
        private int buffer;
        private bool disposedValue = false;

        public UniformBuffer(string name, int blockSize)
        {
            this.name = name;
            this.blockSize = blockSize;

            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, buffer);
            GL.BufferData(BufferTarget.UniformBuffer, blockSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }

        ~UniformBuffer()
        {
            if(disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Bind(int bindingPoint)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, bindingPoint, buffer);
        }

        public void Update<DataType>(DataType data) where DataType : struct
        {
            //Console.WriteLine($"GL BlockSize: {blockSize} | C# StructSize: {Unsafe.SizeOf<DataType>()}");
            GL.BindBuffer(BufferTarget.UniformBuffer, buffer);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, blockSize, ref data);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(buffer);
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
