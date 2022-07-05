#version 330 core

layout(location = 0) in vec3 aPosition;
//menyediakan variabel yang bisa dikirim ke next step -> .frag
layout(location = 1) in vec3 aColor;

out vec3 vertexPosition;
out vec3 normal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void){
	gl_Position = vec4(aPosition,1.0)*model*view*projection;
	vertexPosition = vec3(vec4(aPosition,1.0)*model);
	normal = aColor * mat3(transpose(inverse(model)));
}