Imports System.Threading
Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_FILE = CLS_FILE.CLS_FILE
Imports M_COMMON = CLS_COMMON.CLS_COMMON

Public Class frm_Stress
    '#######################################################################
    '負荷テスト表示画面
    '#######################################################################

    'モジュール変数
    Private M_SYNC_THREAD As Thread
    Private M_THREADS() As Thread
    Private M_SESSION_CNT As Integer = 0
    Private M_SQL As String = ""
    Private M_THREAD_ID As Integer = 0
    Private M_THREAD_ID_SYNC As New Object
    Private M_LOG As New System.Text.StringBuilder()
    Private M_LOG_SYNC As New Object
    Private M_OUTPUT_LOG As Boolean = True
    Private M_INIT_AP_LOG As String
    Private M_TIMEOUT As Integer = 60
    Private M_MIDDLEWARE As Integer = 0
    Private M_USER As String = vbNullString
    Private M_PASSWORD As String = vbNullString
    Private M_CONN_STR As String = vbNullString
    Private M_IS_POOLING As Boolean = False
    Private M_IS_SYSDBA As Boolean = False
    Private M_CONNECT_END_CNT As Integer = 0
    Private M_CONNECT_END_SYNC As New Object
    Private M_END_THREAD_CNT As Integer = 0
    Private M_END_THREAD_CNT_SYNC As New Object
    Private M_CONNECT_SYNC As New Object
    Private M_IS_EXECUTING As Boolean = True
    Private M_CANCEL As Boolean = False

    Private Structure S_THREADINFO
        Private id As Integer
        Private sql As String
        Private startTime As String
        Private endTime As String
        Private status As String
        Private count As Integer

        Public Sub setId(ByVal id As Integer)
            Me.id = id
        End Sub
        Public Sub SetSql(ByVal sql As String)
            Me.sql = sql
        End Sub
        Public Sub SetEndTime(ByVal endTime As String)
            Me.endTime = endTime
        End Sub
        Public Sub SetStartTime(ByVal startTime As String)
            Me.startTime = startTime
        End Sub
        Public Sub SetStatus(ByVal status As String)
            Me.status = status
        End Sub
        Public Sub SetCount(ByVal count As Integer)
            Me.count = count
        End Sub

        Public Function getId() As Integer
            Return Me.id
        End Function
        Public Function getSql() As String
            Return Me.sql
        End Function
        Public Function GetStartTime() As String
            Return Me.startTime
        End Function
        Public Function GetEndTime() As String
            Return Me.endTime
        End Function
        Public Function GetStatus() As String
            Return Me.status
        End Function
        Public Function GetCount() As Integer
            Return Me.count
        End Function
    End Structure

    Private C_THREADINFO() As S_THREADINFO

    Private Sub SyncSub()
        'スレッド同期sub
        Dim timestr As DateTime
        Dim isEnableThread As Boolean = True

        '接続同期のチェックがついている場合はすべての接続処理が終了するまで待機する
        If CHK_CONNECT_SYNC.Checked Then
            SyncLock (M_CONNECT_SYNC)
                While True
                    If M_CONNECT_END_CNT = M_SESSION_CNT Then
                        SyncLock (M_LOG_SYNC)
                            timestr = DateTime.Now
                            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" 接続同期完了").AppendLine()
                        End SyncLock
                        Exit While
                    End If
                    Thread.Sleep(1000)
                End While
            End SyncLock
        End If

        '全てのスレッドが終了するまで待機する
        While True
            If M_END_THREAD_CNT = M_SESSION_CNT Then
                SyncLock (M_LOG_SYNC)
                    timestr = DateTime.Now
                    M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" 全ての処理が終了しました").AppendLine()
                End SyncLock
                'ORACLEコネクションを切断するためGCを実行する
                GC.Collect()
                GC.WaitForPendingFinalizers()
                SyncLock (M_LOG_SYNC)
                    timestr = DateTime.Now
                    M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" Full GCが終了しました").AppendLine()
                End SyncLock
                M_IS_EXECUTING = False
                Exit While
            Else
                isEnableThread = True
                For Each thread As Thread In M_THREADS
                    If thread Is Nothing Then
                        Exit For
                    End If
                    If thread.IsAlive Then
                        Exit For
                    End If
                Next
                ''全てのスレッドが終了していた場合は処理を抜ける
                'M_IS_EXECUTING = False
                'Exit While
            End If
            Thread.Sleep(1000)
        End While
    End Sub

    Private Sub StartThread()
        '#######################################################################
        'スレッド実行
        '#######################################################################
        Dim cls_db As New CLS_DB.CLS_DB
        Dim cnt As Long = 0
        Dim id As Integer = 0
        Dim sqlType As Integer
        Dim timestr As DateTime
        Dim returnCode(0) As String

        If M_CANCEL Then
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            Exit Sub
        End If

        SyncLock (M_THREAD_ID_SYNC)
            id = M_THREAD_ID
            M_THREAD_ID = M_THREAD_ID + 1
            sqlType = M_COMMON.CheckSql(C_THREADINFO(id).getSql)
        End SyncLock

        timestr = DateTime.Now
        C_THREADINFO(id).setId(id)
        C_THREADINFO(id).SetStatus("スレッド開始")
        C_THREADINFO(id).SetStartTime(timestr.ToString("HH:mm:ss.fff"))
        SyncLock (M_LOG_SYNC)
            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" スレッド開始").AppendLine()
        End SyncLock

        If M_CANCEL Then
            cancelTest(id)
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            Exit Sub
        End If

        Try
            cls_db.SET_MIDDLEWARE(M_MIDDLEWARE)
            If cls_db.DB_CONNECT(M_USER, M_PASSWORD, M_CONN_STR, frm_Login.TXT_OPTION.Text, M_IS_SYSDBA, False, id & ".txt") = False Then
                '接続失敗
                timestr = DateTime.Now
                If M_OUTPUT_LOG Then
                    SyncLock (M_LOG_SYNC)
                        M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 接続失敗").AppendLine()
                    End SyncLock
                End If
                C_THREADINFO(id).SetStatus("接続失敗")
                C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                SyncLock (M_CONNECT_END_SYNC)
                    M_CONNECT_END_CNT = M_CONNECT_END_CNT + 1
                End SyncLock
                SyncLock (M_END_THREAD_CNT_SYNC)
                    M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                End SyncLock
                Exit Sub
            End If

            If M_OUTPUT_LOG Then
                timestr = DateTime.Now
                SyncLock (M_LOG_SYNC)
                    M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 接続成功 SID=").Append(cls_db.GET_SID).AppendLine()
                End SyncLock
            End If
            C_THREADINFO(id).SetStatus("接続成功")
            SyncLock (M_CONNECT_END_SYNC)
                M_CONNECT_END_CNT = M_CONNECT_END_CNT + 1
            End SyncLock

            If M_CANCEL Then
                cancelTest(id)
                cls_db.DB_CLOSE(id & ".txt")
                SyncLock (M_END_THREAD_CNT_SYNC)
                    M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                End SyncLock
                Exit Sub
            End If
        Catch e As ThreadAbortException
            timestr = DateTime.Now
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 強制停止しました").AppendLine()
            End SyncLock
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock

            C_THREADINFO(id).SetStatus("強制停止しました")
            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
            SyncLock (M_CONNECT_END_SYNC)
                M_CONNECT_END_CNT = M_CONNECT_END_CNT + 1
            End SyncLock
            Exit Sub
        Catch ex As Exception
            timestr = DateTime.Now
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 接続失敗(予期せぬエラー) ").Append(ex.Message).AppendLine.Append(ex.StackTrace).AppendLine()
            End SyncLock
            SyncLock (M_CONNECT_END_SYNC)
                M_CONNECT_END_CNT = M_CONNECT_END_CNT + 1
            End SyncLock
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            C_THREADINFO(id).SetStatus("接続失敗(予期せぬエラー) " & ex.Message & vbCrLf & ex.StackTrace)
            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
            Exit Sub
        End Try

        If M_CANCEL Then
            cancelTest(id)
            cls_db.DB_CLOSE(id & ".txt")
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            Exit Sub
        End If

        If CHK_CONNECT_SYNC.Checked Then
            '接続の同期
            SyncLock M_CONNECT_SYNC
            End SyncLock
        End If

        If M_CANCEL Then
            cancelTest(id)
            cls_db.DB_CLOSE(id & ".txt")
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            Exit Sub
        End If

        Try
            For i As Integer = 0 To Integer.Parse(TXT_EXECUTE_CNT.Text) - 1
                If M_CANCEL Then
                    cancelTest(id)
                    cls_db.DB_CLOSE(id & ".txt")
                    SyncLock (M_END_THREAD_CNT_SYNC)
                        M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                    End SyncLock
                    Exit Sub
                End If

                C_THREADINFO(id).SetCount(i + 1)
                If sqlType = M_CONST.C_SQLTYPE.QUERY Or sqlType = M_CONST.C_SQLTYPE.OTHER Then
                    'SELECT文発行
                    If M_OUTPUT_LOG Then
                        timestr = DateTime.Now
                        SyncLock (M_LOG_SYNC)
                            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" SELECT開始").AppendLine()
                        End SyncLock
                    End If
                    C_THREADINFO(id).SetStatus("SELECT開始")
                    'cls_db.INIT_DATA_TABLE(False)

                    cls_db.SET_SELECT_FLG(False)
                    If cls_db.SET_RESULTSET(C_THREADINFO(id).getSql, M_TIMEOUT, False, id & ".txt") Then
                        If M_OUTPUT_LOG Then
                            timestr = DateTime.Now
                            SyncLock (M_LOG_SYNC)
                                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 解析終了").AppendLine()
                            End SyncLock
                        End If
                        C_THREADINFO(id).SetStatus("解析終了")
                        cls_db.INIT_DATA_TABLE(False)

                        If M_CANCEL Then
                            cancelTest(id)
                            cls_db.DB_CLOSE(id & ".txt")
                            SyncLock (M_END_THREAD_CNT_SYNC)
                                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                            End SyncLock
                            Exit Sub
                        End If

                        'フェッチ&表示
                        If FetchFromResultset(id, cls_db, Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH"))) = False Then
                            If M_CANCEL Then
                                cancelTest(id)
                                cls_db.DB_CLOSE(id & ".txt")
                                SyncLock (M_END_THREAD_CNT_SYNC)
                                    M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                                End SyncLock
                                Exit Sub
                            End If

                            timestr = DateTime.Now
                            SyncLock (M_LOG_SYNC)
                                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" フェッチ異常終了 ").AppendLine()
                            End SyncLock
                            SyncLock (M_END_THREAD_CNT_SYNC)
                                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                            End SyncLock
                            C_THREADINFO(id).SetStatus("フェッチ異常終了")
                            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                            cls_db.DB_CLOSE(id & ".txt")
                            Exit Sub
                        End If

                        If M_OUTPUT_LOG Then
                            timestr = DateTime.Now
                            SyncLock (M_LOG_SYNC)
                                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" フェッチ終了").AppendLine()
                            End SyncLock
                        End If
                        C_THREADINFO(id).SetStatus("フェッチ終了")
                    Else
                        timestr = DateTime.Now
                        SyncLock (M_LOG_SYNC)
                            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 解析異常終了 ").AppendLine()
                        End SyncLock
                        SyncLock (M_END_THREAD_CNT_SYNC)
                            M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                        End SyncLock
                        C_THREADINFO(id).SetStatus("解析異常終了")
                        C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                        cls_db.DB_CLOSE(id & ".txt")
                        Exit Sub
                    End If
                ElseIf sqlType = M_CONST.C_SQLTYPE.DML Or sqlType = M_CONST.C_SQLTYPE.DDL_DCL Then
                    If M_OUTPUT_LOG Then
                        timestr = DateTime.Now
                        SyncLock (M_LOG_SYNC)
                            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 処理開始").AppendLine()
                        End SyncLock
                    End If
                    C_THREADINFO(id).SetStatus("処理開始")
                    cls_db.INIT_DATA_TABLE(False)

                    If cls_db.EXEC_NONQUERY(C_THREADINFO(id).getSql, returnCode(0), , False, M_TIMEOUT, id & ".txt") Then
                        If M_OUTPUT_LOG Then
                            timestr = DateTime.Now
                            SyncLock (M_LOG_SYNC)
                                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 処理終了 ").Append(returnCode(0)).AppendLine()
                            End SyncLock
                        End If
                        C_THREADINFO(id).SetStatus("処理終了")

                        If M_CANCEL Then
                            cancelTest(id)
                            cls_db.DB_CLOSE(id & ".txt")
                            SyncLock (M_END_THREAD_CNT_SYNC)
                                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                            End SyncLock
                            Exit Sub
                        End If

                    Else
                        timestr = DateTime.Now
                        SyncLock (M_LOG_SYNC)
                            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 処理異常終了").AppendLine()
                        End SyncLock
                        SyncLock (M_END_THREAD_CNT_SYNC)
                            M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                        End SyncLock
                        C_THREADINFO(id).SetStatus("処理異常終了")
                        C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                        cls_db.DB_CLOSE(id & ".txt")
                        Exit Sub
                    End If

                    'プロシージャを実行する場合EXEC_NONQUERYを使うため不要
                    'ElseIf sqlType = M_CONST.C_SQLTYPE.PROCEDURE Then
                    '    If cls_db.EXEC_PROCEDURE(C_THREADINFO(id).getSql) Then
                    '        If M_OUTPUT_LOG Then
                    '            timestr = DateTime.Now
                    '            SyncLock (M_LOG_SYNC)
                    '                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 処理しました").AppendLine()
                    '            End SyncLock
                    '        End If
                    '        C_THREADINFO(id).SetStatus("処理しました")
                    '    Else
                    '        timestr = DateTime.Now
                    '        SyncLock (M_LOG_SYNC)
                    '            M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" count=").Append(i + 1).Append(" 処理が異常終了しました ").Append(returnCode(0)).AppendLine()
                    '        End SyncLock
                    '        SyncLock (M_END_THREAD_CNT_SYNC)
                    '            M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                    '        End SyncLock
                    '        C_THREADINFO(id).SetStatus("処理が異常終了しました " & returnCode(0))
                    '        C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                    '        cls_db.DB_CLOSE()
                    '        Exit Sub
                    '    End If
                End If
            Next

            If M_CANCEL Then
                cancelTest(id)
                cls_db.DB_CLOSE(id & ".txt")
                SyncLock (M_END_THREAD_CNT_SYNC)
                    M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                End SyncLock
                Exit Sub
            End If

            If CHK_WAIT_SESSION.Checked = False Then
                Do While True
                    timestr = DateTime.Now
                    SyncLock (M_LOG_SYNC)
                        M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 終了、キャンセル指示を待っています ").Append(returnCode(0)).AppendLine()
                    End SyncLock
                    C_THREADINFO(id).SetStatus("終了、キャンセル指示を待っています " & returnCode(0))
                    C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
                    While True
                        If M_CANCEL Then
                            cancelTest(id)
                            cls_db.DB_CLOSE(id & ".txt")
                            SyncLock (M_END_THREAD_CNT_SYNC)
                                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
                            End SyncLock
                            Exit Sub
                        End If
                        Thread.Sleep(1000)
                    End While
                Loop
            End If

            timestr = DateTime.Now
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 終了").AppendLine()
            End SyncLock
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            C_THREADINFO(id).SetStatus("終了しました")
            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))

            cls_db.DB_CLOSE(id & ".txt")
        Catch threadAbortException As ThreadAbortException
            timestr = DateTime.Now
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 強制停止しました").AppendLine()
            End SyncLock
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            C_THREADINFO(id).SetStatus("強制停止しました")
            If C_THREADINFO(id).GetEndTime() = vbNullString Then
                'セッションの終了待ちになっているスレッドはすでに終了時刻が入っているためNULLの場合は書き換えない
                C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
            End If
            cls_db.DB_CLOSE(id & ".txt")
        Catch e As Exception
            timestr = DateTime.Now
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" 異常終了 ").Append(e.Message).AppendLine.Append(e.StackTrace).AppendLine()
            End SyncLock
            SyncLock (M_END_THREAD_CNT_SYNC)
                M_END_THREAD_CNT = M_END_THREAD_CNT + 1
            End SyncLock
            C_THREADINFO(id).SetStatus("異常終了 " & e.Message & " " & e.StackTrace)
            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
            cls_db.DB_CLOSE(id & ".txt")
        Finally
            cls_db = Nothing
        End Try
    End Sub

    Private Function FetchFromResultset(ByVal id As Integer, ByRef cls_db As CLS_DB.CLS_DB, Optional ByVal fetchRowCnt As Long = 10000000000) As Boolean
        '#######################################################################
        'SELECTデータのフェッチ
        '#######################################################################
        Dim ReadedRowCnt As Long
        Dim RefleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String

        Using DataTable As New DataTable
            Do While True
                If M_CANCEL Then
                    Return False
                End If

                If cls_db.FETCH_TO_DATATABLE(DataTable, RefleshCnt, 1, 1, False, id & ".txt") = False Then
                    Return False
                End If
                msg(0) = cls_db.GET_ROW_COUNT.ToString
                If cls_db.GET_SELECT_FLG = False Then
                    Return True
                End If
                ReadedRowCnt = ReadedRowCnt + RefleshCnt
                C_THREADINFO(id).SetStatus(ReadedRowCnt & "件FETCH済み")
            Loop
        End Using
        Return True
    End Function

    Private Sub cancelTest(ByVal id As Integer)
        '通常キャンセル指示
        Dim timestr As DateTime

        timestr = DateTime.Now
        If M_OUTPUT_LOG Then
            SyncLock (M_LOG_SYNC)
                M_LOG.Append(timestr.ToString("HH:mm:ss.fff")).Append(" id=").Append(id).Append(" キャンセルしました").AppendLine()
            End SyncLock
        End If

        C_THREADINFO(id).SetStatus("キャンセルしました")
        If C_THREADINFO(id).GetEndTime = vbNullString Then
            C_THREADINFO(id).SetEndTime(timestr.ToString("HH:mm:ss.fff"))
        End If

    End Sub

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXECUTE.Enabled = True
            BTN_ABORT.Enabled = False
            BTN_CANCEL.Enabled = False
            BTN_BIND.Enabled = True
            TXT_THREAD_CNT.Enabled = True
            If frm_Login.G_DB.GET_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                TXT_TIMEOUT_VALUE.Enabled = True
            Else
                TXT_TIMEOUT_VALUE.Enabled = False
            End If
            TXT_INTERVAL.Enabled = True
            CHK_WAIT_SESSION.Enabled = True
            TXT_EXECUTE_CNT.Enabled = True
            CHK_NO_LOG.Enabled = True
            CHK_CONNECT_SYNC.Enabled = True
            BTN_COPY.Enabled = True
        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXECUTE.Enabled = False
            BTN_ABORT.Enabled = True
            BTN_CANCEL.Enabled = True
            BTN_BIND.Enabled = False
            TXT_THREAD_CNT.Enabled = False
            TXT_TIMEOUT_VALUE.Enabled = False
            TXT_INTERVAL.Enabled = False
            CHK_WAIT_SESSION.Enabled = False
            TXT_EXECUTE_CNT.Enabled = False
            CHK_NO_LOG.Enabled = False
            CHK_CONNECT_SYNC.Enabled = False
            BTN_COPY.Enabled = False
        End If
    End Sub

    Private Sub BTN_EXECUTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXECUTE.Click
        Dim errmsg As String = vbNullString

        '入力値チェック
        M_COMMON.ValidateControls(Me.Controls, errmsg)
        If errmsg <> vbNullString Then
            MsgBox(errmsg)
            Exit Sub
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        M_LOG = New System.Text.StringBuilder()
        M_SQL = TXT_SQL.Text
        M_SESSION_CNT = Integer.Parse(TXT_THREAD_CNT.Text)
        ReDim M_THREADS(M_SESSION_CNT - 1)
        ReDim C_THREADINFO(M_SESSION_CNT - 1)
        DGV_THREAD.RowCount = M_SESSION_CNT
        M_THREAD_ID = 0
        Timer1.Enabled = True
        M_MIDDLEWARE = frm_Login.CMB_MIDDLE.SelectedIndex
        M_USER = frm_Login.G_DB.GET_LOGIN_USER
        M_PASSWORD = frm_Login.TXT_PASS.Text
        M_CONN_STR = frm_Login.G_DB.GET_CONNECT_STRING
        M_CONNECT_END_CNT = 0
        M_END_THREAD_CNT = 0
        M_IS_EXECUTING = True
        M_TIMEOUT = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        M_CANCEL = False

        'ログ出力
        If CHK_NO_LOG.Checked Then
            M_OUTPUT_LOG = False
            M_CONST.SetInitParam(M_CONST.C_INITPARAM_INIT_APLOG & ",アプリケーションロギングを有効にします(0:無効 1:有効),0")
        Else
            M_OUTPUT_LOG = True
            M_CONST.SetInitParam(M_CONST.C_INITPARAM_INIT_APLOG & ",アプリケーションロギングを有効にします(0:無効 1:有効),1")
        End If

        'SYSDBA接続の有無
        If frm_Login.CHK_SYSDBA.Checked Then
            M_IS_SYSDBA = True
        Else
            M_IS_SYSDBA = False
        End If

        LBL_INFO.Text = M_COMMON.GetMessage("I0027")
        M_LOG.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append(" 開始しました").AppendLine()
        Me.Refresh()

        M_SYNC_THREAD = New Thread(AddressOf SyncSub)
        M_SYNC_THREAD.Start()

        Thread.Sleep(500)

        For i As Integer = 0 To M_SESSION_CNT - 1
            Application.DoEvents()
            If M_IS_EXECUTING = False Then
                Exit Sub
            End If
            '実行SQLの設定
            If DGV_THREAD.Rows(i).Cells(1).Value <> vbNullString Then
                C_THREADINFO(i).SetSql(DGV_THREAD.Rows(i).Cells(1).Value.ToString)
            Else
                C_THREADINFO(i).SetSql(M_SQL)
            End If
            'スレッド実行
            M_THREADS(i) = New Thread(AddressOf StartThread)
            M_THREADS(i).Start()
            Thread.Sleep(Integer.Parse(TXT_INTERVAL.Text))
        Next
        LBL_INFO.Text = M_COMMON.GetMessage("I0011")

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        TXT_LOG.Text = M_LOG.ToString
        If M_IS_EXECUTING = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Timer1.Enabled = False
            'ログ出力設定を元に戻す
            M_CONST.SetInitParam(M_CONST.C_INITPARAM_INIT_APLOG & ",アプリケーションロギングを有効にします(0:無効 1:有効)," & M_INIT_AP_LOG)
        End If

        For i As Integer = 0 To M_SESSION_CNT - 1
            DGV_THREAD.Rows(i).Cells(0).Value = C_THREADINFO(i).getId
            DGV_THREAD.Rows(i).Cells(1).Value = C_THREADINFO(i).getSql
            DGV_THREAD.Rows(i).Cells(2).Value = C_THREADINFO(i).GetStartTime
            DGV_THREAD.Rows(i).Cells(3).Value = C_THREADINFO(i).GetEndTime
            DGV_THREAD.Rows(i).Cells(4).Value = C_THREADINFO(i).GetStatus
            DGV_THREAD.Rows(i).Cells(5).Value = C_THREADINFO(i).GetCount
        Next

    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        'M_COMMON.ShowLog()
        M_COMMON.ShowLogDirectory()
    End Sub

    Private Sub frm_Stress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "O-Analyzer - 負荷テスト(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        If frm_Login.G_DB.GET_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            'ODP.net以外はタイムアウトプロパティをサポートしていない
            TXT_TIMEOUT_VALUE.Enabled = True
        Else
            TXT_TIMEOUT_VALUE.Enabled = False
            TXT_TIMEOUT_VALUE.Text = "60"
        End If
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        M_FILE.SetControls(Me.Name, Me.Controls)
        M_INIT_AP_LOG = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_APLOG)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub frm_Stress_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        If Not M_THREADS Is Nothing Then
            For i As Integer = 0 To M_THREADS.Length - 1
                If Not (M_THREADS(i) Is Nothing) Then
                    M_THREADS(i).Abort()
                End If
            Next
            'ログ出力設定を元に戻す
            M_CONST.SetInitParam(M_CONST.C_INITPARAM_INIT_APLOG & ",アプリケーションロギングを有効にします(0:無効 1:有効)," & M_INIT_AP_LOG)
        End If
    End Sub

    Private Sub BTN_ABORT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_ABORT.Click
        '強制停止
        BTN_ABORT.Enabled = False
        For i As Integer = 0 To M_SESSION_CNT - 1
            If Not (M_THREADS(i) Is Nothing) Then
                M_THREADS(i).Abort()
            End If
        Next
        M_SYNC_THREAD.Abort()
        M_LOG.Append(DateTime.Now.ToString("HH:mm:ss.fff") & " 強制停止を実行しました").AppendLine()
        M_IS_EXECUTING = False
        GC.Collect()
        GC.WaitForPendingFinalizers()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub TXT_THREAD_CNT_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_THREAD_CNT.Leave
        DGV_THREAD.RowCount = Integer.Parse(TXT_THREAD_CNT.Text)
        For i As Integer = 0 To Integer.Parse(TXT_THREAD_CNT.Text) - 1
            DGV_THREAD.Rows(i).Cells(0).Value = i
        Next
    End Sub

    Private Sub frm_Stress_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50

        SplitContainer1.Width = GroupBox1.Location.X - 10
        SplitContainer1.Height = Me.Height - LBL_INFO.Height - 50

        TXT_SQL.Width = SplitContainer1.Panel1.Width - 10
        BTN_COPY.Width = TXT_SQL.Width
        DGV_THREAD.Width = SplitContainer1.Panel1.Width - 10
        DGV_THREAD.Height = SplitContainer1.Height - TXT_SQL.Height - 95
        TXT_LOG.Height = SplitContainer1.Height - 20
        TXT_LOG.Width = SplitContainer1.Panel2.Width

        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub SplitContainer1_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize
        TXT_SQL.Width = SplitContainer1.Panel1.Width - 10
        BTN_COPY.Width = TXT_SQL.Width
        DGV_THREAD.Width = SplitContainer1.Panel1.Width - 10
    End Sub

    Private Sub SplitContainer1_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel2.Resize
        TXT_LOG.Width = SplitContainer1.Panel2.Width
    End Sub

    Private Sub BTN_COPY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_COPY.Click
        For i As Integer = 0 To DGV_THREAD.Rows.Count - 1
            DGV_THREAD.Rows(i).Cells(1).Value = TXT_SQL.Text
        Next
        'DGV_THREAD.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        DGV_THREAD.AutoResizeRows()
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        M_CANCEL = True
        M_LOG.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append(" キャンセル指示を設定しました").AppendLine()
    End Sub
End Class