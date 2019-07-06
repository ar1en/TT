Shader "Cubik shader" 
{
	Properties
	{
		_MainColorCube("Main color", Color) = (1,1,1,1) // Основной цвет куба, зависящий от типа фигуры
		_SecCubeColor("Cube color", Color) = (1,1,1,1) // Цвет остальных граней куба
		_MainColorCubePw("Main color PW", float) = 1 // коэффициент умножения для основного цвета куба
		_SecCubeColorPw("Cube color Pw", float) = 1 // коэффициент умножения для основного цвета куба
		_CubePower1("Cmn pow 1", Float) = 1 // свободный коэффициент 1 только для шейдера куба
		_CubeAlphaMap("Cube Alpha Map", 2D) = "white" // карта альфа канала для шейдера кубика
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _CubeColorMap, _CubeAlphaMap;
	float4 _MainColorCube, _SecCubeColor, _ColorfromMap;
	float _MainColorCubePw, _SecCubeColorPw, _CubePower1, _AlphaPow;

	struct Input
	{
		float2 uv_CubeAlphaMap;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed2 scrolledUV1 = IN.uv_CubeAlphaMap;
		fixed xscroll1 = scrolledUV1.x;


		_ColorfromMap = tex2D(_CubeColorMap, IN.uv_CubeAlphaMap);
		_AlphaPow = tex2D(_CubeAlphaMap, IN.uv_CubeAlphaMap).a;

		if (scrolledUV1.y < 0.7)
			o.Emission = _SecCubeColor * _SecCubeColorPw; //внешние грани
		else if (scrolledUV1.y > 0.7)
			o.Emission = _MainColorCube * (_AlphaPow + _CubePower1) * _MainColorCubePw; // основна грань

	}
	ENDCG
	}
		FallBack "Diffuse"
}