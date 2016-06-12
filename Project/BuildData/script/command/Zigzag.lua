ULM=function(api,targetFunc2,w,h,f_1,numberFunc){
    local this={};

    local targetX,targetY=0;
    local number=0;

    --一回のカウント(F数)
    local count_1=0;

    --全体のカウント(回数)
    local count=0;

    --左右のベクトル
    local v1X,v1Y,v2X,v2Y=0;

    this.isNeed=function()
        return count < number;
    end

    this.draw=function()
    end

    this.update=function()
        //初期化
        if count==0 and count_1=0 then
            targetX,targetY=targetFunc2();
            number=numberFunc();

            --ベクトル計算
            local v1X_,v1Y_,v2X_,v2Y_=w,h,-w,h;

            api.Rad=math.atan2(targetX-api.X,targetY-api.Y);

            local mapRad1,mapRad2=api.ToMapRad(math.atan2(v1X_,v1Y_)),api.ToMapRad(math.atan2(v2X_,v2Y_));
            local len=math.sqrt(w^2+h^2);

            v1X=math.cos(mapRad1)*len;
            v1Y=math.sin(mapRad1)*len;
            v2X=math.cos(mapRad2)*len;
            v2Y=math.sin(mapRad2)*len;
        end
        if this.isNeed
            local vx,vy = count % 2 == 0 ? v1X,v1Y : v2X,v2Y;
            api.X=api.X+vx;
            api.Y=api.Y+vy;

            count_1=count_1+1;
            --今回の移動が終わったなら(最初と最後は半分しか移動しない)
            if count_1>=(count==0 or count==number?f_1/2:f_1) then
                count_1=0;
                count=count+1;
            end
        end
    end

    return this;
}