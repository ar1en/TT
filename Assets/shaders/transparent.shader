Shader "Custom/transparent" {
	Properties {
		_MainColor11 ("Color", color) = (1,1,1,0)
		_Trans ("Transparent", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}
		
		LOD 200
				
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float4 _MainColor11;
		float _Trans;
		

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{

			o.Emission = _MainColor11.rgb; 
			o.Alpha = _Trans;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
