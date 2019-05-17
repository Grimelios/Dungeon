#version 440 core

layout (location = 0) in vec3 vPosition;

uniform mat4 lightMatrix;

void main()
{
	gl_Position = lightMatrix * vec4(vPosition, 1);
}
