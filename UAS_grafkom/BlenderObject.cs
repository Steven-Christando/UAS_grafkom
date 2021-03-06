using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using LearnOpenTK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace UAS_grafkom
{
    internal class BlenderObject
    {
        //Vector 3 pastikan menggunakan OpenTK.Mathematics
        //tanpa protected otomatis komputer menganggap sebagai private
        List<float> realVertices = new List<float>();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> textureVertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        Vector3 _color;
        int _vertexBufferObject;
        int _vertexArrayObject;
        Shader _shader;
        Matrix4 model;
        Matrix4 view;
        Matrix4 projection;

        //menambahkan hirarki kedalam parent
        public List<BlenderObject> child = new List<BlenderObject>();
        public BlenderObject(Vector3 colors)
        {
            _color = colors;
        }


        
        public void load(float Sizex, float Sizey)
        {
            model = Matrix4.Identity;

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.BufferData<float>(BufferTarget.ArrayBuffer,realVertices.Count * sizeof(float),realVertices.ToArray(),BufferUsageHint.StaticDraw);
            _shader = new Shader("../../../shader/object.vert", "../../../shader/object.frag");

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                             
            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Sizex / Sizey, 0.1f, 100.0f);

        }
        public void render(Camera _camera, Vector3 _lightColor, Vector3 _lightPos)
        {
            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _shader.SetVector3("objectColor", this._color);
            _shader.SetVector3("viewPos", _camera.Position);

            GL.DrawArrays(PrimitiveType.Triangles, 0, realVertices.Count);

        }
      
        public void setDirectionalLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            _shader.SetVector3("dirLight.direction", direction);
            _shader.SetVector3("dirLight.ambient", ambient);
            _shader.SetVector3("dirLight.diffuse", diffuse);
            _shader.SetVector3("dirLight.specular", specular);
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
                _shader.SetVector3($"pointLight[{i}].ambient", ambient);
                _shader.SetVector3($"pointLight[{i}].diffuse", diffuse);
                _shader.SetVector3($"pointLight[{i}].specular", specular);
                _shader.SetFloat($"pointLight[{i}].constant", 1.0f);
                _shader.SetFloat($"pointLight[{i}].linear", 0.09f);
                _shader.SetFloat($"pointLight[{i}].quadratic", 0.032f);
            }

        }

        public void LoadObjFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }
            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    
                    List<string> words = new List<string>(streamReader.ReadLine().Split(' '));
                    words.RemoveAll(s => s == string.Empty);
                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);
                    if(type == "v")
                    {
                        vertices.Add(new Vector3(float.Parse(words[0]) / 10, float.Parse(words[1]) / 10, float.Parse(words[2]) / 10));
                        //Console.WriteLine(words[0]);
                        
                    } else if (type == "vn")
                    {
                        normals.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2])));
                        
                    } else if(type == "f")
                    {
                        foreach (string w in words)
                        {
                            if (w.Length == 0)
                                continue;

                            string[] comps = w.Split('/');


                            //vertices
                            realVertices.Add(vertices[int.Parse(comps[0]) - 1].X);
                            realVertices.Add(vertices[int.Parse(comps[0]) - 1].Y);
                            realVertices.Add(vertices[int.Parse(comps[0]) - 1].Z);
                            
                            //normal
                            realVertices.Add(normals[int.Parse(comps[2]) - 1].X);
                            realVertices.Add(normals[int.Parse(comps[2]) - 1].Y);
                            realVertices.Add(normals[int.Parse(comps[2]) - 1].Z);
                        }
                    }
                }
            }
        }
    }
}