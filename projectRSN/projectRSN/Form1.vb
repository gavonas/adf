
Imports System.Windows.Forms

Public Class Form1
    Dim codev As String
    Dim username As String
    Dim CreationCounter As Integer
    Dim clickcounter As Integer
    Dim ii As Integer
    Dim strEmails() As String
    Dim strUsernames() As String
    Dim completedName As String
    Dim pause As Boolean = False
    Public path As String = Application.StartupPath
    Dim amountPerProgram As Integer
    Dim numberofPrograms As Integer
    Dim primary As Boolean
    Public blnStart As Boolean
    Public email1 As String = "Email"
    Public email2 As String = "Whatever.Something"
    Public password As String = "Password"
    Public numOfAccounts As Integer
    Private Sub loadstart()
        'loads runescape account settings login page
        WebBrowser1.Navigate(path & "\runescape.html")
        wait(10)

        'wait until browser isn't busy
        Do Until WebBrowser1.IsBusy = False
            wait(10)

        Loop


    End Sub

    Private Sub rsImposterSite(ByRef code As String, ByRef username1 As String)
        Dim outfile As IO.StreamWriter
        Dim website As String
        website = 0
        ' creates fake rs page that mimics the account creation page
        website &= "<html>" & ControlChars.NewLine
        website &= "<head>" & ControlChars.NewLine
        website &= "<form method=""post"" action=""https://secure.runescape.com/m=displaynames/c=" & code & "/name.ws"" style=""display:inline"">" & ControlChars.NewLine
        website &= "<input type=""hidden"" name=""name"" value=""" & username1 & """ id=""name"">" & ControlChars.NewLine
        website &= "<input type=""hidden"" name=""ssl"" value=""-1"">" & ControlChars.NewLine
        website &= "<input value=""Yes"" id=""confirm"" name=""confirm"" type=""submit"" title=""Yes""/>" & ControlChars.NewLine
        website &= "</form>" & ControlChars.NewLine
        website &= "</head>" & ControlChars.NewLine
        website &= "</html>" & ControlChars.NewLine
        'creates the file will change the destination to be approriate
        outfile = IO.File.CreateText(path & "\rsnamegenerator.html")

        outfile.Write(website)
        outfile.Close()
        wait(10)
        'navigates to the file
        If IO.File.Exists(path & "\rsnamegenerator.html") Then


            WebBrowser1.Navigate(path & "\rsnamegenerator.html")
            wait(10)
        Else
            MessageBox.Show("doesn't exist")
        End If

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        start()
    End Sub
    Private Sub start()
        Dim whatever111 As Boolean = False
        Dim strEmail As String
        Dim completed As Boolean = False
        Dim error1 As Boolean = False
        Dim dontremove As Boolean = False
        Dim filereader As String
        Dim items() As String
        Dim programs As Integer
        Dim numberOfnames As Integer
        Dim numberOfnames2 As Integer
        Dim numberOfemails As Integer
        pause = False
        Dim counter2 As Integer
        password = TextBox1.Text
        '  MessageBox.Show(primary.ToString) 
        If primary = True Then
            filereader = My.Computer.FileSystem.ReadAllText(path & "\usernames.txt")
            items = Split(filereader, ControlChars.NewLine)
            For Each item As String In items
                If item.Length > 2 Then
                    ListBox1.Items.Add(item)
                    item = Nothing
                End If
            Next
            My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", "", False)
        End If
        If primary = True Then
            programs = InputBox("How many programs would you like to run?")
            numberOfnames = ListBox1.Items.Count
            numberOfnames = Math.Floor(numberOfnames / programs)
            numberOfemails = ListBox2.Items.Count
            Dim x As Integer
            Dim y As Integer
            Dim z As Integer
            Do Until y = programs
                y += 1
                x += y
            Loop
            y = 1
            Do Until programs = 1
                numberOfnames2 = numberOfnames
                Dim strTemp As String
                My.Computer.FileSystem.WriteAllText(path & "\settings.txt", password, False)
                If numberOfemails > x Then
                    Do Until z = y
                        ListBox2.SelectedIndex = 0
                        strTemp &= ListBox2.Text & ControlChars.NewLine
                        ListBox2.Items.RemoveAt(0)
                        z += 1
                    Loop
                    My.Computer.FileSystem.WriteAllText(path & "\emails.txt", strTemp, False)
                    strTemp = Nothing
                    z = 0
                    y += 1
                End If
                Do Until numberOfnames2 = 0
                    ListBox1.SelectedIndex = 0
                    strTemp &= ListBox1.Text & ControlChars.NewLine
                    ListBox1.Items.RemoveAt(0)
                    numberOfnames2 -= 1
                Loop
                If strTemp.length < 3 Then
                Else
                    My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", strTemp, False)
                End If
                strTemp = Nothing
                Shell(path & "\projectRSN.exe")
                Do Until My.Computer.FileSystem.FileExists(path & "\settings.txt") = False
                    wait(10)
                Loop
                programs -= 1
            Loop
            My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", "", False)
        Else
            My.Computer.FileSystem.DeleteFile(path & "\settings.txt")
        End If
        '  MessageBox.Show(length.ToString & " " & repeatcounter.ToString)
        ' saveFiles()
        filereader = My.Computer.FileSystem.ReadAllText(path & "\emails.txt")
        items = Split(filereader, ControlChars.NewLine)
        For Each item As String In items
            If item.Length > 2 Then
                ListBox2.Items.Add(item)
                item = Nothing
            End If

        Next
        My.Computer.FileSystem.WriteAllText(path & "\emails.txt", "", False)


        numOfAccounts = ListBox1.Items.Count

        Do Until numOfAccounts = counter2 Or pause = True
            If ListBox2.Items.Count = 0 Then
                filereader = My.Computer.FileSystem.ReadAllText(path & "\emails.txt")
                items = Split(filereader, ControlChars.NewLine)
                For Each item As String In items
                    ListBox2.Items.Add(item)
                    item = Nothing
                Next
                My.Computer.FileSystem.WriteAllText(path & "\emails.txt", Nothing, False)
            Else
                ListBox2.SelectedIndex = 0
                If ListBox2.Text.Length < 3 Then
                    ListBox2.Items.RemoveAt(0)
                Else



                    'creation counter gets how many names were created
                    ListBox1.SelectedIndex = CreationCounter
                    ListBox2.SelectedIndex = CreationCounter
                    strEmail = ListBox2.Text

                    'loads account settings page
                    loadstart()
                    Do Until whatever111 = True
                        Dim didntwork As Boolean = False
                        Try
                            'trys to login once if fails try again if fail try a 3rd time
                            WebBrowser1.Document.GetElementById("username").SetAttribute("value", strEmail)
                            WebBrowser1.Document.GetElementById("password").SetAttribute("value", password & ControlChars.NewLine)
                            WebBrowser1.Document.GetElementById("submit1").InvokeMember("click")
                            '     WebBrowser1.Document.GetElementById("modalLoginButton").InvokeMember("click")

                        Catch ex As Exception
                            loadstart()
                            didntwork = True
                        End Try
                        If didntwork = False Then
                            whatever111 = True
                        End If
                        didntwork = False
                    Loop

                    wait(5)

                    If WebBrowser1.Url.ToString <> "https://secure.runescape.com/m=weblogin/login.ws" Then
                        'waits til browser loads
                        If WebBrowser1.IsBusy = True Then
                            Do Until WebBrowser1.IsBusy = False
                                wait(10)
                            Loop
                        End If
                        'gets the c= code from the url
                        codev = WebBrowser1.Url.ToString
                        codev = codev.Substring(28, 11)
                        username = ListBox1.Text
                        'creates the imposter site using the codev code
                        rsImposterSite(codev, username)
                        'waits til imposter site is loaded
                        If WebBrowser1.Url.ToString <> path & "\rsnamegenerator.html" Then
                            wait(10)
                            Do Until WebBrowser1.IsBusy = False
                                wait(10)
                            Loop
                        End If
                        'hits the ok key and sets the name
                        WebBrowser1.Document.GetElementById("confirm").InvokeMember("click")


                        Do Until WebBrowser1.Url.ToString.Substring(0, 5) = "https"
                            wait(10)
                        Loop
                        completedName = ListBox1.Text & ":::" & ListBox2.Text & ControlChars.NewLine
                        ListBox1.Items.RemoveAt(0)
                        ListBox2.Items.RemoveAt(0)

                        IO.File.AppendAllText(path & "\Imposters_Completed.txt", completedName)

                        whatever111 = False
                        'My.Computer.FileSystem.DeleteFile(path & "\rsnamegenerator.html")
                    Else
                        ListBox2.Items.RemoveAt(0)
                    End If
                End If
                counter2 += 1
            End If

        Loop
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        saveFiles()
        End
    End Sub
    Private Sub saveFiles()
        Dim results As String
        Do Until ListBox1.Items.Count = 0
            ListBox1.SelectedIndex = 0
            results &= ListBox1.Text & ControlChars.NewLine
            ListBox1.Items.RemoveAt(0)
        Loop
        IO.File.AppendAllText(path & "\usernames.txt", results)
        results = Nothing
        Do Until ListBox2.Items.Count = 0
            ListBox2.SelectedIndex = 0
            results &= ListBox2.Text & ControlChars.NewLine
            ListBox2.Items.RemoveAt(0)
        Loop
        IO.File.AppendAllText(path & "\emails.txt", results)
    End Sub
    Private Sub fileCheck()
        If My.Computer.FileSystem.FileExists(path & "\emails.txt") = False Then
            My.Computer.FileSystem.WriteAllText(path & "\emails.txt", Nothing, False)
        End If
        If My.Computer.FileSystem.FileExists(path & "\proxies.txt") = False Then
            My.Computer.FileSystem.WriteAllText(path & "\proxies.txt", Nothing, False)
        End If
        If My.Computer.FileSystem.FileExists(path & "\usernames.txt") = False Then
            My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", Nothing, False)
        End If
        If My.Computer.FileSystem.FileExists(path & "\runescape.html") = False Then
            Dim strHtml As String
            strHtml = "<html>" & ControlChars.NewLine &
"<head>" & ControlChars.NewLine &
"</head>" & ControlChars.NewLine &
"</body>" & ControlChars.NewLine &
"<form action=""https://secure.runescape.com/m=weblogin/login.ws"" method=""post"" autocomplete=""off"">" & ControlChars.NewLine &
"<input type=""text"" title=""Login"" name=""username"" maxlength=""200"" size=""26"" value="""" id=""username"" class=""NoPlaceholder"" >" & ControlChars.NewLine &
"<input type=""password"" title=""Password"" name=""password"" maxlength=""20"" size=""26"" value="""" id=""password"" class=""NoPlaceholder"" >" & ControlChars.NewLine &
"<input type=""submit"" id=""haha"" name=""haha"" title=""Login""/>" & ControlChars.NewLine &
"<input type=""submit"" id=""submit1"" name=""submit1"" value=""Join Free Now"">" & ControlChars.NewLine &
"<input type=""hidden"" value=""www"" name=""mod"">" & ControlChars.NewLine &
"<input type=""hidden"" value=""1"" name=""ssl"">" & ControlChars.NewLine &
"<input type=""hidden"" value=""account_settings.ws"" name=""dest"">" & ControlChars.NewLine &
"</form>" & ControlChars.NewLine &
"</body>" & ControlChars.NewLine &
"</html>"
            My.Computer.FileSystem.WriteAllText(path & "\runescape.html", strHtml, False)
        End If
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' loads rs account settings page on start
        Validate()
        fileCheck()
        Dim filereader As String
        Dim items() As String
        WebBrowser1.ScriptErrorsSuppressed = True
        If My.Computer.FileSystem.FileExists(path & "\settings.txt") Then
            primary = False
            filereader = My.Computer.FileSystem.ReadAllText(path & "\usernames.txt")
            items = Split(filereader, ControlChars.NewLine)
            For Each item As String In items
                If item.Length > 2 Then
                    ListBox1.Items.Add(item)
                    item = Nothing
                End If
            Next

            My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", Nothing, False)
	    start()
        Else
            primary = True
        End If

    End Sub
    Dim timercounter As Integer

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

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'opens form 2
        email.Enabled = True
        email.Visible = True
        Me.Enabled = False
        Me.Enabled = True
    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        loadstart()
        'refreshes page
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        saveFiles()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        saveFiles()
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim temp As String
        Dim k As Integer
        temp = My.Computer.FileSystem.ReadAllText(path & "\usernames.txt")
        strUsernames = Split(temp, ControlChars.NewLine)
        For Each item As String In strUsernames
            If item.Length > 2 Then
                ListBox1.Items.Add(item)
            End If
            strUsernames(k) = Nothing
            k += 1
        Next
        k = 0
        temp = My.Computer.FileSystem.ReadAllText(path & "\emails.txt")
        strEmails = Split(temp, ControlChars.NewLine)
        For Each item As String In strEmails
            If item.Length > 2 Then
                ListBox2.Items.Add(item)
            End If
            strEmails(k) = Nothing
            k += 1
        Next
        My.Computer.FileSystem.DeleteFile(path & "\emails.txt")
        My.Computer.FileSystem.DeleteFile(path & "\usernames.txt")
        My.Computer.FileSystem.WriteAllText(path & "\emails.txt", Nothing, False)
        My.Computer.FileSystem.WriteAllText(path & "\usernames.txt", Nothing, False)
    End Sub
    
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        pause = True
    End Sub

    Private Sub Validate()
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
End Class
