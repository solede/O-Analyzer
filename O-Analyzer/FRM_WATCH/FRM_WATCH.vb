Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_watch
    '#######################################################################
    'DB監視画面
    '#######################################################################

    'モジュール変数
    Private M_CANCELFLG As Boolean = False

    Private Sub watchDB()

        Dim sql As String
        Dim dataset As New DataSet
        Dim dateStr As String
        Dim dataTime As DateTime

        M_CANCELFLG = False
        My.Application.DoEvents()
        If M_CANCELFLG Then
            Exit Sub
        End If
        frm_Login.G_DB.SET_SELECT_FLG(False)
        'SELECT文発行
        LBL_INFO.Text = "実行中(サーバ)・・・"
        Me.Refresh()
        sql = CreateSQL()

        'SQL実行
        If frm_Login.G_DB.GET_DATASET(sql, dataset, 0, True) = False Then
            dataTime = DateTime.Now
            dateStr = dataTime.Year & "/" & dataTime.Month & "/" & dataTime.Day & " " & dataTime.Hour & ":" & dataTime.Minute & ":" & dataTime.Second
            DGV_ALL.Rows.Add(dataTime.Year & "/" & dataTime.Month & "/" & dataTime.Day & " " & dataTime.Hour & ":" & dataTime.Minute & ":" & dataTime.Second, "Error", "Error", "Error")
            Exit Sub
        End If

        With dataset.Tables(0).Rows(0)
            If CHK_GAMEN.Checked Then
                '画面出力
                DGV_ALL.Rows.Add(.Item(0).ToString, .Item(1).ToString, .Item(2).ToString, .Item(3).ToString)
                If DGV_ALL.Rows.Count > CInt(TXT_DISP_NUM.Text) Then
                    DGV_ALL.Rows.RemoveAt(0)
                End If
                LBL_INFO.Text = .Item(0).ToString & " 更新しました"
            End If

            If CHK_FILE.Checked Then
                If My.Computer.FileSystem.DirectoryExists(System.IO.Directory.GetCurrentDirectory() & "\log") Then
                    M_FILE.CreateDir(System.IO.Directory.GetCurrentDirectory() & "\log")
                End If
                'ファイル出力
                M_FILE.WriteCSV(.Item(0) & "," & .Item(1) & "," & .Item(2) & "," & .Item(3), System.IO.Directory.GetCurrentDirectory() & "\log", "watch.log")
            End If
        End With
        If CHK_GRAFH.Checked Then
            DISP_GRAPH()
        End If
    End Sub

    Sub DISP_GRAPH()
        Dim colors As Color() = {Color.AliceBlue, Color.Red, Color.Yellow}

        'セッション数
        Chart1.Series(0).Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(1).Value)
        'プロセス数
        Chart1.Series(1).Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(2).Value)

        ''GridViewからデータ取得
        'For rowCnt = 0 To DGV_ALL.RowCount - 1
        '    date1 = DGV_ALL.Rows(rowCnt).Cells(0).Value
        '    xDate = New ZedGraph.XDate(date1.Year, date1.Month, date1.Day, date1.Hour, date1.Minute, date1.Second)
        '    y_ses = DGV_ALL.Rows(rowCnt).Cells.Item(1).Value
        '    y_proc = DGV_ALL.Rows(rowCnt).Cells.Item(2).Value
        '    y_PGA = DGV_ALL.Rows(rowCnt).Cells.Item(3).Value
        '    list_ses.Add(xDate.XLDate, y_ses)
        '    list_proc.Add(xDate.XLDate, y_proc)
        '    list_PGA.Add(xDate.XLDate, y_PGA)
        'Next rowCnt

        'PGAグラフ

        Me.Refresh()
    End Sub

    Private Function CreateSQL()
        Dim sql As String
        sql = vbNullString
        sql = sql & "SELECT SYSDATE "
        If CHK_SES.Checked Then
            sql = sql & "    , (SELECT COUNT(*) FROM V$SESSION) SESSIONS "
        Else
            sql = sql & "    , 0 SESSIONS "
        End If
        If CHK_PROC.Checked Then
            sql = sql & "    , (SELECT COUNT(*) FROM V$PROCESS) PROCESSES "
        Else
            sql = sql & "    , 0 SESSIONS "
        End If

        If CHK_PGA.Checked Then
            sql = sql & "    , (SELECT SUM(PGA_ALLOC_MEM) FROM V$PROCESS) PGA_MEM "
        Else
            sql = sql & "    , 0 PGA_MEM "
        End If
        sql = sql & "FROM DUAL"
        Return sql
    End Function

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS_NORMAL Then
            BTN_EXEC.Enabled = True
            BTN_CANCEL.Enabled = False
            GroupBox3.Enabled = True
            GroupBox1.Enabled = True
        ElseIf statusID = M_CONST.C_FORM_STATUS_EXECUTING Then
            BTN_EXEC.Enabled = False
            BTN_CANCEL.Enabled = True
            GroupBox3.Enabled = False
            GroupBox1.Enabled = False
        End If
    End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If TXT_INTERVAL.Text = vbNullString Or TXT_INTERVAL.Text = "0" Then
            MsgBox("間隔は1以上に設定してください")
            Exit Sub
        End If

        'チャート初期化
        Chart1.Series(0) = New System.Windows.Forms.DataVisualization.Charting.Series
        Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Line
        Chart1.Series(1) = New System.Windows.Forms.DataVisualization.Charting.Series
        Chart1.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line

        ChangeFormStatus(M_CONST.C_FORM_STATUS_EXECUTING)
        Timer1.Interval = CInt(TXT_INTERVAL.Text) * 1000
        Timer1.Enabled = True
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        Timer1.Enabled = False
        MsgBox("中止しました")
        ChangeFormStatus(M_CONST.C_FORM_STATUS_NORMAL)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        watchDB()
    End Sub

    Private Sub TXT_DISP_NUM_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        '数字以外の入力を無効化
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) _
         And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub TXT_INTERVAL_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        '数字以外の入力を無効化
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) _
         And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub frm_watch2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim Pane As ZedGraph.GraphPane
        'LBL_INFO.Text = vbNullString
        'Pane = ZG_SES.GraphPane
        'Pane.Title.Text = "セッション"
        'Pane.XAxis.Title.Text = "時系列"
        'Pane.YAxis.Title.Text = "セッション数"
        'Pane = ZG_PROC.GraphPane
        'Pane.Title.Text = "プロセス"
        'Pane.XAxis.Title.Text = "時系列"
        'Pane.YAxis.Title.Text = "プロセス数"
        'Pane = ZG_PGA.GraphPane
        'Pane.Title.Text = "PGA"
        'Pane.XAxis.Title.Text = "時系列"
        'Pane.YAxis.Title.Text = "PGAサイズ"
        Me.Text = "DB監視(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub TXT_DISP_NUM_KeyPress_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXT_DISP_NUM.KeyPress, TXT_INTERVAL.KeyPress
        '数字以外の入力を無効化
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) _
         And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub
End Class