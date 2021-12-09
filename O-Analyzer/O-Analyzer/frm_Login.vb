Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE
Imports System.Runtime.InteropServices

Public Class frm_Login
    '#######################################################################
    '���O�C�����
    '#######################################################################

    '�O���[�o���ϐ�
    Public G_DB As CLS_DB.CLS_DB
    Public G_INTERNAL As FRM_INTERNAL.FRM_INTERNAL

    '�ڑ��惊�X�g�\����
    Private Structure S_CONN_LIST
        Public index As Short
        Public connName As String
        Public userName As String
        Public password As String
        Public connString As String
        Public isSysdba As Short
        Public options As String
        Public middle As Short
        Public isSavePass As Short
        Public Sub SetConnList(ByVal index As Short, ByVal connName As String, ByVal userName As String, _
                                 ByVal password As String, ByVal connString As String, ByVal middle As Short, _
                                 ByVal options As String, ByVal isSysdba As Short, ByVal isSavePass As Short)
            Me.index = index
            Me.connName = connName
            Me.userName = userName
            Me.password = password
            Me.connString = connString
            Me.middle = middle
            Me.options = options
            Me.isSysdba = isSysdba
            Me.isSavePass = isSavePass
        End Sub
    End Structure
    Private Shared CONN_LIST() As S_CONN_LIST

    Function GetConnection() As Boolean
        '#######################################################################
        '�R�l�N�V�����擾
        '#######################################################################
        Dim isUseConnectionPooling As Boolean
        Dim isConnectSuccess As Boolean

        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0002")
        Dim ssform As New SplashScreen1
        Me.Refresh()

        'connection_pooling �g�p�t���O�ݒ�
        If CHK_SYSDBA.Checked Then
            isUseConnectionPooling = True
        Else
            isUseConnectionPooling = False
        End If

        'DB�N���X�C���X�^���X�Đ���
        G_DB = New CLS_DB.CLS_DB

        '�ڑ�����~�h���E�G�A��ݒ�
        G_DB.SET_MIDDLEWARE(CMB_MIDDLE.SelectedIndex)

        '���ϐ�NLS_LANG�̐ݒ�
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_NLS_LANG) <> vbNullString Then
            M_COMMON.SetProcessEnvironment("NLS_LANG", M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_NLS_LANG), System.EnvironmentVariableTarget.Process)
        End If

        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0002")
        Me.Refresh()

        '�ڑ�
        isConnectSuccess = G_DB.DB_CONNECT(TXT_USERID.Text, TXT_PASS.Text, TXT_CONN_STR.Text, TXT_OPTION.Text, CHK_SYSDBA.Checked)
        If isConnectSuccess Then
            '����
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0003")
            'If CHK_SAVE_PASS.Checked Then
            '    '�p�X���[�h��ۑ�����
            '    M_FILE.CreateControlInitFile(Me.Name, Me.Controls, False)
            'Else
            '    '�p�X���[�h��ۑ����Ȃ�
            '    M_FILE.CreateControlInitFile(Me.Name, Me.Controls, True)
            'End If
        Else
            '���s
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0004")
            '�A�Z���u���y�у��W���[���̏��o��
            M_FILE.CreateAssemblyInfoFile()
            M_FILE.CreateModuleInfoFile()
            G_DB = Nothing
        End If
        '���[�h�ς݃��C�u�����̋�����h���׃~�h���E�G�A�̍đI���͕s�ɂ���
        CMB_MIDDLE.Enabled = False

        Me.Refresh()
        Return isConnectSuccess

    End Function

    Private Sub RegistConnListFromForm(ByVal selectedIndex As Integer)
        '�ݒ�ꗗ��TAB��؂�œn��
        Dim registStr As String = vbNullString
        Dim connListName As String = TXT_CONN_LIST.Text.ToString.Replace(ControlChars.Tab, vbNullString)
        Dim userName As String = TXT_USERID.Text.ToString.Replace(ControlChars.Tab, vbNullString)
        Dim password As String
        Dim connStr As String = TXT_CONN_STR.Text.ToString.Replace(ControlChars.Tab, vbNullString)
        Dim middle As String = CMB_MIDDLE.SelectedIndex.ToString
        Dim options As String = TXT_OPTION.Text.ToString.Replace(ControlChars.Tab, vbNullString)
        Dim isSysdba As String
        Dim isSavePass As String

        '�p�X���[�h�͕ۑ����邩�ǂ����`�F�b�N�{�b�N�X�Ŕ��f����
        If CHK_SAVE_PASS.Checked Then
            password = M_COMMON.EncryptString(TXT_PASS.Text)
        Else
            password = vbNullString
        End If

        'SYSDBA�t���O
        If CHK_SYSDBA.Checked Then
            isSysdba = "1"
        Else
            isSysdba = "0"
        End If

        '�p�X���[�h�ۑ��t���O
        If CHK_SAVE_PASS.Checked Then
            isSavePass = "1"
        Else
            isSavePass = "0"
        End If

        '�C���f�b�N�X�ԍ� ���X�g�� ���[�UID �p�X���[�h �ڑ������� �ڑ��~�h���E�G�A SYSDBA�t���O �Z�L�����e�B�t���O
        registStr = selectedIndex & vbTab & connListName & vbTab & userName & vbTab
        registStr = registStr & password & vbTab & connStr & vbTab & middle & vbTab
        registStr = registStr & options & vbTab & isSysdba & vbTab & isSavePass

        M_FILE.CreateConnList(LB_CONN_LIST.SelectedIndex, registStr)
        LoadConnList()
        LB_CONN_LIST.SelectedIndex = selectedIndex
        Me.Focus()
    End Sub

    Private Sub RegistConnListFromValiable(ByVal selectedIndex As Integer)
        Dim registStr As String = vbNullString

        '�ݒ�ꗗ��TAB��؂�œn��
        '�C���f�b�N�X�ԍ� ���X�g�� ���[�UID �p�X���[�h �ڑ������� �ڑ��~�h���E�G�A SYSDBA�t���O �Z�L�����e�B�t���O
        registStr = selectedIndex & vbTab & CONN_LIST(selectedIndex).connName & vbTab & CONN_LIST(selectedIndex).userName & vbTab

        '�p�X���[�h�͕ۑ����邩�ǂ����`�F�b�N�{�b�N�X�Ŕ��f����
        If CHK_SAVE_PASS.Checked Then
            registStr = registStr & M_COMMON.EncryptString(CONN_LIST(selectedIndex).password)
        Else
            registStr = registStr & vbNullString
        End If

        registStr = registStr & vbTab & CONN_LIST(selectedIndex).connString & vbTab & CONN_LIST(selectedIndex).middle & vbTab & CONN_LIST(selectedIndex).options & vbTab
        registStr = registStr & CONN_LIST(selectedIndex).isSysdba & vbTab
        registStr = registStr & CONN_LIST(selectedIndex).isSavePass

        M_FILE.CreateConnList(selectedIndex, registStr)
    End Sub

    Private Sub RemoveConnList()
        Dim registStr As String = vbNullString
        Dim selectedIndex As Short
        selectedIndex = LB_CONN_LIST.SelectedIndex

        M_FILE.RemoveConnList(LB_CONN_LIST.SelectedIndex)
        LoadConnList()
        LB_CONN_LIST.SelectedIndex = 0
    End Sub

    Private Sub LoadConnList()
        Dim rows() As String
        Dim row(8) As String

        LB_CONN_LIST.Items.Clear()
        rows = M_FILE.GetConnList()
        If Not (rows Is Nothing) Then
            ReDim CONN_LIST(rows.Length - 1)

            For i As Integer = 0 To rows.Length - 1
                row = rows(i).Split(vbTab)
                '�p�X���[�h����
                If row(8) = 1 And row(3) <> vbNullString Then
                    row(3) = M_COMMON.DecryptString(row(3))
                End If

                CONN_LIST(i).SetConnList(Integer.Parse(row(0)), row(1), row(2), row(3), row(4), row(5), row(6), row(7), row(8))
                LB_CONN_LIST.Items.Add(row(1))
            Next
        End If
        LB_CONN_LIST.Items.Add("�V�K")
    End Sub

    Private Sub frm_Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim tmpConfigfileRows() As String
        Dim initParams(M_CONST.C_INITPARAM.Length - 1) As String

        Me.Text = "O-Analyzer (" & Process.GetCurrentProcess().Id() & ")"
        'CLS_CONST�̏�����
        M_CONST.Init()

        '�~�h���E�G�A�̃R���{�{�b�N�X�f�[�^�Z�b�g
        For Each middleware As Object In M_CONST.C_MIDDLE
            CMB_MIDDLE.Items.Add(middleware.GetName())
        Next

        '�����l
        CMB_MIDDLE.SelectedIndex = 0

        '�ڑ��惊�X�g�̓ǂݍ���
        LoadConnList()
        '�擪��ԏ�̃��X�g��I��
        LB_CONN_LIST.SelectedIndex = 0

        '��ʐݒ�ǂݍ���
        'M_FILE.SetControls(Me.Name, Me.Controls)

        'ini�t�@�C���ǂݍ���
        If My.Computer.FileSystem.FileExists(System.IO.Directory.GetCurrentDirectory() & "\" & "config.ini") = False Then
            For cnt As Integer = 0 To M_CONST.C_INITPARAM.Length - 1
                initParams(cnt) = M_CONST.C_INITPARAM(cnt).GET_ALL()
            Next
            M_FILE.CreateConfigFile(initParams)
        End If
        tmpConfigfileRows = M_FILE.ReadConfigFile("config.ini")

        For rowCnt As Integer = 0 To tmpConfigfileRows.Length - 1
            If M_CONST.SetInitParam(tmpConfigfileRows(rowCnt)) = False Then
                M_FILE.WriteApLog("�R���t�B�O�t�@�C���̃��[�h�����s���܂����B�f�t�H���g�ݒ�ŋN�����܂�")
                M_CONST.SetDefaultInitParam()
                For cnt As Integer = 0 To M_CONST.C_INITPARAM.Length - 1
                    initParams(cnt) = M_CONST.C_INITPARAM(cnt).GET_ALL()
                Next
                M_FILE.CreateConfigFile(initParams)
                tmpConfigfileRows = M_FILE.ReadConfigFile("config.ini")
                M_CONST.SetInitParam(tmpConfigfileRows(rowCnt))
                Exit For
            End If
        Next
        M_FILE.WriteApLog("�v���O�������N�����܂��B�f�B���N�g��:" & My.Computer.FileSystem.CurrentDirectory & " �v���Z�XID:" & Process.GetCurrentProcess().Id() & " �o�[�W�����F" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision)

        '�G���R�[�h�Z�b�g
        M_FILE.SetEncode()
        '�X�v���b�V���X�N���[�����N�����Ă���ƃt�H�[�������ŋN�����Ă��܂����ۂ̑Ή�
        'Me.TopMost = True
        'Me.Focus()
        Me.Activate2()
    End Sub

    <DllImport("user32.dll")> _
    Public Shared Function AttachThreadInput( _
   ByVal idAttach As Integer, ByVal idAttachTo As Integer, ByVal fAttach As Boolean) As Boolean
    End Function
    <DllImport("user32.dll")> _
    Public Shared Function GetForegroundWindow() As IntPtr
    End Function
    <DllImport("user32.dll")> _
    Public Shared Function GetWindowThreadProcessId(ByVal hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    End Function

    Private Sub Activate2()
        Dim targetThreadProcessId As Integer = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero.ToInt32)
        Dim ThreadProcessId As Integer = GetWindowThreadProcessId(Me.Handle, 0&)
        AttachThreadInput(ThreadProcessId, targetThreadProcessId, True)
        Me.Activate()
        AttachThreadInput(ThreadProcessId, targetThreadProcessId, False)
    End Sub

    Private Sub frm_Login_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Dim isEnableSecuriy As Boolean

        If CHK_SAVE_PASS.Checked Then
            '�p�X���[�h��ۑ�����
            isEnableSecuriy = False
        Else
            '�p�X���[�h��ۑ����Ȃ�
            isEnableSecuriy = True
        End If

        '�t�H�[���R���g���[���̒l��ۑ�
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls, isEnableSecuriy)

        M_FILE.WriteApLog("�v���O�������~���܂���(�v���Z�XID:" & Process.GetCurrentProcess().Id() & ")")
    End Sub

    Private Sub TXT_MAX_POOL_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub TXT_MIN_POOL_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub BTN_EXPLORER_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXPLORER.Click
        '�R�l�N�V�����擾
        If GetConnection() Then
            Me.Visible = False
            If frm_Explorer.Visible = False Then
                '���[�_���ŃG�N�X�v���[����ʕ\��
                frm_Explorer.ShowDialog()
            End If
        End If
    End Sub

    Private Sub BTN_EXECSQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXECSQL.Click
        '�R�l�N�V�����擾
        If GetConnection() Then
            Me.Visible = False
            'SQL���s��ʕ\��
            frm_Execsql.ShowDialog()
        End If
    End Sub

    Private Sub BTN_CONNFIG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CONNFIG.Click
        frm_Config.ShowDialog()
    End Sub

    Private Sub CMB_MIDDLE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMB_MIDDLE.SelectedIndexChanged
        If CMB_MIDDLE.SelectedIndex = M_CONST.C_MIDDLE_TYPE.ODP Then
            CHK_SYSDBA.Enabled = True
            TXT_OPTION.Enabled = True
        ElseIf CMB_MIDDLE.SelectedIndex = M_CONST.C_MIDDLE_TYPE.OO4O Then
            CHK_SYSDBA.Checked = False
            CHK_SYSDBA.Enabled = False
            TXT_OPTION.Enabled = False
            TXT_OPTION.Text = vbNullString
        ElseIf CMB_MIDDLE.SelectedIndex = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            CHK_SYSDBA.Checked = False
            CHK_SYSDBA.Enabled = False
            TXT_OPTION.Enabled = True
        ElseIf CMB_MIDDLE.SelectedIndex = M_CONST.C_MIDDLE_TYPE.ODBC Then
            LBL_SID.Text = "DSN��"
            CHK_SYSDBA.Checked = False
            CHK_SYSDBA.Enabled = False
            TXT_OPTION.Enabled = True
        End If

    End Sub

    Public Sub New()
        ' ���̌Ăяo���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()
        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub BTN_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub BTN_REGIST_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_REGIST.Click
        RegistConnListFromForm(LB_CONN_LIST.SelectedIndex)
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0037")
    End Sub

    Private Sub LB_CONN_LIST_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LB_CONN_LIST.SelectedIndexChanged

        If CONN_LIST Is Nothing Then
            '�ڑ��惊�X�g���Ȃ��ꍇ
            TXT_CONN_LIST.Text = vbNullString
            TXT_USERID.Text = vbNullString
            TXT_PASS.Text = vbNullString
            TXT_CONN_STR.Text = vbNullString
            TXT_OPTION.Text = vbNullString
            If CMB_MIDDLE.Enabled Then
                CMB_MIDDLE.SelectedIndex = 0
            End If
            CHK_SYSDBA.Checked = False
            CHK_SAVE_PASS.Checked = False
            BTN_REMOVE.Enabled = False
            BTN_UP.Enabled = False
            BTN_DOWN.Enabled = False
            Exit Sub
        End If

        If LB_CONN_LIST.SelectedIndex = CONN_LIST.Length Then
            '��ԉ�(�V�K���X�g)
            TXT_CONN_LIST.Text = vbNullString
            TXT_USERID.Text = vbNullString
            TXT_PASS.Text = vbNullString
            TXT_CONN_STR.Text = vbNullString
            TXT_OPTION.Text = vbNullString
            If CMB_MIDDLE.Enabled Then
                CMB_MIDDLE.SelectedIndex = 0
            End If
            CHK_SYSDBA.Checked = False
            CHK_SAVE_PASS.Checked = False
            BTN_REMOVE.Enabled = False
            BTN_UP.Enabled = False
            BTN_DOWN.Enabled = False
        ElseIf LB_CONN_LIST.SelectedIndex = 0 Then
            '��ԏ�
            TXT_CONN_LIST.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connName
            TXT_USERID.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).userName
            TXT_PASS.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).password
            TXT_CONN_STR.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connString
            TXT_OPTION.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).options
            If CMB_MIDDLE.Enabled Then
                CMB_MIDDLE.SelectedIndex = CONN_LIST(LB_CONN_LIST.SelectedIndex).middle
            End If
            CHK_SYSDBA.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSysdba
            CHK_SAVE_PASS.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSavePass
            BTN_REMOVE.Enabled = True
            BTN_UP.Enabled = False
            If LB_CONN_LIST.Items.Count = 2 Then
                '�ݒ�ς݃��X�g��1�s�̏ꍇ
                BTN_DOWN.Enabled = False
            Else
                BTN_DOWN.Enabled = True
            End If

        ElseIf LB_CONN_LIST.SelectedIndex = CONN_LIST.Length - 1 Then
            '������2�Ԗ�
            TXT_CONN_LIST.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connName
            TXT_USERID.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).userName
            TXT_PASS.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).password
            TXT_CONN_STR.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connString
            TXT_OPTION.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).options
            If CMB_MIDDLE.Enabled Then
                CMB_MIDDLE.SelectedIndex = CONN_LIST(LB_CONN_LIST.SelectedIndex).middle
            End If
            CHK_SYSDBA.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSysdba
            CHK_SAVE_PASS.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSavePass
            BTN_REMOVE.Enabled = True
            BTN_UP.Enabled = True
            BTN_DOWN.Enabled = False
        Else
            TXT_CONN_LIST.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connName
            TXT_USERID.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).userName
            TXT_PASS.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).password
            TXT_CONN_STR.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).connString
            TXT_OPTION.Text = CONN_LIST(LB_CONN_LIST.SelectedIndex).options
            If CMB_MIDDLE.Enabled Then
                CMB_MIDDLE.SelectedIndex = CONN_LIST(LB_CONN_LIST.SelectedIndex).middle
            End If
            CHK_SYSDBA.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSysdba
            CHK_SAVE_PASS.Checked = CONN_LIST(LB_CONN_LIST.SelectedIndex).isSavePass
            BTN_REMOVE.Enabled = True
            BTN_UP.Enabled = True
            BTN_DOWN.Enabled = True
        End If
    End Sub

    Private Sub BTN_REMOVE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_REMOVE.Click
        RemoveConnList()
    End Sub

    Private Sub BTN_UP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_UP.Click
        Dim tmpConnList As S_CONN_LIST
        Dim selectedIndex = LB_CONN_LIST.SelectedIndex

        tmpConnList = CONN_LIST(LB_CONN_LIST.SelectedIndex - 1)
        CONN_LIST(LB_CONN_LIST.SelectedIndex - 1) = CONN_LIST(LB_CONN_LIST.SelectedIndex)
        CONN_LIST(LB_CONN_LIST.SelectedIndex) = tmpConnList
        'LB_CONN_LIST.SelectedIndex = LB_CONN_LIST.SelectedIndex - 1
        RegistConnListFromValiable(LB_CONN_LIST.SelectedIndex - 1)

        'CONN_LIST(LB_CONN_LIST.SelectedIndex) = tmpConnList
        'LB_CONN_LIST.SelectedIndex = LB_CONN_LIST.SelectedIndex + 1
        RegistConnListFromValiable(LB_CONN_LIST.SelectedIndex)
        LoadConnList()
        LB_CONN_LIST.SelectedIndex = selectedIndex - 1
    End Sub

    Private Sub BTN_DOWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_DOWN.Click
        Dim tmpConnList As S_CONN_LIST
        Dim selectedIndex = LB_CONN_LIST.SelectedIndex

        tmpConnList = CONN_LIST(LB_CONN_LIST.SelectedIndex + 1)
        CONN_LIST(LB_CONN_LIST.SelectedIndex + 1) = CONN_LIST(LB_CONN_LIST.SelectedIndex)
        CONN_LIST(LB_CONN_LIST.SelectedIndex) = tmpConnList
        'LB_CONN_LIST.SelectedIndex = LB_CONN_LIST.SelectedIndex + 1
        RegistConnListFromValiable(LB_CONN_LIST.SelectedIndex + 1)

        'CONN_LIST(LB_CONN_LIST.SelectedIndex - 1) = tmpConnList
        'LB_CONN_LIST.SelectedIndex = LB_CONN_LIST.SelectedIndex - 1
        RegistConnListFromValiable(LB_CONN_LIST.SelectedIndex)
        LoadConnList()
        LB_CONN_LIST.SelectedIndex = selectedIndex + 1

    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Process.Start("mailto:oanalyzer@gmail.com?cc=&bcc=&subject=���d���˗�&body=���Č��̈˗���p�ł��B����Ȃǂɂ͑Ή����Ă��܂���")
    End Sub

    Private Sub LB_CONN_LIST_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LB_CONN_LIST.MouseDoubleClick
        '�R�l�N�V�����擾
        If GetConnection() Then
            Me.Visible = False
            If frm_Explorer.Visible = False Then
                '���[�_���ŃG�N�X�v���[����ʕ\��
                frm_Explorer.ShowDialog()
            End If
        End If
    End Sub
End Class