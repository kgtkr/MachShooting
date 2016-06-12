ULM=function(api,vec,f){
    local this={};

    local count=0;

    local vx,vy,f=0;

    this.isNeed=function()
        return count < f;
    end

    this.draw=function()
    end

    this.update=function()
        if count==0 then
            vx,vy=vec();
            f=f();
        end
        if this.isNeed
            api.X=api.X+vx;
            api.Y=api.Y+vy;

            count=count+1;
        end
    end

    return this;
}