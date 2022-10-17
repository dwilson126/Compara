Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmAboutPDFTrans
    Inherits System.Windows.Forms.Form
    Dim licensetemp As New QLM.LicenseValidator
    ' Reg Key Security Options...
    Const READ_CONTROL As Integer = &H20000
    Const KEY_QUERY_VALUE As Short = &H1S
    Const KEY_SET_VALUE As Short = &H2S
    Const KEY_CREATE_SUB_KEY As Short = &H4S
    Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
    Const KEY_NOTIFY As Short = &H10S
    Const KEY_CREATE_LINK As Short = &H20S
    Const KEY_ALL_ACCESS As Double = KEY_QUERY_VALUE + KEY_SET_VALUE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

    ' Reg Key ROOT Types...
    Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    Const ERROR_SUCCESS As Short = 0
    Const REG_SZ As Short = 1 ' Unicode nul terminated string
    Const REG_DWORD As Short = 4 ' 32-bit number

    Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
    Const gREGVALSYSINFOLOC As String = "MSINFO"
    Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    Const gREGVALSYSINFO As String = "PATH"

    Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim tmpGD As New GdPicture14.GdPictureImaging

        Dim gdver As String = tmpGD.GetVersion

        Me.Text = "About" & " " & "PDFTrans"
        lblVersion.Text = "Version" & " " & My.Application.Info.Version.ToString & vbCrLf & "ImgLibs:" & gdver

        lblTitle.Text = My.Application.Info.Title

       
            If DocuFiSession.IsEvaluation Then
            lblResult.Text = "Your license is registered as a Trial for " & mvarremainingdays.ToString & " days."
            pictStatus.Image = imgList32.Images("certificate_ok.ico")
        ElseIf DocuFiSession.Authorized Then
            lblResult.Text = "Your license is now registered."
            pictStatus.Image = imgList32.Images("certificate_ok.ico")
            Else
                lblResult.Text = "Your license failed to register."
                pictStatus.Image = imgList32.Images("certificate_broken.ico")
            End If


            lblResult.Text = lblResult.Text & vbCrLf & mvarLicenseMsg

            lblComputer.Text = Environment.MachineName

            'now set the flags as to what is enabled
            chkCompare.Checked = DocuFiSession.isCompare
            chkMultipage.Checked = DocuFiSession.IsMultipage
            chkPDFTrans.Checked = DocuFiSession.IsPDFTrans
            chkTag.Checked = DocuFiSession.isCompare
            chkPublish.Checked = DocuFiSession.IsPublish
            chkMeasure.Checked = DocuFiSession.isMeasure

            Try

                If DocuFiSession.OEM.ToLower = "accela" Then
                    btnClearLicenses.Visible = False
                    cmdAuthorize.Visible = False
                End If
            Catch ex As Exception

            End Try
    End Sub


    Private Sub frmAbout_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()
    End Sub


    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        System.Diagnostics.Process.Start("http://sites.fastspring.com/docufi/product/revisaProducts")

        'oaSec.SerialTXT = InputBox("Please enter your authorization code")
        'If oaSec.CheckNewCode(oaSec.SerialTXT) Then
        '    oaSec.Authorized = True
        '    DocuFiSession.Authorized = True
        '    cmdAuthorize.Enabled = False
        '    Form1.Text = "Revisa Editor"
        '    Form1.btnSave.Enabled = True
        '    Form1.btnSaveAs.Enabled = True
        '    Me.Close()
        'End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearLicenses.Click
        Dim myresp As MsgBoxResult

        myresp = MsgBox("Are you sure you want to delete your license keys?", MsgBoxStyle.YesNo)

        If myresp = MsgBoxResult.Yes Then
            licensetemp.DeleteKeys()
            DocuFiSession.Authorized = False
            If IO.File.Exists(APP_Path() & "PDFTransLicense.lic") Then
                IO.File.Delete(APP_Path() & "PDFTransLicense.lic")
            End If
            Me.Close()
            frmLicense.Show()
        End If

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        System.Diagnostics.Process.Start("http://www.rasterediting.com")
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start("http://www.docufi.com")
    End Sub


End Class