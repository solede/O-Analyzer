Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Bind
    '#######################################################################
    'バインド変数設定画面
    '#######################################################################

    Sub SetBind(Optional ByVal dispInfo As Boolean = True)
        '#######################################################################
        'バインド変数セット
        '#######################################################################
        Dim paramName As String
        Dim bindTypeNumber As Short
        Dim bindInOut As ParameterDirection
        Dim bindValue As String
        Dim bindsize As Integer = 0
        Dim errcd() As String
        Dim errchars(0) As String
        Dim bind(DGV_BIND.RowCount - 2) As M_CONST.S_BIND

        If DGV_BIND.RowCount = 1 Then
            'データが何もない場合
            If dispInfo Then
                MsgBox(M_COMMON.GetMessage("E0005"))
            End If
            Exit Sub
        End If

        errchars(0) = ":"
        For i As Integer = 0 To DGV_BIND.RowCount - 2
            paramName = DGV_BIND.Rows(i).Cells(0).Value.ToString

            If DGV_BIND.Rows(i).Cells(2).Value = "OUT" Then
                'OUTパラメータの場合は値なし
                bindValue = vbNullString
            Else
                bindValue = DGV_BIND.Rows(i).Cells(3).Value.ToString
            End If

            errcd = M_COMMON.ValidateValue(paramName, M_CONST.C_VALIDATE_TYPE.STRING, , , errchars)
            If errcd(0) <> vbNullString Then
                If errcd(1) <> vbNullString Then
                    MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i & " " & "エラー文字：" & errcd(1))
                Else
                    MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i)
                End If
                Exit Sub
            End If

            If DGV_BIND.Rows(i).Cells(1).Value = "VARCHAR2" Or DGV_BIND.Rows(i).Cells(1).Value = vbNullString Then
                bindTypeNumber = M_CONST.C_COLUMN_TYPE.STRING
                bindsize = 32767
                If DGV_BIND.Rows(i).Cells(2).Value = "IN" Then
                    'INパラメータ以外はチェックしない
                    errcd = M_COMMON.ValidateValue(bindValue, M_CONST.C_VALIDATE_TYPE.STRING, 1, 4000)
                End If

                If errcd(0) <> vbNullString Then
                    If errcd(1) <> vbNullString Then
                        MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i & " " & "エラー文字：" & errcd(1))
                    Else
                        MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i)
                    End If
                    Exit Sub
                End If
            ElseIf DGV_BIND.Rows(i).Cells(1).Value = "NUMBER" Then
                bindTypeNumber = M_CONST.C_COLUMN_TYPE.NUMBER
                If DGV_BIND.Rows(i).Cells(2).Value = "IN" Then
                    'INパラメータ以外はチェックしない
                    errcd = M_COMMON.ValidateValue(bindValue, M_CONST.C_VALIDATE_TYPE.NUMBER)
                End If

                If errcd(0) <> vbNullString Then
                    MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i)
                    Exit Sub
                End If
            ElseIf DGV_BIND.Rows(i).Cells(1).Value = "CLOB" Then
                bindTypeNumber = M_CONST.C_COLUMN_TYPE.CLOB
                If DGV_BIND.Rows(i).Cells(2).Value = "IN" Then
                    'INパラメータ以外はチェックしない
                    errcd = M_COMMON.ValidateValue(bindValue, M_CONST.C_VALIDATE_TYPE.NUMBER)
                End If

                If errcd(0) <> vbNullString Then
                    If errcd(1) <> vbNullString Then
                        MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i & " " & "エラー文字：" & errcd(1))
                    Else
                        MsgBox(M_COMMON.GetMessage(errcd(0)) & " 行数:" & i)
                    End If
                    Exit Sub
                End If
            End If

            If DGV_BIND.Rows(i).Cells(2).Value = "IN" Then
                bindInOut = ParameterDirection.Input
            ElseIf DGV_BIND.Rows(i).Cells(2).Value = "OUT" Then
                bindInOut = ParameterDirection.Output
            ElseIf DGV_BIND.Rows(i).Cells(2).Value = "IN/OUT" Or DGV_BIND.Rows(i).Cells(2).Value = vbNullString Then
                bindInOut = ParameterDirection.InputOutput
            End If
            paramName = ":" & paramName

            bind(i).SET_BIND(paramName, bindValue, bindTypeNumber, bindInOut, bindsize)
        Next


        If frm_Login.G_DB.SET_BIND_VARIABLE(bind) = False Then
            If dispInfo Then
                MsgBox(M_COMMON.GetMessage("E0011"))
            End If
        Else
            If dispInfo Then
                MsgBox(M_COMMON.GetMessage("I0031"))
            End If
        End If
        frmClose()
    End Sub

    Private Sub frmClose()
        Me.Visible = False
    End Sub

    Private Sub BTN_UPDATE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_UPDATE.Click
        SetBind()
    End Sub

    Private Sub DGV_1_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_BIND.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub frm_Bind_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        frmClose()

    End Sub

    Private Sub frm_Bind_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        DGV_BIND.Width = GroupBox1.Location.X - 10
        DGV_BIND.Height = Me.Height - LBL_INFO.Height - 50
    End Sub

    Private Sub frm_Bind_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "O-Analyzer - バインド定義(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class