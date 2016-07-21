--[[
NAME=ガトー
HP=1500
IMAGE=res/enemy/Animal/Gato/main.png
R=15
CLASS=Gato
]]

Gato={
    new=function(api)
        local this={
            api=api,
            sync=cmd.Sync.new()
        };

        return setmetatable(this, {__index = Gato}); 
    end,

    update=function(this)
        if not this.sync:isNeed() then
            table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,300,this.api.R*3,255,0,0));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=30; end));
            table.insert(this.sync.list,cmd.Zigzag.new(this.api,
                function() return this.api.PlayerX,this.api.PlayerY end,
                10*this.api.Root2,10*this.api.Root2,
                4,cmd.retFunc(6)));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=0; end));
            table.insert(this.sync.list,cmd.ULM.new(
                this.api,
                function()
                    local rad=this.api:ToMapRad(-math.pi/2);
                    return math.cos(rad) * 10,
                    math.sin(rad) * 10;
                end,
                cmd.retFunc(20)
            ));
            table.insert(this.sync.list,cmd.Action.new(function()
                local vx,vy=mathEX.objVec(this.api.X,this.api.Y,this.api.PlayerX,this.api.PlayerY,10);
                this.api:ShotBullet(
                    this.api.X,this.api.Y,
                    20,
                    vx,vy,
                    0,
                    255,0,0
                ); 
            end));
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