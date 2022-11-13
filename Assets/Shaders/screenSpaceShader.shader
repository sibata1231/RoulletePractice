Shader "Unlit/screenSpaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 pos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            
            v2f vert(appdata v) {
                v2f o;
            
                // MVP�s��Ɋ|����
                // mul(UNITY_MATRIX_MVP, v.vertex) �Ɠ������������ǃp�t�H�[�}���X�ǂ�
                o.vertex = UnityObjectToClipPos(v.vertex);
            
                // z�͎g��Ȃ��̂�xyz��float3�Ɋi�[����
                o.pos = o.vertex.xyw;
            
                // �v���b�g�t�H�[���̈Ⴂ���z��
                o.pos.y *= _ProjectionParams.x;
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                // 0�`1�ɕϊ�
                half2 uv = i.uv; //i.pos.xy / i.pos.z * 0.5 + 0.5
            
                float interporation = uv.x - uv.y;
                interporation = interporation * 10;
                interporation = frac(interporation);
                interporation = step(interporation, 0.5f);
                fixed4 col = lerp(fixed4(235.0 / 255, 97.0 / 255, 1.0 / 255, 1), fixed4(0, 0, 0, 1), interporation);
                return col;
            }
            ENDCG
        }
    }
}
