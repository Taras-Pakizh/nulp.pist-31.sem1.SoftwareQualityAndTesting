;Run("notepad")
;Local $notepadWin = WinWaitActive("Untitled - Notepad")
;WinMove($notepadWin, "", -10, 0)
$text = ControlGetText("Untitled - Notepad", "", "[CLASS:Edit;INSTANCE:1]")

$file = FileRead("text.txt")
$lines = StringSplit($file, @CRLF, 1)
$res = "Lines: "
For $i = 1 To $lines[0] Step +1
    If StringInStr($lines[$i], $text) Then $res = $res & $i & " "
	Next
MsgBox(0, 'Found ' & $text, $res)
ShellExecuteWait("text.txt")