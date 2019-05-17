#version 440 core

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 boneWeights;
layout (location = 2) in ivec2 boneIndexes;
layout (location = 3) in int boneCount;

uniform mat4 lightMatrix;
uniform mat4 bones[2];

void main()
{
	vec4 position = lightMatrix * vec4(vPosition, 1);

	for (int i = 0; i < boneCount; i++)
	{
		int index = boneIndexes[i];

		if (index == -1)
		{
			break;
		}

		position *= bones[index] * boneWeights[i];
	}

//	position.x *= boneWeights.x;
//	position.x *= boneIndexes.x + 1;
//	position.x *= boneCount;

	gl_Position = position;
}
