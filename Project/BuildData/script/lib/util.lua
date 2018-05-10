module("util", package.seeall);

retFunc=function(val)
    return (function()
        return val;
    end);
end;

wait=function(f)
    for i = 1, f do
        coroutine.yield();
    end
end;