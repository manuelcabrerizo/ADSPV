#version 330 core
out vec4 outputColor;

in vec2 texCoord;
in vec4 color;

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    outputColor = mix(mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.5), color, 0.2);
}
