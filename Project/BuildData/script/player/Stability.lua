--[[
NAME=通常型
KG=500
DG=1000
CLASS=Stability
]]

Stability={
    new=function(player)
        local this= EnemyCoBase.new(player);
        this._dopingTime=900;
        return setmetatable(this, {__index = Stability}); 
    end,
    _normal=function(this)
        this.player:AddAO(Bullet(this.player.BulletDot,
        this.player.IsDoping and 14 or 12,
        this.player.ToMapRad(Vec(0,-10)),
        
        ));
    end,
    _special=function(this)
    end,
    _killer=function(this)
    end,
    _counter=function(this)
    end
};
setmetatable(Stability, {__index = EnemyCoBase});