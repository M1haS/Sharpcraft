#version 460 core
layout(location = 0) in vec3 aPosition;
out vec4 vertexColor;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
    vertexColor = vec4(aPosition, 1.0);
}