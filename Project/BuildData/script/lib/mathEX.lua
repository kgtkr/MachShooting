--数学関連拡張
module("mathEX", package.seeall);

radLenVec=function(rad,len)
    local x = math.cos(rad) * len;
    local y = math.sin(rad) * len;

    return x,y;
end

--1(x,y)→2(x,y)へのベクトル
objVec=function(x1,y1,x2,y2,len)
    local vx,vy=x2-x1,y2-y1;
    local vLen=math.sqrt(vx^2+vy^2);
    if vLen~=0 then
        vx=vx/vLen*len;
        vy=vy/vLen*len;
    end

    return vx,vy;
end

toRad=function(angle)
    return angle * math.pi / 180;
end

toAngle=function(rad)
    return rad * 180 / math.pi;
end