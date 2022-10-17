Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports System.Net.Mail
Friend Class frmLicense
  
    Inherits System.Windows.Forms.Form
    Dim licenseValidator As New QLM.LicenseValidator
    Dim computerID As String
 

    Public Sub New(ByRef licenseValidator As QLM.LicenseValidator, ByVal computerID As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.licenseValidator = licenseValidator
        Me.computerID = computerID

    End Sub

  
    'Private Sub txtLicenseKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLicenseKey.TextChanged

    '    Dim serDate As Date
    '    Dim response As String = ""

    '    If Len(txtLicenseKey.Text) > 8 Then

    '        Try
    '            'first make sure we can ping the license manager
    '            licenseValidator.AccessService("http://quicklicensemanager.com/docufi/qlm/qlmservice.asmx", response, serdate)


    '        Catch ex As Exception
    '            lblStatus.Text = "You cannot access the DocuFi license server. " & "Please request a license file for your installation."
    '            lblMessage.Text = "Please Contact DocuFi at info@docufi.com or 603-685-4033 and request a valid license file with your OrderID.  You can then enter the key and select the manual Activation button."
    '            'btnOnline.Visible = False
    '            'txtCompany.Visible = True
    '            'txtEmail.Visible = True
    '            'txtName.Visible = True
    '            'lblEmail.Visible = True
    '            'lblCompany.Visible = True
    '            'lblEmail.Visible = True
    '            Exit Sub
    '        End Try


    '        ' RegisterOnline()
    '    End If

    '    'now send the user registratio
    'End Sub

    Private Sub SendMail(ByVal inName As String, ByVal inCompany As String, ByVal inEmail As String)
        Try

            Dim MailMsg As New MailMessage()

            MailMsg.From = New MailAddress("info@docufi.com")
            MailMsg.To.Add(New MailAddress("register@docufi.com"))


            MailMsg.Subject = "RevisA Registration"

            MailMsg.Body = "RevisA Registration" & vbCrLf & _
                "User: " & inName & vbCrLf & _
                "Company: " & inCompany & vbCrLf & _
                "email: " & inEmail & vbCrLf & _
               "Computer: " & Environment.MachineName & vbCrLf & _
                "License: " & txtLicenseKey.Text

            MailMsg.Priority = MailPriority.Normal
            MailMsg.IsBodyHtml = True

            'send the ini file now

            Dim SmtpMail As New SmtpClient

            SmtpMail.Credentials = New System.Net.NetworkCredential("admin@docufi.com", "admin52k")
            'End If

            SmtpMail.Host = "mail.docufi.com"

            SmtpMail.Send(MailMsg)
        Catch ex As Exception
            ' MsgBox(Err.Description)
        End Try
    End Sub
    Private Sub Register(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            Dim needsActivation As Boolean
            Dim returnMessage As String

            needsActivation = False
            returnMessage = String.Empty
            computerID = "" 'Environment.MachineName
            Dim response As String = ""

            Dim FeatureSet As Integer = 0
            If licenseValidator.ValidateLicense(txtLicenseKey.Text, computerID, Environment.MachineName, needsActivation, FeatureSet, response) Then

                If licenseValidator.EvaluationRemainingDays > 0 Then
                    lblResult.Text = "Your license is now registered for " & licenseValidator.EvaluationRemainingDays.ToString & " days."
                Else
                    lblResult.Text = "Your license is now registered."
                End If
                'now test for extra features

                pictStatus.Image = imgList32.Images("certificate_ok.ico")
                DocuFiSession.Authorized = True
                btnClose.Text = "Continue"
                btnOnline.Enabled = False
                btnManual.Enabled = False
                DocuFiSession.isCompare = (FeatureSet And FeatCompare)
                DocuFiSession.isMeasure = (FeatureSet And FeatMeasure)
                DocuFiSession.IsPublish = (FeatureSet And FeatPublish) 'for multipage, tag, and publish
                DocuFiSession.IsPDFTrans = (FeatureSet And FeatPDFTrans)
                DocuFiSession.IsMultipage = (FeatureSet And FeatMultiPage)

            Else

                lblResult.Text = "Your license failed to register."
                pictStatus.Image = imgList32.Images("certificate_broken.ico")
                DocuFiSession.Authorized = False
            End If

                lblStatus.Text = returnMessage
                mvarLicenseMsg = lblResult.Text

                'Assuming your product defines 8 features, let's find out which ones are enabled.         

        Catch ex As Exception
            lblResult.Text = "Your license failed to register." + " " + ex.Message
        End Try

    End Sub

    Private Sub btnRegister_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Register(sender, e)
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        System.Diagnostics.Process.Start("http://sites.fastspring.com/docufi/product/revisaProducts ")
    End Sub

    Private Sub frmLicense_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        txtLicenseKey.Text = DocuFiSession.LicenseKey 'licenseValidator.ActivationKey

        If DocuFiSession.Authorized Then
            lblResult.Text = "Your license is now registered."
            pictStatus.Image = imgList32.Images("certificate_ok.ico")
        ElseIf DocuFiSession.IsEvaluation Then
            lblResult.Text = "Your trial license expired."
            pictStatus.Image = imgList32.Images("certificate_broken.ico")
        ElseIf DocuFiSession.needsActivation Then
            lblResult.Text = "Your license needs activation."
            pictStatus.Image = imgList32.Images("certificate_ok.ico")
            btnClose.Text = "Continue"
        Else
            lblResult.Text = "Your license failed to register."
            pictStatus.Image = imgList32.Images("certificate_broken.ico")
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            'if the user succeeded in authorizing the product, store the account
            If DocuFiSession.Authorized Then

            End If

        Catch ex As Exception

        End Try
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        licenseValidator.DeleteKeys()
    End Sub
    Private Function RegisterOnline() As Boolean
        Dim response As String = ""
        Dim needsActivation As Boolean

        Dim FeatureSet As Integer = 0

        If licenseValidator.ValidateLicense(txtLicenseKey.Text, computerID, Environment.MachineName, needsActivation, FeatureSet, response) Then
            DocuFiSession.Authorized = True

            If needsActivation And DocuFiSession.Authorized Then

                If licenseValidator.ActivateLicense("http://quicklicensemanager.com/docufi/qlm/qlmservice.asmx", _
                        txtLicenseKey.Text, computerID, "", response) Then
                    licenseValidator.StoreKey(txtLicenseKey.Text, computerID)
                End If
                lblStatus.Text = response
                FeatureSet = licenseValidator.FeatureSet
                DocuFiSession.isCompare = (FeatureSet And FeatCompare)
                DocuFiSession.isMeasure = (FeatureSet And FeatMeasure)
                DocuFiSession.IsPublish = (FeatureSet And FeatPublish) 'for multipage, tag, and publish
                DocuFiSession.IsPDFTrans = (FeatureSet And FeatPDFTrans)
                DocuFiSession.IsMultipage = (FeatureSet And FeatMultiPage)
                Return True
            End If
            If computerID <> "" Then
                licenseValidator.StoreKey(txtLicenseKey.Text, computerID)
                If licenseValidator.EvaluationRemainingDays > 0 Then
                    lblResult.Text = "Your license is now registered for " & licenseValidator.EvaluationRemainingDays.ToString & " days."
                Else
                    lblResult.Text = "Your license is now registered."
                End If

                pictStatus.Image = imgList32.Images("certificate_ok.ico")
                lblStatus.Text = response
                btnClose.Text = "Continue"
                DocuFiSession.Authorized = True

                btnOnline.Enabled = False
                btnManual.Enabled = False

                DocuFiSession.isCompare = True
                Return True
            Else
                licenseValidator.StoreKey(txtLicenseKey.Text, computerID)
                lblResult.Text = "Your license failed to registered."
                pictStatus.Image = imgList32.Images("certificate_broken.ico")
                lblStatus.Text = response
                btnClose.Text = "Continue Trial"
                Return False
            End If

        ElseIf needsActivation Then
            'now test for extra features
            If DocuFiSession.Authorized Then
                licenseValidator.ActivateLicense("http://quicklicensemanager.com/docufi/qlm/qlmservice.asmx", _
                        txtLicenseKey.Text, computerID, "", response)
                If computerID <> "" Then
                    licenseValidator.StoreKey(txtLicenseKey.Text, computerID)
                    If licenseValidator.EvaluationRemainingDays > 0 Then
                        lblResult.Text = "Your license is now registered for " & licenseValidator.EvaluationRemainingDays.ToString & " days."
                    Else
                        lblResult.Text = "Your license is now registered."
                    End If

                    pictStatus.Image = imgList32.Images("certificate_ok.ico")
                    lblStatus.Text = response
                    btnClose.Text = "Continue"
                    Return True
                Else
                    lblResult.Text = response
                    pictStatus.Image = imgList32.Images("certificate_broken.ico")
                    Return False
                End If

            Else
                lblResult.Text = "Your license does not match this product confiuguration."
                pictStatus.Image = imgList32.Images("certificate_broken.ico")
                Return False
            End If
        ElseIf licenseValidator.EvaluationExpired Then
            lblStatus.Text = "Your license has expired, please renew your subscription"
        Else
            If licenseValidator.EvaluationRemainingDays > 0 Then
                Dim ndays As Integer = licenseValidator.EvaluationRemainingDays
                lblStatus.Text = response
                lblStatus.Text = "Your license expires in " & ndays.ToString & " days."
            End If
            Return True
        End If

    End Function

    Private Sub btnManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManual.Click
        If Len(txtLicenseKey.Text) > 8 Then
            Register(sender, e)
        End If
        'If txtName.Text = "" Then
        '    lblResult.Text = "Please enter your Name, Company and Email address"
        '    txtLicenseKey.Text = ""
        '    txtName.BackColor = Color.LightCoral
        '    txtCompany.BackColor = Color.White
        '    txtEmail.BackColor = Color.White
        'ElseIf txtCompany.Text = "" Then
        '    lblResult.Text = "Please enter your Company name"
        '    txtLicenseKey.Text = ""
        '    txtName.BackColor = Color.White
        '    txtCompany.BackColor = Color.LightCoral
        '    txtEmail.BackColor = Color.White
        'ElseIf InStr(txtEmail.Text, "@") = 0 Or InStr(txtEmail.Text, ".") = 0 Then
        '    lblResult.Text = "Please enter your valid Email address before pasting the license"
        '    txtLicenseKey.Text = ""
        '    txtName.BackColor = Color.White
        '    txtCompany.BackColor = Color.White
        '    txtEmail.BackColor = Color.LightCoral
        'Else

        '    'now send the user registration
        '    SendMail(txtName.Text, txtCompany.Text, txtEmail.Text)

        'End If

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOnline.Click
        If RegisterOnline() = False Then 'first try online and then 
            Register(sender, e)
        End If

    End Sub

    Private Sub lblMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblMessage.Click

    End Sub
End Class

