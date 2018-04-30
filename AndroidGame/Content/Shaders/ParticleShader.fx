float4x4 worldMatrix;

float width;
float height;

float currentTime;

struct VertexInput
{
	float2 position : POSITION0;
	float2 localPosition : POSITION1;
	float2 direction : POSITION2;
	
	float speed : PSIZE0;
	float deceleration : PSIZE1;
	float rotation : PSIZE2;
	float startTime : PSIZE3;
	float endTime : PSIZE4;

	float4 color : COLOR0;
};

struct VertexToPixel
{
	float4 position : POSITION;
	float4 color : COLOR0;
};


struct PixelToFrame
{
	float4 color : COLOR0;
};

VertexToPixel vsMain(VertexInput input)
{
	VertexToPixel output = (VertexToPixel)0;

	float liveTime = currentTime - input.startTime;
	input.rotation *= liveTime;
	input.position.x += input.localPosition.x * cos(input.rotation) - input.localPosition.y * sin(input.rotation);
	input.position.y += input.localPosition.y * cos(input.rotation) + input.localPosition.x * sin(input.rotation);
	input.position += input.direction * (input.speed * liveTime -  input.deceleration * liveTime * liveTime / 2.0);

	output.position = mul(float4(input.position.x, input.position.y, 1, 1), worldMatrix);
	output.position.x = output.position.x / width - 1.0;
	output.position.y = -output.position.y / height + 1.0;

	float alpha = (currentTime - input.startTime) / (input.endTime - input.startTime);
	alpha = 1 - alpha * alpha;

	output.color = input.color * alpha;

	return output;
}

PixelToFrame psMain(VertexToPixel input)
{
	PixelToFrame output = (PixelToFrame)0;

	output.color = input.color;

	return output;
}

technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}