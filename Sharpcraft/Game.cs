using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
             0f,     0.5f,  0f, // Top vertex
            -0.5f,  -0.5f,  0f, // Bottom left
             0.5f,  -0.5f,  0f  // Bottom right
        };

        // Render pipelines
        int vertexArraysObject;
        int shaderProgram;

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

            vertexArraysObject = GL.GenVertexArray();
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            GL.BufferData(
                BufferTarget.ArrayBuffer, Vertices.Length*sizeof(float), 
                Vertices, BufferUsageHint.StaticDraw
            );

            // Bind the vertexArraysObject
            GL.BindVertexArray(vertexArraysObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vertexArraysObject, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);     // Unbind the vertexBufferObject
            GL.BindVertexArray(0);

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
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vertexArraysObject);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Draw our triangle 
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vertexArraysObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
