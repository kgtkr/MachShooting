module("ms", package.seeall);

APIBase={
    new=function(go)
        local this={
            _go=go,
            _ao={}
        };
        return setmetatable(this, {__index = APIBase}); 
    end,

    getX=function(this)
        return this._go.X;
    end,
    setX=function(this,val)
        this._go.X=val;
    end,

    getY=function(this)
        return this._go.Y;
    end,
    setY=function(this,val)
        this._go.Y=val;
    end,

    getR=function(this)
        return this._go.R;
    end,

    getRad=function(this)
        return this._go.Rad;
    end,

    getCount=function(this)
        return this._go.Count;
    end,

    isIn=function(this)
        return this._go.In;
    end,

    toMapRad=function(this,rad)
        return this._go:ToMapRad(rad);
    end,

    toObjectRad=function(this,rad)
        return this._go:ToObjectRad(rad);
    end,

    shotBullet=function(this,power,vec,size,color,call)
        local img=0;
        if size==BulletSize.SMALL then
            img=DXImage.Instance.BulletSmall;
        elseif size==BulletSize.MEDIUM then
            DXImage.Instance.BulletMedium;
        elseif size==BulletSize.BIG then
            DXImage.Instance.BulletBig;
        else
            DXImage.Instance.Bomb;
        end


        table.insert(this._ao,Bullet(
            {x=this:getX(),y=this:getY())},
            power,
            vec,
            img,
            color,
            call
        ));
    end
};

Player={
    new=function(player){
        local this=APIBase.new(player);

        return setmetatable(this, {__index = Player}); 
    },
    setAction=function(this,action)
        this._go.Action=action;
    end,
    --static
    Action=PlayerAction
};

BulletSize={
    SMALL=0,
    MEDIUM=1,
    BIG=2,
    BOMB=3
};