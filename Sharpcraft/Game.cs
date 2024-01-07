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
    }
}
