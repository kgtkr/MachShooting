--拡張コルーチン
CoroMold={
    new=function(func)
        local this={_func=func};
        return setmetatable(this, {__index = CoroMold}); 
    end,
    create=function(this,...)
        return _CoroIterator.new(this._func,table.unpack({...}));
    end
};

_CoroIterator={
    new=function(func,...)
        local this={coro=coroutine.create(func),params={...}};
        return setmetatable(this, {__index = _CoroIterator}); 
    end,
    next=function(this)
        if coroutine.status(this.coro)~="dead" then
            if this.params~=nil then
                local params=this.params;
                this.params=nil;
                return coroutine.resume(this.coro,table.unpack(params));
            else
                return coroutine.resume(this.coro);
            end
        else
            return false;
        end
    end,
    yield=function(this)
        while true do
            local result=table.pack(this.next());
            if result[1]==true then
                table.remove(result, 1);
                coroutine.yield(table.unpack(result));
            else
                return;
            end
        end
    end
};