Sync=function()
    local this={};
    local list={};
    this.isNeed=function()
        return table.maxn(list)~=0;
    end

    this.draw=function()
        local loop = true;

        while loop do
            if this.isNeed() then
                if list[1].isNeed() then
                    this[1].draw();
                    loop = false;
                else--最初からNeed=falseの操作ならループする
                    loop = true;
                end

                if not list[1].isNeed() then--必要ないなら削除
                    table.remove(list, 1);
                end
            else
                loop = false;
            end
        end
    end

    this.update=function()
        local loop = true;

            while loop do
                if this.isNeed() then
                    if list[1].isNeed() then
                        loop = false;
                    else--最初からNeed=falseの操作ならループする
                        loop = true;
                    end

                    if not list[1].isNeed() then--必要ないなら削除
                        table.remove(list, 1);
                    end
                else
                    loop = false;
                end
            end
    end

    return this;
end