Imports System.Text
Imports M_FILE = CLS_FILE.CLS_FILE
Imports M_CONST = CLS_CONST.CLS_CONST

Public Class CLS_OO4O
    '#######################################################################
    'OO4O操作クラス
    '#######################################################################

    'グローバル変数
    Public G_ORASESSION As Object
    Public G_ORADB As Object 'オラクルコネクション
    Public G_SELECT_DICTIONARY_FLG As Boolean 'ディクショナリ参照権限

    'モジュール変数
    Public M_DYNASET As Object
    Private M_TYPE() As Short

    'OO4O関連定数
    Private Const C_ORAPARM_INPUT = 1
    Private Const C_ORAPARM_OUTPUT = 2
    Private Const C_ORAPARM_BOTH = 3
    Private Const C_ORATYPE_VARCHAR2 = 1
    Private Const C_ORATYPE_NUMBER = 2
    Private Const C_ORATYPE_SINT = 3
    Private Const C_ORATYPE_FLOAT = 4
    Private Const C_ORATYPE_STRING = 5
    Private Const C_ORATYPE_VARCHAR = 9
    Private Const C_ORATYPE_DATE = 12
    Private Const C_ORATYPE_RAW = 23
    Private Const C_ORATYPE_LONGRAW = 24
    Private Const C_ORATYPE_UINT = 68
    Private Const C_ORATYPE_CHAR = 96
    Private Const C_ORATYPE_CHARZ = 97
    Private Const C_ORATYPE_BFLOAT = 100
    Private Const C_ORATYPE_BDOUBLE = 101
    Private Const C_ORATYPE_CURSOR = 102
    Private Const C_ORATYPE_MLSLABEL = 105
    Private Const C_ORATYPE_OBJECT = 108
    Private Const C_ORATYPE_REF = 110
    Private Const C_ORATYPE_CLOB = 112
    Private Const C_ORATYPE_BLOB = 113
    Private Const C_ORATYPE_BFILE = 114
    Private Const C_ORATYPE_TIMESTAMP = 187
    Private Const C_ORATYPE_TIMESTAMPTZ = 188
    Private Const C_ORATYPE_INTERVALYM = 189
    Private Const C_ORATYPE_INTERVALDS = 190
    Private Const C_ORATYPE_TIMESTAMPLTZ = 232
    Private Const C_ORATYPE_VARRAY = 247
    Private Const C_ORATYPE_TABLE = 248

    'Editmode property values
    ' These are intended to match similar constants in the
    ' Visual Basic file CONSTANT.TXT
    Private Const ORADATA_EDITNONE = 0
    Private Const ORADATA_EDITMODE = 1
    Private Const ORADATA_EDITADD = 2

    ' Field Data Types
    ' These are intended to match similar constants in the
    ' Visual Basic file DATACONS.TXT
    Private Const ORADB_BOOLEAN = 1
    Private Const ORADB_BYTE = 2
    Private Const ORADB_INTEGER = 3
    Private Const ORADB_LONG = 4
    Private Const ORADB_CURRENCY = 5
    Private Const ORADB_SINGLE = 6
    Private Const ORADB_DOUBLE = 7
    Private Const ORADB_DATE = 8
    Private Const ORADB_OBJECT = 9
    Private Const ORADB_TEXT = 10
    Private Const ORADB_LONGBINARY = 11
    Private Const ORADB_MEMO = 12

    'Parameter Status
    Private Const ORAPSTAT_INPUT = &H1&
    Private Const ORAPSTAT_OUTPUT = &H2&
    Private Const ORAPSTAT_AUTOENABLE = &H4&
    Private Const ORAPSTAT_ENABLE = &H8&

    'CreateDynaset Method Options
    Private Const ORADYN_DEFAULT = &H0&
    Private Const ORADYN_NO_AUTOBIND = &H1&
    Private Const ORADYN_NO_BLANKSTRIP = &H2&
    Private Const ORADYN_READONLY = &H4&
    Private Const ORADYN_NOCACHE = &H8&
    Private Const ORADYN_ORAMODE = &H10&
    Private Const ORADYN_NO_REFETCH = &H20&
    Private Const ORADYN_NO_MOVEFIRST = &H40&
    Private Const ORADYN_DIRTY_WRITE = &H80&

    'OpenDatabase Method Options
    Private Const ORADB_DEFAULT = &H0&
    Private Const ORADB_ORAMODE = &H1&
    Private Const ORADB_NOWAIT = &H2&
    Private Const ORADB_DBDEFAULT = &H4&
    Private Const ORADB_DEFERRED = &H8&
    Private Const ORADB_ENLIST_IN_MTS = &H10&

    'Oracle type codes
    Private Const ORATYPE_VARCHAR2 = 1
    Private Const ORATYPE_NUMBER = 2
    Private Const ORATYPE_SINT = 3
    Private Const ORATYPE_FLOAT = 4
    Private Const ORATYPE_STRING = 5
    Private Const ORATYPE_DECIMAL = 7
    Private Const ORATYPE_LONG = 8
    Private Const ORATYPE_VARCHAR = 9
    Private Const ORATYPE_DATE = 12
    Private Const ORATYPE_REAL = 21
    Private Const ORATYPE_DOUBLE = 22
    Private Const ORATYPE_UNSIGNED8 = 23
    Private Const ORATYPE_LONGRAW = 24
    Private Const ORATYPE_UNSIGNED16 = 25
    Private Const ORATYPE_UNSIGNED32 = 26
    Private Const ORATYPE_SIGNED8 = 27
    Private Const ORATYPE_SIGNED16 = 28
    Private Const ORATYPE_SIGNED32 = 29
    Private Const ORATYPE_PTR = 32
    Private Const ORATYPE_OPAQUE = 58
    Private Const ORATYPE_UINT = 68
    Private Const ORATYPE_RAW = 23
    Private Const ORATYPE_CHAR = 96
    Private Const ORATYPE_CHARZ = 97
    Private Const ORATYPE_BFLOAT = 100
    Private Const ORATYPE_BDOUBLE = 101
    Private Const ORATYPE_CURSOR = 102
    Private Const ORATYPE_ROWID = 104
    Private Const ORATYPE_MLSLABEL = 105
    Private Const ORATYPE_OBJECT = 108
    Private Const ORATYPE_REF = 110
    Private Const ORATYPE_CLOB = 112
    Private Const ORATYPE_BLOB = 113
    Private Const ORATYPE_BFILE = 114
    Private Const ORATYPE_CFILE = 115
    Private Const ORATYPE_RSLT = 116
    Private Const ORATYPE_NAMEDCOLLECTION = 122
    Private Const ORATYPE_COLL = 122
    Private Const ORATYPE_TIMESTAMP = 187
    Private Const ORATYPE_TIMESTAMPTZ = 188
    Private Const ORATYPE_INTERVALYM = 189
    Private Const ORATYPE_INTERVALDS = 190
    Private Const ORATYPE_SYSFIRST = 228
    Private Const ORATYPE_TIMESTAMPLTZ = 232
    Private Const ORATYPE_SYSLAST = 235
    Private Const ORATYPE_OCTET = 245
    Private Const ORATYPE_SMALLINT = 246
    Private Const ORATYPE_VARRAY = 247
    Private Const ORATYPE_TABLE = 248
    Private Const ORATYPE_OTMLAST = 320
    Private Const ORATYPE_RAW_BIN = 2000


    'CreateSql Method options 
    Private Const ORASQL_DEFAULT = &H0&
    Private Const ORASQL_NO_AUTOBIND = &H1&
    Private Const ORASQL_FAILEXEC = &H2&
    Private Const ORASQL_NONBLK = &H4&

    'OraLob operation return codes
    Private Const ORALOB_SUCCESS = 0
    Private Const ORALOB_NEED_DATA = 99
    Private Const ORALOB_NODATA = 100

    'OraLob Write operation chunck  modes
    Private Const ORALOB_ONE_PIECE = 0
    Private Const ORALOB_FIRST_PIECE = 1
    Private Const ORALOB_NEXT_PIECE = 2
    Private Const ORALOB_LAST_PIECE = 3

    'OraRef Lock operation
    Private Const ORAREF_NO_LOCK = 1
    Private Const ORAREF_EXCLUSIVE_LOCK = 2
    Private Const ORAREF_NOWAIT_LOCK = 3

    'OraRef Pin operaion
    Private Const ORAREF_READ_ANY = 3
    Private Const ORAREF_READ_RECENT = 4
    Private Const ORAREF_READ_LATEST = 5

    'OIP errors returned as part of the OLE Automation error.
    Private Const OERROR_ADVISEULINK = 4096 ' Invalid advisory connection  
    Private Const OERROR_POSITION = 4098    ' Invalid database position  
    Private Const OERROR_NOFIELDNAME = 4099 ' Field 'field-name' not found  
    Private Const OERROR_TRANSIP = 4101 ' Transaction already in process  
    Private Const OERROR_TRANSNIPC = 4104   ' Commit detected with no active transaction   
    Private Const OERROR_TRANSNIPR = 4105   ' Rollback detected with no active transaction  
    Private Const OERROR_NODSET = 4106  ' No such set attached to connection 
    Private Const OERROR_INVROWNUM = 4108   ' Invalid row reference  
    Private Const OERROR_TEMPFILE = 4109    ' Error creating temporary file  
    Private Const OERROR_DUPSESSION = 4110  ' Duplicate session name  
    Private Const OERROR_NOSESSION = 4111   ' Session not found during detach 
    Private Const OERROR_NOOBJECTN = 4112   ' No such object named 'object-name'  
    Private Const OERROR_DUPCONN = 4113 ' Duplicate connection name  
    Private Const OERROR_NOCONN = 4114  ' No such connection during detach  
    Private Const OERROR_BFINDEX = 4115 ' Invalid field index 
    Private Const OERROR_CURNREADY = 4116   ' Cursor not ready for I/O  
    Private Const OERROR_NOUPDATES = 4117   ' Not an updatable set 
    Private Const OERROR_NOTEDITING = 4118  ' Attempt to update without edit or add operation  
    Private Const OERROR_DATACHANGE = 4119  ' Data has been modified  
    Private Const OERROR_NOBUFMEM = 4120    ' No memory for data transfer buffers  
    Private Const OERROR_INVBKMRK = 4121    ' Invalid bookmark 
    Private Const OERROR_BNDVNOEN = 4122    ' Bind variable not fully enabled
    Private Const OERROR_DUPPARAM = 4123    ' Duplicate parameter name  
    Private Const OERROR_INVARGVAL = 4124   ' Invalid argument value  
    Private Const OERROR_INVFLDTYPE = 4125  ' Invalid field type  
    Private Const OERROR_TRANSFORUP = 4127  ' For Update detected with no active transaction  
    Private Const OERROR_NOTUPFORUP = 4128  ' For Update detected but not updatable set
    Private Const OERROR_TRANSLOCK = 4129   ' Commit/Rollback with SELECT FOR UPDATE in progress 
    Private Const OERROR_CACHEPARM = 4130   ' Invalid cache parameter 
    Private Const OERROR_FLDRQROWID = 4131  ' Field processing requires ROWID 
    Private Const OERROR_OUTOFMEMORY = 4132  ' Internal Error
    Private Const OERROR_MAXSIZE = 4135    ' Element size specified in AddTable exceeds the maximum allowed size for that variable type. See AddTable Method for more details.
    Private Const OERROR_INVDIMENSION = 4136    ' Dimension specified in AddTable is invalid (i.e. negative). See AddTable Method for more details.
    Private Const OERROR_MAXBUFFER = 4137   ' Buffer size for parameter array variable exceeds 32512 bytes (OCI limit).
    Private Const OERROR_ARRAYSIZ = 4138    ' Dimensions of array parameters used in insert/update/delete statements are not equal. 
    Private Const OERROR_ARRAYFAILP = 4139  ' Error processing arrays. For details refer to OO4OERR.LOG in the windows directory.
    Private Const OERROR_CREATEPOOL = 4147  ' Database Pool Already exists for this session.
    Private Const OERROR_GETDB = 4148   ' Unable to obtain a free database object from the pool.

    Private Const OERROR_NOOBJECT = 4796     'Creating Oracle object instance in client side object cache is failed
    Private Const OERROR_BINDERR = 4797      'Binding  Oracle object instance to the SQL statement  is failed
    Private Const OERROR_NOATTRNAME = 4798   'Getting attribute name of Oracle object instance is failed
    Private Const OERROR_NOATTRINDEX = 4799  'Getting attribute index of Oracle object instance is failed
    Private Const OERROR_INVINPOBJECT = 4801 'Invalid input object type for binding operation
    Private Const OERROR_BAD_INDICATOR = 4802 'Fetched Oracle Object instance comes with invalid indicator structure
    Private Const OERROR_OBJINSTNULL = 4803  'Operation on NULL Oracle object instance is failed. See IsNull property on OraObject
    Private Const OERROR_REFNULL = 4804      'Pin Operation on NULL  Ref value is failed. See IsRefNull property on OraRef

    Private Const OERROR_INVPOLLPARAMS = 4896 'Invalid  polling amount and chunksize specified for LOB read/write operation.
    Private Const OERROR_INVSEEKPARAMS = 4897 'Invalid seek value is specified for LOB read/write operation.
    Private Const OERROR_LOBREAD = 4898     'Read operation failed
    Private Const OERROR_LOBWRITE = 4899    'Write operation failure 
    Private Const OERROR_INVCLOBBUF = 4900   'Input buffer type is not string for CLOB write operation
    Private Const OERROR_INVBLOBBUF = 4901   'Input buffer type is not bytes for BLOB write operation
    Private Const OERROR_INVLOBLEN = 4902   'Invalid buffer length for LOB write operation
    Private Const OERROR_NOEDIT = 4903      'Write,Trim ,Append,Copy operation is allowed outside the dynaset edit
    Private Const OERROR_INVINPUTLOB = 4904 'Invalid input LOB for bind operation
    Private Const OERROR_NOEDITONCLONE = 4905 'Write,Trim,Append,Copy is not allowed for clone LOB object
    Private Const OERROR_LOBFILEOPEN = 4906 'Specified file could not be opened in LOB operation 
    Private Const OERROR_LOBFILEIOERR = 4907 'File Read or Write failed in LOB Operation.
    Private Const OERROR_LOBNULL = 4908 'Operation on NULL LOB has failed. 

    Private Const OERROR_AQCREATEERR = 4996   'Error creating AQ object
    Private Const OERROR_MSGCREATEERR = 4997  'Error creating AQMsg object 
    Private Const OERROR_PAYLOADCREATEERR = 4998 ' Error creating Payload object
    Private Const OERROR_MAXAGENTS = 4998     ' Maximum number of subscribers exceeded. 
    Private Const OERROR_AGENTCREATEERR = 5000  ' Error creating AQ Agent

    Private Const OERROR_COLLINSTNULL = 5196 'Operation on NULL Oracle collection is  failed. See IsNull property on OraCollection
    Private Const OERROR_NOELEMENT = 5197    'Element does not exist for given index
    Private Const OERROR_INVINDEX = 5198    'Invalid collection index is specified
    Private Const OERROR_NODELETE = 5199    'Delete operation is not supported for VARRAY collection type
    Private Const OERROR_SAFEARRINVELEM = 5200 'Variant SafeArray cannot be created from the collection having non scalar element types

    Private Const OERROR_NULLNUMBER = 5296   'Operation on NULL Oracle Number  is  failed.

    'Non Blocking return values
    Private Const ORASQL_STILL_EXECUTING = -3123
    Private Const ORASQL_SUCCESS = 0

    'バインド変数構造体
    Public Structure S_BIND
        Private NAME As String
        Private VALUE As String
        Private TYPE As Short
        Private INOUT As Short

        Public Sub SET_BIND(ByVal W_NAME As String, ByVal W_VALUE As String, ByVal W_TYPE As Short, ByVal W_INOUT As Short)
            Me.NAME = W_NAME.Replace(":", vbNullString)
            Me.VALUE = W_VALUE
            Me.TYPE = W_TYPE
            Me.INOUT = W_INOUT
        End Sub
        Public Function GET_NAME() As String
            Return Me.NAME
        End Function
        Public Function GET_VALUE() As String
            Return Me.VALUE
        End Function
        Public Function GET_TYPE() As Short
            Return Me.TYPE
        End Function
        Public Function GET_INOUT() As Short
            Return Me.INOUT
        End Function
        Public Sub SET_TYPE(ByVal W_TYPE As Integer)
            Me.TYPE = W_TYPE
        End Sub
    End Structure
    Private M_PARAMETERS() As S_BIND

    Function DB_CONNECT(ByVal W_USERID As String, ByVal W_PASS As String, ByVal W_CONN_STR As String, Optional ByVal W_OPTION As String = vbNullString, Optional ByVal W_SYSDBA_FLG As Boolean = False) As Boolean
        '#######################################################################
        'DBコネクション取得
        '#######################################################################

        'OO4O関連のDLLロード
        Try
            G_ORASESSION = CreateObject("OracleInProcServer.XOraSession")
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "DLLのロードに失敗しました。OO4Oがインストールされていない可能性があります")
            Return False
        End Try

        'DBへの接続
        Try
            G_ORADB = G_ORASESSION.OpenDatabase(W_CONN_STR, W_USERID + "/" + W_PASS, 0&)
            M_FILE.WriteApLog("接続に成功しました。ミドルウエア:" & M_CONST.GetMiddlewareName(CLS_CONST.CLS_CONST.C_MIDDLE_TYPE.ODBC) & " ユーザ:" & W_CONN_STR)
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteApLog("接続に失敗しました。ミドルウエア:" & M_CONST.GetMiddlewareName(CLS_CONST.CLS_CONST.C_MIDDLE_TYPE.ODBC) & " ユーザ:" & W_CONN_STR & " エラーメッセージ:" & ex.Message)
            Return False
        End Try

        Return True

    End Function

    Sub DB_CLOSE()
        '#######################################################################
        'DBコネクション切断
        '#######################################################################
        G_ORADB.Close()
        M_FILE.WriteApLog("コネクションを切断しました")
        'G_ORADB.Dispose()
    End Sub

    Public Function FETCH_TO_DATATABLE(ByRef M_ROW_COUNT As Long, ByRef M_DATA_TABLE As DataTable, ByRef M_SELECT_FLG As Boolean, ByRef W_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000, Optional ByVal W_MIN_DISP_CNT As Long = 0, Optional ByVal W_MAX_DISP_CNT As Long = 0, Optional ByVal W_ERR_DISPLAY As Boolean = True) As Boolean
        '                                                                                                        
        '#######################################################################
        'フェッチデータ(DATA_TABLE作成)
        '引数：
        ' ByVal W_CNT フェッチ件数
        ' Optional ByVal W_CNT As Long = 10000000000
        '#######################################################################
        Dim W_VALUES(W_COLUMN_COUNT - 1) As String

        If M_ROW_COUNT = 0 Then
            '初回のみreadする(次回以降はread済み状態のため不要)
            SET_RESULT_TYPE(W_COLUMN_COUNT)
        End If

        Try
            Do While M_DYNASET.EOF = False
                If (M_ROW_COUNT >= W_MIN_DISP_CNT And M_ROW_COUNT <= W_MAX_DISP_CNT) Or _
                   (W_MIN_DISP_CNT = 0 And W_MAX_DISP_CNT = 0) Then
                    '行データ取得
                    W_VALUES = GET_ROW_VALUES(W_COLUMN_COUNT, M_TYPE)
                    M_DATA_TABLE.Rows.Add(W_VALUES)
                End If

                M_ROW_COUNT = M_ROW_COUNT + 1
                M_DYNASET.DbMoveNext()

                'W_CNT件まで読み込み
                If M_ROW_COUNT Mod W_FETCH_COUNT = 0 Then
                    M_SELECT_FLG = True
                    Exit Do
                End If

                'W_FETCH_COUNT件まで読み込み
                If M_ROW_COUNT Mod W_FETCH_COUNT = 0 Then
                    M_SELECT_FLG = True
                    Exit Do
                End If
            Loop

            '次の行のデータがあるか判断
            If M_DYNASET.EOF Then
                M_SELECT_FLG = False
                M_DYNASET.Close()
            Else
                M_SELECT_FLG = True
            End If
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
    End Function

    Public Function GET_VERSION() As String()
        Dim W_ORA_VERSION(5) As String
        Dim W_MOTO_POS As Short = 0 '元ポジション
        Dim W_SAKI_POS As Short = 0 '先ポジション

        'バージョン情報取得
        '戻り値の例) Oracle Database 10g Enterprise Edition Release 10.2.0.4.0 - Production With the Partitioning, OLAP, Data Mining and Real Application Testing options)
        '            Oracle8i Enterprise Edition Release 8.1.7.4.0 - Production JServer Release 8.1.7.4.0 - Production
        W_ORA_VERSION(0) = G_ORADB.RDBMSVersion
        W_ORA_VERSION(0) = W_ORA_VERSION(0).Substring(InStr(W_ORA_VERSION(0), ".", ) - 3)
        W_ORA_VERSION(0) = W_ORA_VERSION(0).Trim()
        W_ORA_VERSION(0) = W_ORA_VERSION(0).Substring(0, (InStr(W_ORA_VERSION(0), " ")))
        W_ORA_VERSION(0) = W_ORA_VERSION(0).Trim()

        For W_CNT As Short = 1 To 5
            W_SAKI_POS = InStr(W_SAKI_POS + 1, W_ORA_VERSION(0), ".")
            If W_SAKI_POS = 0 Then
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_ORA_VERSION(0).Length - W_MOTO_POS)
            Else
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_SAKI_POS - W_MOTO_POS - 1)
            End If

            W_MOTO_POS = W_SAKI_POS
        Next

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

        Dim W_SQL As String

        'V$VIEWの検索チェック
        W_SQL = "SELECT 0 FROM V$INSTANCE"
        If SET_ORACLEDYNASET(W_SQL, False) = False Then
            Return False
        End If

        'ディクショナリの検索チェック
        W_SQL = "SELECT 0 FROM DBA_USERS WHERE ROWNUM = 1"
        If SET_ORACLEDYNASET(W_SQL, False) = False Then
            Return False
        End If

        Return True
    End Function

    Public Function SET_ORACLEDYNASET(ByVal W_SQL As String, Optional ByVal W_ERR_DISPLAY As Boolean = True) As Boolean
        '#######################################################################
        'ダイナセットのセット
        '　SELECT文のダイナセットをモジュール変数にセットする
        '引数
        '  W_SQL              :SQL文
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Try
            'バインド変数セット
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    Try
                        G_ORADB.Parameters.Add(M_PARAMETERS(i).GET_NAME, M_PARAMETERS(i).GET_VALUE, M_PARAMETERS(i).GET_INOUT)
                        G_ORADB.Parameters(M_PARAMETERS(i).GET_NAME).ServerType = M_PARAMETERS(i).GET_TYPE
                    Catch ex As Exception
                        M_FILE.WriteErrLog(ex)
                        Exit For
                    End Try
                Next
            End If

            M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL)
            M_DYNASET = G_ORADB.CreateDynaset(W_SQL, &HE&)
            M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode)

            M_DYNASET.FetchSize = 65536

            'バインド変数の削除
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    Try
                        G_ORADB.Parameters.Remove(M_PARAMETERS(i).GET_NAME)
                    Catch ex As Exception
                        M_FILE.WriteErrLog(ex)
                        Exit For
                    End Try
                Next
                M_PARAMETERS = Nothing
            End If

            SET_ORACLEDYNASET = True
        Catch ex As Exception
            If W_ERR_DISPLAY Then
                MsgBox(ex.Message)
            End If
            M_FILE.WriteErrLog(ex)
            M_DYNASET = Nothing
            SET_ORACLEDYNASET = False
        End Try
    End Function

    Public Sub INIT_DATA_TABLE(ByRef M_DATA_TABLE As DataTable, ByRef M_COLUMN_COUNT As Long, ByRef M_ROW_COUNT As Long)
        Dim W_VALUES As String()
        Dim W_INT As Integer = 0
        Dim W_CNT As Integer

        Try
            '列数
            M_COLUMN_COUNT = M_DYNASET.Fields.Count
            ReDim W_VALUES(M_COLUMN_COUNT - 1)
            '行数初期化
            M_ROW_COUNT = 0

            For W_CNT = 0 To M_COLUMN_COUNT - 1
                Try
                    M_DATA_TABLE.Columns.Add(M_DYNASET.Fields(W_CNT).Name.ToString())
                Catch ex As Exception
                    '同じ列名が複数あった場合最後に数字を付与した列名とする
                    W_INT = W_INT + 1
                    M_DATA_TABLE.Columns.Add(M_DYNASET.Fields(W_CNT).Name.ToString() & W_INT)
                End Try

                '数値型の場合は列タイプを数字型に変更
                If M_DYNASET.Fields(W_CNT).Type = ORADB_INTEGER Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Int64")
                ElseIf M_DYNASET.Fields(W_CNT).Type = ORADB_LONG Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Int64")
                ElseIf M_DYNASET.Fields(W_CNT).Type = ORADB_DOUBLE Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Double")
                ElseIf M_DYNASET.Fields(W_CNT).Type = ORADB_TEXT Then
                    '文字列型のまま
                ElseIf M_DYNASET.Fields(W_CNT).Type = ORADB_DOUBLE Then
                    M_DATA_TABLE.Columns(W_CNT).DataType = System.Type.GetType("System.Double")
                ElseIf M_DYNASET.Fields(W_CNT).Type = ORADB_TEXT Then
                    '文字列のまま
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
        End Try
    End Sub

    Public Sub INIT_DATASET(ByRef M_DATASET As DataSet, ByRef M_COLUMN_COUNT As Long, ByRef M_ROW_COUNT As Long)
        Dim W_VALUES As String()
        Dim W_INT As Integer = 0

        Try
            '列数
            M_COLUMN_COUNT = M_DYNASET.Fields.Count
            ReDim W_VALUES(M_COLUMN_COUNT - 1)
            '行数初期化
            M_ROW_COUNT = 0

            For Each W_OBJ As Object In M_DYNASET.Fields
                Try
                    M_DATASET.Tables(0).Columns.Add(W_OBJ.Name.ToString())
                Catch ex As Exception
                    W_INT = W_INT + 1
                    M_DATASET.Tables(0).Columns.Add(W_OBJ.Name.ToString() & W_INT)
                End Try
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
        End Try
    End Sub

    Public Function EXEC_NONQUERY(ByVal W_SQL As String, ByRef W_COUNT As Long) As Boolean
        '#######################################################################
        '更新SQL実行
        '  SELECT以外の更新系SQLや無名プロシージャ等を実行する。
        '引数
        '  W_SQL   :SQL文
        '  W_COUNT :処理件数(戻し用)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Try
            'バインド変数セット
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    Try
                        G_ORADB.Parameters.Add(M_PARAMETERS(i).GET_NAME, M_PARAMETERS(i).GET_VALUE, M_PARAMETERS(i).GET_INOUT)
                        G_ORADB.Parameters(M_PARAMETERS(i).GET_NAME).ServerType = M_PARAMETERS(i).GET_TYPE
                    Catch ex As Exception
                        M_FILE.WriteErrLog(ex)
                        Exit For
                    End Try
                Next
            End If

            M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL)
            W_COUNT = G_ORADB.ExecuteSQL(W_SQL)
            M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode)

            'バインド変数削除
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    Try
                        G_ORADB.Parameters.Remove(M_PARAMETERS(i).GET_NAME)
                    Catch ex As Exception
                        M_FILE.WriteErrLog(ex)
                        Exit For
                    End Try
                Next
                M_PARAMETERS = Nothing
            End If

        Catch ex As Exception
            M_FILE.WriteErrLog(ex)
            MsgBox(ex.Message)
            Return False
        End Try
        Return True
    End Function

    Function GET_CONNECT_STRING() As String
        '#######################################################################
        '接続文字列取得
        '  接続文字列を戻す
        '戻り値
        '  接続文字列
        '#######################################################################
        '未実装・・・
        Return vbNullString
    End Function

    Public Function GET_DATASET(ByRef M_ROW_COUNT As Long, ByRef M_DATASET As DataSet, ByRef M_SELECT_FLG As Boolean, ByRef M_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000) As Boolean
        '#######################################################################
        'DATASET取得
        '引数：
        ' ByVal W_CNT フェッチ件数
        ' Optional ByVal W_CNT As Long = 10000000000
        '#######################################################################
        Dim W_VALUES(M_COLUMN_COUNT - 1) As String

        SET_RESULT_TYPE(M_COLUMN_COUNT)

        Try
            Do While M_DYNASET.EOF = False
                W_VALUES = GET_ROW_VALUES(M_COLUMN_COUNT, M_TYPE)
                M_DATASET.Tables(0).Rows.Add(W_VALUES)
                M_ROW_COUNT = M_ROW_COUNT + 1
                'W_FETCH_COUNT件まで読み込み
                If M_ROW_COUNT Mod W_FETCH_COUNT = 0 Then
                    M_SELECT_FLG = True
                    Exit Do
                End If
                M_DYNASET.DbMoveNext()
            Loop

            '次の行のデータがあるか判断
            If M_DYNASET.EOF Then
                M_SELECT_FLG = False
                M_DYNASET.Close()
            Else
                M_SELECT_FLG = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
        Return True
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
        W_COLUMN_COUNT = M_DYNASET.Fields.Count

        Try
            If M_DYNASET.eof Then
                M_SELECT_FLG = False
            Else
                M_SELECT_FLG = True
            End If

            SET_RESULT_TYPE(W_COLUMN_COUNT)

            If M_SELECT_FLG Then
                W_VALUES = GET_ROW_VALUES(W_COLUMN_COUNT, M_TYPE)
                W_ROW = W_VALUES
                M_ROW_COUNT = M_ROW_COUNT + 1
                M_DYNASET.DbMoveNext()
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
            'If M_DYNASET.Fields(W_TEMP_COL_CNT).value Is DBNull.Value Then
            If IsDBNull(M_DYNASET.Fields(W_TEMP_COL_CNT).value) = False Then
                If W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.STRING Then
                    W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.NUMBER Then
                    W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.DATE Then
                    'DATE型
                    W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                    'TIMESTAMP型(YYYY/MM/DD HH24:MI:SS)
                    W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value.year & "/" & _
                    M_DYNASET.Fields(W_TEMP_COL_CNT).value.Month & "/" & M_DYNASET.Fields(W_TEMP_COL_CNT).value.Day & " " & _
                    M_DYNASET.Fields(W_TEMP_COL_CNT).value.Hour & ":" & M_DYNASET.Fields(W_TEMP_COL_CNT).value.Minute & ":" & _
                    M_DYNASET.Fields(W_TEMP_COL_CNT).value.Second
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.LONG Then
                    W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                    W_VALUES(W_TEMP_COL_CNT) = "TIMESTAMP WITH LOCAL TIMEZONE(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                    W_VALUES(W_TEMP_COL_CNT) = "TIMESTAMP WITH TIMEZONE(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.BLOB Then
                    'BLOB型
                    W_VALUES(W_TEMP_COL_CNT) = "BLOB(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.RAW Then
                    'RAW型
                    'valueで文字列型に変換してしまうと一部のbyteデータ("5890"等)が正常な値で取り出せないため現状非対応。
                    'W_VALUES(W_TEMP_COL_CNT) = BitConverter.ToString(System.Text.Encoding.GetEncoding(0).GetBytes(M_DYNASET.Fields(W_TEMP_COL_CNT).value)).Replace("-", "")
                    'W_VALUES(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).value
                    W_VALUES(W_TEMP_COL_CNT) = "RAW(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.BFILE Then
                    'BFILE型
                    W_VALUES(W_TEMP_COL_CNT) = "BFILE(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.INTERVALDS Then
                    W_VALUES(W_TEMP_COL_CNT) = "INTERVAL DS(非対応)"
                ElseIf W_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.INTERVALYM Then
                    W_VALUES(W_TEMP_COL_CNT) = "INTERVAL YM(非対応)"
                Else
                    W_VALUES(W_TEMP_COL_CNT) = "未対応"
                End If
            Else
                W_VALUES(W_TEMP_COL_CNT) = vbNullString
            End If
        Next

        Return W_VALUES

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
        Return False
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

        Try
            'バインド変数セット
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    Try
                        G_ORADB.Parameters.Add(M_PARAMETERS(i).GET_NAME, M_PARAMETERS(i).GET_VALUE, M_PARAMETERS(i).GET_INOUT)
                        G_ORADB.Parameters(M_PARAMETERS(i).GET_NAME).ServerType = M_PARAMETERS(i).GET_TYPE
                    Catch ex As Exception
                        M_FILE.WriteErrLog(ex)
                        Exit For
                    End Try
                Next
            End If

            M_FILE.WriteApLog("SQL実行開始：" & W_SQL.GetHashCode & ":" & W_SQL)
            G_ORADB.ExecuteSQL(W_SQL)
            M_FILE.WriteApLog("SQL実行終了：" & W_SQL.GetHashCode)

            'バインド変数削除
            If Not M_PARAMETERS Is Nothing Then
                For i As Integer = 0 To M_PARAMETERS.Length - 1
                    W_PARAMSTR(i) = G_ORADB.Parameters(M_PARAMETERS(i).GET_NAME).value
                    G_ORADB.Parameters.Remove(M_PARAMETERS(i).GET_NAME)
                Next
                M_PARAMETERS = Nothing
            End If

        Catch ex As Exception
            M_FILE.WriteErrLog(ex)
            MsgBox(ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function SET_BIND_VARIABLE(ByVal W_BIND() As M_CONST.S_BIND) As Boolean
        '#######################################################################
        'バインド変数セット
        '  
        '引数
        '  W_BIND :バインド変数構造体
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        M_PARAMETERS = Nothing
        ReDim M_PARAMETERS(W_BIND.Length - 1)

        For i As Integer = 0 To W_BIND.Length - 1
            If W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.STRING Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.NUMBER Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_NUMBER)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.DATE Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_DATE)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMP Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_DATE)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.CLOB Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_CHAR)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.RAW Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.BLOB Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.LONG Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALDS Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.INTERVALYM Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ Then
                M_PARAMETERS(i).SET_TYPE(C_ORATYPE_VARCHAR2)
            ElseIf W_BIND(i).GET_TYPE = M_CONST.C_COLUMN_TYPE.UNKNOWN Then
                M_FILE.WriteApLog("予期せぬタイプ番号:" & W_BIND(i).GET_TYPE)
                Return False
            Else
                M_FILE.WriteApLog("予期せぬタイプ番号:" & W_BIND(i).GET_TYPE)
                Return False
            End If
        Next

        For i As Integer = 0 To W_BIND.Length - 1
            If W_BIND(i).GET_INOUT = ParameterDirection.Input Then
                M_PARAMETERS(i).SET_BIND(W_BIND(i).GET_NAME, W_BIND(i).GET_VALUE, M_PARAMETERS(i).GET_TYPE, C_ORAPARM_INPUT)
            ElseIf W_BIND(i).GET_INOUT = ParameterDirection.Output Then
                M_PARAMETERS(i).SET_BIND(W_BIND(i).GET_NAME, W_BIND(i).GET_VALUE, M_PARAMETERS(i).GET_TYPE, C_ORAPARM_OUTPUT)
            ElseIf W_BIND(i).GET_INOUT = ParameterDirection.InputOutput Then
                M_PARAMETERS(i).SET_BIND(W_BIND(i).GET_NAME, W_BIND(i).GET_VALUE, M_PARAMETERS(i).GET_TYPE, C_ORAPARM_BOTH)
            End If
            M_FILE.WriteApLog("バインドしました。名前:" & W_BIND(i).GET_NAME & " 型:" & W_BIND(i).GET_TYPE & " 値:" & W_BIND(i).GET_VALUE & " IN/OUT:" & W_BIND(i).GET_INOUT)
        Next

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
        Dim W_TYPE(W_COLUMN_COUNT - 1) As Integer

        For W_TEMP_COL_CNT = 0 To W_COLUMN_COUNT - 1
            W_TYPE(W_TEMP_COL_CNT) = M_DYNASET.Fields(W_TEMP_COL_CNT).OraIDataType()

            If W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_VARCHAR2 Or W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_CHAR Or _
               W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_CLOB Then
                '文字列型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.STRING
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_NUMBER Or W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_SINT Or _
               W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_BFLOAT Or W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_BDOUBLE Then
                '数字型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.NUMBER
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_DATE Then
                'DATE型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.DATE
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_TIMESTAMP Then
                'TIMESTAMP型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMP
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_BLOB Then
                'BLOB型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.BLOB
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_RAW Then
                'RAW型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.RAW
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_LONGRAW Then
                'LONG型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.LONG
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_BFILE Then
                'BFILE型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.BFILE
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_TIMESTAMPLTZ Then
                'TIMESTAMP WITH LOCAL TIME ZONE型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMPLTZ
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_TIMESTAMPTZ Then
                'TIMESTAMP WITH TIME ZONE型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.TIMESTAMPTZ
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_INTERVALDS Then
                'INTERVAL 日付型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.INTERVALDS
            ElseIf W_TYPE(W_TEMP_COL_CNT) = C_ORATYPE_INTERVALYM Then
                'INTERVAL 年月型
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.INTERVALYM
            Else
                M_FILE.WriteApLog("予期せぬ型がSELECTされました。 列:" & M_DYNASET.Fields(W_TEMP_COL_CNT).Name(W_TEMP_COL_CNT) & " 型:" & W_TYPE(W_TEMP_COL_CNT))
                M_TYPE(W_TEMP_COL_CNT) = M_CONST.C_COLUMN_TYPE.UNKNOWN
            End If
        Next
    End Sub

    Public Function SET_BEGINTRAN() As Boolean
        '#######################################################################
        'トランザクション開始宣言
        '#######################################################################
        Try
            G_ORASESSION.BeginTrans()
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
        Return True
    End Function

    Public Function EXEC_COMMIT() As Boolean
        '#######################################################################
        'COMMIT実行
        '#######################################################################
        Try
            G_ORASESSION.CommitTrans()
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
        Return True
    End Function

    Public Function EXEC_ROLLBACK() As Boolean
        '#######################################################################
        'ROLLBACK実行
        '#######################################################################
        Try
            G_ORASESSION.Rollback()
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            Return False
        End Try
        Return True
    End Function

End Class
