Shader "Ramka"
{
	Properties
	{
		_BorderCurrentColor("Current Color", Color) = (1,1,1,1) // актуальный цвет рамки, соответствующий цвету падающего кубика
		_BorderNextColor("Next Color", Color) = (1,1,1,1) // следующий цвет рамки, соответствующий следующей фигуре
		_BorderSecColor("Sec color", color) = (1,1,1,1) // вторичный цвет рамки, не зависящий от падающей фигуры
		_BorderColorCounter("Color counter", float) = 1 // счетчик, определяющий степень интреполяции (смешивания) между актуальным и следующим цветом
		_BorderCommonPw("Cmn Pw", float) = 1 // общий множитель яркости цветов для рамки
		_BorderCurrentColorPw("Current Color Power", float) = 1 // множитель яркости актуального цвета
		_BorderNextColorPw("Next Color Power", float) = 1 // множитель яркости следующего цвета
		_BorderCurrentColorStep("Curr Color Interpol Step", float) = 1 // степень интерполяции актуального цвета
		_BorderNextColorStep("Next Color Interpol Step", float) = 1 // степень интерполяции следующего цвета
		_BorderPower1("Cmn pow 1", Float) = 1 // свободный коэффициент 1 только для шейдера рамки
		_BorderPower2("Cmn pow 2", Float) = 1 // свободный коэффициент 2 только для шейдера рамки
		_BorderColorMap("Brd Color Map", 2D) = "white" // текстура цветовой карты для шейдера рамки
		_BorderAlphaMap("Brd Alpha Map", 2D) = "white" // карта альфа канала для шейдера рамки
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert


		sampler2D _BorderAlphaMap, _BorderColorMap;
		float3 _BorderCurrentfColorStep, _BorderNextColorStep, _SecColorfromMap;
		float4 _BorderCurrentColor, _BorderNextColor, _BorderSecColor;
		float _AlphaPow, _BorderCommonPw, _BorderColorCounter, _BorderPower1;

	struct Input
	{
		float2 uv_BorderAlphaMap;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		float2 coordUV = IN.uv_BorderAlphaMap;
		float xcoord = coordUV.x;
		float ycoord = coordUV.y;


		_AlphaPow = tex2D(_BorderAlphaMap, fixed2(xcoord,ycoord)).a;
		_SecColorfromMap = tex2D(_BorderColorMap, fixed2(xcoord, ycoord)).rgb;

		if (coordUV.x >= 0.5)
		{
			o.Emission = _SecColorfromMap * _BorderPower1;
		}
		else {
			o.Emission = lerp(_BorderCurrentColor.rgb * pow(_AlphaPow, _BorderColorCounter) * _BorderCommonPw,
				_BorderNextColor.rgb * pow(_AlphaPow, _BorderNextColorStep) * _BorderCommonPw, _BorderColorCounter);
		}

	}
	ENDCG
	}
		FallBack "Diffuse"
}
