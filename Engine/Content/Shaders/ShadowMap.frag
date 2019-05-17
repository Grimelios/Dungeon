#version 440 core

out float fragDepth;

void main()
{
	fragDepth = gl_FragCoord.z;
}
