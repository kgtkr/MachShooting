--[[
NAME=スネーク
HP=1500
IMAGE=resource/snake.png
R=15
CLASS=Snake
]]

package.path =  "script/lib/?.lua";
require("cmd");
require("mathEX");

Snake={
    new=function(api)
        local this={
            api=api,
            sync=cmd.Sync.new()
        };

        return setmetatable(this, {__index = Snake}); 
    end,

    update=function(this)
        if not this.sync:isNeed() then
            table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,500,this.api.R*3,255,0,0));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=30; end));
            table.insert(this.sync.list,cmd.ULMPlayer.new(this.api,10,function() return math.sqrt((this.api.PlayerX-this.api.X)^2+(this.api.PlayerY-this.api.Y)^2)/10; end));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=50; end));
            local async=cmd.Async.new();

            --1Fで変わる半径
            local fR = 5;
            table.insert(async.list,cmd.Circle.new(
                this.api,
                0,
                cmd.retFunc(math.pi * 2 / 60),
                300,
                function(count)
                    if count<=100 then
                        return count*fR;
                    elseif count<=200 then
                        return 100*fR;
                    else
                        return -(count-200)*fR+100*fR;
                    end
                end,

                function()
                    return this.api.X,this.api.Y;
                end
            ));

            local sync=cmd.Sync.new();
            for i = 1, 60 do
                table.insert(sync.list,cmd.Wait.new(4));
                table.insert(sync.list,cmd.Action.new(function()
                    local vx,vy=mathEX.objVec(this.api.X,this.api.Y,this.api.PlayerX,this.api.PlayerY,10);
                    this.api:ShotBullet(
                        this.api.X,this.api.Y,
                        30,
                        vx,vy,
                        0,
                        255,0,0
                    );
                end));
            end
            table.insert(async.list,sync);
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