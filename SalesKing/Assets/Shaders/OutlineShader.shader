Shader "Custom/RimLight"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}  // 기본 텍스처
        _RimColor ("Rim Color", Color) = (1, 0.5, 0, 1)  // 림 색상 (주황색)
        _RimPower ("Rim Power", Range(0.1, 8.0)) = 3.0  // 림 효과의 범위 조정
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldNormal;
        };

        uniform float _RimPower;
        uniform fixed4 _RimColor;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // 기본 텍스처 색상 유지
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _RimColor;
            o.Albedo = c.rgb;

            // 카메라와의 각도에 따라 림 효과 추가
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), IN.worldNormal));
            rim = pow(rim, _RimPower);

            // 경계선에 림 색상 적용
            o.Emission = rim * _RimColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
