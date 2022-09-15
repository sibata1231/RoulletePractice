Shader "Unlit/procedual" {
    Properties {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Intensity("Intensity", Range(0, 1)) = 0.1
        [IntRange] _Loop("Loop", Range(0, 128)) = 32
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata {
        float4 vertex : POSITION;
    };

    struct v2f {
        float4 vertex   : SV_POSITION;
        float3 worldPos : TEXCOORD1;
    };

    #define MAX_LOOP 100
    float4 _Color;
    float _Intensity;
    int _Loop;

    inline float densityFunction(float3 p) {
        return 0.5 - length(p);
    }

    v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        return o;
    }

    float4 frag(v2f i) : SV_Target {
        float3 worldPos = i.worldPos;
        float3 worldDir = normalize(worldPos - _WorldSpaceCameraPos);

        float3 localPos = mul(unity_WorldToObject, float4(worldPos, 1.0));
        float3 localDir = UnityWorldToObjectDir(worldDir);

        float step = 1.0 / _Loop;
        float3 localStep = localDir * step;

        float alpha = 0.0;

        for (int i = 0; i < _Loop; ++i) {
            // ポリゴン中心ほど大きな値が返ってくる
            float density = densityFunction(localPos);

            // 球の外側ではマイナスの値が返ってくるのでそれを弾く
            if (density > 0.001) {
                // 透過率の足し合わせ
                alpha += (1.0 - alpha) * density * _Intensity;
            }

            // ステップを進める
            localPos += localStep;

            // ポリゴンの外に出たら終わり
            if (!all(max(0.5 - abs(localPos), 0.0))) break;
        }

        float4 color = _Color;
        color.a *= alpha;
        return color;
    }
    ENDCG

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass {
            Cull Back
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}