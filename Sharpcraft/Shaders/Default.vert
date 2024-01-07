#version 460 core
layout (location = 0) in vec3 aPosition; // Vertex coordinates
layout (location = 1) in vec2 aTexCoord; // Texture coordinates

out vec2 texCoord;

void main()
{
    gl_Position = vec4(aPosition, 1.0);  // Coordinates
    texCoord = aTexCoord;
}