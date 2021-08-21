﻿ivec2 Helper_TexelFetchScale(ivec2 inputVec, int samplerIndex)
{
    float scale = s_render_scale[1 + samplerIndex];
    if (scale == 1.0)
    {
        return inputVec;
    }
    if (scale < 0.0) // If less than 0, try interpolate between texels by using the screen position.
    {
        return ivec2(vec2(inputVec) * (-scale) + mod(gl_FragCoord.xy, -scale));
    }
    else
    {
        return ivec2(vec2(inputVec) * scale);
    }
}

int Helper_TextureSizeUnscale(int size, int samplerIndex)
{
    float scale = abs(s_render_scale[1 + samplerIndex]);
    if (scale == 1.0)
    {
        return size;
    }
    return int(float(size) / scale);
}