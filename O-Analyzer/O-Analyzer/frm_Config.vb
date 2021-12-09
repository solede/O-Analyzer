Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Config
    '#######################################################################
    'パラメータ設定画面
    '#######################################################################

    Private Sub GetInitParam()
        Dim initparamStructure As Object
        Dim initparam(2) As String

        'コンフィグパラメータ読み込み
        For Each initparamStructure In M_CONST.C_INITPARAM
            initparam(0) = initparamStructure.GET_NAME
            initparam(1) = initparamStructure.GET_INFO
            initparam(2) = initparamStructure.GET_VALUE
            DGV_PARAM.Rows.Add(initparam)
        Next
        DGV_PARAM.AutoResizeColumns(DataGridViewAutoSizeColumnMode.AllCells)
    End Sub

    Private Function CheckInitParam() As Boolean
        Dim dgvRows(DGV_PARAM.Rows.Count - 1) As String
        Dim errMsg As String = vbNullString

        dgvRows = M_COMMON.GetDGVStr(False, DGV_PARAM, ",", vbNullString)
        For rowCnt As Integer = 0 To dgvRows.Length - 1
            errMsg = errMsg & M_COMMON.CheckInitParam(dgvRows(rowCnt))
        Next

        If errMsg <> vbNullString Then
            MsgBox(errMsg)
            Return False
        End If

        Return True

    End Function

    Private Sub SetInitparam()
        Dim dgvRows(DGV_PARAM.Rows.Count - 1) As String
        Dim msg As String = M_COMMON.GetMessage("I0025")

        dgvRows = M_COMMON.GetDGVStr(False, DGV_PARAM, ",", vbNullString)
        For rowCnt As Integer = 0 To dgvRows.Length - 1
            If M_CONST.SetInitParam(dgvRows(rowCnt)) = False Then
                MsgBox(M_COMMON.GetMessage("E0001"))
                Exit Sub
            End If
        Next
        CLS_FILE.CLS_FILE.CreateConfigFile(dgvRows)
        M_FILE.SetEncode()
        CLS_FILE.CLS_FILE.WriteApLog(msg)
        MsgBox(msg)
    End Sub

    Private Sub frm_Config_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "O-Analyzer - パラメータ設定(" & Process.GetCurrentProcess().Id() & ")"
        GetInitParam()
    End Sub

    Private Sub frm_Config_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        DGV_PARAM.RowCount = 0
    End Sub

    Private Sub BTN_UPDATE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_UPDATE.Click
        If CheckInitParam() Then
            SetInitparam()
        End If
        Me.Close()
    End Sub

    Private Sub frm_Config_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - 50

        DGV_PARAM.Width = GroupBox1.Location.X - 10
        DGV_PARAM.Height = Me.Height - 55
    End Sub

    Private Sub DGV_PARAM_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_PARAM.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class