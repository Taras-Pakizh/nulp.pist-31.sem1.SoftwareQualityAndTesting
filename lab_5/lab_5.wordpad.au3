;Run("C:\Program Files\windows nt\accessories\wordpad.exe")
;Local $wordpadWin = WinWaitActive("Document - WordPad")
;WinMove($wordpadWin, "", -10, 0)
$text = ControlGetText("Document - WordPad", "", "[CLASS:RICHEDIT50W;INSTANCE:1]")

$file = FileRead("text.txt")
$lines = StringSplit($file, @CRLF, 1)
$res = "Lines: "
For $i = 1 To $lines[0] Step +1
    If StringInStr($lines[$i], $text) Then $res = $res & $i & " "
	Next
MsgBox(0, 'Found ' & $text, $res)
ShellExecuteWait("text.txt")