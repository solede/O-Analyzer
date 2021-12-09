Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class FRM_INTERNAL

    Public frm_Login As Object

    Private M_CANCELFLG As Boolean = False

    Public Sub SetFrmLogin(ByRef value As Object)
        frm_Login = value
    End Sub

    Sub ChangeFormStatus(ByVal W_STATUS_ID As Short)
        If W_STATUS_ID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXEC.Enabled = True
            BTN_CANCEL.Enabled = False
        ElseIf W_STATUS_ID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXEC.Enabled = False
            BTN_CANCEL.Enabled = True
        End If
    End Sub

    Private Sub DispSharedpoolInfo()
        Dim sql As String = vbNullString

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        If Mid(CStr(CMB_GROUPBY.SelectedItem), 1, 1) = 0 Then
            '集約なし
            sql = CreateSQL(0)
        ElseIf Mid(CStr(CMB_GROUPBY.SelectedItem), 1, 1) = 1 Then
            'ヒープ集約
            sql = CreateSQL(1)
        ElseIf Mid(CStr(CMB_GROUPBY.SelectedItem), 1, 1) = 2 Then
            'ヒープ、属性別集約
            sql = CreateSQL(2)
        ElseIf Mid(CStr(CMB_GROUPBY.SelectedItem), 1, 1) = 3 Then
            'ヒープ、属性、SGAコンポーネント名別集約
            sql = CreateSQL(3)
        End If

        ExecQuery(sql, DGV_SHARED_POOL)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispExtraParameter()
        Dim sql As String = vbNullString

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(4)

        ExecQuery(sql, DGV_HIDDEN_PARAM)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Function CreateSQL(ByVal W_ID As Short) As String
        '#######################################################################
        'SQL作成
        'W_ID:SQL文の条件(3:共有プール断片化確認)
        '#######################################################################

        Dim sql As String = vbNullString
        Dim where As String = vbNullString
        Dim isStart As Boolean = True
        Dim orderSeq As String = vbNullString

        '整列条件
        If CMB_SORT_ORDER1.SelectedIndex = 0 Then
            orderSeq = "ASC "
        ElseIf CMB_SORT_ORDER1.SelectedIndex = 1 Then
            orderSeq = "DESC "
        End If

        '抽出条件
        If CHK_FREE.Checked Or CHK_FREEABLE.Checked Or CHK_PERMANENT.Checked Or _
           CHK_R_FREE.Checked Or CHK_R_FREEABLE.Checked Or CHK_RECREATABLE.Checked Then
            where = "WHERE KSMCHCLS IN ("
            If CHK_FREE.Checked Then
                If isStart Then
                    where = where & "'free' "
                    isStart = False
                Else
                    where = where & ",'free' "
                End If
            End If
            If CHK_FREEABLE.Checked Then
                If isStart Then
                    where = where & "'freeabl' "
                    isStart = False
                Else
                    where = where & ",'freeabl' "
                End If
            End If
            If CHK_PERMANENT.Checked Then
                If isStart Then
                    where = where & "'perm' "
                    isStart = False
                Else
                    where = where & ",'perm' "
                End If
            End If
            If CHK_R_FREE.Checked Then
                If isStart Then
                    where = where & "'R-free' "
                    isStart = False
                Else
                    where = where & ",'R-free' "
                End If
            End If
            If CHK_R_FREEABLE.Checked Then
                If isStart Then
                    where = where & "'R-freea' "
                    isStart = False
                Else
                    where = where & ",'R-freea' "
                End If
            End If
            If CHK_RECREATABLE.Checked Then
                If isStart Then
                    where = where & "'recr' "
                    isStart = False
                Else
                    where = where & ",'recr' "
                End If
            End If

            where = where & ")"
        End If

        If W_ID = 0 Then
            '集約なし
            sql = "SELECT 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' ""SUBPOOL"" ,"
            sql = sql & "KSMCHCLS ATTRIBUTE, KSMCHCOM NAME,KSMCHSIZ CHUNK_SIZE "
            sql = sql & "FROM X$KSMSP "
            sql = sql & where
            sql = sql & "ORDER BY 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')',KSMCHSIZ DESC"
        ElseIf W_ID = 1 Then
            'ヒープ集約
            sql = "SELECT 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' ""SUBPOOL"" ,"
            sql = sql & "SUM(KSMCHSIZ) TOTAL,COUNT(KSMCHSIZ) NUM,TRUNC(SUM(KSMCHSIZ)/COUNT(KSMCHSIZ)) AVG,"
            sql = sql & "MAX(KSMCHSIZ) ""MAX"" "
            sql = sql & "FROM X$KSMSP "
            sql = sql & where
            sql = sql & "GROUP BY "
            sql = sql & "'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' "
            sql = sql & "ORDER BY 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')',SUM(KSMCHSIZ) DESC"
        ElseIf W_ID = 2 Then
            'ヒープ、属性別
            sql = "SELECT 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' ""SUBPOOL"" ,"
            sql = sql & "KSMCHCLS ATTRIBUTE, SUM(KSMCHSIZ) TOTAL,COUNT(KSMCHSIZ) NUM,TRUNC(SUM(KSMCHSIZ)/COUNT(KSMCHSIZ)) AVG,"
            sql = sql & "MAX(KSMCHSIZ) ""MAX"" "
            sql = sql & "FROM X$KSMSP "
            sql = sql & where
            sql = sql & "GROUP BY "
            sql = sql & "'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')',KSMCHCLS "
            sql = sql & "ORDER BY KSMCHCLS,SUM(KSMCHSIZ) DESC"
        ElseIf W_ID = 3 Then
            'ヒープ、属性、SGAコンポーネント名
            sql = "SELECT 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' ""SUBPOOL"" ,"
            sql = sql & "KSMCHCLS ATTRIBUTE, KSMCHCOM,SUM(KSMCHSIZ) TOTAL,COUNT(KSMCHSIZ) NUM,TRUNC(SUM(KSMCHSIZ)/COUNT(KSMCHSIZ)) AVG,"
            sql = sql & "MAX(KSMCHSIZ) ""MAX"" "
            sql = sql & "FROM X$KSMSP "
            sql = sql & where
            sql = sql & "GROUP BY "
            sql = sql & "'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')',KSMCHCLS, KSMCHCOM "
            If CMB_SORT_VAL1.SelectedIndex = 0 Then
                sql = sql & "ORDER BY 'SGA HEAP('||KSMCHIDX||','||(KSMCHDUR-1)||')' "
                sql = sql & orderSeq
            ElseIf CMB_SORT_VAL1.SelectedIndex = 1 Then
                sql = sql & "ORDER BY KSMCHCLS "
                sql = sql & orderSeq
            ElseIf CMB_SORT_VAL1.SelectedIndex = 2 Then
                sql = sql & "ORDER BY KSMCHCOM "
                sql = sql & orderSeq
            End If
        ElseIf W_ID = 4 Then
            sql = "SELECT A.KSPPINM ""PARAMETER"",A.KSPPDESC ""DESCRIPTION"",B.KSPPSTVL ""VALUE"" "
            sql = sql & "FROM X$KSPPI A, X$KSPPCV B "
            sql = sql & "WHERE A.INDX = B.INDX "
            If TXT_USER_DEFINE.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE.Text & " "
            End If
            sql = sql & "ORDER BY A.KSPPINM "
        End If
        Return sql
    End Function

    Private Sub ExecQuery(ByVal sql As String, ByRef DGV As DataGridView)
        '#######################################################################
        'SELECT実行
        '#######################################################################

        'DGVの行の新規追加可能
        DGV.AllowUserToAddRows = True

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)

        'カレントセルの列位置が大きい、列幅が大きい場合などに
        '「DGV_1.DataSource = Nothing」の処理がエラーになるため移動
        If DGV.CurrentCellAddress.X > 0 Then
            DGV.CurrentCell = DGV(0, 0)
        End If
        For Each w_obj As Object In DGV.Columns
            w_obj.Width = 1
        Next

        DGV.DataSource = Nothing

        '列幅自動調整無効
        DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        '接続型
        DGV.RowCount = 1
        DGV.ColumnCount = 0

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV, M_CONST.GetInitParamValue("INIT_FETCH"))
        Else
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If
    End Sub

    Private Sub FetchFromResultset(ByRef DGV As DataGridView, Optional ByVal W_FETCH_ROW_CNT As Long = 10000000000)
        '#######################################################################
        'SELECT実行
        '#######################################################################
        Dim dataTable As New DataTable
        Dim ReadedRowCnt As Long
        Dim RefleshCnt As Integer = M_CONST.GetInitParamValue("INIT_FETCH_REFLESH")
        Dim msg(0) As String

        Do While ReadedRowCnt < W_FETCH_ROW_CNT
            If frm_Login.G_DB.FETCH_TO_DATATABLE(dataTable, RefleshCnt) = False Then
                LBL_INFO.Text = ""
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                DGV.DataSource = dataTable
                DGV.AllowUserToAddRows = False
                Exit Do
            End If

            Try
                'セルドラッグ時の場合のエラー対応
                System.Windows.Forms.Application.DoEvents()
            Catch ex As Exception
                'NULL
            End Try

            'キャンセル指示があった場合
            If M_CANCELFLG Then
                DGV.DataSource = dataTable
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
            ReadedRowCnt = ReadedRowCnt + RefleshCnt
        Loop
        DGV.DataSource = dataTable

        M_COMMON.SetRowheaderWidth(DGV)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)

    End Sub

    Private Sub frm_SGA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '画面初期設定読み込み
        CMB_GROUPBY.SelectedIndex = 0
        CMB_SORT_VAL1.SelectedIndex = 0
        CMB_SORT_ORDER1.SelectedIndex = 0
        CMB_SORT_ORDER2.SelectedIndex = 0
        CMB_SORT_VAL2.SelectedIndex = 0
        CMB_SORT_ORDER3.SelectedIndex = 0
        CMB_SORT_VAL3.SelectedIndex = 0

        M_FILE.SetControls(Me.Name, Me.Controls)
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub TabControl1_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Selecting
        If TabControl1.SelectedIndex = 0 Then

        ElseIf TabControl1.SelectedIndex = 1 Then

        End If
    End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If TabControl1.SelectedIndex = 0 Then
            DispSharedpoolInfo()
        ElseIf TabControl1.SelectedIndex = 1 Then
            DispExtraParameter()
        Else
        End If
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        M_CANCELFLG = True
    End Sub

    Private Sub frm_info_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        TabControl1.Location = New Point(5, 5)
        TabControl1.Width = GroupBox1.Location.X - 10
        TabControl1.Height = Me.Height - LBL_INFO.Height - 55
        TabPage1.Width = TabControl1.Width - 10
        TabPage1.Height = TabControl1.Height - 10

        DGV_SHARED_POOL.Width = TabControl1.Width - 20
        DGV_SHARED_POOL.Height = TabControl1.Height - DGV_SHARED_POOL.Location.Y - 30
        DGV_HIDDEN_PARAM.Width = TabControl1.Width - 20
        DGV_HIDDEN_PARAM.Height = TabControl1.Height - DGV_HIDDEN_PARAM.Location.Y - 30

        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_SHARED_POOL.RowPostPaint, DGV_HIDDEN_PARAM.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub EXCUTING_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DGV_SHARED_POOL.KeyDown
        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub frm_SGA_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
    End Sub

End Class