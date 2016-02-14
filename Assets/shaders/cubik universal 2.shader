Shader "Cubik universal 2"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Color2 ("Sec color", Color) = (1,1,1,1)
		_Power1 ("Pw 2468", float) = 1.6
		_Power2 ("Pw 13579", Float) = 1.5
		_Power3 ("Pw inGrani", Float) = 1.7
		_Power4 ("Pw GrKvadr", Float) = 8
		_Power5 ("Pw All", float) = 0.8
		_Power6 ("Pw outsurf", float) = 0.5
		_Power7 ("Pw outsurf lerp", float) = 0.4
		_LerpSt1 ("LerpSt inGrani", Float) = 0.2
		_LerpSt2 ("LerpSt GrKvadr", Float) = 0.7
		_LerpSt3 ("LerpSt Outsurf", float) = 0.8
		_1stTex ("First Tex", 2D) = "white"
		//_2ndTex ("Second Tex", 2D) = "white" {}	
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _1stTex, _2ndTex;
		float4 _Color1, _Color2;
		float _Power1, _Power2, _Power3, _Power4, _Power5, _Power6, _Power7,
			  alptex1, alptex2, _LerpSt1, _LerpSt2, _LerpSt3, _LerpSt31;
		  
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
			
		//	fixed2 scrolledUV2 = IN.uv_2ndTex;
		//	fixed xscroll2 = scrolledUV2.x;
		//	fixed yscroll2 = scrolledUV2.y;
			
			
			half4 c = tex2D (_1stTex, half2(scrolledUV1.x, scrolledUV1.y));
		//	half4 d = tex2D (_2ndTex, half2(scrolledUV2.x, scrolledUV2.y));
			alptex1 = c.a;
		//	alptex2 = d.a;
			
			_Power1 *= _Power5;
			_Power2 *= _Power5;
			_Power3 *= _Power5;
			_Power4 *= _Power5;
			
			_LerpSt31 = frac(_LerpSt3);
			
			if (scrolledUV1.y > 0.45 && scrolledUV1.x < 0.35) 
				o.Emission = _Color1 * _Power1 * alptex1; //2 4 6 8 квадраты
			else if (scrolledUV1.y > 0.65 && scrolledUV1.x > 0.3) 
				o.Emission = _Color1 * _Power2 * alptex1; // 1 3 5 7 9 квадраты
			
			if (scrolledUV1.y > 0.5 && scrolledUV1.y < 0.6) 
				o.Emission = lerp ((alptex1 * _Color2 * _Power3),((alptex1) * _Color1 * _Power1),_LerpSt1); // внутренние грани
			
			//if (scrolledUV1.x > 0.7) o.Emission = _Color1 * _Power4 * alptex1 
			if (scrolledUV1.x > 0.7) 
				o.Emission = lerp((alptex1 * _Color2 * _Power3),(alptex1 * _Color1 * _Power4),_LerpSt2); // грнаи внутренних квадратов
			else if (scrolledUV1.y < 0.45)
				 o.Emission = lerp((_Color1 * 0.8),(_Color2 * _Power6),_LerpSt31) * _Power7; // внешние грани
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
