Wait=function(wait){
    local this={};

    //残り時間
    local limit=wait;

    this.isNeed=function()
        return limit>0;
    end

    this.draw=function()
    end

    this.update=function()
        if this.isNeed them
            limit=limit-1;
        end
    end

    return this;
}