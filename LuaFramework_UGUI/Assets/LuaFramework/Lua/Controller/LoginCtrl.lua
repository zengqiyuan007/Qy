
LoginCtrl = {};
local this = LoginCtrl;

local login;
local transform;
local gameObject;

--构建函数--
function LoginCtrl.New()
	logWarn("LoginCtrl.New--->>");
	return this;
end

function LoginCtrl.Awake()
	logWarn("LoginCtrl.Awake--->>");
	panelMgr:CreatePanel('Login', this.OnCreate);
end

--启动事件--
function LoginCtrl.OnCreate(obj)
	gameObject = obj;

	login = gameObject:GetComponent('LuaBehaviour');
    login:AddClick(LoginPanel.btnOpen, this.OnClick);
	logWarn("Start lua--->>"..gameObject.name);
end

--单击事件--
function LoginCtrl.OnClick(go)
	local ctrl = CtrlManager.GetCtrl(CtrlNames.Prompt);
    if ctrl ~= nil and AppConst.ExampleMode == 1 then
        ctrl:Awake();
    end
end

--关闭事件--
function LoginCtrl.Close()
	panelMgr:ClosePanel(CtrlNames.Login);
end