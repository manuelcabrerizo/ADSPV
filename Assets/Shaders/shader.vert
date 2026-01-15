#version 330 core
layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;
out vec4 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

layout (std140) uniform PerPass
{
    mat4 View;
    mat4 Proj;
};

layout (std140) uniform PerDraw
{
    mat4 World;
    vec4 Color;
};

void main()
{
    texCoord = aTexCoord;
    color = Color;
    //gl_Position =  vec4(aPosition, 1.0) * model * view * proj;
    gl_Position =  vec4(aPosition, 1.0) * World * View * Proj;
}
