


    function Qr_Code_Column(nameButton, stringId) {
        $.ajax({
            url: "/Tools/GetQrCode/" + stringId,
            type: "GET",
            success: function (result) {
                $("#" + nameButton + stringId).attr("src", 'data:image/png;base64,' + result);
                $("#" + nameButton + stringId).attr("height", 50);
                $("#" + nameButton + stringId).attr("width", 50);

            }
        });
    }

    function OnPopupShow(s, e) {
        s.SetWidth($(window).width() * 0.4);
        s.SetHeight($(window).height() * 0.5);
        s.UpdatePosition();
    }

    function OnGetRowValue(s, e) {

        console.log("buttonId in TestGetRowValue " + e.buttonID);
        GridViewTools.GetRowValues(e.visibleIndex, 'IdTool', ReportPreview_Button);
        console.log("buttonId in TestGetRowValue " + e.buttonID);

    }


    function ReportPreview_Button(value) {
        var stringId = value;

        console.log("ReportPreview_Button" + stringId);

        $("#report_int").attr("align", "");
        //var stringId = GridViewTools.GetRowKey(e.visibleIndex);
        //console.log("Gridview       in ReportPreview " + GridView.toString());
        //console.log("e.visibleIndex in ReportPreview " + e.visibleIndex);
        //console.log("stringId       in ReportPreview " + stringId);

        if (stringId == null) {
            console.log("string null");
        }
        else {
            $.ajax({
                url: "/Tools/GridViewPartialReportPreview/" + stringId,
                type: "GET",
                success: function (result) {
                    $("#report_int").html(result);
                    Report_PopupControl.Show();
                }
            });
        }
    }

    function QrCore_Click(stringId) {
        //var stringId = GridView.GetRowKey(e.visibleIndex);

        var img = document.createElement("img");
        $("#report_int").attr("align", "center");

        $.ajax({
            url: "/Tools/GetQrCode/" + stringId,
            type: "GET",
            async: false,
            success: function (result) {

                img.setAttribute("src", 'data:image/png;base64,' + result);
                img.height = 300;
                img.width = 300;

                $("#report_int").html(img);

                Report_PopupControl.Show();
            }
        });
    }


