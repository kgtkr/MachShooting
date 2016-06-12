--単純に値を返すだけの匿名関数を作る

retFunc=function(val)
    return function()
        return val;
    end
end

retFunc2=function(val1,val2)
    return function()
        return val1,val2;
    end
end