Run("calc")
Local $calcWin = WinWaitActive("Calculator")
WinMove($calcWin, "", -10, 0)
ControlFocus($calcWin, "", "[CLASS:ApplicationFrameInputSinkWindow;INSTANCE:1]")

Send("{NUMPAD1}")
Send("{NUMPAD1}")
Send("{NUMPADADD}")
Send("{NUMPAD1}")
Send("{NUMPAD1}")
Send("{NUMPAD1}")
Send("{NUMPADENTER}")

$txt = "Очікуваний результат: 122"
MsgBox(0, "Result", $txt)