using DevExpress.Web;
using DevExpress.Web.Mvc;
using DocumentFormat.OpenXml.Office.ActiveX;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Tools_WebApp.Commands;
using Tools_WebApp.Models;
using Tools_WebApp.Queries;
using ZXing.QrCode;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using System.Configuration;



namespace Tools_WebApp.Controllers
{

    public class ToolsController : Controller
    {
        private readonly IQuery<Tool> _query;
        private readonly ICommandPost _command;
        // find relative path
        readonly string PATH = System.Web.HttpContext.Current.Server.MapPath("~/Report/");
        //string PATH = @"C:\Users\dpiccinni\source\repos\Formazione_03-master\Formazione_03-master\Tools_WebApp\Report\";
        string fileExtension = ".xlsx";
        string templateName = "Template"; // Template file name and Template sheet name are the same
        string reportString = "Report";
        string partialNameSheetReport = "Report Tool ";
 
        int SIZE_QR_CORE = 150;

        Tools_WebApp.Models.MyDBContext db = new Tools_WebApp.Models.MyDBContext();

        public ToolsController(IQuery<Tool> query, ICommandPost command)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _query = query;
            _command = command;
        }

        public ActionResult Get()
        {
            return PartialView("_Tools", _query.ReadAll());
        }

        [System.Web.Mvc.Route("Tools/{id}/Details")]
        public ActionResult GetById(string id)
        {
            return View("ToolDetails", _query.ReadById(id));
        }

        public ActionResult GetByPartialId(string id)
        {
            // need to return a partial view 
            return PartialView("_Tools", _query.ReadByPartialId(id));
        }

        public bool Delete(string id)
        {
            DeleteToolCommand command = new DeleteToolCommand(id);

            _command.Publish(command);

            return true;
        }

        public ActionResult Post([FromBody] Tool model)
        {
            if (ModelState.IsValid)
            {
                // osserva come dopo il publish il controllo passa a UpdateToolQuantityCommandHandler senza bisogno di chiamarlo direttamente

                // anche _commandBus viene istanziato da Autofac
                CreateToolCommand command = new CreateToolCommand(model);
                _command.Publish(command);

                return PartialView("_AlertSuccess");
            }
            else
            {
                return PartialView("_AlertDanger");
            }

        }

