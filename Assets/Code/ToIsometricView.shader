Shader "Custom/ToIsometricView"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _IsoScale ("Isometric Scale", Float) = 1.0
        _ShearFactor ("Shear Factor", Float) = 0.5 // 기울기 조정값
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // 투명도 지원

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _IsoScale; 
            float _ShearFactor; // 기울이기 변환 값

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // ✅ Isometric 변환 + Shearing 적용
            v2f vert (appdata_t v)
            {
                v2f o;

                // Sprite 정점의 원본 위치
                float3 worldPos = v.vertex.xyz;

                // ✅ 기본 Isometric 변환 (XZ 변환)
                float isoX = (worldPos.x - worldPos.y) * 0.707; // 45도 회전
                float isoZ = (worldPos.x + worldPos.y) * 0.408; // 26.57도 기울기

                // ✅ Shearing 적용 (Y축을 기울이기)
                float isoY = worldPos.z + _ShearFactor * isoZ;

                // 위치 보정 (스케일 적용)
                v.vertex = float4(isoX * _IsoScale, isoY * _IsoScale, 0.0, 1.0);

                // 변환된 좌표를 적용
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.pos.z = 0.0; // Z값 하드코딩하여 깊이 왜곡 방지
                return o;
            }

            // ✅ Fragment Shader (텍스처 샘플링)
            fixed4 frag (v2f IN) : SV_Target
            {
                return tex2D(_MainTex, IN.uv);
            }
            ENDCG
        }
    }
}
