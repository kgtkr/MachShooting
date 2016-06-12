import ("DxLibDotNet","DxLibDLL");

Charge=function(api,image,f,maxR,r,g,b)
    local this={};

    local count=0;

    this.isNeed=function()
        return this.count < this.f;
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