Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmProperties
    Inherits System.Windows.Forms.Form
    
    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim tmpGD As New GdPicture14.GdPictureImaging
        Dim sourceID As Integer
        'Dim gdver As String = mygd.GetVersion
        Me.Text = "About" & " " & My.Application.Info.Title
        ' lblDocument.Text = MSGVersion & " " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Revision & vbCrLf & "ImgLibs:" & gdver

        lblTitle.Text = My.Application.Info.Title
        lblDocument.Text = IO.Path.GetFileName(mvarCurrentFile) & vbCrLf & IO.Path.GetFileName(mvarOverlayFile)

        Dim outString As String
        outString = "Source File: " & IO.Path.GetFileName(mvarCurrentFile)
        outString = outString & vbCrLf & mvarPDFProperties

        'Dim filetype As String = IO.Path.GetExtension(mvarCurrentFile).ToLower

        If IO.File.Exists(mvarOverlayFile) Then
            sourceID = tmpGD.TiffCreateMultiPageFromFile(mvarOverlayFile)
            'outString = "Out File: " & IO.Path.GetFileName(mvarOverlayFile)
            If IO.Path.GetExtension(mvarOverlayFile) = ".pdf" Then
                 
            Else
                ' outString = outString & vbCrLf & "Out PageCount: " & tmpGD.TiffGetPageCount(sourceID)
                'outString = outString & vbCrLf & "Out Resolution: " & tmpGD.GetHorizontalResolution(sourceID)
                ' outString = outString & vbCrLf & "Out Size: " & tmpGD.GetWidth(sourceID) & " by " & tmpGD.GetHeight(sourceID) & " pixels"

            End If
            'xxxx
        End If
        'If filetype = ".tif" Or filetype = ".jpg" Or filetype = ".tiff" Or filetype = ".jpeg" Then
        'sourceID = tmpGD.TiffCreateMultiPageFromFile(mvarCurrentFile)

        'outString = outString & vbCrLf & "PageCount: " & tmpGD.TiffGetPageCount(sourceID)
        'outString = outString & vbCrLf & "Resolution: " & tmpGD.GetHorizontalResolution(sourceID)
        'outString = outString & vbCrLf & "Size: " & tmpGD.GetWidth(sourceID) & " by " & tmpGD.GetHeight(sourceID)
        'Else

        ' End If
        lblProperties.Text = outString
        'tmpGD.ClearGdPicture()

    End Sub

    Private Sub frmAbout_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'UPGRADE_NOTE: Object frmAbout may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        Me.Dispose()
    End Sub

End Class