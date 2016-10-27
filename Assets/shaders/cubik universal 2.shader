Shader "Cubik universal 2"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Color2 ("sec color", Color) = (1,1,1,1)
		_Color22 ("in faces", Color) = (1,1,1,1)
		_Color3 ("out faces", Color) = (1,1,1,1)
		_Power1 ("Pw main", float) = 1.6
		_Power2 ("Pw in faces", Float) = 1.5
		_Power3 ("Pw out faces", Float) = 1.7
		_Power4 ("Pw common", Float) = 8
		_LerpSt1 ("LerpSt inGrani", Float) = 0.2
		_LerpSt2 ("LerpSt GrKvadr", Float) = 0.7
		_LerpSt3 ("LerpSt Outsurf", float) = 0.8
		_1stTex ("First Tex", 2D) = "white"	
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _1stTex, _2ndTex;
		float4 _Color1, _Color2, _Color22, _Color3;
		float _Power1, _Power2, _Power3, _Power4, _LerpSt1, _LerpSt2, _LerpSt3;
		  
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

			
			if (scrolledUV1.y < 0.5)
				o.Emission = _Color22 * _Power2; //внешние грани
			else if (scrolledUV1.y > 0.5 && scrolledUV1.y < 0.7) 
				o.Emission = _Color3 * _Power3; // внутренние грани
			else if (scrolledUV1.y > 0.7)
				o.Emission = _Color1 * _Power1; // основна грань

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
