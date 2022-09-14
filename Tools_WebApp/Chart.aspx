<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Tools_WebApp.Chart" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.2.Web, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraCharts" Assembly="DevExpress.XtraCharts.v18.2, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>

<dx:WebChartControl ID="WebChartControl1" runat="server" Width="300px" Height="200px" CrosshairEnabled="True">
    <DiagramSerializable>
        <cc1:XYDiagram>
            <axisx visibleinpanesserializable="-1"></axisx>

            <axisy visibleinpanesserializable="-1"></axisy>
        </cc1:XYDiagram>
    </DiagramSerializable>

    <SeriesSerializable>
        <cc1:Series Name="Series 1"></cc1:Series>
    </SeriesSerializable>
</dx:WebChartControl>
