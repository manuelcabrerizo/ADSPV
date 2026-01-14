using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Rexar.View
{
    public class Shader : IDisposable
    {
        private int mHandle;
        private bool mDisposedValue = false;
        
        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);
            
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            int success;

            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out success);
            if(success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if(success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }
            
            mHandle = GL.CreateProgram();
            GL.AttachShader(mHandle, vertexShader);
            GL.AttachShader(mHandle, fragmentShader);
            GL.LinkProgram(mHandle);
            
            GL.GetProgram(mHandle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(mHandle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(mHandle, vertexShader);
            GL.DetachShader(mHandle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        ~Shader()
        {
            if (mDisposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Use()
        {
            GL.UseProgram(mHandle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!mDisposedValue)
            {
                GL.DeleteProgram(mHandle);
                mDisposedValue = true;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Destroying Shader");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetInt(string name, int val)
        {
            int location = GL.GetUniformLocation(mHandle, name);
            GL.Uniform1(location, val);
        }

        // TODO: check how ref works on C#
        public void SetMatrix4(string name, ref Matrix4 mat)
        {
            int location = GL.GetUniformLocation(mHandle, name);
            GL.UniformMatrix4(location, true, ref mat);
        }
    }
}
