Run("C:\projects\nulp.pist-11.sem1.OperatingSystems\lab_6\OS3\bin\Debug\OS3.exe")
Local $progWin = WinWaitActive("Form1")
WinMove($progWin, "", -10, 0)
MouseClick("left", 95, 75);кількість потоків
MouseClick("left", 95, 95);4
MouseClick("left", 266, 75);кількість елементів
Send("{NUMPAD1}")
Send("{NUMPAD0}");10
MouseClick("left", 190, 117);створити
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 90);1
MouseClick("left", 405, 105);low
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 105);2
MouseClick("left", 405, 105);low
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 115);3
MouseClick("left", 405, 130);bnorm
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 125);4
MouseClick("left", 405, 150);norm
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 135);5
MouseClick("left", 405, 150);norm
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 155);6
MouseClick("left", 405, 175);anorm
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 165);7
MouseClick("left", 405, 195);high
MouseClick("left", 470, 75);потік
MouseClick("left", 470, 175);8
MouseClick("left", 405, 195);high

MouseClick("left", 470, 270);запустити