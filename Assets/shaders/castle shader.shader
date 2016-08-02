Shader "castle shader"
	{
		Properties
		{
		_Color1("Main color", Color) = (1,1,1,1)
		_Color2("Sec color", Color) = (1,1,1,1)
		_Power1("PwTex", range (0,1)) = 1
		_Power2("PwGradMAP", range (0,1)) = 1
		_LerpSt("LerpSt", Float) = 1
		_1stTex ("Texture", 2D) = "white"
		_2ndTex ("Grad MAP", 2D) = "white" {}	
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _1stTex, _2ndTex;
		float4 _Color1, _Color2;
		float _Power1, _Power2, alptex1, alptex2, _LerpSt;
		  
		struct Input 
		{
			float2 uv_1stTex;
			float2 uv_2ndTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
		
			half4 c = tex2D (_1stTex, IN.uv_1stTex);
			half4 d = tex2D (_2ndTex, IN.uv_2ndTex);

			alptex2 = d.a;

			//o.Albedo = pow ( c.rgb * _Power1, (1-alptex2)*_Power2+1);
			o.Albedo = c.rgb * (1-(alptex2) * _Power2);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
