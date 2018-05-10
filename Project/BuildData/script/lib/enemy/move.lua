module("move", package.seeall);


circle=function(enemy,rad,radSpeedFunc,rFunc,f)
    local x,y=enemy.X,enemy.Y;
    for i = 0, f-1 do
        local v=Vec.FromRadLength(rad,rFunc(i));
        enemy.X=x+v.x;
        enemy.Y=y+v.y;

        rad=rad+radSpeedFunc(i);
        coroutine.yield();
    end
end;

ulm=function(enemy,vec,f)
    for i = 1, f do
        enemy.Move(vec);
        coroutine.yield();
    end
end;

ulmPlayer=function(enemy,speed,f)
    local v=Vec(enemy.PlayerX-enemy.X,enemy.PlayerY,enemy.Y);
    v.Length=speed;
    ulm(enemy,v,f);
end;

zigzag=function(enemy,target,w,h,f,count)
    --ベクトル計算
    local v1,v2=Vec(w,h),Vec(-w,h);
    enemy.Rad=Vec(target.X-enemy.X,target.Y-enemy.Y).Rad;

    v1.Rad=enemy:ToMapRad(v1.Rad);
    v2.Rad=enemy:ToMapRad(v2.Rad);

    for i = 1, count+1 do
        local v=i%2==0 and v1 or v2;
        local isHalf=(i==1 or i==count+1);
        for j = 1, isHalf and f/2 or f do
            enemy.Move(v);
            coroutine.yield();
        end
    end
end;