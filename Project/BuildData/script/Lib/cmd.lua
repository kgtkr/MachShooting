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


Action={
    new=function(action)
        local this={
            need=true,
            action=action
        };

        return setmetatable(this, {__index = Action});
    end,

    isNeed=function(this)
        return this.need;
    end,

    draw=function(this)
    end,

    update=function(this)
        if this.need then
            this.action();
            this.need=false;
        end
    end
};




Async={
    new=function()
        local this={
            list={}
        };

        return setmetatable(this, {__index = Async});
    end,

    isNeed=function(this)
        return table.maxn(this.list)~=0;
    end,

    draw=function(this)
        table.foreach( this.list,
            function( index, item )
                item:draw();
            end
        )
    end,

    update=function(this)
        table.foreach( {unpack(this.list)},
        function( index, item )
            if item:isNeed() then
                item:update();
            end

            if not item:isNeed() then--必要ないなら削除
                table.remove(this.list, index);
            end
        end
        );
    end
};

Charge={
    new=function(api,image,f,maxR,r,g,b)
       local this={
            api=api,
            image=image,
            f=f,
            maxR=maxR,
            r=r,
            g=g,
            b=b,
            count=0
        };

        return setmetatable(this, {__index = Charge}); 
    end,

    isNeed=function(this)
        return this.count < this.f;
    end,

    draw=function(this)
        if this:isNeed() then
            DX.SetDrawBright(this.r, this.g, this.b);
            local ex = 1 - (this.count / this.f);
            DX.DrawRotaGraph(this.api.X, this.api.Y, ex, this.api.Rad,this.image, DX.TRUE);
            DX.SetDrawBright(255, 255, 255);            
        end
    end,
    
    update=function(this)
        if this:isNeed() then
            this.count=this.count+1;
        end
    end
};

Circle={
    new=function(api,defaultRad,radSpeed,f,r,dot)
        local this={
            api=api,
            defaultRad=defaultRad,
            radSpeed=radSpeed,
            f=f,
            r=r,
            dot=dot,
            count=count,
            rad=defaultRad,
            x=0,
            y=y
        };

        return setmetatable(this, {__index = Circle}); 
    end,

    isNeed=function(this)
        return this.count < this.f;
    end,

    draw=function(this)
    end,

    update=function(this)
        if this:isNeed() then
            if this.count==0 then
                this.x,this.y=this.dot();
            end

            this.rad=this.rad+this.radSpeed();
            this.rVal=this.r();

            this.api.X = this.api.X+math.cos(this.rad)*this.rVal;
            this.api.Y = this.api.Y+math.sin(this.rad)*this.rVal;

            this.count=this.count+1;
        end
    end
};

Sync={
    new=function()
        local this={
            list={}
        };

        return setmetatable(this, {__index = Sync}); 
    end,

    isNeed=function(this)
        return table.maxn(this.list)~=0;
    end,

    draw=function(this)
        if this:isNeed() then
            this.list[1]:draw();
        end
    end,

    update=function(this)
        local loop = true;

        while loop do
            if this:isNeed() then
                if this.list[1]:isNeed() then
                    this.list[1]:update();
                    loop = false;
                else--最初からNeed=falseの操作ならループする
                    loop = true;
                end

                if not this.list[1]:isNeed() then--必要ないなら削除
                    table.remove(this.list, 1);
                end
            else
                loop = false;
            end
        end
    end
};

ULM={
    new=function(api,vec,fFunc)
        local this={
            api=api,
            vec=vec,
            fFunc=fFunc,
            count=0,
            vx=0,
            vy=0,
            f=0
        };

        return setmetatable(this, {__index = ULM}); 
    end,

    isNeed=function(this)
        return this.count < this.f or this.f==0;
    end,

    draw=function(this)
    end,

    update=function(this)
        if this.count==0 then
            this.vx,this.vy=this.vec();
            this.f=this.fFunc();
        end
        if this.isNeed then
            this.api.X=this.api.X+this.vx;
            this.api.Y=this.api.Y+this.vy;

            this.count=this.count+1;
        end
    end
};

Wait={
    new=function(wait)
        local this={
            wait=wait,
            limit=wait
        };

        return setmetatable(this, {__index = Wait}); 
    end,

    isNeed=function(this)
        return this.limit>0;
    end,

    draw=function(this)
    end,

    update=function(this)
        if this:isNeed() then
            this.limit=this.limit-1;
        end
    end
};

Zigzag={
    new=function(api,targetFunc2,w,h,f_1,numberFunc)
        local this={
            api=api,
            targetFunc2=targetFunc2,
            w=w,
            h=h,
            f_1=f_1,
            numberFunc=numberFunc,
            targetX=0,
            targetY=0,
            number=0,
            count_1=0,
            count=0,
            v1X=0,
            v1Y=0,
            v2X=0,
            v2Y=0
        };

        return setmetatable(this, {__index = Zigzag}); 
    end,

    isNeed=function(this)
        return this.count < this.number;
    end,

    draw=function(this)
    end,

    update=function(this)
        --初期化
        if this.count==0 and this.count_1==0 then
            this.targetX,this.targetY=this.targetFunc2();
            this.number=this.numberFunc();

            --ベクトル計算
            local v1X_,v1Y_,v2X_,v2Y_=this.w,this.h,-this.w,this.h;

            this.api.Rad=math.atan2(this.targetX-this.api.X,this.targetY-this.api.Y);

            local mapRad1=this.api.ToMapRad(math.atan2(v1X_,v1Y_));
            local mapRad2=this.api.ToMapRad(math.atan2(v2X_,v2Y_));
            local len=math.sqrt(this.w^2+this.h^2);

            v1X=math.cos(mapRad1)*len;
            v1Y=math.sin(mapRad1)*len;
            v2X=math.cos(mapRad2)*len;
            v2Y=math.sin(mapRad2)*len;
        end
        if this:isNeed() then
            local vx,vy = this.count % 2 == 0 and this.v1X,this.v1Y or this.v2X,this.v2Y;
            this.api.X=this.api.X+vx;
            this.api.Y=this.api.Y+vy;

            this.count_1=this.count_1+1;
            --今回の移動が終わったなら(最初と最後は半分しか移動しない)
            if this.count_1>=(this.count==0 or this.count==this.number and this.f_1/2 or this.f_1) then
                this.count_1=0;
                this.count=this.count+1;
            end
        end
    end
};