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