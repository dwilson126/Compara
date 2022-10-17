Option Explicit On

Imports VB = Microsoft.VisualBasic

Imports System
Imports System.Text
Module ModuleStartUp
    Public MainForm As New frmMain()

    Const sCompanyName As String = "DocuFi"
    Const sProductName As String = "DocuFi"

    Public mvarTagFontSize As Integer = 22
    Public mvarDrawColor As Color
    Public mvarBaseColor As Color = Color.Red
    Public mvarOverlayColor As Color = Color.Green
    Public mvarUnits As Integer = 1
    Public Structure MyBatch
        Dim SaveAsPDFA As Boolean
        Dim PDFDisallowPrint As Boolean
        Dim PDFDisallowCutCopy As Boolean
        Dim PDFDisallowModify As Boolean
        Dim PDFLinearize As Boolean
        Dim PDFEncrypt As Boolean
        Dim PDFEncryptType As Integer
        Dim PDFPassword As String
        Dim PDFUserPassword As String
        Dim PDFOwnerPassword As String
    End Structure
    Public batchProps As New MyBatch


    Public Sub main(ByVal sArgs() As String)
        Try

  
        If sArgs.Length = 0 Then                'If there are no arguments
                Console.WriteLine("ComparA! <-no arguments passed->") 'Just output Hello World
        Else                                    'We have some arguments		
                Dim i As Integer = 0

                Dim CommandLine() As String = System.Environment.GetCommandLineArgs()

                Dim blnNextIsOverlay As Boolean = False
                Dim blnNextIsPublish As Boolean = False

                For Each s As String In sArgs
                    If blnNextIsOverlay Then
                        mvarOverlayFile = s
                        blnNextIsOverlay = False
                    ElseIf blnnextispublish Then
                        mvarPublishFile = s
                        blnNextIsPublish = False
                    ElseIf s.ToLower = "-noopen" Then
                        DocuFiSession.isNoOpen = True
                    ElseIf s.ToLower = "-single" Or s.ToLower = "-nooverlay" Then
                        DocuFiSession.isSingle = True
                    ElseIf s.ToLower.StartsWith("-o") Then
                        'got the output file


                        blnNextIsOverlay = True

                    ElseIf s.ToLower.StartsWith("-p") Then
                        'got the publish file

                        blnNextIsPublish = True

                    Else
                        DocuFiSession.openBaseFile = True
                        mvarCurrentFile = s

                    End If

                Next

            End If

        Application.Run(MainForm)

        Catch ex As Exception
            MsgBox("Warning, unknown exception in mainform" & ex.Message)
        End Try
    End Sub

End Module
