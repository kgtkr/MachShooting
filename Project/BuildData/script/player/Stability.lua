--[[
NAME=通常型
KG=500
DG=1000
CLASS=Stability
]]

Stability={
    new=function(api)
        local this={
            api=api,
            --その他メンバ変数
        };
        --初期化処理
        return setmetatable(this, {__index = Stability}); 
    end,

    dispose=function(this)
        --リソース解放処理
        
    end,
 
    draw=function(this)
        --描画処理、任意
        
    end,
 
    normal=function(this)
        --通常攻撃
    end,
 
    special=function(this)
        --特殊攻撃
    end,
 
    killer=function(this)
        --必殺技
    end,
 
    doping=function(this)
        --自己強化
    end,
 
    counter=function(this)
        --カウンター攻撃
    end
};