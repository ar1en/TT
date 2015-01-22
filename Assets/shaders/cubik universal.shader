Shader "Cubik universal"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Color2 ("Sec color", Color) = (1,1,1,1)
		_Power1 ("Power1", Float) = 1
		_Power2 ("Power2", Float) = 1
		_Power3 ("Power3", Range (0,2)) = 1
		_LerpSt ("Lerp Step", Float) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white"
	//	_LightMap ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex, _LightMap;
		float4 _Color1, _Color2,_ClrMnBld;
		float _Power1, _Power2, _Power3, alpCtex, _LerpSt;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 scrolledUV = IN.uv_MainTex;
			fixed xscroll = scrolledUV.x;
			fixed yscroll = scrolledUV.y;
			
			half4 c = tex2D (_MainTex, half2(scrolledUV.x, scrolledUV.y));
			//half4 d = tex2D (_LightMap, half2(scrolledUV.x, scrolledUV.y));
			alpCtex = c.a+0.1;
			_ClrMnBld = alpCtex * _Color1;
			
			if (scrolledUV.y > 0.45 && scrolledUV.x < 0.35) 
				o.Emission = _ClrMnBld * _Power1;
			else if (scrolledUV.y > 0.65 && scrolledUV.x > 0.3) o.Emission = _ClrMnBld * _Power2;
			//else if (scrolledUV.y > 0.55 && scrolledUV.y < 0.65) o.Emission = lerp (_ClrMnBld,_Color2,_LerpSt) * _Power1;
			else o.Emission = c.rgb * _Power3;
			if (scrolledUV.y > 0.5 && scrolledUV.y < 0.6) o.Emission = lerp (_ClrMnBld,_Color2,_LerpSt) * _Power1;
			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
