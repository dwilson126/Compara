Imports QlmLicenseLib

Namespace QLM
    Public Class LicenseValidator

        Dim _license As QlmLicense
        Dim _activationKey As String
        Dim _computerKey As String
        Dim _returnMSG As String
        Dim _featureSet As Integer

        Dim _isEvaluation As Boolean

        Dim _evaluationExpired As Boolean
        Dim _evaluationRemainingDays As Integer


        Sub New()
            _license = New QlmLicenseLib.QlmLicense

            ' Always obfuscate your code. In particular, you should always obfuscate all arguments
            ' of DefineProduct and the Public Key (i.e. encrypt all the string arguments).

            '_license.DefineProduct(1, "Demo", 1, 0, "DemoKey", "{24EAA3C1-3DD7-40E0-AEA3-D20AA17A6005}")
            '_license.PublicKey = "A59Jip0lt73Xig=="
            _license.CommunicationEncryptionKey = "{28C8D840-9801-45F0-8C65-E6A7A0F6119B}"
            _license.DefineProduct(7, "ComparA", 2, 1, "$ellmore", "{E62EEF64-0FB2-4023-818F-168B06D31692}")
            _license.PublicKey = "Fbl95q6IZ7Y+gg=="
            _isEvaluation = False
            _evaluationExpired = True
            _evaluationRemainingDays = -1

        End Sub
        Public Function ActivateLicense(ByVal InURL As String, ByVal inKey As String, ByRef outComputerKey As String, ByRef inEmail As String, ByRef errorMsg As String) As Boolean
            Dim response As String = ""
            errorMsg = String.Empty '
            'Dim outComputerKey As String
            _license.ActivateLicense(InURL, inKey, Environment.MachineName, Environment.MachineName, "5.0.0", "RevisA", response)

            ' _license.ActivateLicenseForUser(InURL, inKey, inEmail, Environment.MachineName, Environment.MachineName, "5.0.0", String.Empty, String.Empty, response)
            Dim licenseInfo As New LicenseInfo
            licenseInfo = New LicenseInfo()

            Dim message As String = ""

            If _license.ParseResults(response, licenseInfo, message) Then
                outComputerKey = licenseInfo.ComputerKey
            Else
                'MessageBox.Show(message)
                errorMsg = message
                Return False
            End If
            errorMsg = message
            Return True

        End Function
        Public Function AccessService(ByVal inURL As String, ByRef outResponse As String, ByRef outDate As Date) As Boolean
            Return _license.Ping(inURL, outResponse, outDate)

        End Function
        ''' <remarks>Call ValidateLicenseAtStartup when your application is launched. 
        ''' If this function returns false, exit your application.
        ''' </remarks>
        ''' <summary>
        ''' Validates the license when the application starts up. 
        ''' The first time a license key is validated successfully,
        ''' it is stored in a hidden file on the system. 
        ''' When the application is restarted, this code will load the license
        ''' key from the hidden file and attempt to validate it again. 
        ''' If it validates succesfully, the function returns true.
        ''' If the license key is invalid, expired, etc, the function returns false.
        ''' </summary>
        Public Function ValidateLicenseAtStartup(ByVal computerID As String, ByRef needsActivation As Boolean, ByRef errorMsg As String) As Boolean

            errorMsg = String.Empty
            needsActivation = False

            _isEvaluation = False
            _evaluationExpired = False
            _evaluationRemainingDays = -1

            Dim storedActivationKey As String
            Dim storedComputerKey As String

            storedActivationKey = String.Empty
            storedComputerKey = String.Empty

            _license.ReadKeys(_activationKey, _computerKey)

            If String.IsNullOrEmpty(storedActivationKey) = False Then

                _activationKey = storedActivationKey

            End If

            If String.IsNullOrEmpty(storedComputerKey) = False Then

                _computerKey = storedComputerKey

            End If

            ValidateLicenseAtStartup = ValidateLicense(_activationKey, _computerKey, computerID, needsActivation, errorMsg)
            _returnMSG = errorMsg

        End Function

        ''' <remarks>Call this function in the dialog where the user enters the license key to validate the license.</remarks>
        ''' <summary>
        ''' Validates a license key. If you provide a computer key, the computer key is validated. 
        ''' Otherwise, the activation key is validated. 
        ''' If you are using machine bound keys (UserDefined), you can provide the computer identifier, 
        ''' otherwise set the computerID to an empty string.
        ''' </summary>
        ''' <param name="activationKey">Activation key</param>
        ''' <param name="computerKey">Computer key</param>
        ''' <param name="computerID">Unique computer identifier</param>
        ''' <param name="errorMsg">returned error message</param>

        Public Function ValidateLicense(ByVal activationKey As String, ByVal computerKey As String, ByVal computerID As String, ByRef needsActivation As Boolean, ByRef errorMsg As String) As Boolean


            Try
                Dim nStatus As Integer
                Dim licenseKey As String
                Dim ret As Boolean

                needsActivation = False


                If Not String.IsNullOrEmpty(computerKey) Then
                    licenseKey = computerKey
                ElseIf Not String.IsNullOrEmpty(activationKey) Then
                    licenseKey = activationKey
                Else
                    ValidateLicense = False
                    Exit Function
                End If


                errorMsg = _license.ValidateLicenseEx(licenseKey, computerID)

                nStatus = _license.GetStatus()


                If IsTrue(nStatus, ELicenseStatus.EKeyInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyProductInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyVersionInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyMachineInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyTampered) Then

                    ' the key is invalid
                    ret = False

                ElseIf (IsTrue(nStatus, ELicenseStatus.EKeyDemo)) Then

                    _isEvaluation = True

                    If (IsTrue(nStatus, ELicenseStatus.EKeyExpired)) Then
                        ' the key has expired
                        ret = False
                        _evaluationExpired = True
                    Else
                        ' the demo key is still valid
                        _evaluationExpired = False
                        ret = True
                        _evaluationRemainingDays = _license.DaysLeft
                        _featureSet = _license.Features
                    End If

                ElseIf (IsTrue(nStatus, ELicenseStatus.EKeyPermanent)) Then
                    ' the key is OK
                    ret = True
                    _evaluationExpired = False
                End If

                If ret = True Then
                    If _license.LicenseType = ELicenseType.Activation Then
                        needsActivation = True
                        _featureset = _license.Features
                        ret = False
                    Else
                        _license.StoreKeys(activationKey, computerKey)
                        _featureset = _license.Features
                        _activationKey = activationKey
                    End If
                End If

                ValidateLicense = ret

            Catch ex As Exception
                MsgBox("Exception: " & ex.Message)
            End Try
        End Function
        Public Sub StoreKey(ByVal inKey As String, ByRef InComputerKey As String)
            'Dim outComputerKey As String
            _license.StoreKeys(inKey, InComputerKey)

        End Sub
        ''' <summary>
        ''' Deletes the license keys stored on the computer. 
        ''' </summary>
        Public Sub DeleteKeys()
            _license.DeleteKeys()
        End Sub


        Public ReadOnly Property ActivationKey() As String

            Get
                Return Me._activationKey

            End Get

        End Property

        Public ReadOnly Property ComputerKey() As String
            Get
                Return Me._computerKey

            End Get

        End Property


        Public ReadOnly Property QlmLicense() As QlmLicense

            Get
                Return Me._license
            End Get

        End Property

        Public ReadOnly Property IsEvaluation() As Boolean
            Get
                Return Me._isEvaluation

            End Get

        End Property
        Public ReadOnly Property FeatureSet() As Integer
            Get
                Return Me._featureSet

            End Get

        End Property

        Public ReadOnly Property EvaluationExpired() As Boolean
            Get
                Return Me._evaluationExpired

            End Get

        End Property

        Public ReadOnly Property EvaluationRemainingDays() As Integer
            Get
                Return Me._evaluationRemainingDays

            End Get

        End Property
        Public ReadOnly Property LicenseKey() As String
            Get
                Return Me._activationKey

            End Get

        End Property
        Public ReadOnly Property ReturnMsg() As String
            Get
                Return Me._returnMSG

            End Get

        End Property

        ''' <summary>
        ''' Compares flags
        ''' </summary>
        ''' <param name="nVal1">Value 1</param>
        ''' <param name="nVal2">Value 2</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsTrue(ByVal nVal1 As Integer, ByVal nVal2 As Integer) As Boolean

            If (((nVal1 And nVal2) = nVal1) Or ((nVal1 And nVal2) = nVal2)) Then

                IsTrue = True
                Exit Function
            End If

            IsTrue = False
        End Function

        Public Function ValidateLicense(ByVal activationKey As String, ByVal computerKey As String, ByVal computerID As String, ByRef needsActivation As Boolean, ByRef FeatureSet As Integer, ByRef errorMsg As String) As Boolean

            Try
                Dim nStatus As Integer
                Dim licenseKey As String
                Dim ret As Boolean

                needsActivation = False

                If Not String.IsNullOrEmpty(computerKey) Then
                    licenseKey = computerKey
                ElseIf Not String.IsNullOrEmpty(activationKey) Then
                    licenseKey = activationKey
                Else
                    ValidateLicense = False
                    Exit Function
                End If

                errorMsg = _license.ValidateLicenseEx(licenseKey, computerID)

                nStatus = _license.GetStatus()

                If IsTrue(nStatus, ELicenseStatus.EKeyInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyProductInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyVersionInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyMachineInvalid) Or _
                    IsTrue(nStatus, ELicenseStatus.EKeyTampered) Then

                    ' the key is invalid
                    ret = False

                ElseIf (IsTrue(nStatus, ELicenseStatus.EKeyDemo)) Then

                    _isEvaluation = True

                    If (IsTrue(nStatus, ELicenseStatus.EKeyExpired)) Then
                        ' the key has expired
                        ret = False
                        _evaluationExpired = True
                        MsgBox(errorMsg)
                    Else
                        ' the demo key is still valid
                        ret = True
                        _evaluationRemainingDays = _license.DaysLeft
                    End If

                ElseIf (IsTrue(nStatus, ELicenseStatus.EKeyPermanent)) Then
                    ' the key is OK
                    ret = True
                End If

                If ret = True Then
                    If _license.LicenseType = ELicenseType.Activation Then
                        needsActivation = True
                        FeatureSet = _license.Features
                        ret = True
                    Else
                        FeatureSet = _license.Features
                        _license.StoreKeys(activationKey, computerKey)
                    End If
                End If

                    ValidateLicense = ret

            Catch ex As Exception
                MsgBox("Exception: " & ex.Message)
            End Try
        End Function
    End Class
End Namespace