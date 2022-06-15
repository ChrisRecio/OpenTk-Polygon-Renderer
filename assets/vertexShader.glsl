#version 330 core

uniform vec2 viewportSize;
uniform float colorFactor;

//uniform mat4 transformationMatrix;

layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec4 aColor;

out vec4 vColor;
                
void main() 
{
    // Normalize x,y coords
    float nx = aPosition.x / viewportSize.x * 2f - 1f;
    float ny = aPosition.y / viewportSize.y * 2f - 1f;
    gl_Position = vec4(nx, ny, 0f, 1f);

    //gl_Position = vec4(nx, ny, 0f, 1f) * transformationMatrix;

    vColor = aColor * colorFactor;
}