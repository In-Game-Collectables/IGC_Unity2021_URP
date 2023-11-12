Shader"IGC/CaptureMasker"
{
    Properties
    {
        _RGB ("RGB", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Brightness ("Brightness", Float) = 1.0
        _Contrast("Contrast", Float) = 1.0
        _Saturation("Saturation", Float) = 1.0
    }
    SubShader
    {
        Tags {"RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _RGB;
            sampler2D _Mask;
            float4 _RGB_ST;
            float _Brightness;
            float _Contrast;
            float _Saturation;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _RGB);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_RGB, i.uv);
                fixed alpha = tex2D(_Mask, i.uv).a;
                fixed b = _Brightness;
                fixed c = _Contrast;
                fixed s = _Saturation;
    
                fixed4 output = col * b;
    
                if (alpha > 0.5)
                {
                    output = fixed4(saturate(lerp(half3(0.5, 0.5, 0.5), output, c)), 1);
                    float greyscale = output.x * 0.21 + output.y * 0.72 * output.z * 0.07; // luminosity level
                    output = lerp(fixed4(greyscale, greyscale, greyscale, alpha), output, s);
                }
                else
                {
                    output = fixed4(0,0,0,0);
                }

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, output);
                return output;
}
            ENDCG
        }
    }
}
