Shader "Custom/CircleFillShader"
{
    Properties
    {
        _FillColor ("Fill Color", Color) = (1,1,1,1) // 채워진 부분 색상
        _FillAmount ("Fill Amount", Range(0,1)) = 0.5  // 0~1 입력값
        _Center ("Center Position", Vector) = (0.5, 0.5, 0, 0) // 중심점

        _InnerRadius ("Inner Radius", Range(0, 1)) = 0.0 // 내부 반지름
        _OuterRadius ("Outer Radius", Range(0, 1)) = 1.0 // 외부 반지름
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha  // 알파 블렌딩 적용
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainTex_ST;
            float _FillAmount;
            float4 _Center;
            float4 _FillColor;

            float _InnerRadius;
            float _OuterRadius;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV 좌표에서 중심점을 기준으로 상대 좌표 계산
                float2 centeredUV = i.uv - _Center.xy;

                if(length(centeredUV) < _InnerRadius || length(centeredUV) > _OuterRadius)
                {
                    return fixed4(0, 0, 0, 0); // 투명
                }

                // 각도 계산 (12시 방향이 0도)
                float angle = atan2(-centeredUV.y, centeredUV.x) * (180 / 3.14159265) + 90;
                if (angle < 0) angle += 360;

                // FillAmount를 360도로 변환
                float filledAngle = _FillAmount * 360;

                // 현재 픽셀의 각도가 filledAngle보다 작으면 렌더링, 아니면 투명
                if (angle <= filledAngle)
                {
                    return _FillColor;
                }
                else
                {
                    return fixed4(0, 0, 0, 0); // 투명
                }
            }
            ENDCG
        }
    }
}
