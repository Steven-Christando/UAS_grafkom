//#version 330

//out vec4 color;

//in vec4 vertexColor;

//uniform vec4 colorColor;

//void main () {
	//color = vertexColor;
	//color = vec4(1.0,1.0,1.0,1.0); 
//}
#version 330

out vec4 outputColor;

in vec4 vertexColor;

//uniform vec4 ourColor;

uniform vec3 ourColor;
uniform vec3 lightColor;
void main()
{
// outputColor = vec4(1.0, 1.0, 1.0, 1.0);  //white color
 //outputColor = vertexColor;  //white color
 //outputColor = ourColor;  //white color
 outputColor = vec4(1.0,1.0,1.0, 1.0);
 //outputColor = vec4(ourColor*lightColor,1.0);

 //float ambientStrength = 0.1f;
 //vec3 ambient = ambientStrength*lightColor;

 //vec3 result = ambient*ourColor;
 //outputColor = vec4(result,1.0f);
}

//#version 330

//out vec4 outputColor;

//void main()
//{
   // outputColor = vec4(1.0, 1.0, 1.0, 0.0);
//}