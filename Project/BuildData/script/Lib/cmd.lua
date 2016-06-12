module("cmd", package.seeall)

--単純に値を返すだけの匿名関数を作る
retFunc=function(val)
    return (function()
        return val;
    end);
end

retFunc2=function(val1,val2)
    return (function()
        return val1,val2;
    end);
end


Action=function(action)
    this={};
    local need=true;
    this.isNeed=function()
        return need;
    end

    this.draw=function()
    end

    this.update=function()
        if need then
            action();
            need=false;
        end
    end
    return this;
end


Async=function()
    local this={};
    this.list={};
    this.isNeed=function()
        return table.maxn(this.list)~=0;
    end

    this.draw=function()
        table.foreach( this.list,
            function( index, item )
                item.draw();
            end
        )
    end

    this.update=function()
        table.foreach( {unpack(this.list)},
        function( index, item )
            if item.isNeed() then
                item.update();
            end

            if not item.isNeed() then--必要ないなら削除
                table.remove(this.list, index);
            end
        end
        );
    end

    return this;
end

Charge=function(api,image,f,maxR,r,g,b)
    local this={};

    local count=0;

    this.isNeed=function()
        return count < f;
    end

    this.draw=function()
        if this.isNeed() then
            DX.SetDrawBright(r, g, b);
            local ex = 1 - (count / f);
            DX.DrawRotaGraph(api.X, api.Y, ex, api.Rad,image, DX.TRUE);
            DX.SetDrawBright(255, 255, 255);            
        end
    end
    
    this.update=function()
        if this.isNeed() then
            count=count+1;
        end
    end

    return this;
end

Circle=function(api,defaultRad,radSpeed,f,r,dot)
    local this={};

    local count=0;
    local rad=defaultRad;
    local x=0;
    local y=0;

    this.isNeed=function()
        return count < f;
    end

    this.draw=function()
    end

    this.update=function()
        if this.isNeed() then
            if count==0 then
                x,y=dot();
            end

            rad=rad+radSpeed();
            rVal=r();

            api.X = api.X+math.cos(rad)*rVal;
            api.Y = api.Y+math.sin(rad)*rVal;

            count=count+1;
        end
    end

    return this;
end

Sync=function()
    local this={};
    this.list={};
    this.isNeed=function()
        return table.maxn(this.list)~=0;
    end

    this.draw=function()
        if this.isNeed() then
            this.list[1].draw();
        end
    end

    this.update=function()
        local loop = true;

            while loop do
                if this.isNeed() then
                    if this.list[1].isNeed() then
                        this.list[1].update();
                        loop = false;
                    else--最初からNeed=falseの操作ならループする
                        loop = true;
                    end

                    if not this.list[1].isNeed() then--必要ないなら削除
                        table.remove(this.list, 1);
                    end
                else
                    loop = false;
                end
            end
    end

    return this;
end

ULM=function(api,vec,fFunc)
    local this={};

    local count=0;

    local vx,vy,f=0,0,0;

    this.isNeed=function()
        return count < f or f==0;
    end

    this.draw=function()
    end

    this.update=function()
        if count==0 then
            vx,vy=vec();
            f=fFunc();
        end
        if this.isNeed then
            api.X=api.X+vx;
            api.Y=api.Y+vy;

            count=count+1;
        end
    end

    return this;
end

Wait=function(wait)
    local this={};

    --残り時間
    local limit=wait;

    this.isNeed=function()
        return limit>0;
    end

    this.draw=function()
    end

    this.update=function()
        if this.isNeed then
            limit=limit-1;
        end
    end

    return this;
end

Zigzag=function(api,targetFunc2,w,h,f_1,numberFunc)
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
        --初期化
        if count==0 and count_1==0 then
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
        if this.isNeed then
            local vx,vy = count % 2 == 0 and v1X,v1Y or v2X,v2Y;
            api.X=api.X+vx;
            api.Y=api.Y+vy;

            count_1=count_1+1;
            --今回の移動が終わったなら(最初と最後は半分しか移動しない)
            if count_1>=(count==0 or count==number and f_1/2 or f_1) then
                count_1=0;
                count=count+1;
            end
        end
    end

    return this;
end

