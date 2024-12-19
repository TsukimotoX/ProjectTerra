#version 330 core
layout(location = 0) in vec3 Position;

void main(){
    gl_Position = vec4(Position, 1.0);
}

<split>

#version 330 core
out vec4 FragmentColor;

void main(){
    FragmentColor = vec4(1.0, 0.5, 0.2, 1.0);
}