Public Class email
    Dim emaillist As String
    Dim primary As Boolean
    Dim numCounter As Integer
    Dim numOfaccounts As Integer
    Dim waitcounter As Integer
    Dim waitcounter1 As Integer
    Dim waitcounter2 As Integer
    Dim numcounter1 As Integer
    Dim proxies() As String
    Dim email As String
    Dim password As String
    Public blnStop As Boolean = False
    Public blnStarted As Boolean = False
    ' The structure we use for the information
    ' to be interpreted correctly by API.
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
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        createAccounts()
    End Sub
    Private Sub createAccounts()

        Dim counter As Integer

        Dim didntwork As Boolean = False
        Dim programs As Integer
        Dim rndnumber As Random
        'If Form1.primary = True Then
        If numOfaccounts = 0 Then
            numOfaccounts = InputBox("How many emails to create?")
        End If
        '    programs = InputBox("How many Programs to be ran (10 max)")
        '    Do Until programs <= 10
        '        MessageBox.Show("Invalid answer.  Maximum ran is 5)")
        '        programs = InputBox("How many Programs to be ran (5 max)")
        '    Loop
        '    numOfaccounts /= programs
        '    '    My.Computer.FileSystem.WriteAllText(Form1.path & "\settings.txt", "email " & numOfaccounts.ToString & " " & TextBox3.Text & " " & TextBox1.Text & " " & TextBox2.Text, False)
        '    Do Until programs = 1
        '        Shell(Form1.path & "\projectRSN.exe")
        '        programs -= 1
        '    Loop
        'Else
        '    rndnumber = New Random
        '    counter = rndnumber.Next(1000)
        '    MessageBox.Show("starting")
        'End If


        password = TextBox3.Text
        Do Until WebBrowser1.IsBusy = False
            wait(10)
        Loop
        Do Until numCounter = numOfaccounts Or blnStop = True
            email = TextBox1.Text & counter.ToString & Label1.Text & TextBox2.Text

            createFakeEmail()
            WebBrowser1.Navigate(Form1.path & "\fakeRsImposters.html")
            wait(5)
            Do Until WebBrowser1.IsBusy = False
                wait(5)
            Loop



            Try
                'trys to login once if fails try again if fail try a 3rd time
                WebBrowser1.Document.GetElementById("submit").InvokeMember("click")
            Catch ex As Exception
                didntwork = True
                WebBrowser1.Stop()
                WebBrowser1.Navigate(Form1.path & "\fakeRsImposters.html")
                '  MessageBox.Show(ex.ToString)
                wait(5)
                Do Until WebBrowser1.IsBusy = False
                    waitcounter1 += 1
                    If waitcounter1 = 3 Then
                        waitcounter1 = 0
                        WebBrowser1.Stop()
                        wait(5)
                    End If
                    wait(5)
                Loop
                waitcounter1 = 0
            End Try
            wait(5)
            Do Until WebBrowser1.IsBusy = False
                wait(5)
            Loop
            Dim temp As String
            temp = WebBrowser1.Url.ToString
            If temp.Length > 100 Then
                temp = temp.Substring(48, 15)
            End If
            If didntwork = False And temp = "congratulations" Then
                numCounter += 1
                numcounter1 += 1
                My.Computer.FileSystem.WriteAllText(Form1.path & "\emails.txt", email & ControlChars.NewLine, True)
                If numcounter1 = 10 Then
                    If ListBox1.Items.Count > 0 Then
                        ListBox1.SelectedIndex = 0
                        RefreshIESettings(ListBox1.Text)
                        numcounter1 = 0
                        ListBox1.Items.RemoveAt(0)
                    Else
                        blnStop = True
                        MessageBox.Show("Out of Proxies")
                    End If
                End If
                Label4.Text = numCounter.ToString

            End If
            If WebBrowser1.Url.ToString = "https://secure.runescape.com/m=account-creation/error.ws?error=1" Then
                If ListBox1.Items.Count > 0 Then
                    ListBox1.SelectedIndex = 0
                    RefreshIESettings(ListBox1.Text)
                    numcounter1 = 0
                    ListBox1.Items.RemoveAt(0)
                Else
                    blnStop = True
                    MessageBox.Show("Out of Proxies")
                End If
            End If
            '      MessageBox.Show(WebBrowser1.Url.ToString)
            If WebBrowser1.DocumentText.Substring(0, 9) = "<!DOCTYPE" Then
                If ListBox1.Items.Count > 0 Then
                    ListBox1.SelectedIndex = 0
                    RefreshIESettings(ListBox1.Text)
                    numcounter1 = 0
                    ListBox1.Items.RemoveAt(0)
                Else
                    blnStop = True
                    MessageBox.Show("Out of Proxies")
                End If

            End If
            rndnumber = New Random
            counter += rndnumber.Next(1000)
            didntwork = False
          
        Loop
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim saveFile As String = String.Empty
        Dim saveFileName As New SaveFileDialog
        Dim saveFileName1 As String
        Dim ok1 As DialogResult
        'saves emails to text file

        saveFileName.Filter = "(*.txt;)|*.txt;"


        ok1 = saveFileName.ShowDialog()
        If ok1 = Windows.Forms.DialogResult.OK Then



            saveFileName1 = saveFileName.FileName.ToString

            My.Computer.FileSystem.WriteAllText(saveFileName1, emaillist, False)
        End If

    End Sub

    Private Sub email_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Form1.Visible = True
        End
    End Sub
    Private Sub createFakeEmail()
        Dim strFakeRsHtml As String = "<html>" & ControlChars.NewLine &
             "<body>" & ControlChars.NewLine &
 "<form action=""https://secure.runescape.com/m=account-creation/create_account_funnel.ws"" method=""post"" id=""creation_form"">" & ControlChars.NewLine &
 "<fieldset>" & ControlChars.NewLine &
 "<input type=""hidden"" id=""onlyOneEmail"" name=""onlyOneEmail"" value=""1"">" & ControlChars.NewLine &
