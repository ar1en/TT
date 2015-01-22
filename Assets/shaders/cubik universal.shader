Shader "Cubik universal"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Color2 ("Sec color", Color) = (1,1,1,1)
		_Power1 ("PwMnCl", Range (0,2)) = 1
		_Power2 ("PwScCl", Range (0,2)) = 1
		_Power3 ("Power3", Range (0,2)) = 1
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
		float _Power1, _Power2, _Power3, alptex1, alptex2, _LerpSt;
		  
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
			
			if (scrolledUV1.y > 0.45 && scrolledUV1.x < 0.35) 
				o.Emission = _Color1 * alptex1 * _Power1; //2 4 6 8 квадраты
			else if (scrolledUV1.y > 0.65 && scrolledUV1.x > 0.3) 
				o.Emission = _Color1 * alptex1 * _Power3; // 1 3 5 7 9 квадраты и грани на них
			o.Albedo = c.rgb; // внешние грани
			if (scrolledUV1.y > 0.5 && scrolledUV1.y < 0.6) 
				o.Emission = lerp ((_Color1 * alptex1 * _Power1),(alptex2 * _Color2 * _Power2),_LerpSt); // внутренние грани
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
