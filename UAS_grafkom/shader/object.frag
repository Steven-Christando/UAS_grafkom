#version 330

out vec4 outputColor;

in vec4 vertexColor;
in vec3 normal;

uniform vec3 ourColor;
//uniform vec3 lightColor;
//uniform vec3 lightPos;
uniform vec3 viewPos;

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform DirLight dirLight;

struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
//uniform PointLight pointLight;
#define NR_POINT_LIGHTS 4
uniform PointLight pointLight[NR_POINT_LIGHTS];
struct SpotLight{
    vec3  position;
    vec3  direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
uniform SpotLight spotLight;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
void main()
{
 //float ambientStrength = 0.1f;
 //vec3 ambient = ambientStrength*lightColor;

 //vec3 result = ambient*ourColor;
 //outputColor = vec4(result,1.0f);

 //vec3 norm = normalize(normal);
 //vec3 lightDir = normalize(lightPos - vec3(vertexColor));

 //float diff = max(dot(norm,lightDir),0.0);
 //vec3 difuse = diff*lightColor;
 
 //float specularStrength = 0.5f;
 //vec3 viewDir = normalize(viewPos - vec3(vertexColor));
 //vec3 reflectDir = reflect(-lightDir,norm);
 //float spec = pow(max(dot(viewDir,reflectDir),0.0),50); 
 //vec3 specular = specularStrength * spec * lightColor;
 //vec3 result = (ambient+difuse+specular) * ourColor;
 //outputColor = vec4(result,1.0f);
 
 //vec3 result = (ambient+difuse) * ourColor;
 //outputColor = vec4(result,1.0f);
 //outputColor = vec4(1.0, 1.0, 1.0, 0.0);
 //outputColor = vec4(ourColor*lightColor,1.0);
 vec3 norm = normalize(normal);
 vec3 viewDir = normalize(viewPos - vec3(vertexColor));
 //vec3 result = CalcDirLight(dirLight,norm,viewDir);
 //vec3 result = CalcPointLight(pointLight,norm,vec3(vertexColor),viewDir);
 vec3 result = CalcDirLight(dirLight,norm,viewDir);
 for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLight[i], norm, vec3(vertexColor), viewDir);
    result += CalcSpotLight(spotLight, norm, vec3(vertexColor), viewDir);
 //vec3 result += CalcSpotLight(spotLight,norm,vec3(vertexColor),viewDir);
 outputColor = vec4(result,1.0f);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-light.direction);
    //diffuse shading
    float diff = max(dot(normal, lightDir),0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir,normal);
    float spec = pow(max(dot(viewDir, reflectDir),0.0),2);
    //combine result
    vec3 ambient = light.ambient * ourColor;
    vec3 diffuse = light.diffuse * diff * ourColor;
    vec3 specular = light.specular * spec *ourColor;
    return (ambient+diffuse+specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0),256);
    //attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));
    //combine results
    vec3 ambient  = light.ambient  * ourColor;
    vec3 diffuse  = light.diffuse  * diff * ourColor;
    vec3 specular = light.specular * spec * ourColor;
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{

    //diffuse shading
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(normal, lightDir), 0.0);

    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 256);

        //attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    //spotlight intensity
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    //combine results
    vec3 ambient = light.ambient * ourColor;
    vec3 diffuse = light.diffuse * diff * ourColor;
    vec3 specular = light.specular * spec * ourColor;
    ambient  *= attenuation;
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);
}
