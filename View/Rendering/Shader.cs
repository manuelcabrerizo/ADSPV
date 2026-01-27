using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace ZooArchitect.View.Rendering
{
    public struct UniformBufferBindInfo
    {
        public UniformBuffer UniformBuffer;
        public int BindingPoint;
    }

    public class Shader : IDisposable
    {
        private int handle;
        private bool disposedValue = false;
        private Dictionary<string, UniformBufferBindInfo> uniformBuffers = new Dictionary<string, UniformBufferBindInfo>();
        private static Dictionary<string, UniformBuffer> sUniformBuffersRegistry = new Dictionary<string, UniformBuffer>();
        
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
            
            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);
            GL.LinkProgram(handle);
            
            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // Set up the Uniform buffer for this shader
            InitializeUniformBuffers();
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Bind()
        {
            GL.UseProgram(handle);
            foreach (var uniformBuffer in uniformBuffers)
            {
                uniformBuffer.Value.UniformBuffer.Bind(uniformBuffer.Value.BindingPoint);
            }
        }

        static public void UpdateUniformBuffer<DataType>(string name, ref DataType data) where DataType : struct
        {
            if(sUniformBuffersRegistry.ContainsKey(name))
            {
                UniformBuffer uniformBuffer = sUniformBuffersRegistry[name];
                uniformBuffer.Update<DataType>(data);
            }
        }

        public void SetInt(string name, int val)
        {
            int location = GL.GetUniformLocation(handle, name);
            GL.Uniform1(location, val);
        }

        // TODO: check how ref works on C#
        public void SetMatrix4(string name, ref Matrix4 mat)
        {
            int location = GL.GetUniformLocation(handle, name);
            GL.UniformMatrix4(location, true, ref mat);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private List<string> GetUniformBufferNames()
        {
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniformBlocks, out int blockCount);
            List<string> names = new List<string>();
            for (int i = 0; i < blockCount; i++)
            {
                GL.GetActiveUniformBlockName(handle, i, 256, out int length, out string name);
                names.Add(name);
            }
            return names;
        }

        private void InitializeUniformBuffers()
        {
            var names = GetUniformBufferNames();
            if (names.Count == 0)
            {
                return;
            }
            
            for(int i = 0; i < names.Count; i++)
            {
                int blockIndex = GL.GetUniformBlockIndex(handle, names[i]);
                UniformBuffer uniformBuffer = null;
                if (sUniformBuffersRegistry.ContainsKey(names[i]))
                {
                    uniformBuffer = sUniformBuffersRegistry[names[i]];
                }
                else 
                {
                    int blockSize;
                    GL.GetActiveUniformBlock(handle, blockIndex, ActiveUniformBlockParameter.UniformBlockDataSize, out blockSize);
                    uniformBuffer = new UniformBuffer(names[i], blockSize);
                    sUniformBuffersRegistry.Add(names[i], uniformBuffer);
                }
                UniformBufferBindInfo info = new UniformBufferBindInfo();
                info.UniformBuffer = uniformBuffer;
                info.BindingPoint = i;
                GL.UniformBlockBinding(handle, blockIndex, info.BindingPoint);
                uniformBuffers.Add(names[i], info);
            }
        }
    }
}
