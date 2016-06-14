--[[
NAME=ボア
HP=1500
IMAGE=Data/Image/Enemy/boar.png
R=15
CLASS=Boar
]]

package.path =  "script/lib/?.lua";
require("cmd");

Boar={
    new=function(api)
        local this={
            api=api,
            sync=cmd.Sync.new()
        };

        return setmetatable(this, {__index = Boar}); 
    end,

    update=function(this)
        if not this.sync:isNeed() then
            table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,300,this.api.R*3,255,0,0));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=20; end));
            table.insert(this.sync.list,cmd.ULM.new(
                this.api,
                function()
                    local vx,vy=this.api.PlayerX-this.api.X,this.api.PlayerY-this.api.Y;
                    local vLen=math.sqrt(vx^2+vy^2);
                    if vLen~=0 then
                        vx=vx/vLen*10;
                        vy=vy/vLen*10;
                    end

                    return vx,vy;
                end,
                cmd.retFunc(60)
            ));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=0; end));
        end
        this.sync:update();
    end,

    draw=function(this)
        this.sync:draw();
        this.api:Draw();
    end,

    dispose=function(this)
    end,
};