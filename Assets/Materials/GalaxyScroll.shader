Shader "Custom/ScrollingGalaxy"
{
    Properties
    {
        _MainTex ("Galaxy Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Usamos Tiling y Offset (_ST) para mantener compatibilidad con Unity
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Aplicar desplazamiento vertical con el tiempo
                float2 uv = i.uv;
                uv.y += frac(_Time.y * _ScrollSpeed); // `frac` mantiene el valor entre 0 y 1 para loop infinito

                // Muestreamos la textura
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
