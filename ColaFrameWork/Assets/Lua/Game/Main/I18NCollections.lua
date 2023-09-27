---
---                 ColaFramework
--- Copyright © 2018-2049 ColaFramework 马三小伙儿
---                i18n多语言化初始化入口
---

local i18n = require("3rd.i18n.init")

-- 加载各种文本的配置文件
i18n.loadFile("LuaConfigs/i18n/chs.lua")
i18n.loadFile("LuaConfigs/i18n/en.lua")

-- 设置当前本地化语言
i18n.setLocale("chs")

return i18n