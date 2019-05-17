#version 440 core

in vec2 fTexCoords;
in vec4 fColor;

out vec4 fragColor;

uniform sampler2D shadowMap;

void main()
{
	float depth = texture(shadowMap, fTexCoords).r;

	fragColor = fColor * vec4(vec3(depth), 1);
}
