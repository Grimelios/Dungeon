#version 440 core

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec4 vColor;

out vec4 fColor;

uniform mat4 mvp;

void main()
{
	gl_Position = mvp * vec4(vPosition, 1);

	fColor = vColor;
}
