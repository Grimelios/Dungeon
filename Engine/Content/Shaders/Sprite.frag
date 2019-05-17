#version 440 core

in vec2 fTexCoords;
in vec4 fColor;

out vec4 fragColor;

uniform sampler2D image;

void main()
{
	fragColor = fColor * texture(image, fTexCoords);
}
