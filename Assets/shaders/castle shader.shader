Shader "castle shader"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Color2 ("Sec color", Color) = (1,1,1,1)
		_Power1 ("PwMnCl", float) = 1
		_Power2 ("PwScCl", Float) = 1
		_LerpSt ("LerpSt", Float) = 0.5
		_1stTex ("First Tex", 2D) = "white"
		_2ndTex ("Second Tex", 2D) = "white" {}	
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _1stTex, _2ndTex;
		float4 _Color1, _Color2,_ClrMnBld;
		float _Power1, _Power2, alptex1, alptex2, _LerpSt;
		  
		struct Input 
		{
			float2 uv_1stTex;
			float2 uv_2ndTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 scrolledUV1 = IN.uv_1stTex;
			fixed xscroll1 = scrolledUV1.x;
			fixed yscroll1 = scrolledUV1.y;
			
			fixed2 scrolledUV2 = IN.uv_2ndTex;
			fixed xscroll2 = scrolledUV2.x;
			fixed yscroll2 = scrolledUV2.y;
			
			
			half4 c = tex2D (_1stTex, half2(scrolledUV1.x, scrolledUV1.y));
			half4 d = tex2D (_2ndTex, half2(scrolledUV2.x, scrolledUV2.y));
			alptex1 = c.a;
			alptex2 = d.a;
			
			if (scrolledUV1.y < 0.6) 
				o.Emission = c.rgb * _Power1;
			else 
				o.Emission = c.rgb * _Power2;

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