"<div>" & ControlChars.NewLine &
"<input type=""text"" name=""email1"" id=""email1"" size=""70"" maxlength=""100"" class=""largeField"" title="""" value=""" & email & """>" & ControlChars.NewLine &
 "<input type=""password"" name=""password1"" id=""password1"" size=""70"" maxlength=""20""  class=""largeField"" title="""" value=""" & password & """>" & ControlChars.NewLine &
 "<input type=""password"" name=""password2"" id=""password2"" size=""70"" maxlength=""20""  class=""largeField"" title="""" value=""" & password & """>" & ControlChars.NewLine &
 "<input type=""text"" name=""age"" id=""age"" size=""5"" maxlength=""3"" placeholder=""Age"" title="""" value=""18"">" & ControlChars.NewLine &
 "<input type=""checkbox"" name=""agree_email"" id=""news-updates"" checked><label for=""news-updates"">Please send me News and Updates</label></div>" & ControlChars.NewLine &
 "<input type=""hidden"" name=""agree_pp_and_tac"" id=""terms"" value=""1"">" & ControlChars.NewLine &
 "<input type=""submit"" id=""submit"" name=""submit"" value=""Join Free Now"">" & ControlChars.NewLine &
"<p id=""tandc"">By clicking 'Join Free Now' I agree to the <a href=""http://www.jagex.com/g=runescape/terms/terms.ws"" target=""_blank"">Terms and Conditions</a> and <a href=""http://www.jagex.com/g=runescape/privacy/privacy.ws"" target=""_blank"">Privacy Policy</a></p>" & ControlChars.NewLine &
"</form>" & ControlChars.NewLine &
 "</body>" & ControlChars.NewLine &
 "</html>"
        My.Computer.FileSystem.WriteAllText(Form1.path & "\fakeRsImposters.html", strFakeRsHtml, False)
    End Sub

    Private Sub email_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WebBrowser1.ScriptErrorsSuppressed = True
        Form1.Visible = False
        Form1.Enabled = False
        Dim temp As String
        temp = My.Computer.FileSystem.ReadAllText(Form1.path & "\proxies.txt")
        proxies = Split(temp, ControlChars.NewLine)
        For Each item As String In proxies
            ListBox1.Items.Add(item)
        Next
        wait(20)
        If Form1.blnStart = True Then
            TextBox1.Text = Form1.email1
            TextBox2.Text = Form1.email2
            TextBox3.Text = Form1.password
            numOfaccounts = Form1.numOfAccounts
        End If
    End Sub

End Class