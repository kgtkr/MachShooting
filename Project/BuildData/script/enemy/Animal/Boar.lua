--[[
NAME=ボア
HP=1500
IMAGE=res/enemy/Animal/Boar/main.png
R=15
CLASS=Boar
]]

Boar={
    new=function(enemy)
        local this={
            enemy=enemy
        };

        return setmetatable(this, {__index = Boar}); 
    end,

    update=function(this)
        while true do
            util.wait(300);
            this.enemy.Power=30;
            move.ulmPlayer(this.enemy,10,60);
            this.enemy.Power=0;
        end
        if not this.sync:isNeed() then
            table.insert(this.sync.list,cmd.Charge.new(this.api,this.api.Image.Charge,300,this.api.R*3,255,0,0));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=20; end));
            table.insert(this.sync.list,cmd.ULMPlayer.new(this.api,10,cmd.retFunc(60)));
            table.insert(this.sync.list,cmd.Action.new(function()this.api.Power=0; end));
        end
        this.sync:update();
    end,

    draw=function(this)
        --チャージエフェクト
        this.enemy:DrawEnemy();
    end,

    dispose=function(this)
    end,
};