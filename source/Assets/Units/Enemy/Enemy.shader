﻿
 Shader "Purify/Enemy"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        [Header(Outline)]
        _OutlineColor ("OutlineColor",Color ) = (1,1,1,1)
        _OutlineSize ("OutlineSize",Range(0, 0.01)) = 0.01
        
        [Header(Animation)]
        _Speed ("Speed", Range(0, 100)) = 25
        _Max ("Max", Range(0, 1)) = 0.1
        _Pos("Position",Vector) = (0,0,0)

       
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            float4 _OutlineColor;
            float _OutlineSize;
            
            float _Speed;
            float _Max;
            float3 _Pos;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                
                float3 vPos = v.vertex - _Pos;
                float y = sin((_Time.x * _Speed))*(10) * 5;
                v.vertex.y += step(0,vPos.y) * y * _Max;
                
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
               
				float up = tex2D(_MainTex, IN.texcoord + float2(0, _OutlineSize+1*_MainTex_TexelSize.y)).a;
				float down = tex2D(_MainTex, IN.texcoord + float2(0, -_OutlineSize+1*_MainTex_TexelSize.y)).a;
                float left = tex2D(_MainTex, IN.texcoord + float2(-_OutlineSize+1*_MainTex_TexelSize.x, 0)).a;
				float right = tex2D(_MainTex, IN.texcoord + float2(_OutlineSize+1*_MainTex_TexelSize.x, 0)).a;

                float outline = left + right + up + down;

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                
                outline *= (1-color.a);
                
                return  lerp(color, _OutlineColor, outline * ceil(_OutlineSize));
            }
        ENDCG
        }
    }
}