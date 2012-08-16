Public Class Form3
    Dim rndNum2 As Random
    Dim rndnumber12 As Double
    Dim emailNum As Integer
    Public path As String = Application.StartupPath

    Public Structure Struct_INTERNET_PROXY_INFO
        Public dwAccessType As Integer
        Public proxy As IntPtr
        Public proxyBypass As IntPtr
    End Structure

    ' The Windows API function that allows us to manipulate
    ' IE settings programmatically.
    Private Declare Auto Function InternetSetOption Lib "wininet.dll" _
    (ByVal hInternet As IntPtr, ByVal dwOption As Integer, ByVal lpBuffer As IntPtr, _
     ByVal lpdwBufferLength As Integer) As Boolean

    ' The function we will be using to set the proxy settings.
    Private Sub RefreshIESettings(ByVal strProxy As String)
        Const INTERNET_OPTION_PROXY As Integer = 38
        Const INTERNET_OPEN_TYPE_PROXY As Integer = 3
        Dim struct_IPI As Struct_INTERNET_PROXY_INFO

        ' Filling in structure
        struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY
        struct_IPI.proxy = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(strProxy)
        struct_IPI.proxyBypass = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi("local")

        ' Allocating memory
        Dim intptrStruct As IntPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(struct_IPI))

        ' Converting structure to IntPtr
        System.Runtime.InteropServices.Marshal.StructureToPtr(struct_IPI, intptrStruct, True)
        Dim iReturn As Boolean = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, System.Runtime.InteropServices.Marshal.SizeOf(struct_IPI))
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim rndString As String
        Dim username As String
        Dim password As String = TextBox3.Text
        Dim numberOfAccounts As Integer
        Dim accountsCreated As String
        Dim fakeRS As String
        Dim counter As Integer
        numberOfAccounts = ListBox1.Items.Count
        Do Until numberOfAccounts = 0
            ListBox1.SelectedIndex = 0
            If ListBox1.Text.Length > 3 Then


                rndNum2 = New Random
                rndString = rndNum2.Next(100000, 999999).ToString & rndNum2.Next(100000, 999999).ToString
                rndNum2 = New Random
                Dim email As String = TextBox1.Text & "@" & rndNum2.Next(2000, 100000).ToString & TextBox2.Text
                ListBox1.SelectedIndex = 0
                username = ListBox1.Text
                WebBrowser1.Navigate("https://secure.runescape.com/m=billing_amazonddd/create?token=1687ea00-6153-4dd7-841d-" & rndString)
                wait(10)
                Do Until WebBrowser1.IsBusy = False
                    wait(10)
                Loop
                Dim url As String
                url = WebBrowser1.Url.ToString
                Try
                    WebBrowser1.Document.GetElementById("displayName").SetAttribute("value", username)
                    WebBrowser1.Document.GetElementById("password").SetAttribute("value", password)
                    WebBrowser1.Document.GetElementById("age").SetAttribute("value", "18")
                    WebBrowser1.Document.GetElementById("emailAddress").SetAttribute("value", email)
                    WebBrowser1.Document.GetElementById("confirmPassword").SetAttribute("value", password)
                    WebBrowser1.Document.GetElementById("submit").InvokeMember("click")
                Catch ex As Exception
                End Try
                wait(10)
                Do Until WebBrowser1.IsBusy = False
                    wait(10)
                Loop
                If url = WebBrowser1.Url.ToString Then
                    wait(20)
                End If
                url = WebBrowser1.Url.ToString
                If url.Length > 60 Then
                    url = url.Substring(49, 11)
                End If
                If url <> "linkSuccess" Then
                    If ListBox2.Items.Count > 40 Then
                        ListBox2.SelectedIndex = 0
                        RefreshIESettings(ListBox2.Text)
                        ListBox2.Items.Add(ListBox2.Text)
                        ListBox2.Items.RemoveAt(0)
                    Else
                        ListBox2.SelectedIndex = 0
                        RefreshIESettings(ListBox2.Text)
                        ListBox2.Items.RemoveAt(0)
                    End If

                Else
                    numberOfAccounts -= 1
                    counter += 1
                    If counter = 11 Then
                        If ListBox2.Items.Count > 40 Then
                            ListBox2.SelectedIndex = 0
                            RefreshIESettings(ListBox2.Text)
                            ListBox2.Items.Add(ListBox2.Text)
                            ListBox2.Items.RemoveAt(0)
                        Else
                            ListBox2.SelectedIndex = 0
                            RefreshIESettings(ListBox2.Text)
                            ListBox2.Items.RemoveAt(0)
                        End If
                        counter = 0
                    End If
                    accountsCreated = email & "::" & username
                    My.Computer.FileSystem.WriteAllText(path & "\Imposters_Completed.txt", accountsCreated & ControlChars.NewLine, True)
                    ListBox1.Items.RemoveAt(0)
                End If
            Else
                ListBox1.Items.RemoveAt(0)
                numberOfAccounts -= 1
            End If
        Loop
    End Sub

    Private Sub Form3_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        saveFiles()
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim filereader As String
        Dim temp() As String
        Dim rndString As String
        rndNum2 = New Random
        rndString = rndNum2.Next(100000, 999999).ToString & rndNum2.Next(100000, 999999).ToString
        WebBrowser1.Navigate("https://secure.runescape.com/m=billing_amazonddd/create?token=1687ea00-6153-4dd7-841d-" & rndString)

        fileCheck()
        Validate1()
        filereader = My.Computer.FileSystem.ReadAllText(path & "\usernames.txt")
        temp = Split(filereader, ControlChars.NewLine)
        For Each item As String In temp
            If temp.Length > 3 Then
                ListBox1.Items.Add(item)
                item = Nothing
            End If
        Next
        filereader = My.Computer.FileSystem.ReadAllText(path & "\proxies.txt")
        temp = Split(filereader, ControlChars.NewLine)
        For Each item As String In temp
            If temp.Length > 3 Then
                ListBox2.Items.Add(item)
                item = Nothing
            End If
        Next
    End Sub
    Private Sub wait(ByRef seconds As Integer)

        'wait function
        Dim sw As New Stopwatch

        sw.Start()
        seconds *= 100
        Do Until sw.ElapsedMilliseconds > seconds
            System.Windows.Forms.Application.DoEvents()
        Loop
        sw.Stop()
    End Sub
    Private Sub fileCheck()
        If My.Computer.FileSystem.FileExists(path & "\proxies.txt") = False Then
            My.Computer.FileSystem.WriteAllText(path & "\proxies.txt", Nothing, False)
        End If
        If My.Computer.FileSystem.FileExists(path & "\usernames.txt") = False Then
            My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", Nothing, False)
        End If
    End Sub
    Private Sub Validate1()
        If My.Computer.FileSystem.FileExists("C:\Program Files\projectRSN\Ok.txt") = True Then
        Else
            Dim authcode As String
            Dim verification As String
            WebBrowser1.Navigate("http://gavonas.comlu.com/default.html")
            wait(10)
            Do Until WebBrowser1.IsBusy = False
                wait(10)
            Loop
            authcode = InputBox("Please Enter your auth code")
            Dim temp() As String
            verification = WebBrowser1.DocumentText.ToString
            verification = verification.Substring(14, 10)
            temp = Split(verification, ControlChars.NewLine)
            verification = verification.Trim
            If authcode = verification Then
                Try
                    My.Computer.FileSystem.CreateDirectory("C:\Program Files\projectRSN")
                    My.Computer.FileSystem.WriteAllText("C:\Program Files\projectRSN\Ok.txt", Nothing, False)
                Catch ex As Exception
                    MessageBox.Show("Please run as Administrator")
                    End
                End Try

            Else
                MessageBox.Show("Invalid Auth Code Program will now End")
                End
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        saveFiles()
    End Sub
    Private Sub saveFiles()
        Dim results As String
        Do Until ListBox1.Items.Count = 0
            ListBox1.SelectedIndex = 0
            results &= ListBox1.Text & ControlChars.NewLine
            ListBox1.Items.RemoveAt(0)
        Loop
        My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", results, False)
        results = Nothing
        Do Until ListBox2.Items.Count = 0
            ListBox2.SelectedIndex = 0
            results &= ListBox2.Text & ControlChars.NewLine
            ListBox2.Items.RemoveAt(0)
        Loop
        My.Computer.FileSystem.WriteAllText(path & "\proxies.txt", results, False)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Form2.Visible = True
        Form2.Enabled = True
    End Sub
End Class