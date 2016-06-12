Async=function()
    local this={};
    local list={};
    this.isNeed=function()
        return table.maxn(this.list)~=0;
    end

    this.draw=function()
        table.foreach( list,
            function( index, item )
                item.update();
            end
        )
    end

    this.update=function()
        table.foreach( {unpack(list)},
        function( index, item )
            if item.isNeed() then
                item.update();
            end

            if not item.isNeed() then--必要ないなら削除
                table.remove(list, index);
            end
        end
        );
    end

    return this;
end