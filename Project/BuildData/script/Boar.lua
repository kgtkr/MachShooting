package.path =  "script/lib/?.lua";
require("cmd");

Boar=function(api)
    local this={};
    local sync=cmd.Sync();
    local chargeImage=DX.LoadGraph("Data/Image/Effect/Charge.png");

    this.update=function()
        if not sync.isNeed() then
            table.insert(sync.list,cmd.Charge(api,chargeImage,300,api.R*3,255,0,0));
            table.insert(sync.list,cmd.Action(function()api.Power=20; end));
            table.insert(sync.list,cmd.ULM(
                api,
                function()
                    local vx,vy=api.MyX-api.X,api.MyY-api.Y;
                    local vLen=math.sqrt(vx^2+vy^2);
                    if vLen~=0 then
                        vx=vx/vLen*10;
                        vy=vy/vLen*10;
                    end

                    return vx,vy;
                end,
                cmd.retFunc(60)
            ));
            table.insert(sync.list,cmd.Action(function()api.Power=0; end));
        end
        sync.update();
    end

    this.draw=function()
        sync.draw();
        api:Draw();
    end

    this.dispose=function()
        DX.DeleteGraph(chargeImage);
    end


    return this;
end