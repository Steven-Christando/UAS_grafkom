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
        List<Asset3d> collisionList = new List<Asset3d>();
        double time;
        Camera camera;

        //Asset di cafetaria
        BlenderObject try2 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try3 = new BlenderObject(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
        BlenderObject try4 = new BlenderObject(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
        BlenderObject try5 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try6 = new BlenderObject(new Vector3(125 / 255f, 125 / 255f, 12 / 255f));
        BlenderObject try7 = new BlenderObject(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
        BlenderObject try8 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try9 = new BlenderObject(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
        BlenderObject try10 = new BlenderObject(new Vector3(0 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try11 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 225 / 255f));
        BlenderObject try12 = new BlenderObject(new Vector3(0 / 255f, 0 / 255f, 225 / 255f));
        BlenderObject try13 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 225 / 255f));
        BlenderObject try30 = new BlenderObject(new Vector3(255 / 255f, 255 / 255f, 225 / 255f));

        //Asset di electrical
        BlenderObject try14 = new BlenderObject(new Vector3(255 / 255f, 255 / 255f, 255 / 255f));
        BlenderObject try15 = new BlenderObject(new Vector3(0 / 255f, 0 / 255f, 255 / 255f));
        BlenderObject try16 = new BlenderObject(new Vector3(0 / 255f, 255 / 255f, 0 / 255f));
        BlenderObject try17 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try18 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try19 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try20 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try21 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));

        //Asset di Weapon
        BlenderObject try22 = new BlenderObject(new Vector3(32 / 255f, 50 / 255f, 176 / 255f));
        BlenderObject try23 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f));
        BlenderObject try24 = new BlenderObject(new Vector3(194 / 255f, 229 / 255f, 211 / 255f));
        BlenderObject try25 = new BlenderObject(new Vector3(255 / 255f, 0 / 255f, 0 / 255f)); 
        BlenderObject try26 = new BlenderObject(new Vector3(208 / 255f, 213 / 255f, 219 / 255f));
        BlenderObject try27 = new BlenderObject(new Vector3(255 / 255f, 64 / 255f, 35 / 255f));
        BlenderObject try31 = new BlenderObject(new Vector3(255 / 255f, 64 / 255f, 35 / 255f));
        //ruangan
        BlenderObject try28 = new BlenderObject(new Vector3(170 / 255f, 240 / 255f, 209 / 255f)); 
        BlenderObject try29 = new BlenderObject(new Vector3(200 / 255f, 200 / 255f, 200 / 255f));

        private readonly Vector3[] _pointLightPositions =
        {
            new Vector3(3f,  0.6f,  0.0f),
            new Vector3(0.75f, 0.6f, 0.0f),
            new Vector3(-1.0f, 0.6f, 0.0f)
        };

        private readonly Vector4[] _collisionPosition =
        {
            //ruangan elektrik
            new Vector4(-1.0f, 0.5f, -0.53f, 0.4f),
            new Vector4(-0.85f, 0.5f, -0.53f, 0.4f),
            new Vector4(-0.685f, 0.5f, -0.53f, 0.4f),
            new Vector4(-0.65f, 0.445f, -0.85f, 0.15f),
            new Vector4(-0.5f, 0.5f, -1.05f, 0.4f),
            new Vector4(-0.3f, 0.5f, -1.05f, 0.4f),
        };

        Vector3 specularlight = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 lightColor = new Vector3(0.05f, 0.05f, 0.05f);
        Vector3 incrementLightColor = new Vector3(0.0002f, 0.0f, 0.0f);
        bool blink = false;
        Vector3 spotlightColor = new Vector3(0.0f, 0.0f, 0.0f);
        bool dark = false;

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
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            //inisialisasi camera
            camera = new Camera(new Vector3(3f, 0.475f, 0.5f), Size.X / (float)Size.Y);

            /*load asset di cafetaria*/
            try2.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/button.obj");
            try2.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try2);

            try3.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/mejabutton.obj");
            try3.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try3);

            try4.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/pizza.obj");
            try4.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try4);

            try5.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/mangkok.obj");
            try5.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try5);

            try6.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja1.obj");
            try6.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try6);

            try7.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja2.obj");
            try7.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try7);

            try8.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja3.obj");
            try8.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try8);

            try9.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/meja4.obj");
            try9.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try9);

            try10.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/laptop.obj");
            try10.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try10);

            try11.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/room_chara2.obj");
            try11.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try11);

            try12.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/room_chara.obj");
            try12.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try12);

            try13.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/room_chara2.obj");
            try13.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try13);

            try30.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetCafetaria/tulisan.obj");
            try30.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try30);

            //load assets di electrical
            try14.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/deadBody.obj");
            try14.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try14);

            try15.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelBiru.obj");
            try15.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try15);

            try16.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelHijau.obj");
            try16.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try16);

            try17.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/impostor.obj");
            try17.load((float)Size.X, (float)Size.Y);

            try18.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/kabelMerah.obj");
            try18.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try18);

            try19.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/lemari1.obj");
            try19.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try19);

            try20.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/box1.obj");
            try20.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try20);

            try21.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetElectrical/box2.obj");
            try21.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try21);

            //load ruangan di weapon
            try22.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/bulatan.obj");
            try22.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try22);

            try23.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/kursi.obj");
            try23.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try23);

            try24.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/layar.obj");
            try24.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try24);

            try25.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/pagar.obj");
            try25.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try25);

            try26.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/pembatasPagar.obj");
            try26.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try26);

            try27.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/pipa.obj");
            try27.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try27);

            try31.LoadObjFile("D:/materi/semester4/grafkom/assets/AssetWeapon/mainChara.obj");
            try31.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try31);

            //load ruangan
            try28.LoadObjFile("D:/materi/semester4/grafkom/assets/Ruangan/atap.obj");
            try28.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try28);

            try29.LoadObjFile("D:/materi/semester4/grafkom/assets/Ruangan/final.obj");
            try29.load((float)Size.X, (float)Size.Y);
            _objectList.Add(try29);

            //inisialisasi light
            for (int i = 0; i < 3; i++)
            {
                var LightObj = new Asset3d(new Vector4(1f, 1f, 1f, 1f));
                LightObj.createBoxVertices2(_pointLightPositions[i].X, _pointLightPositions[i].Y, _pointLightPositions[i].Z, 0.05f);
                _lights.Add(LightObj);
            }

            foreach (Asset3d i in _lights)
            {
                i.load_normal(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            }

            //collision object
            for (int i = 0; i < _collisionPosition.Length; i++)
            {
                var cube = new Asset3d(new Vector4(1f, 0.5f, 0.31f, 0.0f));
                cube.createBoxVertices(_collisionPosition[i].X, _collisionPosition[i].Y, _collisionPosition[i].Z, _collisionPosition[i].W);
                cube.load("D:../../../shader/shader.vert", "D:../../../shader/shader.frag", Size.X, Size.Y);
                collisionList.Add(cube);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 temp = Matrix4.Identity;
            //light render
            foreach (Asset3d i in _lights)
            {
                i.render(1, temp, time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }

            //collision render
            /*foreach (Asset3d j in collisionList)
            {
                j.render(1, temp, time, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }*/

            if (blink && (lightColor.X < 0.0f || lightColor.X > 0.2f))
            {
                incrementLightColor *= new Vector3(-1.0f, 0.0f, 0.0f);
            }

            foreach (BlenderObject i in _objectList)
            {
                //Vector3 lightColor = new Vector3(0.1f, 0.1f, 0.1f);
                if (blink)
                {
                    lightColor += incrementLightColor;
                }                

                i.setDirectionalLight(new Vector3(0.2f, 1.0f, 0f), lightColor, new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                i.setSpotLight(camera.Position, camera.Front, new Vector3(0.0f, 0.0f, 0.0f), spotlightColor, new Vector3(1.0f, 1.0f, 1.0f),
                    1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
                i.setPointLights(_pointLightPositions, lightColor, new Vector3(0.3f, 0.3f, 0.3f), specularlight, 1.0f, 0.09f, 0.032f);
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
                Vector3 newPos = camera.Position + camera.Front * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }

            }
            if (input.IsKeyDown(Keys.A))
            {
                Vector3 newPos = camera.Position - camera.Right * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }

            }
            if (input.IsKeyDown(Keys.S))
            {
                Vector3 newPos = camera.Position - camera.Front * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }
            }
            if (input.IsKeyDown(Keys.D))
            {
                Vector3 newPos = camera.Position + camera.Right * cameraSpeed * (float)args.Time;
                if (!Collision(newPos))
                {
                    camera.Position = newPos;
                }
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
                Vector3 temp = -camera.Up * cameraSpeed * (float)args.Time;
                try31.translate(temp.X, temp.Y, temp.Z);
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

            if (input.IsKeyReleased(Keys.P))
            {
                if (blink)
                {
                    blink = false;
                    lightColor = new Vector3(0.05f, 0.05f, 0.05f);
                }
                else
                {
                    blink = true;
                    lightColor = new Vector3(0.0f, 0.0f, 0.0f);
                    incrementLightColor = new Vector3(0.0002f, 0.0f, 0.0f);
                }
            }

            if (input.IsKeyReleased(Keys.O))
            {
                if (dark)
                {
                    dark = false;
                    lightColor = new Vector3(0.05f, 0.05f, 0.05f);
                    specularlight = new Vector3(1.0f, 1.0f, 1.0f);
                    spotlightColor = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    dark = true;
                    lightColor = new Vector3(0.0f, 0.0f, 0.0f);
                    specularlight = new Vector3(0.0f, 0.0f, 0.0f);
                    spotlightColor = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }

            if (input.IsKeyReleased(Keys.I))
            {
                _objectList.Add(try17);
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            camera.Fov = camera.Fov - e.OffsetY;
            Console.WriteLine(e.OffsetY);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            camera.AspectRatio = Size.X / (float)Size.Y;
        }

        private bool Collision(Vector3 newPos)
        {
            foreach(Asset3d obj in collisionList)
            {
                float distance = Vector3.Distance(newPos, obj._centerPosition);
                if(distance < obj.lengthBox/4)
                {
                    return true;
                }
            }
            //|| newPos.Y > 0.485f
            if (newPos.Y < 0.475f || newPos.Y > 0.52f)
            {
                return true;
            }

            return false;
        }
    }
}
