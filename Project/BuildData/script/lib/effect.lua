module("effect", package.seeall);

charge=function(obj,img,f,maxR,color,count)
    DX.SetDrawBright(color.R, color.G, color.B);
    local ex = 1 - (count / f);
    DX.DrawRotaGraph(obj.X, obj.Y, ex, obj.Rad,img, DX.TRUE);
    DX.SetDrawBright(255, 255, 255);
end;