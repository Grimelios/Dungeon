#version 440 core

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 vSource;
layout (location = 2) in vec3 vNormal;

out vec2 fSource;
out vec3 fNormal;
out vec4 fShadowMapCoords;

uniform mat4 orientation;
uniform mat4 mvp;
uniform mat4 lightBiasMatrix;

void main()
{
	vec4 position = vec4(vPosition, 1);

	gl_Position = mvp * position;
	
	fSource = vSource;
	fNormal = (orientation * vec4(vNormal, 1)).xyz;
	fShadowMapCoords = lightBiasMatrix * position;
}
