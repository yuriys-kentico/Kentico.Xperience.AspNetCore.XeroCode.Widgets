<%@ Page 
  Language="C#" 
  AutoEventWireup="true" 
  Inherits="Widgets"
  Theme="Default" 
  EnableEventValidation="false" 
  MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
  Codebehind="Widgets.aspx.cs" %>

<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
  <iframe ID="widgetsIframe" runat="server" style="width:100%;height:100%;position:absolute;top:0;left:0;" frameborder="0" Visible="true"></iframe>
</asp:Content>