using Assimp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace ZooArchitect.View.Rendering
{
    public sealed class Model : IDisposable
    {
        private List<Mesh> meshes;
        public Model(string filepath)
        {
            meshes = new List<Mesh>();

            AssimpContext importer = new AssimpContext();
            Scene scene = importer.ImportFile(filepath,
                    PostProcessSteps.Triangulate |
                    PostProcessSteps.GenerateSmoothNormals |
                    PostProcessSteps.CalculateTangentSpace);

            for (int i = 0; i < scene.MeshCount; i++)
            {
                List<Vertex> vertices = new List<Vertex>();
                List<int> indices = new List<int>();

                Assimp.Mesh assimpMesh = scene.Meshes[i];
                for (int j = 0; j < assimpMesh.VertexCount; j++)
                { 
                    Vertex vertex = new Vertex();
                    vertex.Pos = Vector3FromAssimp(assimpMesh.Vertices[j]);
                    vertex.Nor = Vector3FromAssimp(assimpMesh.Normals[j]);
                    vertex.Tan = Vector3FromAssimp(assimpMesh.Tangents[j]);
                    vertex.Bit = Vector3FromAssimp(assimpMesh.BiTangents[j]);
                    vertex.Uvs = Vector2FromAssimp(assimpMesh.TextureCoordinateChannels[0][j]);
                    vertices.Add(vertex);
                }

                for (int j = 0; j < assimpMesh.FaceCount; j++)
                {
                    Face face = assimpMesh.Faces[j];
                    for (int k = 0; k < face.IndexCount; k++)
                    {
                        indices.Add(face.Indices[k]);
                    }
                }

                meshes.Add(new Mesh(vertices, indices));
            }
            
            importer.Dispose();
        }

        public void Draw()
        {
            foreach (Mesh mesh in meshes)
            {
                mesh.Draw();
            }
        }

        public void Dispose()
        {
            foreach (Mesh mesh in meshes)
            {
                mesh.Dispose();
            }
            meshes.Clear();
        }

        private static Vector2 Vector2FromAssimp(Vector3D vector)
        {
            Vector2 vec = new Vector2();
            vec.X = vector.X;
            vec.Y = vector.Y;
            return vec;
        }

        private static Vector3 Vector3FromAssimp(Vector3D vector)
        { 
            Vector3 vec = new Vector3();
            vec.X = vector.X;
            vec.Y = vector.Y;
            vec.Z = vector.Z;
            return vec;
        }
    }
}