        [System.Web.Mvc.Route("api/Tools/{id}/ChangeQuantity")]
        public bool PostToolQuantity([FromBody] Tool model)
        {
            UpdateToolQuantityCommand command = new UpdateToolQuantityCommand(model.IdTool, model.Quantity);

            // osserva come dopo il publish il controllo passa a UpdateToolQuantityCommandHandler senza bisogno di chiamarlo direttamente

            // anche _commandBus viene istanziato da Autofac

            _command.Publish(command);

            return true;
        }
        
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult TestViewTest(string id)
        {
            return PartialView("TestViewTest");
        }


        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = db.Tools;
            return PartialView("_GridViewPartial", _query.ReadAll());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Tools_WebApp.Models.Tool item)
        {
            var model = db.Tools;
            if (ModelState.IsValid)
            {                             
                CreateToolCommand command = new CreateToolCommand(item);
                _command.Publish(command);            
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", _query.ReadAll());
        }

        [System.Web.Http.HttpGet, ValidateInput(false)]
        [System.Web.Http.Route("/Tools/GridViewPartialReportPreview/")]
        public ActionResult GridViewPartialReportPreview(string id)
        {
            Tool item = _query.ReadById(id);
            Byte[] qrCore = CreateQrCode(item.BoschCode, SIZE_QR_CORE);
            return PartialView("ToolReport", item);
        }



        [System.Web.Http.HttpGet, ValidateInput(false)]
        [System.Web.Http.Route("/Tools/GridViewPartialReportDownload/")]
        public FileResult GridViewPartialReportDownload(string id)
        {
            Tool item = _query.ReadById(id);
            Byte[] qrCore = CreateQrCode(item.BoschCode, SIZE_QR_CORE);
            return CreateReport(item, qrCore);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Tools_WebApp.Models.Tool item)
        {
            if (ModelState.IsValid)
            {
                CreateToolCommand command = new CreateToolCommand(item);
                _command.Publish(command);
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", _query.ReadAll());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] Tools_WebApp.Models.Tool item)
        {
            var model = db.Tools;
            if (item.IdTool != null)
            {
                DeleteToolCommand command = new DeleteToolCommand(item.IdTool);
                _command.Publish(command);
            }
            return PartialView("_GridViewPartial", _query.ReadAll());
        }

        Tools_WebApp.Models.MyDBContext db1 = new Tools_WebApp.Models.MyDBContext();

        [ValidateInput(false)]
        public ActionResult DataViewPartial()
        {
            var model = db1.Tools;
            return PartialView("_DataViewPartial", _query.ReadAll());
        }




        public Byte[] CreateQrCode(string qrText, int sizeQrCode = 150)
        {
            Byte[] byteArray;

            string fileGuid = Guid.NewGuid().ToString().Substring(0, 4);

            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = sizeQrCode,
                    Width = sizeQrCode
                }
            };
            var pixelData = qrCodeWriter.Write(qrText);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    // save to folder
                    // bitmap.Save(PATH + "/file-" + fileGuid + ".png", System.Drawing.Imaging.ImageFormat.Png);

                    // save to stream as PNG
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = ms.ToArray();
                }
            }
            return byteArray;

        }

        [System.Web.Mvc.HttpGet, ValidateInput(false)]
        [System.Web.Mvc.Route("Tools/GetQrCode/{qrText}")]
        public string GetQrCode(string qrText, int sizeQrCode = 150)
        {
            Byte[] byteArray;
            string nameBitmap;
            string fileGuid = Guid.NewGuid().ToString().Substring(0, 4);
            string conv;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = sizeQrCode,
                    Width = sizeQrCode
                }
            };
            var pixelData = qrCodeWriter.Write(qrText);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    // save to folder
                    nameBitmap = PATH + "/file-" + fileGuid + ".png";
                    bitmap.Save(nameBitmap, System.Drawing.Imaging.ImageFormat.Png);

                    // save to stream as PNG
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = ms.ToArray();
                }
            }
            string FileVirtualPath = Path.Combine(PATH, nameBitmap);
            //return File(FileVirtualPath, "application/force-download", nameBitmap);
            //return byteArray;
            return conv = Convert.ToBase64String(byteArray);

        }

        private void AddImageToExcel(IWorkbook workbook, ISheet sheet, byte[] data)
        {
            int D_1 = 0;
            int D_2 = 1023;
            int Col1 = 1;
            int Row1 = 8;

            int pictureIndex = workbook.AddPicture(data, PictureType.PNG);
            ICreationHelper helper = workbook.GetCreationHelper();
           
            IDrawing drawing = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor = helper.CreateClientAnchor();

            anchor.Col1 = Col1;
            anchor.Row1 = Row1;
            anchor.Col2 = Col1 + 1;
            anchor.Row2 = Row1 + 5;
            anchor.Dx1 = D_1;
            anchor.Dy1 = D_1;
            anchor.Dx2 = D_2;
            anchor.Dy2 = D_2;

            IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
        }

        private void PopulateSheet(ISheet sheet, Tool data)
        {
            int indexCellColumn = 1;

            sheet.GetRow(3).CreateCell(indexCellColumn).SetCellValue(data.BoschCode);
            sheet.GetRow(4).CreateCell(indexCellColumn).SetCellValue(data.Description);
            sheet.GetRow(5).CreateCell(indexCellColumn).SetCellValue(data.PrimarySupplier);
            sheet.GetRow(6).CreateCell(indexCellColumn).SetCellValue(data.SecondarySupplier);
            sheet.GetRow(7).CreateCell(indexCellColumn).SetCellValue(data.Quantity.ToString());
        }

        private FileResult CreateReport(Tool data, Byte[] qrCode)
        {
            IWorkbook workbook;

            string nameFileReport = reportString + Guid.NewGuid().ToString().Substring(0, 4) + fileExtension;


            // Loading Template.xlsx and save the workbook
            using (FileStream sw = System.IO.File.Open(PATH + templateName + fileExtension, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(sw);
                sw.Close();
            }

            // Loading sheet, change sheet's name and poputate its
            ISheet sheet = workbook.GetSheet(templateName);
            workbook.SetSheetName(0, partialNameSheetReport + data.IdTool);

            // Find download folder and create path with nameFileReport
            string FileVirtualPath = Path.Combine(PATH, nameFileReport);

            using (FileStream sw = System.IO.File.Open(FileVirtualPath, FileMode.Create, FileAccess.Write))
            {
                string nameSheetReport = partialNameSheetReport + data.IdTool;

                ISheet sheetReport = workbook.GetSheet(nameSheetReport);
                if (sheetReport is null) workbook.CreateSheet(nameSheetReport);

                PopulateSheet(sheetReport, data);
                AddImageToExcel(workbook, sheetReport, qrCode);
                workbook.Write(sw);
                sw.Close();
            }
            return File(FileVirtualPath, "application/force-download", nameFileReport); ;
        }

    }
}