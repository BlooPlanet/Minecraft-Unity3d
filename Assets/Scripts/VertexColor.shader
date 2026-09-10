Shader "Custom/VertexColorLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

            o.Albedo = tex.rgb * IN.color.rgb * _Color.rgb;
            o.Alpha = tex.a * IN.color.a * _Color.a;
        }

        ENDCG
    }

    FallBack "Standard"
}