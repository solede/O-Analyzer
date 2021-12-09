Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_FILE = CLS_FILE.CLS_FILE
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class CLS_ODP
    '#######################################################################
    'ODP.NET操作クラス
    '#######################################################################

    'グローバル変数
    Public G_CONN As New OracleConnection 'オラクルコネクション
    Public G_CONN2 As Object = G_CONN 'G_CONNを参照する変数
    Public G_TRAN As OracleTransaction

    'モジュール変数
    Private M_READER As OracleDataReader
    Private M_ADAPTER As OracleDataAdapter
    Private M_PARAMETERS() As OracleParameter
    Private M_TYPE() As Short
    Private M_ORA_VERSION(5) As String


    Function DB_CONNECT(ByVal W_USERID As String, ByVal W_PASS As String, ByVal W_CONN_STR As String, Optional ByVal W_OPTION As String = vbNullString, Optional ByVal W_SYSDBA_FLG As Boolean = False, Optional ByVal W_OUTPUT_ERR As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        'DBコネクション取得
        '  取得したコネクションをG_CONNに保持()
        '引数
        '  W_USERID        :接続ユーザID
        '  W_PASS          :接続パスワード
        '  W_CONN_STR      :接続文字列
        '  W_OPTION        :オプション文字列
        '  W_SYSDBA_FLG    :SYSDBAでの接続有無
        '  W_OUTPUT_ERR    :エラー時のメッセージボックスの出力有無
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim connStr As String = vbNullString
        Dim stopWatch As Stopwatch

        If W_OPTION = vbNullString Then
            'コネクションプーリングを無効にしないと明示的に切断されないため設定
            W_OPTION = "Pooling=false;"
        End If

        If W_USERID = vbNullString And W_PASS = vbNullString Then
            'OS認証
            connStr = "User Id= / " & ";Data Source=" + W_CONN_STR & ";" & W_OPTION
        ElseIf W_SYSDBA_FLG And W_PASS = vbNullString Then
            'SYSDBA接続(パスワード入力無(OS認証))
            connStr = "User Id= / ;Data Source=" + W_CONN_STR & ";DBA Privilege=SYSDBA;" & W_OPTION
        ElseIf W_SYSDBA_FLG And W_PASS <> vbNullString Then
            'SYSDBA接続(パスワード入力有(OS認証＋パスワード認証))
            connStr = "User Id=" & W_USERID & ";Password=" & W_PASS & ";Data Source=" + W_CONN_STR & ";DBA Privilege=SYSDBA;" & W_OPTION
        Else
            '通常接続
            connStr = "User Id=" & W_USERID & ";Password=" & W_PASS & ";Data Source=" + W_CONN_STR & ";" & W_OPTION
        End If

        Try
            G_CONN.ConnectionString = connStr
        Catch ex As Exception
            If W_OUTPUT_ERR Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteApLog("接続文字列の定義に失敗しました。ミドルウエア:" & M_CONST.GetMiddlewareName(CLS_CONST.CLS_CONST.C_MIDDLE_TYPE.ODP) & " 接続文字列:" & connStr.Replace("Password=" & W_PASS, "Password=XXXX") & " エラーメッセージ：" & ex.Message, , logName)
            Return False
        End Try

        Try
            stopWatch = stopWatch.StartNew()
            G_CONN.Open()
            stopWatch.Stop()

            G_CONN2 = G_CONN
            M_FILE.WriteApLog("接続に成功しました。処理時間:" & stopWatch.ElapsedMilliseconds & " ミドルウエア:" & M_CONST.GetMiddlewareName(CLS_CONST.CLS_CONST.C_MIDDLE_TYPE.ODP) & " 接続定義:" & G_CONN.ConnectionString.Replace("Password=" & W_PASS, "Password=XXXX"), , logName)
            Return True
        Catch ex As OracleException
            If W_OUTPUT_ERR Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteApLog("接続に失敗しました。ミドルウエア:" & M_CONST.GetMiddlewareName(CLS_CONST.CLS_CONST.C_MIDDLE_TYPE.ODP) & " 接続定義:" & G_CONN.ConnectionString.Replace("Password=" & W_PASS, "Password=XXXX") & " エラーメッセージ：" & ex.Message, , logName)
            Return False
        Catch ex2 As Exception
            If W_OUTPUT_ERR Then
                MsgBox(ex2.Message)
            End If
            M_FILE.WriteErrLog(ex2, , logName)
            Return False
        End Try
    End Function

    Public Function GET_VERSION() As String()
        Dim W_ORA_VERSION(5) As String
        Dim W_MOTO_POS As Short = 0 '元ポジション
        Dim W_SAKI_POS As Short = 0 '先ポジション

        'バージョン情報取得
        W_ORA_VERSION(0) = G_CONN.ServerVersion.ToString()
        For W_CNT As Short = 1 To 5
            W_SAKI_POS = InStr(W_SAKI_POS + 1, W_ORA_VERSION(0), ".")
            If W_SAKI_POS = 0 Then
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_ORA_VERSION(0).Length - W_MOTO_POS)
            Else
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_SAKI_POS - W_MOTO_POS - 1)
            End If

            W_MOTO_POS = W_SAKI_POS
        Next

        M_ORA_VERSION = W_ORA_VERSION
        Return W_ORA_VERSION
    End Function
    Function GET_COMPETENCE() As Boolean
        '#######################################################################
        '権限情報取得
        '  V$ビューやデータディクショナリにアクセスできるか確認する
        '戻り値
        '  TRUE  :参照成功
        '  FALSE :参照失敗
        '#######################################################################
        Try
            'V$VIEWの検索チェック
            If SET_ORACLEDATAREADER("SELECT 0 FROM V$INSTANCE WHERE ROWNUM = 1", 0, False) Then
                M_READER.Close()
            Else
                Return False
            End If

            'ディクショナリの検索チェック
            If SET_ORACLEDATAREADER("SELECT 0 FROM DBA_TABLES WHERE ROWNUM = 1", 0, False) Then
                M_READER.Close()
            Else
                Return False
            End If

            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
            Exit Try
        End Try
    End Function

    Function GET_CONNECT_STRING() As String
        '#######################################################################
        '接続文字列取得
        '  接続文字列を戻す
        '戻り値
        '  接続文字列
        '#######################################################################

        '接続文字列取得
        Return G_CONN.DataSource.ToString

    End Function

    Sub DB_CLOSE(Optional ByVal logName As String = vbNullString)
        '#######################################################################
        'DBコネクション切断
        ' コネクションを切断する
        '※コネクションプーリングを使用している場合は
        '　実行しても物理コネクションが破棄されないケースがある
        '#######################################################################
        Try
            OracleConnection.ClearPool(G_CONN)
            G_CONN.Close()
            G_CONN.Dispose()
            M_FILE.WriteApLog("コネクションを切断しました", , logName)
        Catch ex As Exception
            M_FILE.WriteApLog("コネクションの切断に失敗しました", , logName)
        End Try
    End Sub

    Public Function GET_DATA_ADAPTER(ByVal W_SQL As String, Optional ByVal W_TIMEOUT_VALUE As Integer = 0) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　SQLを実行し、OracleDataAdapterを取得する
        '引数
        '  W_SQL           :SQL文
        '  W_DATASET       :DataSetオブジェクト(参照戻し)
        '  W_TIMEOUT_VALUE :タイムアウト値(秒)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand(W_SQL, G_CONN)

        M_ADAPTER = Nothing
        'SQLタイムアウトの設定
        If W_TIMEOUT_VALUE > 0 Then
            W_CMD.CommandTimeout = W_TIMEOUT_VALUE
        End If

        If Not M_PARAMETERS Is Nothing Then
            W_CMD.BindByName = True
            For i As Integer = 0 To M_PARAMETERS.Length - 1
                Try
                    W_CMD.Parameters.Add(M_PARAMETERS(i))
                Catch ex As Exception
                    M_FILE.WriteErrLog(ex)
                    Exit For
                End Try
            Next
        End If

        Try
            M_ADAPTER = New OracleDataAdapter(W_CMD)
            'M_FILE.WriteApLog("OracleDataAdapter初期化終了")
            M_PARAMETERS = Nothing
            GET_DATA_ADAPTER = True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            M_ADAPTER = Nothing
            GET_DATA_ADAPTER = False
        End Try

    End Function

    Public Function FETCH_TO_DATASET(ByRef W_DATASET As DataSet, ByRef M_ROW_CNT As Integer, ByVal W_READ_CNT As Integer, _
                                ByVal W_TABLE_NAME As String, ByRef M_SELECT_FLG As Boolean, ByRef M_JITSU_READ_CNT As Integer) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　モジュールレベル変数のM_ADAPTERをDataSetに変換したものを参照渡しで戻す
        '引数
        '  W_DATASET    :DataSetオブジェクト(参照戻し)
        '  W_START_CNT  :読み込みスタート行数
        '  W_END_CNT    :読み込み行数
        '  W_TABLE_NAME :テーブル名
        '  M_ROW_CNT    :実際に読み込まれた行数
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Try
            M_JITSU_READ_CNT = M_ADAPTER.Fill(W_DATASET, M_ROW_CNT, W_READ_CNT, W_TABLE_NAME)
            If M_JITSU_READ_CNT < W_READ_CNT Then
                M_SELECT_FLG = False
            End If
            M_ROW_CNT = M_ROW_CNT + M_JITSU_READ_CNT
            FETCH_TO_DATASET = True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            W_DATASET = Nothing
            FETCH_TO_DATASET = False
        End Try
    End Function

    Public Function GET_DATASET(ByVal W_SQL As String, ByRef W_DATASET As DataSet, Optional ByVal W_TIMEOUT_VALUE As Integer = 0, Optional ByVal W_ERR_DISPLAY As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　OracleDataAdapterから結果セット(dataset)で取得する
        '引数
        '  W_SQL           :SQL文
        '  W_DATASET       :DataSetオブジェクト(参照戻し)
        '  W_TIMEOUT_VALUE :タイムアウト値(秒)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim stopWatch As Stopwatch

        Try
            M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL, , logName)
            stopWatch = stopWatch.StartNew()
            GET_DATA_ADAPTER(W_SQL, W_TIMEOUT_VALUE)
            M_ADAPTER.Fill(W_DATASET)
            stopWatch.Stop()
            M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode & ":" & stopWatch.ElapsedMilliseconds, , logName)
            GET_DATASET = True
        Catch ex As Exception
            If W_ERR_DISPLAY Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex, logName)
            W_DATASET = Nothing
            GET_DATASET = False
        End Try
    End Function

    Public Function SET_ORACLEDATAREADER(ByVal W_SQL As String, Optional ByVal W_TIMEOUT_VALUE As Integer = 0, Optional ByVal W_OUTPUT_ERR As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        'SELECTデータの接続型取得
        '　SELECT文の結果セットをクラス変数のM_READERにセットする
        '引数
        '  W_SQL              :SQL文
        '  W_TIMEOUT_VALUE    :タイムアウト値(秒)
        '  W_OUTPUT_ERR       :エラー表示フラグ
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim stopWatch As System.Diagnostics.Stopwatch
        M_READER = Nothing

        Try
            Using W_CMD As New OracleCommand(W_SQL, G_CONN)
                'SQLタイムアウトの設定
                If W_TIMEOUT_VALUE > 0 Then
                    W_CMD.CommandTimeout = W_TIMEOUT_VALUE
                End If

                'バインド値がある場合セット
                If Not M_PARAMETERS Is Nothing Then
                    W_CMD.BindByName = True
                    If W_CMD.Parameters.Count = 0 Then
                        For i As Integer = 0 To M_PARAMETERS.Length - 1
                            Try
                                W_CMD.Parameters.Add(M_PARAMETERS(i))
                            Catch ex As Exception
                                M_FILE.WriteErrLog(ex)
                                Exit For
                            End Try
                        Next
                    End If
                End If

                W_CMD.InitialLONGFetchSize = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ODP_LONGFETCHSIZE)

                M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL, , logName)
                stopWatch = stopWatch.StartNew()
                M_READER = W_CMD.ExecuteReader()
                stopWatch.Stop()
                M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode & ":" & stopWatch.ElapsedMilliseconds, , logName)

                M_READER.FetchSize = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ODP_FETCHSIZE)
                If M_ORA_VERSION(1) <= 9 And M_READER.FetchSize >= 65536 Then
                    '9.2のDBへの接続時にFetchSizeの値が大きい場合select * from dual等のSQLがORA-24381でエラーになる場合があるので512Kに制限する
                    M_READER.FetchSize = 524288
                End If
                M_PARAMETERS = Nothing
                SET_ORACLEDATAREADER = True
            End Using
        Catch ex As Exception
            If W_OUTPUT_ERR Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex, , logName)
            M_READER = Nothing
            SET_ORACLEDATAREADER = False
        End Try

    End Function

    Public Sub INIT_DATA_TABLE(ByRef M_DATA_TABLE As DataTable, ByRef W_COLUMN_COUNT As Long, ByRef W_ROW_COUNT As Long, Optional ByVal W_OUTPUT_ERR As Boolean = True)
        '#######################################################################
        'ORACLEDATAREADERをフェッチした結果を格納するデータテーブルオブジェクトの列定義を設定する
        '引数：
        ' M_DATA_TABLE          :データテーブルオブジェクト
        ' W_COLUMN_COUNT        :結果セットの列数
        ' W_ROW_COUNT           :結果セットの行数
        '#######################################################################
        Dim W_VALUES As String()
        Dim W_INT As Integer = 0
        Dim W_CNT As Integer

        Try
            '列数初期化
            W_COLUMN_COUNT = M_READER.FieldCount
            ReDim W_VALUES(W_COLUMN_COUNT - 1)

            '行数初期化
            W_ROW_COUNT = 0

            'データテーブルの列設定
            For W_CNT = 0 To W_COLUMN_COUNT - 1
                Try
                    M_DATA_TABLE.Columns.Add(M_READER.GetName(W_CNT))
                Catch ex As Exception
                    '同じ列名が複数あった場合最後に数字を付与した列名とする
                    W_INT = W_INT + 1
                    M_DATA_TABLE.Columns.Add(M_READER.GetName(W_CNT) & "(" & W_INT & ")")
                End Try

                '数値型の場合は列タイプを変更
                If M_READER.GetDataTypeName(W_CNT).ToUpper = "INT16" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Int16")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "INT32" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Int32")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "INT64" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Int64")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "DECIMAL" Then
                    'DECIMALでは桁あふれするデータを受け取る場合があるためDoubleで定義
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Double")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "SINGLE" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Single")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "DOUBLE" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Double")
                ElseIf M_READER.GetDataTypeName(W_CNT).ToUpper = "NUMBER" Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Decimal")
                End If
            Next
        Catch ex As Exception
            If W_OUTPUT_ERR Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex)
        End Try
    End Sub

    Public Function FETCH_TO_DATATABLE(ByRef M_ROW_COUNT As Long, ByRef M_DATA_TABLE As DataTable, ByRef M_SELECT_FLG As Boolean, _
                      ByRef W_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000, Optional ByVal W_MIN_DISP_CNT As Long = 0, Optional ByVal W_MAX_DISP_CNT As Long = 0, Optional ByVal W_ERR_DISPLAY As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        'フェッチデータ
        '  W_FETCH_COUNTで指定された行数分M_DATA_TABLEにデータを追加する
        '引数：
        ' M_ROW_COUNT          :フェッチ済み行数
        ' M_DATA_TABLE         :DataGridViewに設定するDataTableオブジェクト
        ' M_SELECT_FLG         :SELECT実行中フラグ
        ' W_COLUMN_COUNT       :SELECTした列数
        ' W_FETCH_COUNT        :フェッチ行数
        ' W_MIN_DISP_CNT       :表示開始行数
        ' W_MIN_DISP_CNT       :表示終了行数
        '#######################################################################
        Dim W_VALUES() As String

        Try
            W_COLUMN_COUNT = M_READER.FieldCount
            ReDim W_VALUES(W_COLUMN_COUNT - 1)

            If M_ROW_COUNT = 0 Then
                '初回のみreadする(次回以降はread済み状態のため不要)
                M_SELECT_FLG = M_READER.Read()
                SET_RESULT_TYPE(W_COLUMN_COUNT)
            End If

            While M_SELECT_FLG
                M_ROW_COUNT = M_ROW_COUNT + 1
                If (M_ROW_COUNT >= W_MIN_DISP_CNT And M_ROW_COUNT <= W_MAX_DISP_CNT) Or _
                   (W_MIN_DISP_CNT = 0 And W_MAX_DISP_CNT = 0) Then
                    '行データ取得
                    W_VALUES = GET_ROW_VALUES(W_COLUMN_COUNT, M_TYPE)
                    M_DATA_TABLE.Rows.Add(W_VALUES)
                End If

                'W_CNT件まで読み込み
                If M_ROW_COUNT Mod W_FETCH_COUNT = 0 Then
                    M_SELECT_FLG = True
                    Exit While
                End If

                M_SELECT_FLG = M_READER.Read()
            End While

            '読み込み件数+1件目のデータがあるか判断
            If M_READER.Read() Then
                M_SELECT_FLG = True
            Else
                M_SELECT_FLG = False
                M_READER.Close()
            End If
            Return True
        Catch ex As Exception
            M_SELECT_FLG = False
            If W_ERR_DISPLAY Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex, , logName)
            Return False
        End Try
    End Function

    Public Function FETCH_TO_STRING(ByRef W_ROW As String(), ByRef M_ROW_COUNT As Long, ByRef M_SELECT_FLG As Boolean, _
                          ByRef W_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000) As Boolean
        '#######################################################################
        'フェッチ行取得
        '  
        '引数：
        ' W_ROW         :戻す行データ
        ' M_ROW_COUNT   :現在の行数
        ' M_SELECT_FLG  :SELECT中フラグ
        ' W_COLUMN_COUNT:列数
        ' W_FETCH_COUNT :FETCHする行数
        '#######################################################################

        Dim W_VALUES() As String
        W_COLUMN_COUNT = M_READER.FieldCount

        Try
            M_SELECT_FLG = M_READER.Read()
            SET_RESULT_TYPE(W_COLUMN_COUNT)

            If M_SELECT_FLG Then
                W_VALUES = GET_ROW_VALUES(W_COLUMN_COUNT, M_TYPE)
                W_ROW = W_VALUES
                M_ROW_COUNT = M_ROW_COUNT + 1
                Return True
            Else
                W_ROW = Nothing
                Return False
            End If
        Catch ex As Exception
            M_SELECT_FLG = False
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
    End Function

    Private Function GET_ROW_VALUES(ByVal W_COLUMN_COUNT As Integer, ByRef W_TYPE As Short()) As String()
        '#######################################################################
        '行データ取得
        '  
        '引数：
        ' W_COLUMN_COUNT    :列数
        ' W_TYPE            :列のタイプ
        '#######################################################################
        Dim W_VALUES(W_COLUMN_COUNT - 1) As String

        For W_TEMP_COL_CNT = 0 To W_COLUMN_COUNT - 1
            W_VALUES(W_TEMP_COL_CNT) = vbNullString
            If Not M_READER.IsDBNull(W_TEMP_COL_CNT) Then
                If W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.STRING Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleString(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT16 Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetInt16(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT32 Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetInt32(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT64 Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetInt64(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.NUMBER Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleDecimal(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.DATE Then
                    'DATE型の場合は YYYY/MM/DD HH24:MI:SSの書式で表示
                    Dim sb As New System.Text.StringBuilder(19)
                    sb.Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Year).Append("/").Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Month).Append("/").Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Day).Append(" ").Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Hour).Append(":").Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Minute).Append(":").Append(M_READER.GetOracleDate(W_TEMP_COL_CNT).Second)
                    W_VALUES(W_TEMP_COL_CNT) = sb.ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleTimeStamp(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.LONG Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetString(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleTimeStampLTZ(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleTimeStampTZ(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.BLOB Then
                    'BLOB型は「BLOB:<length>byte」を表示
                    W_VALUES(W_TEMP_COL_CNT) = "BLOB:" & M_READER.GetOracleBlob(W_TEMP_COL_CNT).Length.ToString & "byte"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.RAW Then
                    W_VALUES(W_TEMP_COL_CNT) = BitConverter.ToString(M_READER.GetOracleBinary(W_TEMP_COL_CNT).Value, 0).Replace("-", "")
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.BFILE Then
                    'BFILE型は「BFILE:<ディレクトリ>:<ファイル名>」を表示
                    W_VALUES(W_TEMP_COL_CNT) = "BFILE:" & M_READER.GetOracleBFile(W_TEMP_COL_CNT).DirectoryName & ":" & M_READER.GetOracleBFile(W_TEMP_COL_CNT).FileName
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INTERVALDS Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleIntervalDS(W_TEMP_COL_CNT).ToString
                ElseIf W_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INTERVALYM Then
                    W_VALUES(W_TEMP_COL_CNT) = M_READER.GetOracleIntervalYM(W_TEMP_COL_CNT).ToString
                Else
                    W_VALUES(W_TEMP_COL_CNT) = "未対応"
                End If
            End If
        Next

        Return W_VALUES

    End Function

    Public Function EXEC_NONQUERY(ByVal W_SQL As String, ByRef W_COUNT As Long, Optional ByRef W_PARAMSTR As String() = Nothing, Optional ByVal W_ERR_DISPLAY As Boolean = True, Optional ByVal W_TIMEOUT_VALUE As Integer = 0, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        '更新SQL実行
        '  SELECT以外の更新系SQLや無名プロシージャ等を実行する。
        '引数
        '  W_SQL   :SQL文
        '  W_COUNT :DMLの処理件数(戻し用)
        '  W_PARAMSTR:バインド変数
        '  W_ERR_DISPLAY:エラー表示フラグ
        '  W_TIMEOUT_VALUE:タイムアウト値
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim stopWatch As Stopwatch

        Try
            Using W_CMD As New OracleCommand
                W_CMD.Connection = G_CONN
                W_CMD.CommandText = W_SQL
                W_CMD.CommandType = CommandType.Text

                If W_TIMEOUT_VALUE > 0 Then
                    W_CMD.CommandTimeout = W_TIMEOUT_VALUE
                End If

                If Not M_PARAMETERS Is Nothing Then
                    W_CMD.BindByName = True
                    For i As Integer = 0 To M_PARAMETERS.Length - 1
                        Try
                            W_CMD.Parameters.Add(M_PARAMETERS(i))
                        Catch ex As Exception
                            M_FILE.WriteErrLog(ex)
                            Exit For
                        End Try
                    Next
                End If

                M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL, , logName)
                stopWatch = stopWatch.StartNew()
                W_COUNT = W_CMD.ExecuteNonQuery()
                stopWatch.Stop()
                M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode & ":" & stopWatch.ElapsedMilliseconds, , logName)

                If Not W_PARAMSTR Is Nothing Then
                    For i = W_CMD.Parameters.Count - 1 To 0 Step -1
                        If W_CMD.Parameters(i).CollectionType = OracleCollectionType.PLSQLAssociativeArray Then
                            'バインド配列の場合
                            W_PARAMSTR(i) = vbNullString
                            For j As Integer = 0 To W_CMD.Parameters(i).Value.Length - 1
                                'crlfで区切っているためcrlfが含まれる可能性のがあるバインド変数には使わないこと
                                If W_CMD.Parameters(i).ArrayBindStatus(j) = OracleParameterStatus.NullFetched = False Then
                                    W_PARAMSTR(i) = W_PARAMSTR(i) & W_CMD.Parameters(i).Value(j).value & ControlChars.CrLf
                                End If
                            Next
                        Else
                            '非バインド配列の場合
                            If W_CMD.Parameters(i).Status = OracleParameterStatus.NullFetched Then
                                W_PARAMSTR(i) = vbNullString
                            Else
                                W_PARAMSTR(i) = W_CMD.Parameters(i).Value.ToString
                            End If
                        End If
                        W_CMD.Parameters.RemoveAt(i)
                    Next
                End If
                
            End Using

            M_PARAMETERS = Nothing

            Return True
        Catch ex As Exception
            If W_ERR_DISPLAY Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex, , logName)
            W_COUNT = -1
            Return False
        End Try
    End Function

    Public Function EXEC_PROCEDURE(ByVal W_SQL As String, Optional ByRef W_PARAMSTR As String() = Nothing) As Boolean
        '#######################################################################
        'プロシージャ実行
        '  ストアドプロシージャを実行する
        '引数
        '  W_SQL : SQL文
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim i As Integer = 0
        Dim stopWatch As Stopwatch

        Try
            Using W_CMD As New OracleCommand
                W_CMD.Connection = G_CONN
                W_CMD.CommandText = W_SQL
                W_CMD.CommandType = CommandType.StoredProcedure

                If Not M_PARAMETERS Is Nothing Then
                    W_CMD.BindByName = True
                    For i = 0 To M_PARAMETERS.Length - 1
                        Try
                            W_CMD.Parameters.Add(M_PARAMETERS(i))
                        Catch ex As Exception
                            M_FILE.WriteErrLog(ex)
                            Exit For
                        End Try
                    Next
                End If

                M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL, , )
                stopWatch = stopWatch.StartNew()
                W_CMD.ExecuteNonQuery()
                stopWatch.Stop()
                M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode & ":" & stopWatch.ElapsedMilliseconds, , )

                For i = W_CMD.Parameters.Count - 1 To 0 Step -1
                    W_PARAMSTR(i) = W_CMD.Parameters(i).Value.ToString
                    W_CMD.Parameters.RemoveAt(i)
                Next
            End Using
            M_PARAMETERS = Nothing

            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try

    End Function

    Public Function SET_BIND_VARIABLE(ByVal bind() As M_CONST.S_BIND) As Boolean
        '#######################################################################
        'バインド変数セット
        '  
        '引数
        '  W_BIND :バインド変数構造体
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim oraType(bind.Length - 1) As OracleDbType

        For i As Integer = 0 To bind.Length - 1
            If bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.STRING Then
                oraType(i) = OracleDbType.Varchar2
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.NUMBER Then
                oraType(i) = OracleDbType.Int64
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.DATE Then
                oraType(i) = OracleDbType.Date
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                oraType(i) = OracleDbType.TimeStamp
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.CLOB Then
                oraType(i) = OracleDbType.Clob
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.RAW Then
                oraType(i) = OracleDbType.Raw
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.BLOB Then
                oraType(i) = OracleDbType.Blob
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.LONG Then
                oraType(i) = OracleDbType.Long
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALDS Then
                oraType(i) = OracleDbType.IntervalDS
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALYM Then
                oraType(i) = OracleDbType.IntervalYM
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                oraType(i) = OracleDbType.TimeStampLTZ
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                oraType(i) = OracleDbType.TimeStampTZ
            ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.UNKNOWN Then
                M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                Return False
            Else
                M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                Return False
            End If
        Next

        M_PARAMETERS = Nothing
        ReDim M_PARAMETERS(bind.Length - 1)
        For i As Integer = 0 To bind.Length - 1
            If bind(i).GET_INOUT = ParameterDirection.Input Then
                M_PARAMETERS(i) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.Input)
                M_PARAMETERS(i).Value = bind(i).GET_VALUE
            ElseIf bind(i).GET_INOUT = ParameterDirection.Output Then
                M_PARAMETERS(i) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.Output)
            ElseIf bind(i).GET_INOUT = ParameterDirection.InputOutput Then
                M_PARAMETERS(i) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.InputOutput)
                M_PARAMETERS(i).Value = bind(i).GET_VALUE
            End If
            If bind(i).GET_SIZE <> 0 Then
                M_PARAMETERS(i).Size = bind(i).GET_SIZE
            End If
            M_FILE.WriteApLog("バインドしました。名前:" & bind(i).GET_NAME & " 型:" & bind(i).GET_TYPE & " サイズ:" & bind(i).GET_SIZE & " 値:" & bind(i).GET_VALUE & " IN/OUT:" & bind(i).GET_INOUT)
        Next

        Return True
    End Function

    Public Function SET_BIND_ARRAYVARIABLE(Optional ByVal bindArray() As M_CONST.S_BIND_ARR = Nothing, Optional ByVal bind() As M_CONST.S_BIND = Nothing) As Boolean
        '#######################################################################
        'バインド変数セット
        '  
        '引数
        '  W_BIND :バインド変数構造体
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim oraType() As OracleDbType
        Dim arrayBindValueMsg As String
        Dim arrayBindSizeMsg As String

        M_PARAMETERS = Nothing

        If Not (bindArray Is Nothing) Then
            ReDim oraType(bindArray.Length - 1)
            'PL/SQL連想配列のバインド変数が渡された場合
            For i As Integer = 0 To bindArray.Length - 1
                If bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.STRING Then
                    oraType(i) = OracleDbType.Varchar2
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.NUMBER Then
                    oraType(i) = OracleDbType.Int64
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.DATE Then
                    oraType(i) = OracleDbType.Date
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                    oraType(i) = OracleDbType.TimeStamp
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.CLOB Then
                    oraType(i) = OracleDbType.Clob
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.RAW Then
                    oraType(i) = OracleDbType.Raw
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.BLOB Then
                    oraType(i) = OracleDbType.Blob
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.LONG Then
                    oraType(i) = OracleDbType.Long
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALDS Then
                    oraType(i) = OracleDbType.IntervalDS
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALYM Then
                    oraType(i) = OracleDbType.IntervalYM
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                    oraType(i) = OracleDbType.TimeStampLTZ
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                    oraType(i) = OracleDbType.TimeStampTZ
                ElseIf bindArray(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.UNKNOWN Then
                    M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                    Return False
                Else
                    M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                    Return False
                End If
            Next

            'M_PARAMETERS = Nothing
            ReDim M_PARAMETERS(bindArray.Length - 1)
            For i As Integer = 0 To bindArray.Length - 1

                If bindArray(i).GET_INOUT = ParameterDirection.Input Then
                    M_PARAMETERS(i) = New OracleParameter(bindArray(i).GET_NAME, oraType(i), ParameterDirection.Input)
                    M_PARAMETERS(i).Value = bindArray(i).GET_VALUE
                ElseIf bindArray(i).GET_INOUT = ParameterDirection.Output Then
                    M_PARAMETERS(i) = New OracleParameter(bindArray(i).GET_NAME, oraType(i), ParameterDirection.Output)
                ElseIf bindArray(i).GET_INOUT = ParameterDirection.InputOutput Then
                    M_PARAMETERS(i) = New OracleParameter(bindArray(i).GET_NAME, oraType(i), ParameterDirection.InputOutput)
                    M_PARAMETERS(i).Value = bindArray(i).GET_VALUE
                End If

                'PL/SQL連想配列
                M_PARAMETERS(i).CollectionType = OracleCollectionType.PLSQLAssociativeArray
                M_PARAMETERS(i).Size = bindArray(i).GET_ARRAY_COUNT
                M_PARAMETERS(i).ArrayBindSize = bindArray(i).GET_ARRAY_BIND_SIZE

                '配列バインド変数の値をすべて出力する場合。ログが長くなりすぎることと現状個別に値を設定するケースがないため簡易ログを出力するよう変更
                'arrayBindValueMsg = "{"
                'arrayBindSizeMsg = "{"
                'For j As Integer = 0 To bindArray(i).GET_VALUE.Length - 1
                '    If j = 0 Then
                '        arrayBindValueMsg = arrayBindValueMsg & """" & bindArray(i).GET_VALUE(j) & """"
                '        arrayBindSizeMsg = arrayBindSizeMsg & """" & bindArray(i).GET_ARRAY_BIND_SIZE(j) & """"
                '    Else
                '        arrayBindValueMsg = arrayBindValueMsg & ",""" & bindArray(i).GET_VALUE(j) & """"
                '        arrayBindSizeMsg = arrayBindSizeMsg & ",""" & bindArray(i).GET_ARRAY_BIND_SIZE(j) & """"
                '    End If
                'Next
                'arrayBindValueMsg = arrayBindValueMsg & "}"
                'arrayBindSizeMsg = arrayBindSizeMsg & "}"
                'M_FILE.WriteApLog("配列バインドしました。名前:" & bindArray(i).GET_NAME & " 型:" & bindArray(i).GET_TYPE & " サイズ:" & arrayBindSizeMsg & " 値:" & arrayBindValueMsg & " IN/OUT:" & bindArray(i).GET_INOUT)

                '省略形
                arrayBindValueMsg = bindArray(i).GET_VALUE(0)
                arrayBindSizeMsg = bindArray(i).GET_ARRAY_BIND_SIZE(0)
                M_FILE.WriteApLog("配列バインドしました。名前:" & bindArray(i).GET_NAME & " 型:" & bindArray(i).GET_TYPE & " サイズ:" & arrayBindSizeMsg & " 値:" & arrayBindValueMsg & " IN/OUT:" & bindArray(i).GET_INOUT & "(残り " & bindArray(i).GET_VALUE.Length - 1 & " 個の値は省略しました)")
            Next
        End If

        If Not (bind Is Nothing) Then
            '配列ではないバインド変数が渡された場合

            '''''配列バインド変数の個数と非配列バインド変数の個数をセット
            Dim arrayBindCnt As Integer
            Dim BindCnt As Integer

            If Not (M_PARAMETERS Is Nothing) Then
                arrayBindCnt = M_PARAMETERS.Length
            Else
                arrayBindCnt = 0
            End If
            BindCnt = bind.Length

            ReDim oraType(BindCnt - 1)
            For i As Integer = 0 To BindCnt - 1
                If bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.STRING Then
                    oraType(i) = OracleDbType.Varchar2
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.NUMBER Then
                    oraType(i) = OracleDbType.Int64
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.DATE Then
                    oraType(i) = OracleDbType.Date
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                    oraType(i) = OracleDbType.TimeStamp
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.CLOB Then
                    oraType(i) = OracleDbType.Clob
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.RAW Then
                    oraType(i) = OracleDbType.Raw
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.BLOB Then
                    oraType(i) = OracleDbType.Blob
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.LONG Then
                    oraType(i) = OracleDbType.Long
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALDS Then
                    oraType(i) = OracleDbType.IntervalDS
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALYM Then
                    oraType(i) = OracleDbType.IntervalYM
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                    oraType(i) = OracleDbType.TimeStampLTZ
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                    oraType(i) = OracleDbType.TimeStampTZ
                ElseIf bind(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.UNKNOWN Then
                    M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                    Return False
                Else
                    M_FILE.WriteApLog("予期せぬタイプ番号:" & oraType(i))
                    Return False
                End If
            Next

            ReDim Preserve M_PARAMETERS(arrayBindCnt + BindCnt - 1)
            For i As Integer = 0 To BindCnt - 1
                If bind(i).GET_INOUT = ParameterDirection.Input Then
                    M_PARAMETERS(i + arrayBindCnt) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.Input)
                    M_PARAMETERS(i + arrayBindCnt).Value = bind(i).GET_VALUE
                ElseIf bind(i).GET_INOUT = ParameterDirection.Output Then
                    M_PARAMETERS(i + arrayBindCnt) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.Output)
                ElseIf bind(i).GET_INOUT = ParameterDirection.InputOutput Then
                    M_PARAMETERS(i + arrayBindCnt) = New OracleParameter(bind(i).GET_NAME, oraType(i), ParameterDirection.InputOutput)
                    M_PARAMETERS(i + arrayBindCnt).Value = bind(i).GET_VALUE
                End If
                If bind(i).GET_SIZE <> 0 Then
                    M_PARAMETERS(i + arrayBindCnt).Size = bind(i).GET_SIZE
                End If
                M_FILE.WriteApLog("バインドしました。名前:" & bind(i).GET_NAME & " 型:" & bind(i).GET_TYPE & " サイズ:" & bind(i).GET_SIZE & " 値:" & bind(i).GET_VALUE & " IN/OUT:" & bind(i).GET_INOUT)
            Next
        End If


        Return True
    End Function

    Private Sub SET_RESULT_TYPE(ByVal W_COLUMN_COUNT As Integer)
        '#######################################################################
        '結果セットの列タイプ設定
        '  M_TYPE変数に各列の型タイプを設定
        '引数
        '  W_COLUMN_COUNT     :結果セットの列数
        '#######################################################################
        ReDim M_TYPE(W_COLUMN_COUNT - 1)
        Dim W_TYPE(W_COLUMN_COUNT - 1) As String

        For W_TEMP_COL_CNT = 0 To W_COLUMN_COUNT - 1
            W_TYPE(W_TEMP_COL_CNT) = M_READER.GetDataTypeName(W_TEMP_COL_CNT)

            If W_TYPE(W_TEMP_COL_CNT).ToUpper = "VARCHAR2" Or W_TYPE(W_TEMP_COL_CNT).ToUpper = "CHAR" Or _
               W_TYPE(W_TEMP_COL_CNT).ToUpper = "NCHAR" Or W_TYPE(W_TEMP_COL_CNT).ToUpper = "NVARCHAR2" Or _
               W_TYPE(W_TEMP_COL_CNT).ToUpper = "CLOB" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.STRING
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "INT16" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT16
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "INT32" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT32
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "INT64" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INT64
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "DECIMAL" Or W_TYPE(W_TEMP_COL_CNT).ToUpper = "SINGLE" Or _
                   W_TYPE(W_TEMP_COL_CNT).ToUpper = "DOUBLE" Or W_TYPE(W_TEMP_COL_CNT).ToUpper = "NUMBER" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.NUMBER
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "DATE" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.DATE
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "TIMESTAMP" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMP
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "BLOB" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.BLOB
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "RAW" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.RAW
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "LONG" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.LONG
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "BFILE" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.BFILE
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "TIMESTAMPLTZ" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "TIMESTAMPTZ" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.TIMESTAMPTZ
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "INTERVALDS" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INTERVALDS
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "INTERVALYM" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.INTERVALYM
            ElseIf W_TYPE(W_TEMP_COL_CNT).ToUpper = "XMLTYPE" Then
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.STRING
            Else
                M_FILE.WriteApLog("予期せぬ型がSELECTされました。 列:" & M_READER.GetName(W_TEMP_COL_CNT) & " 型:" & W_TYPE(W_TEMP_COL_CNT))
                M_TYPE(W_TEMP_COL_CNT) = CLS_CONST.CLS_CONST.C_COLUMN_TYPE.UNKNOWN
            End If
        Next
    End Sub

    Public Function SET_BEGINTRAN() As Boolean
        '#######################################################################
        'トランザクション開始宣言
        '#######################################################################
        Try
            G_TRAN = G_CONN.BeginTransaction
            Return True
        Catch ex1 As InvalidOperationException
            EXEC_ROLLBACK()
            G_TRAN = G_CONN.BeginTransaction
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
    End Function

    Public Function EXEC_COMMIT() As Boolean
        '#######################################################################
        'COMMIT実行
        '#######################################################################
        Try
            G_TRAN.Commit()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
    End Function

    Public Function EXEC_ROLLBACK() As Boolean
        '#######################################################################
        'ROLLBACK実行
        '#######################################################################
        Try
            G_TRAN.Rollback()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
    End Function
End Class
