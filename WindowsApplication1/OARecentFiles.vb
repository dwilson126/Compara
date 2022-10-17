Option Strict Off
Option Explicit On
Module Module1
	
	Declare Function WinHelp Lib "user32"  Alias "WinHelpA"(ByVal Hwnd As Integer, ByVal lpHelpFile As String, ByVal wCommand As Integer, ByVal dwData As String) As Integer
	
	Public Const HELP_QUIT As Short = 2
	Public Const HELP_INDEX As Short = 3
	Public Const HELP_HELPONHELP As Short = 4
	Public Const HELP_PARTIALKEY As Short = &H105s
	
	Sub HelpFunction(ByRef lhWnd As Integer, ByRef HelpCmd As Short, ByRef HelpKey As String)
		
		Dim lRtn As Integer 'declare the needed variables
		Dim tmpHelpFile As String ' variable for name of the help file
        tmpHelpFile = "\RevisA.hlp"
		
        If IO.File.Exists(My.Application.Info.DirectoryPath & tmpHelpFile) Then

            If HelpCmd = HELP_PARTIALKEY Then
                lRtn = WinHelp(lhWnd, My.Application.Info.DirectoryPath & tmpHelpFile, HelpCmd, HelpKey)
            Else
                lRtn = WinHelp(lhWnd, My.Application.Info.DirectoryPath & tmpHelpFile, HelpCmd, CStr(0))
            End If
        Else
            MsgBox("The Help File is not installed.  If you would like to obtain it, " & vbCrLf & "visit www.openarchive.com\downloads\Rasterrenewhelp.zip", MsgBoxStyle.Information)
        End If
	End Sub

	
	Sub UpdateFileMenu(ByRef filename As Object)
		Dim intRetVal As Short
		' Check if the open filename is already in the File menu control array.
        'intRetVal = OnRecentFilesList(filename)
		If Not intRetVal Then
			' Write open filename to the registry.
			WriteRecentFiles((filename))
		End If
		' Update the list of the most recently opened files in the File menu control array.
		GetRecentFiles()
	End Sub
	Sub GetRecentFiles()
		' This procedure demonstrates the use of the GetAllSettings function,
		' which returns an array of values from the Windows registry. In this
		' case, the registry contains the files most recently opened.  Use the
		' SaveSetting statement to write the names of the most recent files.
		' That statement is used in the WriteRecentFiles procedure.
		Dim I As Short
		Dim varFiles As Object ' Varible to store the returned array.
		
		' Get recent files from the registry using the GetAllSettings statement.
		' ThisApp and ThisKey are constants defined in this module.
		'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If IsNothing(GetSetting(ThisApp, ThisKey, "RecentFile1")) Then Exit Sub
		Dim iCount As Short
		
		'UPGRADE_WARNING: Couldn't resolve default property of object varFiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		varFiles = GetAllSettings(ThisApp, ThisKey)
		iCount = 1
        For I = 1 To UBound(varFiles, 1)

            'frmMain.mnuRecentFile(1).Visible = True
            ''UPGRADE_WARNING: Couldn't resolve default property of object varFiles(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'If IO.File.Exists(varFiles(I, 1)) Then
            '    'UPGRADE_WARNING: Couldn't resolve default property of object varFiles(I, 1). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            '    'UPGRADE_WARNING: Couldn't resolve default property of object varFiles(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            '    frmMain.mnuRecentFile(iCount).Text = varFiles(I, 1)
            '    frmMain.mnuRecentFile(iCount).Visible = True
            '    iCount = iCount + 1
            'End If
        Next I
		
	End Sub
	Sub WriteRecentFiles(ByRef OpenFileName As Object)
		' This procedure uses the SaveSettings statement to write the names of
		' recently opened files to the System registry. The SaveSetting
		' statement requires three parameters. Two of the parameters are
		' stored as constants and are defined in this module.  The GetAllSettings
		' function is used in the GetRecentFiles procedure to retrieve the
		' file names stored in this procedure.
		
		Dim I As Short
		Dim strFile As Object
		Dim Key As String
		
		
		' Copy RecentFile1 to RecentFile2, and so on.
		For I = 3 To 1 Step -1
			Key = "RecentFile" & I
			'UPGRADE_WARNING: Couldn't resolve default property of object strFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strFile = GetSetting(ThisApp, ThisKey, Key)
			'UPGRADE_WARNING: Couldn't resolve default property of object strFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If strFile <> "" Then
                'frmMain.mnuRecentFile(I + 1).Text = frmMain.mnuRecentFile(I).Text
                'Key = "RecentFile" & (I + 1)
                'frmMain.mnuRecentFile(I + 1).Visible = True
                ''UPGRADE_WARNING: Couldn't resolve default property of object strFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'SaveSetting(ThisApp, ThisKey, Key, strFile)
			End If
		Next I
		'UPGRADE_WARNING: Couldn't resolve default property of object OpenFileName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'frmMain.mnuRecentFile(1).Text = OpenFileName
        'frmMain.mnuRecentFile(1).Visible = True
		' Write the open file to first recent file.
		'UPGRADE_WARNING: Couldn't resolve default property of object OpenFileName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		SaveSetting(ThisApp, ThisKey, "RecentFile1", OpenFileName)
	End Sub
End Module