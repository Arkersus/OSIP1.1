﻿using OpenTK;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OSIP1._1
{
    class GraphicsLab
    {
        private Configuration config;
        private Symbol symbImage;
        private GameWindow window;
        private List<int[]> strongComps;
        int scale;

        public GraphicsLab(Configuration cfg)
        {
            config = cfg;
            scale = cfg.gridScale;
        }

        private GameWindow GetWindow(int width, int height)
        {
            var win = new GameWindow(width, height);

            win.Load += Window_Load;
            win.RenderFrame += Window_RenderFrame;
            win.KeyPress += Window_KeyPress;

            return win;
        }

        private void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                symbImage.LocalizeRecSet(2);
                scale *= 2;

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0.0f, scale, scale, 0.0f, 0.0f, 1.0f);
            }
        }

        private void Window_Load(object sender, EventArgs e)
        {
            GL.ClearColor(new Color4(1.0f, 1.0f, 1.0f, 1.0f));

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Ortho(0.0f, window.Width, window.Height, 0.0f, 0.0f, 1.0f);
            GL.Ortho(0.0f, scale, scale, 0.0f, 0.0f, 1.0f);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Render();

            GL.Flush();

            window.SwapBuffers();
        }

        private void Render()
        {
            DrawCoords();

            foreach (int[] i in strongComps)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.Black);

                foreach (int j in i)
                    DrawCell(j);
                GL.End();
            }
        }

        private void DrawCoords()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Black);
            GL.Vertex2(0, scale / 2);
            GL.Vertex2(scale, scale / 2);
            GL.Vertex2(scale / 2, 0);
            GL.Vertex2(scale / 2, scale);
            GL.End();
        }
        private void DrawCell(int n)
        {
            int x = n % scale;
            int y = n / scale;

            GL.Vertex2(x, y);
            GL.Vertex2(x, y+1);
            GL.Vertex2(x+1, y+1);
            GL.Vertex2(x+1, y);
        }
        public void Run()
        {
            Symbol symb = new Symbol(config);
            strongComps = symb.GetStrongComps();
            symbImage = symb;

            window = GetWindow(1000, 1000);
            window.Run();


        }
    }
}
