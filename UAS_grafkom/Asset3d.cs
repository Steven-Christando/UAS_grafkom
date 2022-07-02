using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UAS_grafkom
{
    internal class Asset3d
    {
        public List<Vector3> _vertices = new List<Vector3>();
        private List<uint> _indices = new List<uint>();

        private Vector3 _color, _lightColor;

        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;

        Matrix4 view;
        Matrix4 projection;

        public Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();
        Vector3 objectCenter;
        public Asset3d(Vector3 color)
        {
            this._color = color;
            /*this._lightColor = lightColor;*/
        }
        public Asset3d()
        {
            _vertices = new List<Vector3>();
            //sumbu X
            _euler.Add(new Vector3(1, 0, 0));
            //sumbu y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbu z
            _euler.Add(new Vector3(0, 0, 1));
        }
        public void load(string shadervert, string shaderfrag, float sizex, float sizey)
        {
            //inisialisasi generate buffer
            _vertexBufferObject = GL.GenBuffer();               //menyimpan vertex bisa warna, texture dll untuk dikirim ke GPU (cuman dikirim)

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();           //kasih tau GPU dibaginya datanya kek gmn
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint),
                    _indices.ToArray(), BufferUsageHint.StaticDraw);
            }


            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();          //ngasih tau GPU ini mau diapain

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), sizex / (float)sizey, 0.1f, 100.0f);
        }
        public void load_normal(string shadervert, string shaderfrag, float sizex, float sizey)
        {
            //inisialisasi generate buffer
            _vertexBufferObject = GL.GenBuffer();               //menyimpan vertex bisa warna, texture dll untuk dikirim ke GPU (cuman dikirim)

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();           //kasih tau GPU dibaginya datanya kek gmn
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint),
                    _indices.ToArray(), BufferUsageHint.StaticDraw);
            }


            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();          //ngasih tau GPU ini mau diapain

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), sizex / (float)sizey, 0.1f, 100.0f);
        }

        public void setFragVariable(Vector3 lightColor, Vector3 lightPos, Vector3 viewPos)
        {
            _shader.SetVector3("ourColor", this._color);
            /*_shader.SetVector3("lightColor", lightColor);*/
            /*_shader.SetVector3("lightPos", lightPos);*/
            _shader.SetVector3("viewPos", viewPos);
        }
        public void render(int pilihan, Matrix4 temp, double time, Matrix4 cameraView, Matrix4 cameraProjection)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            //uniform untuk color
            /*setFragVariable(this._color,this._lightColor);*/
            /*_shader.SetVector3("ourColor", _color);*/
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.2f, 0.0f, 1.0f);
            //GL.Uniform4(vertexColorLocation, _color);

            /*kalo pake yang time pake yang ini
            /*Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time));*/

            /*kalo pake yang degree pake ini*/
            Matrix4 model = Matrix4.Identity;
            model = temp;
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", cameraView);
            _shader.SetMatrix4("projection", cameraProjection);


            /*if (_indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count,
                    DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (pilihan == "circle")
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (_vertices.Count + 1) / 3);
                }
                else if (pilihan == "line")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, index);
                }
                else if (pilihan == "lineBezier")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (_vertices.Count + 1) / 3);
                }
                else
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
            }*/

            if (_indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                switch (pilihan)
                {
                    case 1:
                        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count);
                        break;
                }
            }
        }

        public void createEllipsoidWireframe(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 100.0f)
            {
                for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = radiusX * MathF.Cos(v) * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * MathF.Cos(v) * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * MathF.Sin(v) + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createHyperboloidSatu(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 100.0f)
            {
                for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = radiusX * 1 / MathF.Cos(v) * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * 1 / MathF.Cos(v) * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * MathF.Tan(v) + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createHyperboloidDua(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 100.0f)
            {
                for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = radiusX * MathF.Tan(v) * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * MathF.Tan(v) * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * 1 / MathF.Cos(v) + z;
                    _vertices.Add(tempVertex);
                }
                for (float v = -MathF.PI / 2.0f; v < 3 * MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = radiusX * MathF.Tan(v) * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * MathF.Tan(v) * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * 1 / MathF.Cos(v) + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createBoxVertices(float x, float y, float z)
        {
            //biar lebih fleksibel jangan inisialiasi posisi dan 
            //panjang kotak didalam tapi ditaruh ke parameter
            float _positionX = x;
            float _positionY = y;
            float _positionZ = z;

            float _boxLength = 1.0f;

            //Buat temporary vector
            Vector3 temp_vector;
            //1. Inisialisasi vertex
            // Titik 1
            temp_vector.X = _positionX - _boxLength / 5f; // x 
            temp_vector.Y = _positionY + _boxLength / 5f; // y
            temp_vector.Z = _positionZ - _boxLength / 5f; // z

            _vertices.Add(temp_vector);

            // Titik 2
            temp_vector.X = _positionX + _boxLength / 5f; // x
            temp_vector.Y = _positionY + _boxLength / 5f; // y
            temp_vector.Z = _positionZ - _boxLength / 5f; // z

            _vertices.Add(temp_vector);
            // Titik 3
            temp_vector.X = _positionX - _boxLength / 5f; // x
            temp_vector.Y = _positionY - _boxLength / 5f; // y
            temp_vector.Z = _positionZ - _boxLength / 5f; // z
            _vertices.Add(temp_vector);

            // Titik 4
            temp_vector.X = _positionX + _boxLength / 5f; // x
            temp_vector.Y = _positionY - _boxLength / 5f; // y
            temp_vector.Z = _positionZ - _boxLength / 5f; // z

            _vertices.Add(temp_vector);

            // Titik 5
            temp_vector.X = _positionX - _boxLength / 5f; // x
            temp_vector.Y = _positionY + _boxLength / 5f; // y
            temp_vector.Z = _positionZ + _boxLength / 5f; // z

            _vertices.Add(temp_vector);

            // Titik 6
            temp_vector.X = _positionX + _boxLength / 5f; // x
            temp_vector.Y = _positionY + _boxLength / 5f; // y
            temp_vector.Z = _positionZ + _boxLength / 5f; // z

            _vertices.Add(temp_vector);

            // Titik 7
            temp_vector.X = _positionX - _boxLength / 5f; // x
            temp_vector.Y = _positionY - _boxLength / 5f; // y
            temp_vector.Z = _positionZ + _boxLength / 5f; // z

            _vertices.Add(temp_vector);

            // Titik 8
            temp_vector.X = _positionX + _boxLength / 5f; // x
            temp_vector.Y = _positionY - _boxLength / 5f; // y
            temp_vector.Z = _positionZ + _boxLength / 5f; // z

            _vertices.Add(temp_vector);
            //2. Inisialisasi index vertex
            _indices = new List<uint> {
                // Segitiga Depan 1
                0, 1, 2,
                // Segitiga Depan 2
                1, 2, 3,
                // Segitiga Atas 1
                0, 4, 5,
                // Segitiga Atas 2
                0, 1, 5,
                // Segitiga Kanan 1
                1, 3, 5,
                // Segitiga Kanan 2
                3, 5, 7,
                // Segitiga Kiri 1
                0, 2, 4,
                // Segitiga Kiri 2
                2, 4, 6,
                // Segitiga Belakang 1
                4, 5, 6,
                // Segitiga Belakang 2
                5, 6, 7,
                // Segitiga Bawah 1
                2, 3, 6,
                // Segitiga Bawah 2
                3, 6, 7
            };

        }
        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }
        public void tabung(float _positionX, float _positionY, float _positionZ, float _radiusx, float _radiusy, float _radiusz)
        {
            Vector3 temp_vector; float _pi = (float)Math.PI;
            for (float v = -20.0f; v <= _pi / 100; v += 0.01f)
            {
                for (float u = -20.0f; u <= _pi; u += (_pi / 10))
                {
                    temp_vector.X = _positionX + _radiusx * (float)Math.Cos(u); //ganti ke v jadi kumis kucing
                    temp_vector.Y = _positionY + _radiusy * (float)Math.Sin(u); //ganti ke v jadi kumis kucing
                    temp_vector.Z = _positionZ + _radiusz * v;
                    _vertices.Add(temp_vector);
                }
            }
        }
        public void createElipticCone(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 1000.0f)
            {
                for (float v = -10.0f; v < 10.0f; v += 1.0f)
                {
                    tempVertex.X = radiusX * v * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * v * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * v + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createElipticParaboloid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)

        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 1000.0f)
            {
                for (float v = 0.0f; v < 5.0f; v += 0.01f)
                {
                    tempVertex.X = radiusX * v * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * v * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * v * v + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createHyperboloidParaboloid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)

        {
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 100.0f)
            {
                for (float v = 0.0f; v < 30.0f; v += 0.1f)
                {
                    tempVertex.X = radiusX * v * MathF.Tan(u) + x;
                    tempVertex.Y = radiusY * v * 1 / MathF.Cos(v) + y;
                    tempVertex.Z = radiusZ * v * v + z;
                    _vertices.Add(tempVertex);
                }
            }
        }
        public void createBoxVertices2(float x, float y, float z, float length)
        {
            objectCenter.X = x;
            objectCenter.Y = y;
            objectCenter.Z = z;
            Vector3 temp_vector;

            //FRONT FACE

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));


            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, -1.0f));

            //BACK FACE
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 0.0f, 1.0f));

            //LEFT FACE
            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(-1.0f, 0.0f, 0.0f));

            //RIGHT FACE
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(1.0f, 0.0f, 0.0f));

            //BOTTOM FACES
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, -1.0f, 0.0f));

            //TOP FACES
            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z - length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 2.0f;
            temp_vector.Z = z + length / 2.0f;
            _vertices.Add(temp_vector);
            _vertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
        }

        public void setDirectionalLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            _shader.SetVector3("dirLight.direction", direction);
            _shader.SetVector3("dirLight.ambient", ambient);
            _shader.SetVector3("dirLight.diffuse", diffuse);
            _shader.SetVector3("dirLight.specular", specular);
        }
        public void setPointLight(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic)
        {
            _shader.SetVector3("pointLight.position", position);
            _shader.SetVector3("pointLight.ambient", ambient);
            _shader.SetVector3("pointLight.diffuse", diffuse);
            _shader.SetVector3("pointLight.specular", specular);
            _shader.SetFloat("pointLight.constant", constant);
            _shader.SetFloat("pointLight.linear", linear);
            _shader.SetFloat("pointLight.quadratic", quadratic);
        }

        public void setSpotLight(Vector3 position, Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic, float cutOff, float outerCutOff)
        {
            _shader.SetVector3("spotLight.position", position);
            _shader.SetVector3("spotLight.direction", direction);
            _shader.SetVector3("spotLight.ambient", ambient);
            _shader.SetVector3("spotLight.diffuse", diffuse);

            _shader.SetVector3("spotLight.specular", specular);
            _shader.SetFloat("spotLight.constant", constant);
            _shader.SetFloat("spotLight.linear", linear);
            _shader.SetFloat("spotLight.quadratic", quadratic);
            _shader.SetFloat("spotLight.cutOff", cutOff);
            _shader.SetFloat("spotLight.outerCutOff", outerCutOff);
        }

        public void setPointLights(Vector3[] position, Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic)
        {
            for (int i = 0; i < position.Length; i++)
            {
                _shader.SetVector3($"pointLight[{i}].position", position[i]);
                _shader.SetVector3($"pointLight[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
                _shader.SetVector3($"pointLight[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
                _shader.SetVector3($"pointLight[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
                _shader.SetFloat($"pointLight[{i}].constant", 1.0f);
                _shader.SetFloat($"pointLight[{i}].linear", 0.09f);
                _shader.SetFloat($"pointLight[{i}].quadratic", 0.032f);
            }

        }
    }
}
