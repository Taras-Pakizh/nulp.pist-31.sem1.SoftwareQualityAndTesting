Run("notepad")
Local $notepadWin = WinWaitActive("Untitled - Notepad")
WinMove($notepadWin, "", -10, 0)
ControlSend("Untitled - Notepad", "", "[CLASS:Edit;INSTANCE:1]", "qwe 12 @@")

$text = ControlGetText("Untitled - Notepad", "", "[CLASS:Edit;INSTANCE:1]")

$lines = StringSplit($text, " ", 1)
$words = 0
$letters = 0
$symbols = 0
$nums = 0
For $i = 1 To $lines[0] Step +1
   $words = $words + 1
   If StringRegExp($lines[$i], "[0-9]") = 1 Then
	  $nums = $nums + 1
   ElseIf StringRegExp($lines[$i], "[a-zA-Z]") = 1 Then
	  $letters = $letters + 1
   Else
	  $symbols = $symbols + 1
   EndIf
Next
MsgBox(0, 'Result: ', "Words:" & $words & @CRLF & "Letters: " & $letters & @CRLF & "Symbols:" & $symbols & @CRLF & "Nums:" & $nums)