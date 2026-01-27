#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aCol; 
layout (location = 2) in vec2 aUvs; 
layout (location = 3) in float aTex; 

out vec2 texCoord;
out vec3 color;

layout (std140) uniform PerPass
{
    mat4 View;
    mat4 Proj;
};

void main()
{
    texCoord = aUvs;
    color = aCol;
    gl_Position =  vec4(aPos, 1.0) * View * Proj;
}
