module("base", package.seeall);
--ベースクラス

PlayerBase={
    new=function(player)
        local this={player};
        return setmetatable(this, {__index = PlayerBase}); 
    end,
    draw=function(this,start)
    end,
    update=function(this,start)
    end,
    dispose=function(this,start)
    end,
    normal=function(this,start)
        this.player:SetActionNone();
    end,
    special=function(this,start)
        this.player:SetActionNone();
    end,
    killer=function(this,start)
        this.player:SetActionNone();
    end,
    doping=function(this,start)
        if start then
            this.player.DopingTime=this:_dopingTime;
        else
            this.player.DopingTime=this.player.DopingTime-1;
        end
    end,
    counter=function(this,start)
        this.player:SetActionNone();
    end
};

PlayerCoBase={
    new=function(player)
        local this=PlayerBase.new(player);
        this._co=nil;
        return setmetatable(this, {__index = PlayerCoBase}); 
    end,
    normal=function(this,start)
        this:_coFunc(start,_normal);
    end,
    special=function(this,start)
        this:_coFunc(start,_special);
    end,
    killer=function(this,start)
        this:_coFunc(start,_killer);
    end,
    counter=function(this,start)
        this:_coFunc(start,_counter);
    end,
    _coFunc=function(this,start,func)
        if start then
            this._co=coroutine.create(func);
        else
            coroutine.resume(this._co);
        end
    end
};
setmetatable(PlayerCoBase, {__index = PlayerBase});

EnemyBase={
    new=function(enemy)
        local this={player};
        return setmetatable(this, {__index = EnemyBase});
    end,
    update=function(this)
    end,
    draw=function(this)
        this.enemy:DrawEnemy();
    end,
    dispose=function(this)
    end
};

EnemyCoBase={
    new=function(enemy)
        local this=EnemyBase.new(enemy);
        this._co=coroutine.create(_update);
        return setmetatable(this, {__index = EnemyCoBase}); 
    end,
    update=function(this)
        coroutine.resume(this._co);
    end
};
setmetatable(EnemyCoBase, {__index = EnemyBase});

