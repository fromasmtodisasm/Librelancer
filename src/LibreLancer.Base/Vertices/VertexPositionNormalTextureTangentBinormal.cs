using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;

namespace LibreLancer.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTextureTangentBinormal
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector3 Tangent;
        public Vector3 Binormal;

        public VertexPositionNormalTextureTangentBinormal(BinaryReader reader)
            : this()
        {
            this.Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            this.Normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.TextureCoordinate = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());
            this.Tangent = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            this.Binormal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

    }
}