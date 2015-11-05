// Shader created with Shader Forge v1.17 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.17;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:33087,y:32665,varname:node_4013,prsc:2|diff-9450-RGB,voffset-105-OUT;n:type:ShaderForge.SFN_TexCoord,id:9699,x:31820,y:33443,varname:node_9699,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:8444,x:31888,y:32802,varname:node_8444,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6175-Y;n:type:ShaderForge.SFN_Vector1,id:1756,x:32110,y:32802,varname:node_1756,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:3146,x:32316,y:32802,varname:node_3146,prsc:2|A-1756-OUT,B-3224-OUT,C-7339-OUT;n:type:ShaderForge.SFN_Tau,id:7339,x:32072,y:33141,varname:node_7339,prsc:2;n:type:ShaderForge.SFN_Sin,id:2861,x:32314,y:33099,varname:node_2861,prsc:2|IN-3146-OUT;n:type:ShaderForge.SFN_Clamp01,id:2257,x:32668,y:32961,varname:node_2257,prsc:2|IN-251-OUT;n:type:ShaderForge.SFN_RemapRange,id:251,x:32500,y:33099,varname:node_251,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-2861-OUT;n:type:ShaderForge.SFN_NormalVector,id:469,x:32668,y:33120,prsc:2,pt:False;n:type:ShaderForge.SFN_Vector1,id:376,x:32840,y:33353,varname:node_376,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:105,x:32933,y:33175,varname:node_105,prsc:2|A-2257-OUT,B-469-OUT,C-376-OUT,D-5917-OUT,E-7369-OUT;n:type:ShaderForge.SFN_Add,id:3224,x:32084,y:32930,varname:node_3224,prsc:2|A-8444-OUT,B-7732-OUT;n:type:ShaderForge.SFN_Time,id:4519,x:31685,y:33108,varname:node_4519,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7732,x:31929,y:33013,varname:node_7732,prsc:2|A-4519-T,B-684-OUT;n:type:ShaderForge.SFN_Vector1,id:684,x:31786,y:33237,varname:node_684,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Color,id:9450,x:32730,y:32673,ptovrint:False,ptlb:Emission Color,ptin:_EmissionColor,varname:node_9450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8455882,c2:0.2238322,c3:0.2238322,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:6175,x:31511,y:32722,varname:node_6175,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:1335,x:32055,y:33284,varname:node_1335,prsc:2,cc1:0,cc2:0,cc3:0,cc4:-1|IN-9699-V;n:type:ShaderForge.SFN_OneMinus,id:7406,x:32244,y:33294,varname:node_7406,prsc:2|IN-1335-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2176,x:32244,y:33470,ptovrint:False,ptlb:Amount y,ptin:_Amounty,varname:node_2176,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_Multiply,id:896,x:32438,y:33365,varname:node_896,prsc:2|A-7406-OUT,B-2176-OUT;n:type:ShaderForge.SFN_RemapRange,id:5917,x:32588,y:33318,varname:node_5917,prsc:2,frmn:0,frmx:3,tomn:0,tomx:5|IN-896-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:2679,x:31975,y:33566,varname:node_2679,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7030,x:32195,y:33585,varname:node_7030,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-2679-Z;n:type:ShaderForge.SFN_OneMinus,id:7369,x:32409,y:33562,varname:node_7369,prsc:2|IN-7030-OUT;proporder:9450-2176;pass:END;sub:END;*/

Shader "Shader Forge/VertextDef" {
    Properties {
        _EmissionColor ("Emission Color", Color) = (0.8455882,0.2238322,0.2238322,1)
        _Amounty ("Amount y", Float ) = 0.4
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float4 _EmissionColor;
            uniform float _Amounty;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4519 = _Time + _TimeEditor;
                float3 node_7406 = (1.0 - o.uv0.g.rrr);
                float node_7030 = mul(_Object2World, v.vertex).b.r;
                v.vertex.xyz += (saturate((sin((2.0*(mul(_Object2World, v.vertex).g.r+(node_4519.g*0.1))*6.28318530718))*0.5+0.5))*v.normal*0.2*((node_7406*_Amounty)*1.666667+0.0)*(1.0 - node_7030));
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = _EmissionColor.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float4 _EmissionColor;
            uniform float _Amounty;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4519 = _Time + _TimeEditor;
                float3 node_7406 = (1.0 - o.uv0.g.rrr);
                float node_7030 = mul(_Object2World, v.vertex).b.r;
                v.vertex.xyz += (saturate((sin((2.0*(mul(_Object2World, v.vertex).g.r+(node_4519.g*0.1))*6.28318530718))*0.5+0.5))*v.normal*0.2*((node_7406*_Amounty)*1.666667+0.0)*(1.0 - node_7030));
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = _EmissionColor.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Amounty;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4519 = _Time + _TimeEditor;
                float3 node_7406 = (1.0 - o.uv0.g.rrr);
                float node_7030 = mul(_Object2World, v.vertex).b.r;
                v.vertex.xyz += (saturate((sin((2.0*(mul(_Object2World, v.vertex).g.r+(node_4519.g*0.1))*6.28318530718))*0.5+0.5))*v.normal*0.2*((node_7406*_Amounty)*1.666667+0.0)*(1.0 - node_7030));
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
