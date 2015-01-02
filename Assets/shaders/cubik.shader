Shader "Custom/cubik" {
	Properties {
		_Color1("ramka", color) = (1,1,1,1)
		_Color2("maincube", color) = (1,1,1,1)
		_Color3("signal", color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color1, _Color2, _Color3;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutput o) {
		
			
			fixed2 scrUV = IN.uv_MainTex;
			half scrX, scrY;
			scrX = scrUV.x;
			scrY = scrUV.y;
			
			half4 c = tex2D (_MainTex, fixed2(scrX , scrY));
			
			
			if (scrUV.x > 0.7 && scrUV.y > 0.6)
				{
				if (scrUV.y > 0.7)
					{
					o.Emission = _Color1.rgb;
					}
				else o.Emission = _Color3.rgb;
				}
				else o.Albedo = _Color2.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
