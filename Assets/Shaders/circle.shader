Shader "Unlit/circle" {
    Properties {
				   _MainTex ("Texture", 2D)					= "white" {}
		[IntRange] _DivisionCount("Division", Range(1,256)) = 17	     // member
				   _Speed("Speed",Float)					= 1		     // Speed
				   _Count("Count", Range(0,359))			= 0		     // counter
				   _FanColor("FanColor", Color)				= (1,1,1,1)  // color
    }

    SubShader {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4	  _MainTex_ST;
			int		  _DivisionCount;
			float	  _Speed;
			float	  _Count;
			float4	  _FanColor;

            float disc(float2 st) {
                float d = distance(st, float2(0.5, 0.5));
                float s = step(0.5, d);
                return s;
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			#define PI 3.14159265359
			float disc(float2 st, float size) {
				float d = distance(st, float2(0.5, 0.5));
				float s = step(size, d);
				return s;
			}

			float fan_shape(float2 st, float angle) {
				float dx = 0.5 - st.x;
				float dy = 0.5 - st.y;

				float rad = atan2(dx, dy);
				rad = rad * 180 / PI + 180;
				
				float time = _Count *_Speed;

				float n1 = floor((time + angle) / (360));
				float n2 = floor((time) / (360));
				float offset1 = time - 360 * n1;
				float offset2 = time - 360 * n2;
				float s1 = step(rad, angle + offset1);
				float s2 = step(offset2, rad);

				float4 color = (s1 * s2) * step(offset2, 360 - angle) +
					(s1 + s2) * (1 - step(offset2, 360 - angle)) -
					disc(st, 0.5);
				return step(float4(1,1,1,1), color);
			}

			//float grad_rot(float2 st) {
			//	float dx = 0.5 - st.x;
			//	float dy = 0.5 - st.y;
			//
			//	float rad = atan2(dx, dy);
			//	rad = rad * 180 / PI + 180;
			//
			//	float n = floor((_Time * 1000) / 360);
			//	float offset = _Time * 1000;
			//	rad = rad + n * 360;
			//	float d1 = distance(rad, offset) / 50;
			//	float d2 = distance(rad, offset + 360) / 50;
			//	float d3 = distance(rad, offset - 360) / 50;
			//
			//	return min(min(d1, d2), d3);
			//}
			//
			//float grid(float2 st) {
			//	float r1 = -disc(st, 0.5) + disc(st, 0.495);
			//	float r2 = -disc(st, 0.3) + disc(st, 0.295);
			//	float r3 = -disc(st, 0.1) + disc(st, 0.095);
			//	return r1 + r2 + r3;
			//}

			fixed4 frag(v2f i) : SV_Target{
				return fan_shape(i.uv, 360/ _DivisionCount) * _FanColor;// *grad_rot(i.uv) + grid(i.uv);
			}
            ENDCG
        }
    }
}
