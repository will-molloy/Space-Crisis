Shader "Custom/OriWater" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_isFront ("Front", int) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha 
		LOD 200
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			int _isFront;
			half contactX;
			half contactTime;

			struct appdata {
				half4 vertex : POSITION;
				half4 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			/**
			 * A convenient variation on Lerping that returns a value between start and end as the input value goes from min to max
			 * 
			 * So if value <= min return start, if value >= max return end, otherwise return something lerpy in between
			 */
			half rangeLerp(half start, half end, half min, half max, half value)
			{
				half lerpPercent = (value - min) / (max - min);
				lerpPercent = clamp(lerpPercent, 0, 1);
				return lerp(start, end, lerpPercent);
			}

			v2f vert(appdata i)
			{
				v2f o;

				half4 v = i.vertex;

				half xDif = abs(v.x - contactX);
				half invXDif = rangeLerp(2, 0, 0, 5, xDif);
				half offset = .4 * (invXDif) * cos(xDif + _Time[3]);
				
				// For the front we offset the z, but only for the top row
				v.z += (_isFront) * offset * (i.uv.y == 0);

				// For the top we offset all vertices
				v.y += (1-_isFront) * offset;

				o.pos = mul(UNITY_MATRIX_MVP, v);
				
				o.uv = i.uv;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 c = tex2D(_MainTex, i.uv);
				c.a = .5;
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
