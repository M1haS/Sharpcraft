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

        // Set of vertices to draw the triangle with (x,y,z) for each vertex
        List<Vector3> vertices = new List<Vector3>()
        {
            // Front face
            new Vector3(-0.5f,  0.5f,  0.5f), // Top left vert
            new Vector3( 0.5f,  0.5f,  0.5f), // Top right vert
            new Vector3( 0.5f, -0.5f,  0.5f), // Bottom right vert
            new Vector3(-0.5f, -0.5f,  0.5f), // Bottom left vert
            // Right face
            new Vector3(0.5f,   0.5f,  0.5f), // Top left vert
            new Vector3(0.5f,   0.5f, -0.5f), // Top right vert
            new Vector3(0.5f,  -0.5f, -0.5f), // Bottom right vert
            new Vector3(0.5f,  -0.5f,  0.5f), // Bottom left vert
            // Back face
            new Vector3( 0.5f,  0.5f, -0.5f), // Top left vert
            new Vector3(-0.5f,  0.5f, -0.5f), // Top right vert
            new Vector3(-0.5f, -0.5f, -0.5f), // Bottom right vert
            new Vector3( 0.5f, -0.5f, -0.5f), // Bottom left vert
            // Left face
            new Vector3(-0.5f,  0.5f, -0.5f), // Topleft vert
            new Vector3(-0.5f,  0.5f,  0.5f), // Topright vert
            new Vector3(-0.5f, -0.5f,  0.5f), // Bottom right vert
            new Vector3(-0.5f, -0.5f, -0.5f), // Bottom left vert
            // Top face
            new Vector3(-0.5f,  0.5f, -0.5f), // Top left vert
            new Vector3(0.5f,   0.5f, -0.5f), // Top right vert
            new Vector3(0.5f,   0.5f,  0.5f), // Bottom right vert
            new Vector3(-0.5f,  0.5f,  0.5f), // Bottom left vert
            // Bottom face
            new Vector3(-0.5f, -0.5f,  0.5f), // Top left vert
            new Vector3( 0.5f, -0.5f,  0.5f), // Top right vert
            new Vector3( 0.5f, -0.5f, -0.5f), // Bottom right vert
            new Vector3(-0.5f, -0.5f, -0.5f), // Bottom left vert
        };

        List<Vector2> texCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        uint[] indices =
        {
            0, 1, 2,
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };

        // Render Pipeline vars
        int vao;
        int shaderProgram;
        int vbo;
        int textureVBO;
        int ebo;
        int textureID;


        // Transformation variables
        float yRot = 0f;

        // Width and height of screen
        int width, height;
        
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            // Center window
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
             
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Generate the vbo
            vao = GL.GenVertexArray();

            // Bind the vao
            GL.BindVertexArray(vao);

            // --- Vertices VBO ---

            // Generate a buffer
            vbo = GL.GenBuffer();
            // Bind the buffer as an array buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            // Store data in the vbo
            GL.BufferData(
                BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(), BufferUsageHint.StaticDraw
            );


            // Put the vertex VBO in slot 0 of our VAO

            // Point slot (0) of the VAO to the currently bound VBO (vbo)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            // Enable the slot
            GL.EnableVertexArrayAttrib(vao, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // --- Texture VBO ---

            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(
                BufferTarget.ArrayBuffer, texCoords.Count * Vector2.SizeInBytes,
                texCoords.ToArray(), BufferUsageHint.StaticDraw
            );


            // Put the texture VBO in slot 1 of our VAO

            // Point slot (1) of the VAO to the currently bound VBO (vbo)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            // Enable the slot
            GL.EnableVertexArrayAttrib(vao, 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vbo and vao respectively

            GL.BindVertexArray(0);


            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint),
                indices, BufferUsageHint.StaticDraw
            );
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            // Create the shader program
            shaderProgram = GL.CreateProgram();

            // Create the vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            // Add the source code from "Default.vert" in the Shaders file
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            // Compile the Shader
            GL.CompileShader(vertexShader);

            // Same as vertex shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            // Attach the shaders to the shader program
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            // Link the program to OpenGL
            GL.LinkProgram(shaderProgram);

            // Delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // --- TEXTURES ---
            textureID = GL.GenTexture();
            // Activate the texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            // Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(
                File.OpenRead("../../../Textures/dirtTex.PNG"), ColorComponents.RedGreenBlueAlpha
            );

            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, 
                dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data
            );

            // Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete, VAO, VBO, Shader Program
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteTexture(textureID);
            GL.DeleteProgram(shaderProgram);
        }
        // called every frame. All rendering happens here
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Set the color to fill the screen with
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            // Fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // draw our triangle
            GL.UseProgram(shaderProgram); // bind vao
            GL.BindVertexArray(vao);      // use shader program
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindTexture(TextureTarget.Texture2D, textureID);


            // Rransformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.Identity;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60.0f), width / height, 0.1f, 100.0f
            );


            model = Matrix4.CreateRotationY(yRot);
            yRot += 0.001f;

            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            // Swap the buffers
            Context.SwapBuffers();

            base.OnRenderFrame(args);
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
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load shader source file: " + ex.Message);
            }

            return shaderSource;
        }
    }
}
