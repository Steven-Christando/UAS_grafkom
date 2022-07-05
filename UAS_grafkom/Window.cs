using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace UAS_grafkom
{
    static class Constants
    {
        public const string path = "D:../../../shader/";
    }
    internal class Window : GameWindow
    {
        List<BlenderObject> _objectList = new List<BlenderObject>();
        List<Asset3d> _lights = new List<Asset3d>();
        List<Asset3d> coba = new List<Asset3d>();
        double time;
        Camera camera;

        BlenderObject try2 = new BlenderObject();
        BlenderObject try3 = new BlenderObject();
        BlenderObject try4 = new BlenderObject();
        BlenderObject try5 = new BlenderObject();
        BlenderObject try6 = new BlenderObject();
        BlenderObject try7 = new BlenderObject();
        BlenderObject try8 = new BlenderObject();
        BlenderObject try9 = new BlenderObject();
        BlenderObject try10 = new BlenderObject();
        BlenderObject try19 = new BlenderObject();
        BlenderObject try20 = new BlenderObject();
        BlenderObject try21 = new BlenderObject();

        BlenderObject try11 = new BlenderObject();
        BlenderObject try12 = new BlenderObject();
        BlenderObject try13 = new BlenderObject();
        BlenderObject try14 = new BlenderObject();
        BlenderObject try15 = new BlenderObject();
        BlenderObject try16 = new BlenderObject();
        BlenderObject try17 = new BlenderObject();
        BlenderObject try18 = new BlenderObject();
        BlenderObject try22 = new BlenderObject();
        BlenderObject try23 = new BlenderObject();
        /*private readonly Vector3[] _cobaPositions =
        {
            new Vector3(1.0f, 0.5f, 0f),
            new Vector3(1.0f, -1.5f, 0f),
        };*/
        private readonly Vector3[] _pointLightPositions =
        {
            /*new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)*/
            new Vector3(0.75f, 1f, 0.0f),
            new Vector3(-1.0f, 1f, 0.0f)
        };

        private readonly Vector3[] _cameraBoundary =
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 1.0f)
        };

        Asset3d LightObject;

        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //background color
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            //inisialisasi camera
            camera = new Camera(new Vector3(0.5f, 0.475f, 0.5f), Size.X / (float)Size.Y);
            try2.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/button.obj");
            try2.load((float)Size.X, (float)Size.Y, "lighting");
            try2.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try2);

            try3.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/mejabutton.obj");
            try3.load((float)Size.X, (float)Size.Y, "lighting");
            try3.setColor(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
            _objectList.Add(try3);

            try4.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/pizza.obj");
            try4.load((float)Size.X, (float)Size.Y, "lighting");
            try4.setColor(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
            _objectList.Add(try4);

            try5.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/mangkok.obj");
            try5.load((float)Size.X, (float)Size.Y, "lighting");
            try5.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try5);

            try6.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja1.obj");
            try6.load((float)Size.X, (float)Size.Y, "lighting");
            try6.setColor(new Vector3(125 / 255f, 125 / 255f, 12 / 255f));
            _objectList.Add(try6);

            try7.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja2.obj");
            try7.load((float)Size.X, (float)Size.Y, "lighting");
            try7.setColor(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
            _objectList.Add(try7);

            try8.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja3.obj");
            try8.load((float)Size.X, (float)Size.Y, "lighting");
            try8.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try8);

            try9.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja4.obj");
            try9.load((float)Size.X, (float)Size.Y, "lighting");
            try9.setColor(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
            _objectList.Add(try9);

            try10.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/laptop.obj");
            try10.load((float)Size.X, (float)Size.Y, "lighting");
            try10.setColor(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
            _objectList.Add(try10);

            try11.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelBiru.obj");
            try11.load((float)Size.X, (float)Size.Y, "lighting");
            try11.setColor(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
            _objectList.Add(try11);

            try12.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelHitam.obj");
            try12.load((float)Size.X, (float)Size.Y, "lighting");
            try12.setColor(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
            _objectList.Add(try12);

            try13.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelKuning.obj");
            try13.load((float)Size.X, (float)Size.Y, "lighting");
            try13.setColor(new Vector3(125 / 255f, 125 / 255f, 125 / 255f));
            _objectList.Add(try13);

            try14.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelMerah.obj");
            try14.load((float)Size.X, (float)Size.Y, "lighting");
            try14.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try14);

            try15.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/lemari1.obj");
            try15.load((float)Size.X, (float)Size.Y, "lighting");
            try15.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try15);

            try16.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/box1.obj");
            try16.load((float)Size.X, (float)Size.Y, "lighting");
            try16.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try16);

            try17.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/box2.obj");
            try17.load((float)Size.X, (float)Size.Y, "lighting");
            try17.setColor(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
            _objectList.Add(try17);

            try18.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/ruangan3.obj");
            try18.load((float)Size.X, (float)Size.Y, "lighting");
            try18.setColor(new Vector3(200 / 255f, 200 / 255f, 200 / 255f));
            _objectList.Add(try18);

            try21.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/omongus0001.obj");
            try21.load((float)Size.X, (float)Size.Y, "lighting");
            try21.setColor(new Vector3(0 / 255f, 0 / 255f, 225 / 255f));
            try21.scale(0.05f);
            try21.translate(0.5f, 0.425f, 0.25f);
            _objectList.Add(try21);

            try22.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/bone.obj");
            try22.load((float)Size.X, (float)Size.Y, "lighting",1);
            try22.setColor(new Vector3(255 / 255f, 255 / 255f, 255 / 255f));
            _objectList.Add(try22);

            try23.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/badan.obj");
            try23.load((float)Size.X, (float)Size.Y, "lighting",1);
            try23.setColor(new Vector3(0 / 255f, 0 / 255f, 125 / 255f));
            _objectList.Add(try23);
            /*try2.scale(0.5f);*/

            //inisialisasi benda
            //inisialisasi light
            for (int i = 0; i < 2; i++)
            {
                var LightObj = new Asset3d(new Vector3(1, 0.5f, 0.31f));
                LightObj.createBoxVertices2(_pointLightPositions[i].X, _pointLightPositions[i].Y, _pointLightPositions[i].Z, 0.5f);
                _lights.Add(LightObj);
            }

            foreach (Asset3d i in _lights)
            {
                i.load_normal(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            }
            foreach (Asset3d i in coba)
            {
                i.load_normal("D:../../../shader/object.vert", "D:../../../shader/object.frag", Size.X, Size.Y);
            }
            //insialisasi

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 temp = Matrix4.Identity;
            foreach (Asset3d i in _lights)
            {
                i.render(1, temp, time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }
            foreach (BlenderObject i in _objectList)
            {
                /*i.setFragVariable(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.2f, 1.0f, 2.0f), camera.Position);*/
                i.setDirectionalLight(new Vector3(0.2f, 1.0f, 0f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                /*i.setSpotLight(new Vector3(0, 5, 0), new Vector3(0, -1, 0), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));*/
                i.setPointLights(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
                i.render(camera, new Vector3(1, 1, 1), new Vector3(0.75f, 1, 0));
            }

            SwapBuffers();

        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;
            float cameraSpeed = 0.5f;

            /*Console.WriteLine(camera.Position);*/

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (input.IsKeyDown(Keys.W))
            {
                /*camera.Position += camera.Front * cameraSpeed * (float)args.Time;*/
                Vector3 newPos = camera.Position + camera.Front * cameraSpeed * (float)args.Time;
                camera.Position = newPos;
                
            }
            if (input.IsKeyDown(Keys.A))
            {
                Vector3 newPos = camera.Position - camera.Right * cameraSpeed * (float)args.Time;
                camera.Position = newPos;

            }
            if (input.IsKeyDown(Keys.S))
            {
                Vector3 newPos = camera.Position - camera.Front * cameraSpeed * (float)args.Time;
                camera.Position = newPos;
            }
            if (input.IsKeyDown(Keys.D))
            {
                Vector3 newPos = camera.Position + camera.Right * cameraSpeed * (float)args.Time;
                camera.Position = newPos;
            }
            if (input.IsKeyDown(Keys.Space))
            {
                Vector3 newPos = camera.Position + camera.Up * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }
            }
            if (input.IsKeyDown(Keys.LeftControl))
            {
                Vector3 newPos = camera.Position - camera.Up * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }
            }

            if (input.IsKeyDown(Keys.Right))
            {
                camera.Yaw += cameraSpeed * (float)args.Time * 50.0f;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                camera.Yaw -= cameraSpeed * (float)args.Time * 50.0f;
            }
            if (input.IsKeyDown(Keys.Up))
            {
                camera.Pitch += cameraSpeed * (float)args.Time * 30.0f;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                camera.Pitch -= cameraSpeed * (float)args.Time * 30.0f;
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            camera.Fov = camera.Fov - e.OffsetY;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            Console.WriteLine("halo");
            GL.Viewport(0, 0, Size.X, Size.Y);
            camera.AspectRatio = Size.X / (float)Size.Y;
        }

        private bool Collision(Vector3 newPos)
        {
            if (newPos.Y < 0.475f || newPos.Y > 0.6f)
            {
                return true;
            }

            return false;
        }
    }
}
