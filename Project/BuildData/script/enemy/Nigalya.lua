--[[
NAME=ネガリャー
HP=2000
IMAGE=resource/nigalya.png
R=29
CLASS=Nigalya
]]

package.path =  "script/lib/?.lua";
require("cmd");
require("mathEX");

Nigalya={
    new=function(api)
        local this={
            api=api,
            sync=cmd.Sync.new()
        };

        return setmetatable(this, {__index = Nigalya}); 
    end,

    update=function(this)
        if not this.sync:isNeed() then
            table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,300,this.api.R*3,255,0,0));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=40; end));
            table.insert(this.sync.list,cmd.ULMPlayer.new(this.api,10,cmd.retFunc(30)));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=0; end));
            local async=cmd.Async.new();
            for i = 1, 36 do
                local j=i;
                table.insert(async.list,cmd.Action.new(function()
                    local vx,vy=mathEX.radLenVec(mathEX.toRad(j*10),10);
                    this.api:ShotBullet(
                        this.api.X,this.api.Y,
                        50,
                        vx,vy,
                        2,
                        255,0,0
                    ); 
                end));
            end
            table.insert(this.sync.list,async);
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