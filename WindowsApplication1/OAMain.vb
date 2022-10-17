Option Strict Off
Option Explicit On
Imports System.Text
Imports System.IO
Module OAMain
    Public Structure DocuFiSessionDef
        Dim Authorized As Boolean
        Dim OEM As String
        Dim HasAddon As Boolean
        Dim LicenseDB As String
        Dim LicenseKey As String
        Dim IsEvaluation As Boolean
        Dim Enhance As Boolean
        Dim PDFConvert As Boolean
        Dim EraseMode As Boolean
        Dim MultiFileConvert As Boolean
        Dim CropMode As Boolean
        Dim isSingle As Boolean
        Dim isCompare As Boolean
        Dim isNoOpen As Boolean
        Dim openBaseFile As Boolean
        Dim MeasureMode As Integer
        Dim IsPDFTrans As Boolean
        Dim isMeasure As Boolean
        Dim IsPublish As Boolean
        Dim IsMultipage As Boolean
        Dim ThreshMode As Integer
        Dim ExtractTIF As Boolean
        Dim InvertExtraction As Boolean
        Dim needsActivation As Boolean
    End Structure

    Public DocuFiSession As New DocuFiSessionDef


    Public Structure myBatch
        Dim [Continue] As Boolean
        Dim PagesPer As Short
        Dim OutputRes As Short
        Dim PDFResolution As Integer
        Dim PDFMode As Short
        Dim UseSourceFolder As Boolean
        'Dim UseBaseName As Boolean
        'Dim NameMask As String
        Dim DestFolder As String
        Dim BackupFolder As String
        Dim DeleteSourceFile As Boolean
        Dim SourceFolder As String
        Dim SplitMode As Short
        Dim RemovePDFPages As Boolean
        Dim BlankPageSize As Double
        Dim BorderSize As Double
        Dim SpeckleSize As Integer
        Dim LineRemovalMode As Short
        Dim LineCurvature As Short
        Dim GapSize As Double
        Dim MinimalLine As Double
        Dim LineAngle As Double
        Dim RemoveLines As Boolean
        Dim AutoCrop As Boolean
        Dim AutoInvert As Boolean
        Dim MorphDirection As Short
        Dim RemoveNoise As Boolean
        Dim RemoveBorder As Boolean
        Dim RemovePunchHoles As Boolean
        Dim RemoveBlankPages As Boolean
        Dim RemoveSeparationSheet As Boolean
        Dim RemoveHalfTones As Boolean
        Dim SmoothCharacters As Boolean
        Dim StraightenPage As Boolean
        Dim AutoRotate As Boolean
        Dim Deskew As Boolean
        Dim LineMode As Short
        Dim MultiPage As Short
        Dim ThickThin As Short
        Dim Rotation As Short
        Dim ObjectSize As Integer
        Dim LineCurvatureMode As Integer
        Dim SavetoPDF As Boolean
        Dim OCRtoPDF As Boolean
        Dim SplitNativePDF As Boolean
        Dim PDFGreyscale As Boolean
        Dim PDFColor As Boolean
        Dim OnlySplitBCContains As Boolean
        Dim BCContainsText As String
        Dim InvertText As Boolean
        Dim AppendToExisting As Boolean
        Dim SequenceFileName As Boolean
        Dim AdvClarify As Boolean
        Dim FormClarify As Boolean
        Dim AdvThreshold As Integer
        Dim AdvSensitivity As Integer
        Dim Layout As String
        Dim OCRMode As Integer
        Dim OCRLanguage As String
        Dim PDFAllowPrint As Boolean
        Dim PDFAllowCutCopy As Boolean
        Dim PDFAllowFormsFill As Boolean
        Dim PDFAllowModify As Boolean
        Dim PDFLinearize As Boolean
        Dim PDFEncrypt As Boolean
        Dim PDFEncryptType As Integer
        Dim ZoomMode As Integer
        Dim OCRTextIsThick As Boolean
        Dim OCRRemoveGridLines As Boolean
        Dim OCRUseArchitecturalRules As Boolean
        Dim OCRReplaceDOts As Boolean
    End Structure
    Public oaBatch As New myBatch

    Public mvarCurrentFile As String
    Public mvarOverlayFile As String
    Public mvarPublishFile As String 'holder for the outboundpublishing file
    Public mvarLicenseMsg As String
    Public mvarLicense As String
    Public moAuthorProperty As String
	Public Debugger As Boolean
    Public mvarPDFProperties As String
    Public gSourceisPDF As Boolean
    Public G_OpenFlag As Boolean
    Public G_viewCompareMode As String
    Public DemoMode As Boolean
    Public I_PlotQueue As Boolean
    Public SensitivityValue As Integer
    Public ThresholdValue As Integer

	'global redliner variables
	Public OARedlineAutoLoad As Boolean ' flag to autoload redlines
	Public G_PreloadExplorer As Boolean 'global variable for small/large db handling
	Public G_HasChanges As Boolean 'flag to say we have active redlines
	Public G_HasNewEdits As Boolean
	
	Public DebugFlag As Short
	Public DBDebug As Short
	Public LastDebug As String
	Public g_ToolWidth As Double
	
    Public mvarRemainingDays As Integer
	'global for storing target directory to save and copy files
	Public G_CopyToDirectory As String
	
	'XML variables
	Public G_XMLPath As String
	Public G_XMLPUBPath As String
	
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal lSize As Integer, ByVal lpfilename As String) As Integer
	Declare Function WritePrivateProfileString Lib "kernel32"  Alias "WritePrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lplFileName As String) As Integer
    Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
	'
	Private Const MAX_PATH As Short = 260
    Public g_Language As String
	Private Declare Function GetPrivateProfileInt Lib "kernel32"  Alias "GetPrivateProfileIntA"(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpfilename As String) As Integer
    Public Const FeatCompare As Integer = 1 << 0
    Public Const FeatMeasure As Integer = 1 << 1
    Public Const FeatPublish As Integer = 1 << 2
    Public Const FeatPDFTrans As Integer = 1 << 3
    Public Const FeatMultiPage As Integer = 1 << 4
    Public Const ThisApp As String = "SP Renamer"
    Public Const ThisKey As String = "Recent Files"
    Private Declare Function GetWindowsDirectory Lib "
kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer

    Public Function GetWindowsDir() As Object

        Dim Buffer As String
        Dim L As Integer

        GetWindowsDir = ""
        Buffer = Space(255)
        L = GetWindowsDirectory(Buffer, Len(Buffer))
        If L Then
            GetWindowsDir = Left(Buffer, L)
        End If

    End Function

    Public Function APP_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory

    End Function
    Public Function User_Path() As String
        Return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    End Function
    Public Sub WriteLog(ByVal strText As String)
        Try
            Dim LogFolder As String
            Dim LogFile As String
             
            LogFolder = APP_Path() & "\ProcessLog"
            LogFile = "Log_" & getSysDate() & ".txt"

            If Not IO.Directory.Exists(LogFolder) Then
                IO.Directory.CreateDirectory(LogFolder)
            End If

            Dim LogFileFullPath As String = LogFolder & "\" & LogFile

            Dim objWrite As TextWriter
            objWrite = New StreamWriter(LogFileFullPath, True)
            strText = vbNewLine & strText
            objWrite.Write(strText)
            objWrite.Close()
        Catch ex As Exception

        End Try

    End Sub
    Private Function getSysDate() As String
        Try
            Dim strDate As String
            Dim currDate As Date = Now()
            Dim sMonth As String
            Dim sDay As String
            If Month(currDate) < 10 Then
                sMonth = "0" & CStr(Month(currDate))
            Else
                sMonth = CStr(Month(currDate))
            End If
            If Microsoft.VisualBasic.Day(currDate) < 10 Then
                sDay = "0" & CStr(Microsoft.VisualBasic.Day(currDate))
            Else
                sDay = CStr(Microsoft.VisualBasic.Day(currDate))
            End If
            strDate = CStr(Year(currDate)) & "-" & sMonth & "-" & sDay
            Return strDate
        Catch ex As Exception
            Return ""
        End Try


    End Function
    
	Public Sub OAMsgBox(ByRef inMsg As String)
		
		If DemoMode Then
			MsgBox("This function is not available in the Trial Mode ")
		Else
			MsgBox(inMsg, MsgBoxStyle.Exclamation)
		End If
		
	End Sub
	
	
	Public Sub WriteDebug(ByRef Buffer As String)
		
		On Error GoTo errorhandle
		Dim FileNum As Short
		
		FileNum = FreeFile '-- Get the next free file handle
		FileOpen(FileNum, My.Application.Info.DirectoryPath & "\" & "REnewviewdebug.txt", OpenMode.Append) '-- Open the file
		PrintLine(FileNum, "   " & Buffer) '-- Write the buffer
		FileClose(FileNum)
		Exit Sub
errorhandle: 
		Err.Raise(2583, "WriteDebug", Err.Description)
		
	End Sub
	Public Sub RaiseError(ByRef vlErrorNumber As Integer, ByRef vsSource As String, ByRef vsDescription As String)
		
		Err.Raise(vlErrorNumber, vsSource, vsDescription)
		
		If Debugger Then WriteDebug("Error in " & vsSource & " " & vsDescription)
    End Sub

    Public Function SaveINISetting(ByRef sIniFile As String, ByRef sSection As String, ByRef sKey As String, ByRef sValue As String) As Integer
        SaveINISetting = PutPPString(sSection, sKey, sValue, sIniFile)
    End Function

    Private Function PutPPString(ByRef AppName As String, ByRef KeyName As String, ByRef KeyVal As String, ByRef PPFILE As String) As Boolean
        'Writes a private profile string parameter
        PutPPString = WritePrivateProfileString(AppName, KeyName, KeyVal, PPFILE)
    End Function

    Public Function GetINISetting(ByRef sIniFile As String, ByRef sSection As String, ByRef sKey As String) As String
        GetINISetting = GetPPString(sSection, sKey, sIniFile)
    End Function

    Private Function GetPPString(ByRef AppName As String, ByRef KeyName As String, ByRef PPFILE As String) As String
        Dim SO As String
        'Returns the contents of a private profile string paramter
        Dim lReturn As Integer
        SO = New String(Chr(0), 255)
        lReturn = GetPrivateProfileString(AppName, KeyName, "", SO, 255, PPFILE)
        GetPPString = Left(SO, lReturn)
    End Function

    Public Function DeleteINISection(ByVal sIniFile As String, ByVal sSection As String) As Boolean
        DeleteINISection = PutPPString(sSection, vbNullString, vbNullString, sIniFile)
    End Function


    Public Function OASaveProfile(ByRef inProfile As String) As Boolean 'new module		
        Dim outf As Short
        Dim strdata As String
        Dim inifile As String

        inifile = My.Application.Info.DirectoryPath & "\SPRenamer.ini"
        On Error GoTo errorhandle
        'conversion settings
        SaveINISetting(inifile, inProfile, "SavetoPDF", CStr(oaBatch.SavetoPDF))
        SaveINISetting(inifile, inProfile, "OCRRemoveGridLines", CStr(oaBatch.OCRRemoveGridLines))
        SaveINISetting(inifile, inProfile, "ReplaceDots", CStr(oaBatch.OCRReplaceDOts))
        SaveINISetting(inifile, inProfile, "OCRTextisThick", CStr((oaBatch.OCRTextIsThick)))
        SaveINISetting(inifile, inProfile, "ZoomMode", CStr((oaBatch.ZoomMode)))
        SaveINISetting(inifile, inProfile, "UseArchitecturalRules", CStr(oaBatch.OCRUseArchitecturalRules))

        SaveINISetting(inifile, inProfile, "AutoDeskew", CStr((oaBatch.StraightenPage)))
        SaveINISetting(inifile, inProfile, "RemoveNoise", CStr((oaBatch.RemoveNoise)))
        SaveINISetting(inifile, inProfile, "AutoCrop", CStr((oaBatch.AutoCrop)))
        'SaveINISetting(inifile, inProfile, "RemoveBorder", CStr((oaBatch.RemoveBorder)))
        'SaveINISetting(inifile, inProfile, "RemoveLines", CStr((oaBatch.RemoveLines)))
        SaveINISetting(inifile, inProfile, "SpeckleSize", CStr((oaBatch.SpeckleSize)))
        SaveINISetting(inifile, inProfile, "BorderSize", CStr((oaBatch.BorderSize)))
        SaveINISetting(inifile, inProfile, "SourceFolder", oaBatch.SourceFolder)


        SaveINISetting(inifile, inProfile, "Threshold", ThresholdValue)
        SaveINISetting(inifile, inProfile, "Sensitivity", SensitivityValue)


        OASaveProfile = True

        Exit Function
errorhandle:
        OASaveProfile = False
    End Function

    Public Sub LoadSettings(ByVal inProfile As String)

        Dim inifile As String
        Dim tmpString As String

        On Error GoTo errorhandle

        SensitivityValue = 30
        ThresholdValue = 125

        inifile = My.Application.Info.DirectoryPath & "\SPRenamer.ini"

        'OABatch.UseBaseName = GetINISetting(inifile, inProfile, "UseBaseName")
        oaBatch.SavetoPDF = GetINISetting(inifile, inProfile, "SavetoPDF")
        oaBatch.SourceFolder = GetINISetting(inifile, inProfile, "SourceFolder")

        oaBatch.OCRRemoveGridLines = CBool(GetINISetting(inifile, inProfile, "OCRRemoveGridLines"))
        oaBatch.OCRTextIsThick = CBool(GetINISetting(inifile, inProfile, "OCRTextisThick"))
        oaBatch.OCRReplaceDOts = CBool(GetINISetting(inifile, inProfile, "ReplaceDots"))
        oaBatch.OCRUseArchitecturalRules = CBool(GetINISetting(inifile, inProfile, "UseArchitecturalRules"))

        tmpString = CStr(GetINISetting(inifile, inProfile, "zoomMode"))
        If IsNumeric(tmpString) Then
            oaBatch.ZoomMode = CInt(tmpString)
        End If

        'image enhancement settings

        oaBatch.SpeckleSize = CLng(GetINISetting(inifile, inProfile, "SpeckleSize"))
        oaBatch.BorderSize = CLng(GetINISetting(inifile, inProfile, "BorderSize"))
        oaBatch.GapSize = CDbl(GetINISetting(inifile, inProfile, "GapSize"))
        oaBatch.MinimalLine = CDbl(GetINISetting(inifile, inProfile, "LineLength"))
        oaBatch.LineAngle = CDbl(GetINISetting(inifile, inProfile, "LineAngle"))

        tmpString = CStr(GetINISetting(inifile, inProfile, "Sensitivity"))
        If IsNumeric(tmpString) Then SensitivityValue = CLng(tmpString)

        tmpString = CStr(GetINISetting(inifile, inProfile, "Threshold"))
        If IsNumeric(tmpString) Then ThresholdValue = CLng(tmpString)
  

        oaBatch.StraightenPage = CBool(GetINISetting(inifile, inProfile, "AutoDeskew"))
        'OABatch.SmoothChars = CInt(GetINISetting(inifile, inProfile, "SmoothCharacters"))
        oaBatch.RemoveNoise = CBool(GetINISetting(inifile, inProfile, "RemoveNoise"))
        oaBatch.RemoveBorder = CBool(GetINISetting(inifile, inProfile, "RemoveBorder"))

        'oaBatch.AutoRotate = CBool(GetINISetting(inifile, inProfile, "AutoRotate"))
        'oaBatch.AutoInvert = CBool(GetINISetting(inifile, inProfile, "AutoInvert"))

        oaBatch.AutoCrop = CBool(GetINISetting(inifile, inProfile, "AutoCrop"))

        Exit Sub
errorhandle:
        Err.Clear()
        Resume Next
    End Sub
  
  
End Module