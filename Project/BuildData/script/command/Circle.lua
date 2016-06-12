Circle=function(api,defaultRad,radSpeed,f,r,dot)
    local this={};

    local count=0;
    local rad=defaultRad:
    local x=0;
    local y=0;

    this.isNeed=function
        return count < f;
    end

    this.draw=function
    end

    this.update=function
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