using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Sharpcraft_Mineecraft_Clone_OpenTK
{
    internal class Game : GameWindow
    {
        private float[] Vertices =
        {
            -0.5f,   0.5f,  0f, // Top left     - 0
             0.5f,   0.5f,  0f, // Top right    - 1
             0.5f,  -0.5f,  0f, // Bottom left  - 2
            -0.5f,  -0.5f,  0f  // Bottom right - 3
        };

        private float[] TexCoords =
        {
            0f, 1f,
            1f, 1f,
            1f, 0f,
            1f, 0f
        };

        private uint[] Indices =
        {
            // Top triangles
            0, 1, 2,
            // Bottom trinagle
            2, 3, 0
        };

        // Render pipelines
        int vao;
        int vbo;
        int shaderProgram;
        int ebo;
        int textureVBO;
        int textureID;

        // Constants
        private int Width, Height;

        public Game(int width, int height)
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.Width = width;
            this.Height = height;
            
            // Center window on monitor
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
            this.Width = e.Width;
            this.Height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();

            // Bind the vao
            GL.BindVertexArray(vao);

            // -- Vertices VBO ---

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(
                BufferTarget.ArrayBuffer, Vertices.Length*sizeof(float), 
                Vertices, BufferUsageHint.StaticDraw
            );

            // Put the vertex VBO in slot 0

            // Point slot (0) of the VAO to the currently bound VBO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // --- Texture VBO ---
            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(
                BufferTarget.ArrayBuffer, TexCoords.Length * sizeof(float),
                TexCoords, BufferUsageHint.StaticDraw
            );

            // Put the texture VBO in slot 1

            // Point slot (1) of the VAO to the currently bound VBO
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 1);

            GL.EnableVertexArrayAttrib(vao, 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);     // Unbind the vbo
            GL.BindVertexArray(0);


            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer, Indices.Length*sizeof(uint), 
                Indices, BufferUsageHint.StaticDraw
            );
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            // Creates the shader
            shaderProgram = GL.CreateProgram();
            
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            // Delete the shadrs
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // --- TEXTURES ---
            textureID = GL.GenTexture();
            // Active the texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            // Texture parameters
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest
            );
            GL.TexParameter(
                TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMinFilter.Nearest
            );

            // Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(
                File.OpenRead("../../../Textures/grassTex.png"), ColorComponents.RedGreenBlueAlpha
            );
            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, dirtTexture.Data
            );

            // Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteTexture(textureID);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Draw
            GL.UseProgram(shaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using StreamReader reader = new StreamReader("../../../Shaders/" + filePath);
                shaderSource = reader.ReadToEnd();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Filed to load shader source file: " + ex.Message);
            }

            return shaderSource;
        }
    }
}
