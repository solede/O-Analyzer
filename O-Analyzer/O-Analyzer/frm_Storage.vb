Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Storage
    '#######################################################################
    '記憶域情報画面
    '2011/05/26 トランザクション情報に重複データが出力されるバグ修正
    '#######################################################################

    '定数
    Private Const C_TABLESPACE As Integer = 0
    Private Const C_SEGMENT As Integer = 1
    Private Const C_TEMP As Integer = 2
    Private Const C_TX_UNDO As Integer = 3
    Private Const C_DATAFILE As Integer = 4
    Private Const C_EXTENT As Integer = 5
    Private Const C_DATAFILE_STAT As Integer = 6
    Private Const C_UNDO_SEGMENT_AUTO As Integer = 9
    Private Const C_UNDO_SEGMENT_MANUAL As Integer = 8
    Private Const C_UNDO_EXTENT_AUTO As Integer = 11
    Private Const C_UNDO_EXTENT_MANUAL As Integer = 12
    Private Const C_CONTROL_FILE As Integer = 13
    Private Const C_CONTROL_FILE_RECORD As Integer = 14
    Private Const C_REDO As Integer = 15
    Private Const C_REDO_HISTORY As Integer = 16
    Private Const C_ARCHIVELOG As Integer = 17

    'モジュール変数
    Private M_CANCELFLG As Boolean = False
    Private undoManagement As String = vbNullString

    'データファイル情報の構造体
    Private Structure FILE_INFO
        Public FILE_ID As Integer
        Public FILE_NAME As String
        Public ALLBYTES As Long
        Public USER_BYTES As Long
        Public BLOCK_SIZE As Integer
        Public BLOCK_OFFSET As Integer
    End Structure
    Private M_FILE_INFO() As FILE_INFO

    Public Function GetIndex(ByVal fileId As String) As Integer
        Dim i As Integer

        If M_FILE_INFO Is Nothing Then
            Return -1
        End If
        For i = 0 To M_FILE_INFO.Length - 1
            If M_FILE_INFO(i).FILE_ID = Integer.Parse(fileId) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Sub ChangeFormStatus(ByVal statusId As Short)
        '#######################################################################
        'フォームステータス変更
        '#######################################################################
        If statusId = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXEC.Enabled = True
            BTN_CANCEL.Enabled = False
        ElseIf statusId = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXEC.Enabled = False
            BTN_CANCEL.Enabled = True
        End If
    End Sub

    Private Sub SetDatafileInfo()
        '#######################################################################
        'データファイル情報表示
        '#######################################################################
        Dim sql As String

        Dim dataRow As DataRow
        Dim cnt As Integer = 0

        sql = CreateSQL(C_DATAFILE)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Using dataset As New DataSet()
            If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                LBL_INFO.Text = vbNullString
                CMB_DATAFILE.Items.Add(" ")
                Exit Sub
            End If

            ReDim M_FILE_INFO(dataset.Tables(0).Rows.Count - 1)

            For Each dataRow In dataset.Tables(0).Rows
                M_FILE_INFO(cnt).FILE_ID = Integer.Parse(dataRow.Item(0).ToString)
                M_FILE_INFO(cnt).FILE_NAME = dataRow.Item(1).ToString
                M_FILE_INFO(cnt).ALLBYTES = Long.Parse(dataRow.Item(2).ToString)
                M_FILE_INFO(cnt).USER_BYTES = Long.Parse(dataRow.Item(3).ToString)
                M_FILE_INFO(cnt).BLOCK_SIZE = Integer.Parse(dataRow.Item(4).ToString)
                M_FILE_INFO(cnt).BLOCK_OFFSET = Integer.Parse(dataRow.Item(5).ToString)
                CMB_DATAFILE.Items.Add(dataRow.Item(0).ToString & " " & dataRow.Item(1).ToString)
                cnt = cnt + 1
            Next
        End Using

        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Private Sub getUndoManagement()
        '#######################################################################
        'UNDO管理方式取得
        '#######################################################################
        If undoManagement = vbNullString Then
            Using undoManagementDataset As New DataSet()
                '初回のみUNDO管理方式を確認
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                    '9i以降は自動UNDO管理であるかどうか判別
                    If frm_Login.G_DB.GET_DATASET("SELECT VALUE FROM V$PARAMETER WHERE UPPER(NAME) = 'UNDO_MANAGEMENT'", undoManagementDataset) = False Then
                        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                        LBL_INFO.Text = vbNullString
                        Exit Sub
                    End If
                    undoManagement = undoManagementDataset.Tables(0).Rows(0).Item(0).ToString
                Else
                    undoManagement = "MANUAL"
                End If
            End Using
        End If

        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispTablespace()
        '#######################################################################
        '表領域情報表示
        '#######################################################################
        Dim sql As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_TABLESPACE)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_TABLESPACE.DataSource = bindingSource
        DGV_TABLESPACE.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispDatafile()
        '#######################################################################
        'データファイル情報表示
        '#######################################################################
        Dim sql As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_DATAFILE_STAT)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_DATAFILE.DataSource = bindingSource
        DGV_DATAFILE.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispTemp()
        '#######################################################################
        '一時セグメント情報表示
        '#######################################################################
        Dim sql As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_TEMP)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_TEMP.DataSource = bindingSource
        DGV_TEMP.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Public Sub DispExtent()
        '#######################################################################
        'エクステント情報表示
        '#######################################################################
        Dim sql As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_EXTENT)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_EXTENT.DataSource = bindingSource

        For i As Integer = 0 To DGV_EXTENT.Rows.Count - 1
            '空き領域のみ色付け
            If DGV_EXTENT.Rows(i).Cells(3).Value.ToString = "FREE" Then
                DGV_EXTENT.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan
            End If
        Next

        DGV_EXTENT.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub
    Public Sub DispUndo()
        '#######################################################################
        'UNDO情報表示
        '#######################################################################
        Dim sql As String = vbNullString
        Dim undoExtentDataset As New DataSet()
        Dim txDataset As New DataSet()
        Dim undoExtentBindingSource As New BindingSource
        Dim txBindingSource As New BindingSource

        'UNDO領域情報の表示
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        If CMB_UNDO_TYPE.SelectedItem.ToString.Substring(0, 1) = "0" Then
            'セグメント表示
            If undoManagement = "AUTO" Then
                sql = CreateSQL(C_UNDO_SEGMENT_AUTO)
            ElseIf undoManagement = "MANUAL" Then
                sql = CreateSQL(C_UNDO_SEGMENT_MANUAL)
            End If
        ElseIf CMB_UNDO_TYPE.SelectedItem.ToString.Substring(0, 1) = "1" Then
            'エクステント表示
            If undoManagement = "AUTO" Then
                sql = CreateSQL(C_UNDO_EXTENT_AUTO)
            ElseIf undoManagement = "MANUAL" Then
                sql = CreateSQL(C_UNDO_EXTENT_MANUAL)
            End If
        End If

        If frm_Login.G_DB.GET_DATASET(sql, undoExtentDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        undoExtentBindingSource.DataSource = undoExtentDataset.Tables(0)
        DGV_UNDO.DataSource = undoExtentBindingSource
        DGV_UNDO.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

        'トランザクション情報取得
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_TX_UNDO)
        If frm_Login.G_DB.GET_DATASET(sql, txDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        txBindingSource.DataSource = txDataset.Tables(0)
        DGV_TX.DataSource = txBindingSource


        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Private Sub DispSegment()
        '#######################################################################
        'セグメント情報表示
        '#######################################################################
        Dim sql As String
        Dim rowstr(4) As String
        Dim cnt As Integer = 0
        Dim unusedSpaceParam() As String = {"0", "0", "0", "0", "0", "0", "0"}
        Dim spaceUsageParam() As String = {"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"}
        Dim spaceUsageSecurefileParam() As String = {"0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"}

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        DGV_FREELIST.Focus()

        '列幅自動調整
        DGV_FREELIST.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DGV_ASSM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DGV_SECURELOB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)
        DGV_FREELIST.DataSource = Nothing
        DGV_ASSM.DataSource = Nothing
        DGV_SECURELOB.DataSource = Nothing
        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_SEGMENT)

        frm_Login.G_DB.SET_SELECT_FLG(False)
        DGV_FREELIST.RowCount = 0
        DGV_ASSM.RowCount = 0
        DGV_SECURELOB.RowCount = 0

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) = False Then
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        While frm_Login.G_DB.FETCH_ROW_TO_STRING(rowstr)
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                '9iまでのLOB PARTITION,LOB SUBPARTITIONとLOBINDEXは処理できない為SQLの結果のみ出力
                If rowstr(2) = "LOB PARTITION" Or rowstr(2) = "LOB SUBPARTITION" Or rowstr(2) = "LOBINDEX" Then
                    DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1), rowstr(2), vbNullString, vbNullString, _
                                          vbNullString, vbNullString, vbNullString)
                Else
                    'フリーリスト管理(8iまではフリーリスト管理のみ)
                    If frm_Login.G_DB.EXEC_DBMS_SPACE_UNUSED_SPACE(rowstr(0), rowstr(1), rowstr(2), rowstr(3), unusedSpaceParam) = False Then
                        '処理失敗
                        Exit While
                    End If
                    '列追加
                    If rowstr(3) = vbNullString Then
                        DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1), rowstr(2), vbNullString, Integer.Parse(unusedSpaceParam(0)), _
                                              Integer.Parse(unusedSpaceParam(2)), Integer.Parse(unusedSpaceParam(4)), Integer.Parse(unusedSpaceParam(5)))
                    Else
                        'SEGMENT_NAMEをセグメント:パーティション名形式で表示
                        DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1) & ":" & rowstr(3), rowstr(2), vbNullString, _
                                              Integer.Parse(unusedSpaceParam(0)), Integer.Parse(unusedSpaceParam(2)), _
                                              Integer.Parse(unusedSpaceParam(4)), Integer.Parse(unusedSpaceParam(5)))
                    End If
                End If
            Else
                If rowstr(4) = "MANUAL" Then
                    '表領域がフリーリスト管理の場合
                    '9iまでのLOB PARTITION,LOB SUBPARTITIONとLOBINDEXについては処理できない為SQLの結果のみ出力
                    If (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) <= 9 And _
                        (rowstr(2) = "LOB PARTITION" Or rowstr(2) = "LOB SUBPARTITION")) Or rowstr(2) = "LOBINDEX" Then
                        DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1), rowstr(2), Integer.Parse(rowstr(5)), vbNullString, vbNullString, vbNullString, vbNullString)
                    Else
                        If frm_Login.G_DB.EXEC_DBMS_SPACE_UNUSED_SPACE(rowstr(0), rowstr(1), rowstr(2), rowstr(3), unusedSpaceParam) = False Then
                            '処理失敗
                            Exit While
                        End If
                        If DGV_FREELIST.ColumnCount = 0 Then
                            '処理途中で画面を閉じた場合
                            Exit Sub
                        End If
                        '列追加
                        If rowstr(3) = vbNullString Then
                            DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1), rowstr(2), Integer.Parse(rowstr(5)), _
                                                  Integer.Parse(unusedSpaceParam(0)), Integer.Parse(unusedSpaceParam(2)), _
                                                  Integer.Parse(unusedSpaceParam(4)), Integer.Parse(unusedSpaceParam(5)))
                        Else
                            'SEGMENT_NAMEをセグメント:パーティション名形式で表示
                            DGV_FREELIST.Rows.Add(rowstr(0), rowstr(1) & ":" & rowstr(3), rowstr(2), Integer.Parse(rowstr(5)), _
                                                  Integer.Parse(unusedSpaceParam(0)), Integer.Parse(unusedSpaceParam(2)), _
                                                  Integer.Parse(unusedSpaceParam(4)), Integer.Parse(unusedSpaceParam(5)))
                        End If
                    End If
                ElseIf rowstr(4) = "AUTO" Then
                    '表領域がASSMかつSECURELOBではない場合
                    '9iまでのLOB PARTITION,LOB SUBPARTITIONとLOBINDEXについては処理できない為SQLの結果のみ出力
                    If (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) <= 9 And _
                        (rowstr(2) = "LOB PARTITION" Or rowstr(2) = "LOB SUBPARTITION")) Or rowstr(2) = "LOBINDEX" Then
                        DGV_ASSM.Rows.Add(rowstr(0), rowstr(1), rowstr(2), Integer.Parse(rowstr(5)), _
                                          Integer.Parse(rowstr(6)), vbNullString, vbNullString, vbNullString, _
                                          vbNullString, vbNullString, vbNullString)
                    Else
                        If frm_Login.G_DB.EXEC_DBMS_SPACE_SPACE_USAGE(rowstr(0), rowstr(1), rowstr(2), rowstr(3), spaceUsageParam) = False Then
                            '処理失敗
                            Exit While
                        End If
                        '列追加
                        If rowstr(3) = vbNullString Then
                            DGV_ASSM.Rows.Add(rowstr(0), rowstr(1), rowstr(2), Integer.Parse(rowstr(5)), _
                                              Integer.Parse(rowstr(6)), Integer.Parse(spaceUsageParam(0)), _
                                              Integer.Parse(spaceUsageParam(2)), Integer.Parse(spaceUsageParam(4)), _
                                              Integer.Parse(spaceUsageParam(6)), Integer.Parse(spaceUsageParam(8)), _
                                              Integer.Parse(spaceUsageParam(10)))
                        Else
                            'SEGMENT_NAMEをセグメント:パーティション名形式で表示
                            DGV_ASSM.Rows.Add(rowstr(0), rowstr(1) & ":" & rowstr(3), rowstr(2), Integer.Parse(rowstr(5)), _
                                              Integer.Parse(rowstr(6)), Integer.Parse(spaceUsageParam(0)), _
                                              Integer.Parse(spaceUsageParam(2)), Integer.Parse(spaceUsageParam(4)), _
                                              Integer.Parse(spaceUsageParam(6)), Integer.Parse(spaceUsageParam(8)), _
                                              Integer.Parse(spaceUsageParam(10)))
                        End If
                    End If
                ElseIf rowstr(4) = "SECUREFILE" Then
                    'セキュアLOBの場合(11g～)
                    If frm_Login.G_DB.EXEC_DBMS_SPACE_SPACE_USAGE_SECUREFILE(rowstr(0), rowstr(1), rowstr(2), rowstr(3), spaceUsageSecurefileParam) = False Then
                        '処理失敗
                        Exit While
                    End If
                    '列追加
                    If rowstr(3) = vbNullString Then
                        DGV_SECURELOB.Rows.Add(rowstr(0), rowstr(1), rowstr(2), Integer.Parse(rowstr(5)), _
                                          Integer.Parse(rowstr(6)), Integer.Parse(spaceUsageSecurefileParam(0)), _
                                          Integer.Parse(spaceUsageSecurefileParam(2)), Integer.Parse(spaceUsageSecurefileParam(4)), _
                                          Integer.Parse(spaceUsageSecurefileParam(6)))
                    Else
                        'SEGMENT_NAMEをセグメント:パーティション名形式で表示
                        DGV_SECURELOB.Rows.Add(rowstr(0), rowstr(1) & ":" & rowstr(3), rowstr(2), Integer.Parse(rowstr(5)), _
                                          Integer.Parse(rowstr(6)), Integer.Parse(spaceUsageSecurefileParam(0)), _
                                          Integer.Parse(spaceUsageSecurefileParam(2)), Integer.Parse(spaceUsageSecurefileParam(4)), _
                                          Integer.Parse(spaceUsageSecurefileParam(6)))
                    End If
                End If
            End If

            cnt = cnt + 1

            LBL_INFO.Text = cnt & "件表示しました"
            System.Windows.Forms.Application.DoEvents()
            'キャンセル指示があった場合
            If M_CANCELFLG Then
                M_CANCELFLG = False
                MsgBox("キャンセルしました")
                Exit While
            End If
        End While

        LBL_INFO.Text = cnt & "件表示しました"
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Public Sub DispControlFile()
        '#######################################################################
        '制御ファイル情報表示
        '#######################################################################
        Dim sql As String = vbNullString
        Dim ctlFileDataset As New DataSet()
        Dim ctlFileBindingSource As New BindingSource
        Dim ctlRecDataset As New DataSet()
        Dim ctlRecBindingSource As New BindingSource

        '制御ファイル情報取得
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_CONTROL_FILE)
        If frm_Login.G_DB.GET_DATASET(sql, ctlFileDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        ctlFileBindingSource.DataSource = ctlFileDataset.Tables(0)
        DGV_CONFROLFILE.DataSource = ctlFileBindingSource
        DGV_CONFROLFILE.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        '制御ファイルレコードセクション情報取得
        sql = CreateSQL(C_CONTROL_FILE_RECORD)
        If frm_Login.G_DB.GET_DATASET(sql, ctlRecDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        ctlRecBindingSource.DataSource = ctlRecDataset.Tables(0)
        DGV_CONFROLFILE_RECORD.DataSource = ctlRecBindingSource
        DGV_CONFROLFILE_RECORD.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Public Sub DispRedo()
        '#######################################################################
        'REDOログ情報表示
        '#######################################################################
        Dim sql As String = vbNullString
        Dim redoDataset As New DataSet()
        Dim redoBindingSource As New BindingSource
        Dim redoHistoryDataset As New DataSet()
        Dim redoHistoryBindingSource As New BindingSource

        'REDOログ情報取得
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_REDO)
        If frm_Login.G_DB.GET_DATASET(sql, redoDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        redoBindingSource.DataSource = redoDataset.Tables(0)
        DGV_REDO.DataSource = redoBindingSource
        DGV_REDO.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        'アーカイブログ情報取得
        sql = CreateSQL(C_REDO_HISTORY)
        If frm_Login.G_DB.GET_DATASET(sql, redoHistoryDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        redoHistoryBindingSource.DataSource = redoHistoryDataset.Tables(0)
        DGV_REDO_HISTORY.DataSource = redoHistoryBindingSource
        DGV_REDO_HISTORY.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Public Sub DispArchivelog()
        '#######################################################################
        'アーカイブログ情報表示
        '#######################################################################
        Dim sql As String = vbNullString
        Dim archivelogDataset As New DataSet()
        Dim archivelogBindingSource As New BindingSource

        'アーカイブログ情報取得
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = CreateSQL(C_ARCHIVELOG)
        If frm_Login.G_DB.GET_DATASET(sql, archivelogDataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        archivelogBindingSource.DataSource = archivelogDataset.Tables(0)
        DGV_ARCHIVELOG.DataSource = archivelogBindingSource
        DGV_ARCHIVELOG.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)

        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Private Function CreateSQL(ByVal id As Short) As String
        '#######################################################################
        'SQL作成
        'id:作成するSQL文の識別ID
        '#######################################################################

        Dim sql As String = vbNullString
        Dim isStart As Boolean = True

        sql = vbNullString
        If id = C_TABLESPACE Then
            '表領域情報取得SQL
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "SELECT /*+ FIRST_ROWS */"
                sql = sql & "  D.TABLESPACE_NAME TABLESPACE_NAME,"
                sql = sql & "  NVL(A.BYTES, 0)/1024 KBYTES,"
                sql = sql & "  DECODE(D.CONTENTS,'UNDO', NVL(U.BYTES, 0)/1024 ,NVL(A.BYTES - NVL(F.BYTES, 0), 0)/1024) USED_KBYTES,"
                sql = sql & "  TRUNC(DECODE(D.CONTENTS,'UNDO', NVL(U.BYTES, 0) ,NVL(A.BYTES - NVL(F.BYTES, 0), 0))/NVL(A.BYTES, 1)*100,2) ""RATIO(%)"","
                sql = sql & "  D.STATUS STATUS,"
                sql = sql & "  D.CONTENTS CONTENTS,"
                sql = sql & "  D.BLOCK_SIZE BLOCK_SIZE,"
                sql = sql & "  D.INITIAL_EXTENT INITIAL_EXTENT,"
                sql = sql & "  D.NEXT_EXTENT NEXT_EXTENT, "
                sql = sql & "  D.MIN_EXTENTS MIN_EXTENTS,"
                sql = sql & "  D.MAX_EXTENTS MAX_EXTENTS,"
                sql = sql & "  D.PCT_INCREASE PCT_INCREASE,"
                sql = sql & "  D.MIN_EXTLEN,"
                sql = sql & "  D.LOGGING LOGGING, "
                sql = sql & "  D.EXTENT_MANAGEMENT,"
                sql = sql & "  D.ALLOCATION_TYPE ,"
                sql = sql & "  D.SEGMENT_SPACE_MANAGEMENT "
                sql = sql & "FROM "
                sql = sql & "  DBA_TABLESPACES D,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_DATA_FILES GROUP BY TABLESPACE_NAME) A,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_UNDO_EXTENTS WHERE STATUS = 'ACTIVE' GROUP BY TABLESPACE_NAME) U,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_FREE_SPACE GROUP BY TABLESPACE_NAME) F "
                sql = sql & "WHERE"
                sql = sql & "  (D.EXTENT_MANAGEMENT <> 'LOCAL' OR"
                sql = sql & "   D.CONTENTS <> 'TEMPORARY') AND"
                sql = sql & "   D.TABLESPACE_NAME = A.TABLESPACE_NAME(+) AND "
                sql = sql & "   D.TABLESPACE_NAME = U.TABLESPACE_NAME(+) AND "
                sql = sql & "   D.TABLESPACE_NAME = F.TABLESPACE_NAME(+) "
                sql = sql & "UNION ALL "
                sql = sql & "SELECT /*+ FIRST_ROWS */"
                sql = sql & "  D.TABLESPACE_NAME,"
                sql = sql & "  NVL(A.BYTES, 0)/1024, "
                sql = sql & "  NVL(F.BYTES, 0)/1024, "
                sql = sql & "  TRUNC(NVL(F.BYTES, 0)/NVL(A.BYTES, 1)*100,2), "
                sql = sql & "  D.STATUS, D.CONTENTS, D.BLOCK_SIZE, D.INITIAL_EXTENT, D.NEXT_EXTENT, "
                sql = sql & "  D.MIN_EXTENTS, "
                sql = sql & "  D.MAX_EXTENTS, "
                sql = sql & "  D.PCT_INCREASE, "
                sql = sql & "  D.MIN_EXTLEN, "
                sql = sql & "  D.LOGGING, "
                sql = sql & "  D.EXTENT_MANAGEMENT, "
                sql = sql & "  D.ALLOCATION_TYPE ,"
                sql = sql & "  D.SEGMENT_SPACE_MANAGEMENT "
                sql = sql & "FROM"
                sql = sql & "  DBA_TABLESPACES D,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_TEMP_FILES GROUP BY TABLESPACE_NAME) A,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES_USED) BYTES FROM V$TEMP_EXTENT_POOL GROUP BY TABLESPACE_NAME) F "
                sql = sql & "WHERE"
                sql = sql & "  D.EXTENT_MANAGEMENT = 'LOCAL' AND"
                sql = sql & "  D.CONTENTS = 'TEMPORARY' AND"
                sql = sql & "  D.TABLESPACE_NAME = A.TABLESPACE_NAME(+) AND"
                sql = sql & "  D.TABLESPACE_NAME = F.TABLESPACE_NAME(+) "
                sql = sql & "ORDER BY"
                sql = sql & "  CONTENTS, TABLESPACE_NAME"
            Else
                sql = sql & "SELECT /*+ FIRST_ROWS */"
                sql = sql & "  D.TABLESPACE_NAME TABLESPACE_NAME,"
                sql = sql & "  NVL(A.BYTES, 0)/1024 KBYTES,"
                sql = sql & "  NVL(A.BYTES - NVL(F.BYTES, 0), 0)/1024 USED_KBYTES,"
                sql = sql & "  TRUNC((NVL(A.BYTES,0) - NVL(F.BYTES, 0))/(NVL(A.BYTES, 1))*100) ""RATIO(%)"","
                sql = sql & "  D.STATUS STATUS,"
                sql = sql & "  D.CONTENTS CONTENTS,"
                sql = sql & "  D.INITIAL_EXTENT INITIAL_EXTENT,"
                sql = sql & "  D.NEXT_EXTENT NEXT_EXTENT, "
                sql = sql & "  D.MIN_EXTENTS MIN_EXTENTS,"
                sql = sql & "  D.MAX_EXTENTS MAX_EXTENTS,"
                sql = sql & "  D.PCT_INCREASE PCT_INCREASE,"
                sql = sql & "  D.MIN_EXTLEN,"
                sql = sql & "  D.LOGGING LOGGING, "
                sql = sql & "  D.EXTENT_MANAGEMENT,"
                sql = sql & "  D.ALLOCATION_TYPE "
                sql = sql & "FROM "
                sql = sql & "  DBA_TABLESPACES D,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_DATA_FILES GROUP BY TABLESPACE_NAME) A,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_FREE_SPACE GROUP BY TABLESPACE_NAME) F "
                sql = sql & "WHERE"
                sql = sql & "  (D.EXTENT_MANAGEMENT <> 'LOCAL' OR"
                sql = sql & "   D.CONTENTS <> 'TEMPORARY') AND"
                sql = sql & "   D.TABLESPACE_NAME = A.TABLESPACE_NAME(+) AND "
                sql = sql & "   D.TABLESPACE_NAME = F.TABLESPACE_NAME(+) "
                sql = sql & "UNION ALL "
                sql = sql & "SELECT /*+ FIRST_ROWS */"
                sql = sql & "  D.TABLESPACE_NAME,"
                sql = sql & "  NVL(A.BYTES, 0)/1024, "
                sql = sql & "  (NVL(A.BYTES, 0) - NVL(F.BYTES, 0))/1024, "
                sql = sql & "  TRUNC((NVL(F.BYTES, 0)/1024)/NVL(A.BYTES, 1)*100,2), "
                sql = sql & "  D.STATUS, D.CONTENTS, D.INITIAL_EXTENT, D.NEXT_EXTENT, "
                sql = sql & "  D.MIN_EXTENTS, "
                sql = sql & "  D.MAX_EXTENTS, "
                sql = sql & "  D.PCT_INCREASE, "
                sql = sql & "  D.MIN_EXTLEN, "
                sql = sql & "  D.LOGGING, "
                sql = sql & "  D.EXTENT_MANAGEMENT, "
                sql = sql & "  D.ALLOCATION_TYPE "
                sql = sql & "FROM"
                sql = sql & "  DBA_TABLESPACES D,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES) BYTES FROM DBA_TEMP_FILES GROUP BY TABLESPACE_NAME) A,"
                sql = sql & "  (SELECT TABLESPACE_NAME, SUM(BYTES_USED) BYTES FROM V$TEMP_EXTENT_POOL GROUP BY TABLESPACE_NAME) F "
                sql = sql & "WHERE"
                sql = sql & "  D.EXTENT_MANAGEMENT = 'LOCAL' AND"
                sql = sql & "  D.CONTENTS = 'TEMPORARY' AND"
                sql = sql & "  D.TABLESPACE_NAME = A.TABLESPACE_NAME(+) AND"
                sql = sql & "  D.TABLESPACE_NAME = F.TABLESPACE_NAME(+) "
            End If
        ElseIf id = C_SEGMENT Then
            'セグメント情報取得SQL
            'SELECT句
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                sql = sql & "SELECT "
                sql = sql & "  A.OWNER, SEGMENT_NAME, "
                sql = sql & "  DECODE(A.SEGMENT_TYPE,'LOBSEGMENT','LOB','NESTED TABLE','TABLE',A.SEGMENT_TYPE) SEGMENT_TYPE, A.PARTITION_NAME, "
                sql = sql & "  DECODE(SEGMENT_SUBTYPE,'SECUREFILE',SEGMENT_SUBTYPE,B.SEGMENT_SPACE_MANAGEMENT) SEGMENT_SPACE_MANAGEMENT, B.BLOCK_SIZE, A.BLOCKS "
                sql = sql & "FROM"
                sql = sql & "  DBA_SEGMENTS A, DBA_TABLESPACES B "
                sql = sql & "WHERE "
                sql = sql & "  A.TABLESPACE_NAME = B.TABLESPACE_NAME "
            ElseIf Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 Or Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 Then
                sql = sql & "SELECT "
                sql = sql & "  A.OWNER, SEGMENT_NAME, "
                sql = sql & "  DECODE(A.SEGMENT_TYPE,'LOBSEGMENT','LOB','NESTED TABLE','TABLE',A.SEGMENT_TYPE) SEGMENT_TYPE, A.PARTITION_NAME, B.SEGMENT_SPACE_MANAGEMENT, B.BLOCK_SIZE, A.BLOCKS "
                sql = sql & "FROM"
                sql = sql & "  DBA_SEGMENTS A, DBA_TABLESPACES B "
                sql = sql & "WHERE "
                sql = sql & "  A.TABLESPACE_NAME = B.TABLESPACE_NAME "
            ElseIf Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = sql & "SELECT "
                sql = sql & "  A.OWNER, SEGMENT_NAME, "
                sql = sql & "  DECODE(A.SEGMENT_TYPE,'LOBSEGMENT','LOB','NESTED TABLE','TABLE',A.SEGMENT_TYPE) SEGMENT_TYPE, A.PARTITION_NAME, A.BLOCKS "
                sql = sql & "FROM"
                sql = sql & "  DBA_SEGMENTS A, DBA_TABLESPACES B "
                sql = sql & "WHERE "
                sql = sql & "  A.TABLESPACE_NAME = B.TABLESPACE_NAME "
            End If

            'スキーマ条件
            If CMB_SCHEMA.SelectedItem.ToString <> " " Then
                sql = sql + "AND OWNER = UPPER('" + CMB_SCHEMA.SelectedItem.ToString + "') "
            End If

            'オブジェクト条件
            If CHK_TBL.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                    isStart = False
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'TABLE' "
            End If
            If CHK_IND.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                    isStart = False
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'INDEX' "
            End If
            If CHK_TBL_P.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                    isStart = False
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'TABLE PARTITION' OR SEGMENT_TYPE = 'TABLE SUBPARTITION' "
            End If
            If CHK_IND_P.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                    isStart = False
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'INDEX PARTITION' OR SEGMENT_TYPE = 'INDEX SUBPARTITION' "
            End If
            If CHK_LOB.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'LOBSEGMENT' OR SEGMENT_TYPE = 'LOBINDEX' "
                isStart = False
            End If
            If CHK_LOB_P.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'LOB PARTITION' OR SEGMENT_TYPE = 'LOB SUBPARTITION'"
                isStart = False
            End If
            If CHK_CLUS.Checked Then
                If isStart Then
                    sql = sql + "AND ("
                Else
                    sql = sql + "OR "
                End If
                sql = sql + " SEGMENT_TYPE = 'CLUSTER' "
                isStart = False
            End If
            If isStart = False Then
                sql = sql & ") "
            End If

            '表領域条件
            If Not TXT_TABLESPACE.Text = vbNullString Then
                sql = sql + "AND A.TABLESPACE_NAME LIKE UPPER('" + TXT_TABLESPACE.Text + "') "
            End If

            'オブジェクトタイプ条件
            If Not TXT_OBJECT.Text = vbNullString Then
                sql = sql + "AND SEGMENT_NAME LIKE UPPER('" + TXT_OBJECT.Text + "') "
            End If
            sql = sql & "ORDER BY A.OWNER, SEGMENT_NAME"
        ElseIf id = C_TEMP Then
            '一時表領域情報取得SQL
            sql = sql & "SELECT "
            sql = sql & "  A.TABLESPACE TABLESPACE_NAME, "
            sql = sql & "  A.SEGTYPE SEGMENT_TYPE, "
            sql = sql & "  A.BLOCKS BLOCKS, "
            sql = sql & "  C.SID SID, "
            sql = sql & "  C.SERIAL# SERIAL#, "
            sql = sql & "  C.USERNAME USERNAME, "
            sql = sql & "  C.OSUSER OSUSER, "
            sql = sql & "  C.LOGON_TIME LOGON_TIME, "
            sql = sql & "  C.MACHINE MACHINE, "
            sql = sql & "  C.PROGRAM PROGRAM, "
            sql = sql & "  B.SQL_TEXT SQL_TEXT, "
            sql = sql & "  D.SQL_TEXT PREV_SQL_TEXT "
            sql = sql & "FROM "
            sql = sql & "  V$SORT_USAGE A, "
            sql = sql & "  (SELECT DISTINCT SQL_TEXT,ADDRESS,HASH_VALUE FROM V$SQL) B, "
            sql = sql & "  V$SESSION C, "
            sql = sql & "  (SELECT DISTINCT SQL_TEXT,ADDRESS,HASH_VALUE FROM V$SQL) D "
            sql = sql & "WHERE "
            sql = sql & "    C.SADDR = A.SESSION_ADDR AND "
            sql = sql & "    C.SQL_ADDRESS = B.ADDRESS(+) AND "
            sql = sql & "    C.SQL_HASH_VALUE = B.HASH_VALUE(+) AND "
            sql = sql & "    C.PREV_SQL_ADDR = D.ADDRESS(+) AND "
            sql = sql & "    C.PREV_HASH_VALUE = D.HASH_VALUE(+) "
        ElseIf id = C_UNDO_EXTENT_AUTO Then
            'UNDOエクステント情報取得SQL(自動UNDO管理)
            sql = sql & "SELECT OWNER,SEGMENT_NAME,TABLESPACE_NAME,EXTENT_ID,FILE_ID,BLOCK_ID,BYTES,BLOCKS,RELATIVE_FNO,STATUS "
            sql = sql & "FROM DBA_UNDO_EXTENTS "
            sql = sql & "UNION ALL "
            sql = sql & "SELECT OWNER,SEGMENT_NAME,TABLESPACE_NAME,EXTENT_ID,FILE_ID,BLOCK_ID,BYTES,BLOCKS,RELATIVE_FNO,NULL "
            sql = sql & "FROM DBA_EXTENTS "
            sql = sql & "WHERE SEGMENT_TYPE = 'ROLLBACK'"
        ElseIf id = C_UNDO_EXTENT_MANUAL Then
            'UNDOエクステント情報取得SQL(手動UNDO管理)
            sql = sql & "SELECT OWNER,SEGMENT_NAME,TABLESPACE_NAME,EXTENT_ID,FILE_ID,BLOCK_ID,BYTES,BLOCKS,RELATIVE_FNO,NULL FROM DBA_EXTENTS WHERE SEGMENT_TYPE = 'ROLLBACK'"
        ElseIf id = C_UNDO_SEGMENT_AUTO Then
            'UNDOセグメント情報取得SQL(自動UNDO管理)
            sql = sql & "SELECT "
            sql = sql & "  D.TABLESPACE_NAME, "
            sql = sql & "  D.SEGMENT_NAME, "
            sql = sql & "  (D.BYTES/1024) ""SEGMENT_SIZE(KB)"", "
            sql = sql & "  NVL(U1.BYTES, 0)/1024 ""ACTIVE(KB)"", "
            sql = sql & "  NVL(U2.BYTES, 0)/1024 ""UNEXPIRE(KB)"", "
            sql = sql & "  NVL(U3.BYTES, 0)/1024 ""EXPIRE(KB)"", "
            sql = sql & "  E.XACTS TX_CNT,"
            sql = sql & "  E.WRITES,"
            sql = sql & "  E.GETS,"
            sql = sql & "  E.WAITS,"
            sql = sql & "  E.OPTSIZE,"
            sql = sql & "  E.HWMSIZE,"
            sql = sql & "  E.SHRINKS,"
            sql = sql & "  E.WRAPS,"
            sql = sql & "  E.EXTENDS,"
            sql = sql & "  E.AVESHRINK,"
            sql = sql & "  E.STATUS,"
            sql = sql & "  E.CUREXT,"
            sql = sql & "  E.CURBLK "
            sql = sql & "FROM "
            sql = sql & "  (SELECT TABLESPACE_NAME, SEGMENT_NAME, SUM(BYTES) BYTES FROM DBA_UNDO_EXTENTS GROUP BY TABLESPACE_NAME,SEGMENT_NAME) D, "
            sql = sql & "  (SELECT TABLESPACE_NAME, SEGMENT_NAME, XACTS ,WRITES,GETS,WAITS,OPTSIZE,HWMSIZE,SHRINKS,WRAPS,EXTENDS,AVESHRINK,AVEACTIVE,B.STATUS,CUREXT,CURBLK FROM DBA_ROLLBACK_SEGS A,GV$ROLLSTAT B WHERE A.SEGMENT_ID=B.USN(+)) E,"
            sql = sql & "  (SELECT TABLESPACE_NAME, SEGMENT_NAME, SUM(BYTES) BYTES FROM DBA_UNDO_EXTENTS WHERE STATUS='ACTIVE' GROUP BY TABLESPACE_NAME,SEGMENT_NAME, STATUS) U1, "
            sql = sql & "  (SELECT TABLESPACE_NAME, SEGMENT_NAME, SUM(BYTES) BYTES FROM DBA_UNDO_EXTENTS WHERE STATUS='UNEXPIRED' GROUP BY TABLESPACE_NAME,SEGMENT_NAME, STATUS) U2, "
            sql = sql & "  (SELECT TABLESPACE_NAME, SEGMENT_NAME, SUM(BYTES) BYTES FROM DBA_UNDO_EXTENTS WHERE STATUS='EXPIRED' GROUP BY TABLESPACE_NAME,SEGMENT_NAME, STATUS) U3 "
            sql = sql & "WHERE "
            sql = sql & "  D.TABLESPACE_NAME = U1.TABLESPACE_NAME(+) AND "
            sql = sql & "  D.TABLESPACE_NAME = U2.TABLESPACE_NAME(+) AND "
            sql = sql & "  D.TABLESPACE_NAME = U3.TABLESPACE_NAME(+) AND "
            sql = sql & "  D.TABLESPACE_NAME = E.TABLESPACE_NAME(+) AND "
            sql = sql & "  D.SEGMENT_NAME = U1.SEGMENT_NAME(+) AND "
            sql = sql & "  D.SEGMENT_NAME = U2.SEGMENT_NAME(+) AND "
            sql = sql & "  D.SEGMENT_NAME = U3.SEGMENT_NAME(+) AND "
            sql = sql & "  D.SEGMENT_NAME = E.SEGMENT_NAME(+) "
            sql = sql & "ORDER BY "
            sql = sql & "  D.TABLESPACE_NAME, "
            sql = sql & "  D.SEGMENT_NAME "
        ElseIf id = C_UNDO_SEGMENT_MANUAL Then
            'UNDOセグメント取得SQL(手動UNDO管理)
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "SELECT "
                sql = sql & "  A.TABLESPACE_NAME, "
                sql = sql & "  A.SEGMENT_NAME, "
                sql = sql & "  D.BLOCKS*E.BLOCK_SIZE SEGMENT_SIZE, "
                sql = sql & "  B.XACTS TX_CNT, "
                sql = sql & "  B.WRITES, "
                sql = sql & "  B.GETS, "
                sql = sql & "  B.WAITS, "
                sql = sql & "  B.OPTSIZE, "
                sql = sql & "  B.HWMSIZE, "
                sql = sql & "  B.SHRINKS, "
                sql = sql & "  B.WRAPS, "
                sql = sql & "  B.EXTENDS, "
                sql = sql & "  B.AVESHRINK, "
                sql = sql & "  B.STATUS, "
                sql = sql & "  B.CUREXT, "
                sql = sql & "  B.CURBLK "
                sql = sql & "FROM "
                sql = sql & "  DBA_ROLLBACK_SEGS A, "
                sql = sql & "  V$ROLLSTAT B, "
                sql = sql & "  DBA_SEGMENTS D, "
                sql = sql & "  DBA_TABLESPACES E "
                sql = sql & "WHERE "
                sql = sql & "  A.SEGMENT_ID=B.USN(+) AND "
                sql = sql & "  A.SEGMENT_NAME=D.SEGMENT_NAME AND"
                sql = sql & "  A.TABLESPACE_NAME=D.TABLESPACE_NAME AND"
                sql = sql & "  D.TABLESPACE_NAME=E.TABLESPACE_NAME "
                sql = sql & "ORDER BY "
                sql = sql & "  A.TABLESPACE_NAME,A.SEGMENT_NAME"
            Else
                '↓DBA_SEGMENTSではなくDBA_EXTENTSを使用したほうが↑と似た感じになるが、今のところ未対応。
                sql = sql & "SELECT "
                sql = sql & "  A.TABLESPACE_NAME, "
                sql = sql & "  A.SEGMENT_NAME, "
                sql = sql & "  D.BLOCKS*E.BLOCK_SIZE SEGMENT_SIZE, "
                sql = sql & "  B.XACTS TX_CNT, "
                sql = sql & "  B.WRITES, "
                sql = sql & "  B.GETS, "
                sql = sql & "  B.WAITS, "
                sql = sql & "  B.OPTSIZE, "
                sql = sql & "  B.HWMSIZE, "
                sql = sql & "  B.SHRINKS, "
                sql = sql & "  B.WRAPS, "
                sql = sql & "  B.EXTENDS, "
                sql = sql & "  B.AVESHRINK, "
                sql = sql & "  B.STATUS, "
                sql = sql & "  B.CUREXT, "
                sql = sql & "  B.CURBLK  "
                sql = sql & "FROM "
                sql = sql & "  DBA_ROLLBACK_SEGS A, "
                sql = sql & "  V$ROLLSTAT B, "
                sql = sql & "  DBA_SEGMENTS D, "
                sql = sql & "  (SELECT B.NAME TABLESPACE_NAME,A.BLOCK_SIZE FROM V$DATAFILE A,V$TABLESPACE B WHERE A.TS#=B.TS#) E "
                sql = sql & "WHERE "
                sql = sql & "  A.SEGMENT_ID=B.USN AND "
                sql = sql & "  A.SEGMENT_NAME=D.SEGMENT_NAME AND"
                sql = sql & "  A.TABLESPACE_NAME=D.TABLESPACE_NAME AND"
                sql = sql & "  D.TABLESPACE_NAME=E.TABLESPACE_NAME "
                sql = sql & "ORDER BY "
                sql = sql & "  A.TABLESPACE_NAME,A.SEGMENT_NAME"
            End If
        ElseIf id = C_TX_UNDO Then
            'トランザクション毎のUNDO取得
            sql = sql & "SELECT DISTINCT"
            sql = sql & "  A.START_TIME TX_START_TIME, "
            sql = sql & "  A.XIDUSN ROLLBACK_SEGMENT_ID, "
            sql = sql & "  A.USED_UBLK UNDO_BLOCK, "
            sql = sql & "  A.USED_UREC UNDO_RECORD, "
            sql = sql & "  B.SID SID,  "
            sql = sql & "  B.SERIAL# SERIAL#,  "
            sql = sql & "  B.USERNAME USERNAME,  "
            sql = sql & "  B.OSUSER OSUSER,  "
            sql = sql & "  B.LOGON_TIME LOGON_TIME, "
            sql = sql & "  B.MACHINE MACHINE,  "
            sql = sql & "  B.PROGRAM PROGRAM, "
            sql = sql & "  C.SQL_TEXT SQL_TEXT, "
            sql = sql & "  D.SQL_TEXT PREV_SQL_TEXT "
            sql = sql & "FROM "
            sql = sql & "  V$TRANSACTION A, "
            sql = sql & "  V$SESSION B, "
            sql = sql & "  V$SQL C, "
            sql = sql & "  V$SQL D "
            sql = sql & "WHERE "
            sql = sql & "  A.SES_ADDR =B.SADDR AND "
            sql = sql & "  B.SQL_ADDRESS = C.ADDRESS(+) AND "
            sql = sql & "  B.SQL_HASH_VALUE = C.HASH_VALUE(+) AND "
            sql = sql & "  B.PREV_SQL_ADDR = D.ADDRESS(+) AND "
            sql = sql & "  B.PREV_HASH_VALUE = D.HASH_VALUE(+) "
        ElseIf id = C_DATAFILE Then
            'データファイル情報取得
            'V$DATAFILEは11gでデータファイル数が多い場合検索が遅いので使わないこと
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "SELECT "
                sql = sql & "  A.FILE_ID, A.FILE_NAME, NVL(A.BYTES,0), NVL(A.USER_BYTES,0), B.BLOCK_SIZE, DECODE(SUBSTR(FILE_NAME,1,1),'+',0,B.BLOCK_SIZE) "
                sql = sql & "FROM"
                sql = sql & "  DBA_DATA_FILES A, DBA_TABLESPACES B "
                sql = sql & "WHERE"
                sql = sql & "  A.TABLESPACE_NAME = B.TABLESPACE_NAME "
                sql = sql & "ORDER BY"
                sql = sql & "  A.FILE_ID"
            ElseIf Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = sql & "SELECT "
                sql = sql & "  A.FILE_ID, A.FILE_NAME, NVL(A.BYTES,0), NVL(A.USER_BYTES,0), B.BLOCK_SIZE,B.BLOCK_SIZE "
                sql = sql & "FROM"
                sql = sql & "  DBA_DATA_FILES A, V$DATAFILE B "
                sql = sql & "WHERE"
                sql = sql & "  A.FILE_ID = B.FILE# "
                sql = sql & "ORDER BY"
                sql = sql & "  A.FILE_ID"
            End If
        ElseIf id = C_EXTENT Then
            sql = sql & "SELECT"
            sql = sql & "  BLOCK_ID START_BLOCK, (BLOCK_ID+BLOCKS-1) END_BLOCK, BLOCKS BLOCK_COUNT, OWNER, SEGMENT_NAME "
            sql = sql & "FROM"
            sql = sql & "  DBA_EXTENTS "
            sql = sql & "WHERE"
            sql = sql & "  FILE_ID = " & CMB_DATAFILE.SelectedItem.ToString.Substring(0, InStr(CMB_DATAFILE.SelectedItem.ToString, " ") - 1) & " "

            If Not (CMB_SCHEMA2.SelectedItem Is Nothing) Then
                If CMB_SCHEMA2.SelectedItem.ToString <> " " Then
                    sql = sql & "    AND OWNER = '" & CMB_SCHEMA2.SelectedItem.ToString & "' "
                End If
            End If

            If TXT_SEGMENT.Text <> vbNullString Then
                sql = sql & "    AND SEGMENT_NAME LIKE '" & TXT_SEGMENT.Text & "' "
            End If

            '空き領域の表示はオーナーとセグメント指定がない場合のみ
            If Not (CMB_SCHEMA2.SelectedItem Is Nothing) Then
                If CMB_SCHEMA2.SelectedItem.ToString = " " And TXT_SEGMENT.Text = vbNullString Then
                    sql = sql & "UNION ALL "
                    sql = sql & "SELECT"
                    sql = sql & "  BLOCK_ID START_BLOCK, (BLOCK_ID+BLOCKS-1) END_BLOCK, BLOCKS BLOCK_COUNT, 'FREE', 'FREE' "
                    sql = sql & "FROM"
                    sql = sql & "  DBA_FREE_SPACE "
                    sql = sql & "WHERE"
                    sql = sql & "  FILE_ID = " & CMB_DATAFILE.SelectedItem.ToString.Substring(0, InStr(CMB_DATAFILE.SelectedItem.ToString, " ") - 1) & " "
                    sql = sql & "ORDER BY "
                    sql = sql & "  START_BLOCK"
                End If
            End If

        ElseIf id = C_DATAFILE_STAT Then
            'データファイル＋ファイルの統計情報
            sql = sql & "SELECT "
            sql = sql & "  FILE_ID,TABLESPACE_NAME,FILE_NAME,BLOCKS,BYTES,STATUS,RELATIVE_FNO,AUTOEXTENSIBLE,MAXBLOCKS,MAXBYTES,INCREMENT_BY,USER_BLOCKS,USER_BYTES,"
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
               (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                sql = sql & "ONLINE_STATUS,"
            End If

            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "  PHYRDS,PHYWRTS,PHYBLKRD,PHYBLKWRT,SINGLEBLKRDS,READTIM,WRITETIM,SINGLEBLKRDTIM,AVGIOTIM,LSTIOTIM,MINIOTIM,MAXIORTM,MAXIOWTM "
            Else
                sql = sql & "  PHYRDS,PHYWRTS,PHYBLKRD,PHYBLKWRT,READTIM,WRITETIM,AVGIOTIM,LSTIOTIM,MINIOTIM,MAXIORTM,MAXIOWTM "
            End If
            sql = sql & "FROM"
            sql = sql & "  DBA_DATA_FILES D,"
            sql = sql & "  V$FILESTAT V "
            sql = sql & "WHERE"
            sql = sql & "  D.FILE_ID=FILE# "
            sql = sql & "UNION ALL "
            sql = sql & "SELECT "
            sql = sql & "  FILE_ID,TABLESPACE_NAME,FILE_NAME,BLOCKS,BYTES,STATUS,RELATIVE_FNO,AUTOEXTENSIBLE,MAXBLOCKS,MAXBYTES,INCREMENT_BY,USER_BLOCKS,USER_BYTES, "
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
               (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                sql = sql & "  NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL "
            ElseIf Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "  NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL "
            Else
                '8iの場合NULLにするとORA-01790: 式には対応する式と同じデータ型がなければなりません。が発生するため０にする。
                sql = sql & "  0,0,0,0,0,0,0,0,0,0,0 "
            End If
            sql = sql & "FROM "
            sql = sql & "  DBA_TEMP_FILES D "
            sql = sql & "ORDER BY FILE_ID "
        ElseIf id = C_CONTROL_FILE Then
            sql = "SELECT * FROM V$CONTROLFILE"
        ElseIf id = C_CONTROL_FILE_RECORD Then
            sql = "SELECT * FROM V$CONTROLFILE_RECORD_SECTION"
        ElseIf id = C_REDO Then
            sql = "SELECT B.GROUP#,B.THREAD#,B.SEQUENCE#,B.BYTES,B.MEMBERS,B.ARCHIVED,B.STATUS,B.FIRST_CHANGE#,B.FIRST_TIME, "
            '11gR2以降
            If (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 11 And Integer.parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Or Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 12 Then
                sql = sql & "B.BLOCKSIZE,B.NEXT_CHANGE#,B.NEXT_TIME, "
            End If

            sql = sql & "A.STATUS, "

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & "A.TYPE, "
            End If

            sql = sql & "A.MEMBER "

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                sql = sql & ",IS_RECOVERY_DEST_FILE "
            End If
            sql = sql & "FROM V$LOGFILE A,V$LOG B "
            sql = sql & "WHERE A.GROUP# = b.GROUP# "
            sql = sql & "ORDER BY B.GROUP#,B.THREAD#,B.SEQUENCE#"
        ElseIf id = C_REDO_HISTORY Then
            sql = "SELECT * FROM V$LOG_HISTORY"
        ElseIf id = C_ARCHIVELOG Then
            sql = "SELECT * FROM V$ARCHIVED_LOG"
        End If

        Return sql
    End Function

    Private Sub frm_Session_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim obj As Object
        'resizeイベントを実行する
        Me.Width = 800
        CMB_SCHEMA.Items.Add(" ") 'ダミー
        CMB_SCHEMA2.Items.Add(" ") 'ダミー
        For Each obj In frm_Login.G_DB.G_USERS
            CMB_SCHEMA.Items.Add(obj.ToString)
            CMB_SCHEMA2.Items.Add(obj.ToString)
        Next

        CMB_SCHEMA.SelectedIndex = 0
        SetDatafileInfo()
        getUndoManagement()
        CMB_DATAFILE.SelectedIndex = 0
        CMB_SCHEMA2.SelectedIndex = 0
        CMB_UNDO_TYPE.SelectedIndex = 0
        Me.Text = "O-Analyzer - 記憶域情報(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        Me.Width = Me.Width + 1
        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If tabControl1.SelectedIndex = 0 Then
            DispTablespace()
        ElseIf tabControl1.SelectedIndex = 1 Then
            DispDatafile()
        ElseIf tabControl1.SelectedIndex = 2 Then
            DispSegment()
        ElseIf tabControl1.SelectedIndex = 3 Then
            DispExtent()
        ElseIf tabControl1.SelectedIndex = 4 Then
            DispTemp()
        ElseIf tabControl1.SelectedIndex = 5 Then
            DispUndo()
        ElseIf tabControl1.SelectedIndex = 6 Then
            DispControlFile()
        ElseIf tabControl1.SelectedIndex = 7 Then
            DispRedo()
        ElseIf tabControl1.SelectedIndex = 8 Then
            DispArchivelog()
        Else

        End If
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        M_CANCELFLG = True
    End Sub

    Private Sub frm_info_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        LBL_INFO.Location = New Point(5, Me.Height - 55)

        tabControl1.Width = GroupBox1.Location.X - 10
        tabControl1.Height = Me.Height - LBL_INFO.Height - 50
        TabControl2.Width = tabControl1.Width - 15
        TabControl2.Height = tabControl1.Height - GroupBox3.Height - LBL_INFO.Height - 30

        TAB_TABLESPACE.Width = tabControl1.Width - 10
        TAB_TABLESPACE.Height = tabControl1.Height - 10
        TAB_DATAFILE.Width = tabControl1.Width - 10
        TAB_DATAFILE.Height = tabControl1.Height - 10
        TAB_SEGMENT.Width = tabControl1.Width - 10
        TAB_SEGMENT.Height = tabControl1.Height - 10
        TAB_EXTENT.Width = tabControl1.Width - 10
        TAB_EXTENT.Height = tabControl1.Height - 10
        TAB_TEMP.Width = tabControl1.Width - 10
        TAB_TEMP.Height = tabControl1.Height - 10
        TAB_UNDO.Width = tabControl1.Width - 10
        TAB_UNDO.Height = tabControl1.Height - 10
        TAB_CONTROLFILE.Width = tabControl1.Width - 10
        TAB_CONTROLFILE.Height = tabControl1.Height - 10
        TAB_REDO.Width = tabControl1.Width - 10
        TAB_REDO.Height = tabControl1.Height - 10
        TAB_ARCHIVELOG.Width = tabControl1.Width - 10
        TAB_ARCHIVELOG.Height = tabControl1.Height - 10

        TAB_MSSM.Width = TabControl2.Width - 10
        TAB_MSSM.Height = TabControl2.Height - 10
        TAB_ASSM.Width = TAB_MSSM.Width
        TAB_ASSM.Height = TAB_MSSM.Height
        TAB_SECURE.Width = TAB_MSSM.Width
        TAB_SECURE.Height = TAB_MSSM.Height

        SplitContainer1.Width = TAB_UNDO.Width - 5
        SplitContainer1.Height = TAB_UNDO.Height - 5

        DGV_TABLESPACE.Width = TAB_SEGMENT.Width - 10
        DGV_TABLESPACE.Height = TAB_SEGMENT.Height - 10
        DGV_FREELIST.Width = TabControl2.Width - 15
        DGV_FREELIST.Height = TabControl2.Height - 35
        DGV_ASSM.Width = TabControl2.Width - 15
        DGV_ASSM.Height = DGV_FREELIST.Height
        DGV_SECURELOB.Width = TabControl2.Width - 15
        DGV_SECURELOB.Height = DGV_FREELIST.Height
        DGV_TEMP.Width = TAB_SEGMENT.Width - 10
        DGV_TEMP.Height = TAB_SEGMENT.Height - 10
        GroupBox3.Width = TAB_SEGMENT.Width - 10
        GroupBox8.Width = TAB_SEGMENT.Width - 10

        DGV_UNDO.Width = SplitContainer1.Panel1.Width - 5
        DGV_UNDO.Height = SplitContainer1.Panel1.Height - 30
        DGV_TX.Width = SplitContainer1.Panel2.Width - 5
        DGV_TX.Height = SplitContainer1.Panel2.Height - 25
        DGV_EXTENT.Width = TAB_EXTENT.Width - 10
        DGV_EXTENT.Height = TAB_EXTENT.Height - 105
        DGV_DATAFILE.Width = TAB_SEGMENT.Width - 10
        DGV_DATAFILE.Height = TAB_SEGMENT.Height - 10

        '制御ファイル画面
        SplitContainer2.Width = TAB_CONTROLFILE.Width
        SplitContainer2.Height = TAB_CONTROLFILE.Height - 5
        DGV_CONFROLFILE.Width = SplitContainer2.Width - 10
        DGV_CONFROLFILE.Height = SplitContainer2.Panel1.Height - 5
        DGV_CONFROLFILE_RECORD.Width = SplitContainer2.Width - 10
        DGV_CONFROLFILE_RECORD.Height = SplitContainer2.Panel2.Height - 25

        'REDO画面
        SplitContainer3.Width = TAB_CONTROLFILE.Width
        SplitContainer3.Height = TAB_CONTROLFILE.Height - 5
        DGV_REDO.Width = SplitContainer3.Width - 10
        DGV_REDO.Height = SplitContainer3.Panel1.Height - 5
        DGV_REDO_HISTORY.Width = SplitContainer3.Width - 10
        DGV_REDO_HISTORY.Height = SplitContainer3.Panel2.Height - 25

        'アーカイブログ
        DGV_ARCHIVELOG.Width = TAB_ARCHIVELOG.Width - 10
        DGV_ARCHIVELOG.Height = TAB_ARCHIVELOG.Height - 10

    End Sub

    Private Sub DGV_2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DGV_FREELIST.KeyDown
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

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMB_DATAFILE.SelectedIndexChanged
        Dim indexNo As Integer
        indexNo = GetIndex(CMB_DATAFILE.SelectedItem.ToString.Substring(0, InStr(CMB_DATAFILE.SelectedItem.ToString, " ") - 1))
        If indexNo = -1 Then
            Exit Sub
        End If
        TXT_DATAFILE_INFO.Text = _
        "ファイルサイズ：" & CLng(M_FILE_INFO(indexNo).ALLBYTES) + CLng(M_FILE_INFO(indexNo).BLOCK_OFFSET) & _
        "(ファイルヘッダ：" & M_FILE_INFO(indexNo).BLOCK_OFFSET & _
        " + その他(ビットマップ領域等)：" & CLng(M_FILE_INFO(indexNo).ALLBYTES) - CLng(M_FILE_INFO(indexNo).USER_BYTES) & _
        " + ユーザ領域：" & M_FILE_INFO(indexNo).USER_BYTES & ") byte"
    End Sub

    Private Sub BTN_EXEC_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles BTN_EXEC.KeyDown
        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub frm_Segment_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_TABLESPACE.RowPostPaint, DGV_FREELIST.RowPostPaint, DGV_ASSM.RowPostPaint, DGV_EXTENT.RowPostPaint, DGV_SECURELOB.RowPostPaint, DGV_UNDO.RowPostPaint, DGV_CONFROLFILE.RowPostPaint, DGV_CONFROLFILE_RECORD.RowPostPaint, DGV_REDO.RowPostPaint, DGV_REDO_HISTORY.RowPostPaint, DGV_DATAFILE.RowPostPaint, DGV_TX.RowPostPaint, DGV_TEMP.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub SplitContainer1_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize
        DGV_UNDO.Height = SplitContainer1.Panel1.Height - 30
    End Sub
    Private Sub SplitContainer1_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel2.Resize
        DGV_TX.Height = SplitContainer1.Panel2.Height - 25
    End Sub

    Private Sub SplitContainer2_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel1.Resize
        DGV_CONFROLFILE.Height = SplitContainer2.Panel1.Height - 10
    End Sub
    Private Sub SplitContainer2_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel2.Resize
        DGV_CONFROLFILE_RECORD.Height = SplitContainer2.Panel2.Height - 25
    End Sub

    Private Sub SplitContainer3_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer3.Panel1.Resize
        DGV_REDO.Height = SplitContainer3.Panel1.Height - 10
    End Sub

    Private Sub SplitContainer3_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer3.Panel2.Resize
        DGV_REDO_HISTORY.Height = SplitContainer3.Panel2.Height - 25
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class