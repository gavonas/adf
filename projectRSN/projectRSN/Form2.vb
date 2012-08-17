Imports System.Threading
Public Class Form2
    Dim codev As String



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim listboxitems As Integer
        Dim numoflistboxitems As Integer
        Dim counter As Integer
        'login info to a bank account used to check usernames
        '    ListBox2.SelectedIndex = 0
        Dim email As String
        Dim password As String
        Dim username As String
        Dim counter2 As Integer
        Dim url As String

        listboxitems = ListBox1.Items.Count

        email = InputBox("First Username")
        password = InputBox("First Password")


        WebBrowser1.Navigate(Form3.path & "\runescape.html")
        wait(5)
        If WebBrowser1.IsBusy = True Then


            Do Until WebBrowser1.IsBusy = False
                wait(1)
                ' MessageBox.Show("broswer loading")
            Loop
        End If
        Try
            WebBrowser1.Document.GetElementById("username").SetAttribute("value", email)
            WebBrowser1.Document.GetElementById("password").SetAttribute("value", password & ControlChars.NewLine)
            WebBrowser1.Document.GetElementById("submit1").InvokeMember("click")
        Catch


        End Try

        ' MessageBox.Show("Checking Now, you have:" & numoflistboxitems.ToString & " names.  This will take aproximately " & (numoflistboxitems * 2).ToString & "seconds to complete")
        wait(20)
        Do Until WebBrowser1.IsBusy = False
            wait(1)

        Loop
        codev = WebBrowser1.Url.ToString
        codev = codev.Substring(28, 11)
        'loops counter 

        listboxitems = ListBox1.Items.Count
        Integer.TryParse(listboxitems, numoflistboxitems)
        'loads account settings page

        Do Until counter2 = numoflistboxitems


            ListBox1.SelectedIndex = counter
            username = ListBox1.Text

            'loads the url runescape uses to check if names are valid
            url = "https://secure.runescape.com/m=displaynames/g=runescape/c=" & codev & "/check_name.ws?displayname=" & username
            namechecker(url)
            If WebBrowser1.DocumentText.Length > 200 Then
                wait(5)
            End If

            'page will either bring back "OK" or "NOK list of names"
            'if page is ok go to next if not remove from list and go to next
            If WebBrowser1.DocumentText.Length < 200 Then


                If WebBrowser1.DocumentText.Substring(0, 2) <> "OK" Then

                    ListBox1.Items.RemoveAt(counter)
                    counter -= 1

                End If
            Else
                wait(20)
                If WebBrowser1.DocumentText.Substring(0, 2) <> "OK" Then

                    ListBox1.Items.RemoveAt(counter)
                    counter -= 1

                End If
            End If
            counter += 1
            counter2 += 1


        Loop
        MessageBox.Show("finished")
    End Sub

    Private Sub namechecker(ByRef url As String)
        'function for above
        WebBrowser1.Navigate(url)
        wait1(5)
        Do Until WebBrowser1.IsBusy = False
            wait1(5)
        Loop
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
    Private Sub wait1(ByRef seconds As Integer)

        'wait function
        Dim sw As New Stopwatch

        sw.Start()
        seconds *= 10
        Do Until sw.ElapsedMilliseconds > seconds
            System.Windows.Forms.Application.DoEvents()
        Loop

        sw.Stop()

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim filename As String
        Dim infile As IO.StreamReader
        Dim filename1 As New OpenFileDialog
        Dim ok1 As DialogResult

        'loads usernames from .txt
        filename1.Filter = "(*.txt)|*.txt"

        ok1 = filename1.ShowDialog()
        filename = filename1.FileName.ToString



        If ok1 = Windows.Forms.DialogResult.OK Then
            infile = IO.File.OpenText(filename)

            Do Until infile.Peek = -1
                ListBox1.Items.Add(infile.ReadLine)
            Loop
            infile.Close()
        End If



    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim saveFile As String = String.Empty
        Dim saveFileName As New SaveFileDialog
        Dim saveFileName1 As String
        Dim listboxitems As String
        Dim numoflisboxitems As Integer
        Dim counter As Integer
        Dim ok1 As DialogResult

        'saves usernames to .txt
        saveFileName.Filter = "(*.txt)|*.txt"


        ok1 = saveFileName.ShowDialog()
        If ok1 = Windows.Forms.DialogResult.OK Then


            listboxitems = ListBox1.Items.Count.ToString()
            Integer.TryParse(listboxitems, numoflisboxitems)

            Do Until counter = numoflisboxitems
                ListBox1.SelectedIndex = counter
                saveFile &= ListBox1.Text & ControlChars.NewLine
                counter += 1
            Loop
            saveFileName1 = saveFileName.FileName.ToString

            My.Computer.FileSystem.WriteAllText(saveFileName1, saveFile, False)
        End If
    End Sub

    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WebBrowser1.ScriptErrorsSuppressed = True
        Form3.Visible = False
        Form3.Enabled = False
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
        My.Computer.FileSystem.WriteAllText(Form3.path & "\runescape.html", strHtml, False)
    End Sub
End Class
