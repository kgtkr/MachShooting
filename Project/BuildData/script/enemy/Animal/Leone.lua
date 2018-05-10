--[[
NAME=レオーネ
HP=3000
IMAGE=res/enemy/Animal/Leone/main.png
R=50
CLASS=Leone
]]

Leone={
    new=function(api)
        local this={
            api=api,
            sync=cmd.Sync.new()
        };

        return setmetatable(this, {__index = Leone}); 
    end,

    update=function(this)
        if not this.sync:isNeed() then
            local action = DX.GetRand(4);

            if action==0 then--突進
                table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,90,this.api.R*3,255,255,0));
                table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=30; end));
                table.insert(this.sync.list,cmd.ULMPlayer.new(this.api,10,cmd.retFunc(60)));
                table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=0; end));
            elseif action==1 then--連射
                table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,120,this.api.R*3,255,0,0));
                for i = 1, 12 do
                    table.insert(this.sync.list,cmd.Action.new(function()
                        local vx,vy=mathEX.objVec(this.api.X,this.api.Y,this.api.PlayerX,this.api.PlayerY,10);
                        this.api:ShotBullet(
                            this.api.X,this.api.Y,
                            30,
                            vx,vy,
                            2,
                            255,0,0
                        );
                    end));
                    table.insert(this.sync.list,cmd.Wait.new(10));
                end
            elseif action==2 then--円形
                table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,120,this.api.R*3,0,255,0));
                for j=1,12 do
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
                    table.insert(this.sync.list,cmd.Wait.new(10));
                end
            end
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