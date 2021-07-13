<%@ Page Title="" Language="VB" MasterPageFile="~/Master/CommonMasterPage.Master" AutoEventWireup="false" CodeFile="SC3100302Stub.aspx.vb" Inherits="Pages_SC3100302Stub" %>

<%@ Register src="SC3100302.ascx" tagname="SC3100302" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div style="float:left;">
<uc1:SC3100302 ID="SC31003021" runat="server" />
</div>
<div id="updateVisitActual">来店実績一覧更新</div>
<script type="text/javascript">
	$(function () {
		$("#updateVisitActual").click(function () {
			SC3100302Script.update();
		});
	}

	)

</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

