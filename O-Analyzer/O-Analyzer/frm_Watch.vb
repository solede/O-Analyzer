Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Watch
    '#######################################################################
    'DB監視画面
    '#######################################################################

    'モジュール変数
    Private M_CANCELFLG As Boolean = False

    ' ''''''''''''''''''''スレッドテスト中S
    'Private M_db As New CLS_DB.CLS_DB
    'Private M_MIDDLEWARE As Integer = 0
    'Private M_USER As String = vbNullString
    'Private M_PASSWORD As String = vbNullString
    'Private M_CONN_STR As String = vbNullString
    'Private M_IS_POOLING As Boolean = False
    'Private M_IS_SYSDBA As Boolean = False
    'Private M_I As String
    ' ''''''''''''''''''''スレッドテスト中E

    Private Sub watchDB()
        Dim sql As String
        Dim dateStr As New System.Text.StringBuilder(19)
        Dim dataTime As DateTime

        M_CANCELFLG = False
        My.Application.DoEvents()
        If M_CANCELFLG Then
            Exit Sub
        End If
        frm_Login.G_DB.SET_SELECT_FLG(False)
        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL()

        Using dataset As New DataSet
            'SQL実行
            If frm_Login.G_DB.GET_DATASET(sql, dataset, 0, False) = False Then
                '失敗
                dataTime = DateTime.Now
                dateStr.Append(dataTime.Year).Append("/").Append(dataTime.Month).Append("/").Append(dataTime.Day).Append(" ").Append(dataTime.Hour).Append(":").Append(dataTime.Minute).Append(":").Append(dataTime.Second)
                DGV_ALL.Rows.Add(dateStr.ToString, "Error", "Error", "Error", "Error", "Error", "Error")
                Exit Sub
            End If

            With dataset.Tables(0).Rows(0)
                If CHK_GAMEN.Checked Then
                    '画面出力
                    If TXT_USER_DEFINE.Text = vbNullString Then
                        DGV_ALL.Rows.Add(.Item(0).ToString, .Item(1).ToString, .Item(2).ToString, .Item(3).ToString, .Item(4).ToString, .Item(5).ToString, vbNullString)
                    Else
                        DGV_ALL.Rows.Add(.Item(0).ToString, .Item(1).ToString, .Item(2).ToString, .Item(3).ToString, .Item(4).ToString, .Item(5).ToString, .Item(6).ToString)
                    End If

                    If DGV_ALL.Rows.Count > Integer.parse(TXT_DISP_NUM.Text) Then
                        DGV_ALL.Rows.RemoveAt(0)
                    End If
                    LBL_INFO.Text = .Item(0).ToString & " 更新しました"
                End If

                If CHK_FILE.Checked Then
                    If My.Computer.FileSystem.DirectoryExists(System.IO.Directory.GetCurrentDirectory() & "\log") Then
                        M_FILE.CreateDir(System.IO.Directory.GetCurrentDirectory() & "\log")
                    End If
                    'ファイル出力
                    M_FILE.WriteString(.Item(0).ToString & "," & .Item(1).ToString & "," & .Item(2).ToString & "," & .Item(3).ToString & .Item(4).ToString & .Item(5).ToString & ControlChars.CrLf, System.IO.Directory.GetCurrentDirectory() & "\log", "watch.log")
                End If
            End With
        End Using

        If CHK_GRAFH.Checked Then
            '            DISP_GRAPH()
        End If
    End Sub

    Private Sub watchDB2()
        'While True
        '    Dim sql As String
        '    Dim dateStr As New System.Text.StringBuilder(19)
        '    Dim dataTime As DateTime

        '    M_db.SET_SELECT_FLG(False)
        '    'SELECT文発行
        '    'LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        '    'Me.Refresh()
        '    sql = CreateSQL()

        '    Using dataset As New DataSet
        '        'SQL実行
        '        If M_db.GET_DATASET(sql, dataset, 0, True) = False Then
        '            '失敗
        '            dataTime = DateTime.Now
        '            dateStr.Append(dataTime.Year).Append("/").Append(dataTime.Month).Append("/").Append(dataTime.Day).Append(" ").Append(dataTime.Hour).Append(":").Append(dataTime.Minute).Append(":").Append(dataTime.Second)
        '            'DGV_ALL.Rows.Add(dateStr.ToString, "Error", "Error", "Error")
        '            Exit Sub
        '        End If

        '        With dataset.Tables(0).Rows(0)
        '            If CHK_GAMEN.Checked Then
        '                '画面出力
        '                'DGV_ALL.Rows.Add(.Item(0).ToString, .Item(1).ToString, .Item(2).ToString, .Item(3).ToString, .Item(4).ToString, .Item(5).ToString)
        '                'If DGV_ALL.Rows.Count > Integer.Parse(TXT_DISP_NUM.Text) Then
        '                '    DGV_ALL.Rows.RemoveAt(0)
        '                'End If
        '                M_I = .Item(0).ToString
        '                'LBL_INFO.Text = .Item(0).ToString & " 更新しました"
        '                'LBL_INFO.Text = i & " 更新しました"
        '            End If
        '        End With
        '    End Using

        '    System.Threading.Thread.Sleep(2000)
        'End While
    End Sub

    'Sub DISP_GRAPH()
    '    Dim now_time As DateTime
    '    Dim colors As Color() = {Color.AliceBlue, Color.Red, Color.Yellow}
    '    Dim memory_total As ULong
    '    Dim min_value(0) As ULong
    '    Dim max_value(0) As ULong


    '    'dateTimeの取得
    '    DateTime.TryParseExact(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, "yyyy/MM/dd hh':'mm':'ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, now_time)

    '    'セッション数
    '    If Chart1.Series(0).Points.Count >= Integer.parse(TXT_DISP_NUM.Text) Then
    '        Chart1.Series("session").Points.RemoveAt(0)
    '        Chart1.Series("process").Points.RemoveAt(0)
    '        Chart2.Series("buffer").Points.RemoveAt(0)
    '        Chart2.Series("other").Points.RemoveAt(0)
    '        Chart2.Series("pga").Points.RemoveAt(0)
    '    End If
    '    Chart1.Series("session").Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(1).Value)
    '    'Chart1.Series(0).Points.AddXY(now_time, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(1).Value)

    '    'プロセス数
    '    Chart1.Series("process").Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(2).Value)
    '    'Chart1.Series(1).Points.AddXY(now_time, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(1).Value)

    '    'SGA(バッファプール)
    '    Chart2.Series("buffer").Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(4).Value)

    '    'SGA(その他)
    '    Chart2.Series("other").Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(5).Value)

    '    'PGA
    '    Chart2.Series("pga").Points.AddXY(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(0).Value, DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(3).Value)

    '    'y軸の最大値調整
    '    memory_total = Integer.parse(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(3).Value) + Integer.parse(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(4).Value) + Integer.parse(DGV_ALL.Rows(DGV_ALL.RowCount - 1).Cells(5).Value)
    '    If memory_total > Chart2.ChartAreas(0).AxisY.Maximum Then
    '        Chart2.ChartAreas(0).AxisY.Maximum = memory_total * 1.1
    '    End If

    '    min_value(0) = Chart1.Series("session").Points.FindMinByValue("Y").YValues(0)
    '    If min_value(0) > Chart1.Series("process").Points.FindMinByValue("Y").YValues(0) Then
    '        min_value(0) = Chart1.Series("process").Points.FindMinByValue("Y").YValues(0)
    '    End If

    '    max_value(0) = Chart1.Series("session").Points.FindMaxByValue("Y").YValues(0)
    '    If max_value(0) < Chart1.Series("process").Points.FindMaxByValue("Y").YValues(0) Then
    '        max_value(0) = Chart1.Series("process").Points.FindMaxByValue("Y").YValues(0)
    '    End If

    '    If Chart1.ChartAreas(0).AxisY.Minimum * 1.5 <= min_value(0) Then
    '        Chart1.ChartAreas(0).AxisY.Minimum = min_value(0) * 0.8
    '    End If
    '    If Chart1.ChartAreas(0).AxisY.Minimum >= min_value(0) Then
    '        Chart1.ChartAreas(0).AxisY.Minimum = min_value(0) * 0.5
    '    End If

    '    If Chart1.ChartAreas(0).AxisY.Maximum <= max_value(0) Then
    '        Chart1.ChartAreas(0).AxisY.Maximum = max_value(0) * 1.2
    '    End If
    '    If Chart1.ChartAreas(0).AxisY.Maximum * 0.7 > max_value(0) Then
    '        Chart1.ChartAreas(0).AxisY.Maximum = max_value(0) * 1.2
    '    End If

    '    'タイトルの最大値と最小値を更新
    '    Chart1.Titles(0).Text = "セッション数 max=" & Chart1.Series("session").Points.FindMinByValue("Y").YValues(0) & _
    '                                        " min=" & Chart1.Series("session").Points.FindMaxByValue("Y").YValues(0) & _
    '                            "プロセス数 max=" & Chart1.Series("process").Points.FindMinByValue("Y").YValues(0) & _
    '                                        " min=" & Chart1.Series("process").Points.FindMaxByValue("Y").YValues(0)

    '    Me.Refresh()
    'End Sub

    Private Function CreateSQL() As String
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
            If frm_Login.G_DB.G_ORA_VERSION(1) >= 9 Then
                sql = sql & "    , (SELECT TRUNC(VALUE/1024) FROM V$PGASTAT WHERE NAME = 'total PGA allocated') ""PGA_MEM(K)"" "
            Else
                sql = sql & "    , (SELECT TRUNC(SUM(VALUE)/1024) FROM V$SESSTAT WHERE STATISTIC#='20') ""PGA_MEM(K)"" "
            End If

        Else
            sql = sql & "    , 0 PGA_MEM "
        End If
        If CHK_BUFFER.Checked Then
            sql = sql & "    , (SELECT trunc(VALUE/1024) FROM V$SGA WHERE NAME = 'Database Buffers') ""SGA_BUFER(K)"" "
        Else
            sql = sql & "    , 0 ""SGA_BUFER(K)"" "
        End If
        If CHK_OTHER.Checked Then
            sql = sql & "    , (SELECT trunc(VALUE/1024) FROM V$SGA WHERE NAME = 'Variable Size') ""SGA_OTHER(K)"" "
        Else
            sql = sql & "    , 0 ""SGA_OTHER(K)"" "
        End If

        If TXT_USER_DEFINE.Text <> vbNullString Then
            sql = sql & ",(" & TXT_USER_DEFINE.Text & ")"
        End If
        sql = sql & "FROM DUAL"
        Return sql
    End Function

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXEC.Enabled = True
            BTN_CANCEL.Enabled = False
            GroupBox2.Enabled = True
            GroupBox3.Enabled = True
            GroupBox4.Enabled = True
        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXEC.Enabled = False
            BTN_CANCEL.Enabled = True
            GroupBox2.Enabled = False
            GroupBox3.Enabled = False
            GroupBox4.Enabled = False
        End If
    End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If TXT_INTERVAL.Text = vbNullString Or TXT_INTERVAL.Text = "0" Then
            MsgBox("間隔は1以上に設定してください")
            Exit Sub
        End If

        ''チャート初期化
        'Chart1.Series(0) = New System.Windows.Forms.DataVisualization.Charting.Series("session")
        'Chart1.Series(1) = New System.Windows.Forms.DataVisualization.Charting.Series("process")
        'Chart2.Series(0) = New System.Windows.Forms.DataVisualization.Charting.Series("buffer")
        'Chart2.Series(1) = New System.Windows.Forms.DataVisualization.Charting.Series("other")
        'Chart2.Series(2) = New System.Windows.Forms.DataVisualization.Charting.Series("pga")

        ''グラフ種類
        ''折れ線グラフ
        'Chart1.Series("session").ChartType = DataVisualization.Charting.SeriesChartType.Line
        'Chart1.Series("process").ChartType = DataVisualization.Charting.SeriesChartType.Line
        ''積み上げグラフ
        'Chart2.Series("buffer").ChartType = DataVisualization.Charting.SeriesChartType.StackedArea
        'Chart2.Series("other").ChartType = DataVisualization.Charting.SeriesChartType.StackedArea
        'Chart2.Series("pga").ChartType = DataVisualization.Charting.SeriesChartType.StackedArea

        ''値の型
        'Chart1.Series("session").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        'Chart1.Series("process").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        'Chart2.Series("buffer").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        'Chart2.Series("other").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        'Chart2.Series("pga").XValueType = DataVisualization.Charting.ChartValueType.DateTime
        'Chart1.Series("session").YValueType = DataVisualization.Charting.ChartValueType.Int64
        'Chart1.Series("process").YValueType = DataVisualization.Charting.ChartValueType.Int64
        'Chart2.Series("buffer").YValueType = DataVisualization.Charting.ChartValueType.Int64
        'Chart2.Series("other").YValueType = DataVisualization.Charting.ChartValueType.Int64
        'Chart2.Series("pga").YValueType = DataVisualization.Charting.ChartValueType.Int64

        ''凡例テキスト
        'Chart1.Series("session").LegendText = "セッション数"
        'Chart1.Series("process").LegendText = "プロセス数"
        'Chart2.Series("buffer").LegendText = "SGA(バッファプール)"
        'Chart2.Series("other").LegendText = "SGA(その他)"
        'Chart2.Series("pga").LegendText = "PGA"

        DGV_ALL.RowCount = 0

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        Timer1.Interval = Integer.Parse(TXT_INTERVAL.Text) * 1000
        Timer1.Enabled = True
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        Timer1.Enabled = False
        MsgBox(M_COMMON.GetMessage("I0008"))
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        watchDB()
    End Sub

    ' '''''''''''''''''''''''''スレッド実装バージョンテスト中S
    'Private Function aaa() As Boolean
    '    watchDB2()
    '    Return True
    'End Function
    'Delegate Sub DispWaitDelegate()
    'Sub worker2()
    '    Invoke(New DispWaitDelegate(AddressOf t2))
    'End Sub
    'Private Sub t2()
    '    aaa()
    'End Sub

    'Private Sub t3()
    '    watchDB2()
    'End Sub
    ' '''''''''''''''''''''''''スレッド実装バージョンテスト中E

    Private Sub frm_Watch_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
    End Sub

    Private Sub frm_Watch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        ''''グラフを表示するのタブページは今後開発。チャートコントロールには.netV4が必要なので別プロジェクトでライブラリを追加するか。
        TabPage2.Dispose()
        TabPage3.Dispose()
        CHK_GRAFH.Checked = False
        CHK_GRAFH.Visible = False

        Me.Text = "O-Analyzer - DB監視(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        M_FILE.SetControls(Me.Name, Me.Controls)
        LBL_INFO.Text = vbNullString
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

    Private Sub frm_Watch_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        TabControl1.Width = Me.Width - GroupBox1.Width - 30
        TabControl1.Height = Me.Height - LBL_INFO.Height - 50

        TABCTL.Width = TabControl1.Width - 15
        TABCTL.Height = TabControl1.Height - GroupBox3.Height - 40
        DGV_ALL.Width = TABCTL.Width - 15
        DGV_ALL.Height = TABCTL.Height - DGV_ALL.Location.Y - 30
        'Chart1.Width = DGV_ALL.Width
        'Chart1.Height = DGV_ALL.Height
        'Chart2.Width = DGV_ALL.Width
        'Chart2.Height = DGV_ALL.Height
        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub DGV_ALL_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_ALL.RowPostPaint
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    '''''''''''''''''''''''スレッド実装バージョンテスト中S
    '    'SQL実行
    '    M_db.SET_MIDDLEWARE(M_MIDDLEWARE)
    '    If M_db.G_CONN Is Nothing Then
    '        M_MIDDLEWARE = frm_Login.CMB_MIDDLE.SelectedIndex
    '        M_USER = frm_Login.G_DB.GET_LOGIN_USER
    '        M_PASSWORD = frm_Login.TXT_PASS.Text
    '        M_CONN_STR = frm_Login.G_DB.GET_CONNECT_STRING
    '        If M_db.DB_CONNECT(M_USER, M_PASSWORD, M_CONN_STR, frm_Login.TXT_OPTION.Text, M_IS_SYSDBA, False, True) = False Then
    '            '接続失敗
    '            MsgBox("接続失敗")
    '        End If
    '    End If

    '    M_db.SET_SELECT_FLG(False)

    '    Dim t2 As System.Threading.Thread
    '    t2 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf watchDB2))
    '    t2.Start()
    '    ''''''''''''''''''''''''スレッド実装バージョンテスト中E
    'End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class