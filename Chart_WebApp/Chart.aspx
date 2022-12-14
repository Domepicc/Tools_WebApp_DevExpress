<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Chart_WebApp.Chart" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v18.2, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v18.2, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v18.2.Web, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraCharts" Assembly="DevExpress.XtraCharts.v18.2, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftPanelContent" runat="server">
        <dx:ASPxNavBar ID="ASPxNavBar1" runat="server" Width="100%" ClientInstanceName="navbar">
            <ClientSideEvents ItemClick="function(s, e)
            {
                var options = chart.GetPrintOptions();
                options.SetLandscape(false);
                options.SetSizeMode('Stretch')
                if (e.item.name === 'nbPrint')
                {
                    chart.Print();
                }
                else if (e.item.name === 'nbExport')
                {
                    chart.SaveToWindow('pdf');
                }
            }"></ClientSideEvents>
            <Groups>
                <dx:NavBarGroup Text="Output" ItemImagePosition="Top">
                    <Items>
                        <dx:NavBarItem Name="nbPrint" Text="Print">
                            <Image IconID="outlookinspired_defaultprinter_svg_32x32"></Image>
                        </dx:NavBarItem>
                        <dx:NavBarItem Name="nbExport" Text="Export">
                            <Image IconID="pdfviewer_documentpdf_svg_32x32"></Image>
                        </dx:NavBarItem>
                    </Items>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </dx:NavBarGroup>
            </Groups>
        </dx:ASPxNavBar>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightPanelContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageContent" runat="server">



        <asp:sqldatasource runat="server" id="SqlDataSource1" connectionstring='<%$ ConnectionStrings:ToolsConnectionString2 %>' selectcommand="SELECT * FROM [Config].[Tools]"></asp:sqldatasource>
<asp:entitydatasource runat="server" id="EntityDataSource1"></asp:entitydatasource>

    <asp:sqldatasource runat="server" id="SqlDataSource" connectionstring='<%$ ConnectionStrings:ToolsConnectionString2 %>' selectcommand="SELECT * FROM [SAP].[CostoGiornalieroEncodedEquipment]"></asp:sqldatasource>
<asp:entitydatasource runat="server" id="EntityDataSource"></asp:entitydatasource>

      <dx:WebChartControl ID="WebChartControl1" runat="server" ClientInstanceName="chart" CrosshairEnabled="True" DataSourceID="SqlDataSource" Height="600px" Width="750px" SeriesDataMember="DailyProducedPcs">
    <EmptyChartText Text="Data"></EmptyChartText>
    <DiagramSerializable>
        <cc1:XYDiagram>
            <axisx labelvisibilitymode="AutoGeneratedAndCustom" visibility="True" visibleinpanesserializable="-1">
<CrosshairAxisLabelOptions Visibility="True"></CrosshairAxisLabelOptions>
</axisx>

            <axisy labelvisibilitymode="AutoGeneratedAndCustom" visibility="True" visibleinpanesserializable="-1"></axisy>
            <axisx visibility="True" visibleinpanesserializable="-1"></axisx>

            <axisy visibility="True" visibleinpanesserializable="-1"></axisy>
            <axisx visibleinpanesserializable="-1"></axisx>

            <axisy visibleinpanesserializable="-1"></axisy>
        </cc1:XYDiagram>
    </DiagramSerializable>

          <Legend Visibility="True" Name="Default Legend"></Legend>
          <SeriesSerializable>
              <cc1:Series Name="Series 1" ArgumentDataMember="CheckDate" ValueDataMembersSerializable="DailyProducedPcs" Visible="False"></cc1:Series>
    </SeriesSerializable>
    <SeriesTemplate SeriesDataMember="DailyProducedPcs" ArgumentDataMember="CheckDate" LabelsVisibility="True" ValueDataMembersSerializable="DailyProducedPcs">
        <viewserializable>
<cc1:SideBySideBarSeriesView ColorEach="True"></cc1:SideBySideBarSeriesView>
</viewserializable>
    </SeriesTemplate>
    <Titles>
        <cc1:ChartTitle Text="Produzione Giornaliera"></cc1:ChartTitle>
    </Titles>
</dx:WebChartControl>


<%--    <asp:SqlDataSource runat="server" ID="SqlDataSource" ConnectionString='<%$ ConnectionStrings:ToolsConnectionString %>' SelectCommand="SELECT * FROM [SAP].[CostoGiornalieroEncodedEquipment]"></asp:SqlDataSource>--%>

    <dx:WebChartControl ID="WebChartControl2" runat="server" CrosshairEnabled="True" DataSourceID="SqlDataSource" ClientInstanceName="chart2" Height="400px" Width="600px">
    <BorderOptions Visibility="True"></BorderOptions>
    <DiagramSerializable>
        <cc1:XYDiagram>
            <axisx visibleinpanesserializable="-1"></axisx>

            <axisy visibleinpanesserializable="-1"></axisy>
        </cc1:XYDiagram>
    </DiagramSerializable>

    <SeriesSerializable>
        <cc1:Series Name="Series 1" ArgumentDataMember="CheckDate" ValueDataMembersSerializable="DailyProducedPcs">
            <viewserializable>
<cc1:LineSeriesView></cc1:LineSeriesView>
</viewserializable>
        </cc1:Series>
    </SeriesSerializable>
    <SeriesTemplate LabelsVisibility="True">
        <viewserializable>
<cc1:SideBySideBarSeriesView ColorEach="True"></cc1:SideBySideBarSeriesView>
</viewserializable>
    </SeriesTemplate>
</dx:WebChartControl>


    <dx:WebChartControl ID="WebChartControl3" runat="server" DataSourceID="SqlDataSource1" Width="600px" Height="400px" CrosshairEnabled="True">

        <Legend Visibility="False" Name="Default Legend"></Legend>
        <SeriesSerializable>
            <cc1:Series Name="Series 1" ArgumentDataMember="IdTool" ValueDataMembersSerializable="Quantity">
                <viewserializable>
<cc1:PieSeriesView></cc1:PieSeriesView>
</viewserializable>
            </cc1:Series>
        </SeriesSerializable>
    </dx:WebChartControl>
</asp:Content>
