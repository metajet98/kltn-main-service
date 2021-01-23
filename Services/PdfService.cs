using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using main_service.Databases;
using main_service.Storage;

namespace main_service.Services
{
    public class PdfService
    {
        private readonly IConverter _converter;
        private readonly StorageManager _storageManager;

        public PdfService(IConverter converter, StorageManager storageManager)
        {
            _converter = converter;
            _storageManager = storageManager;
        }

        public Task<string> MaintenancePdf(Maintenance maintenance)
        {
            var sb = new StringBuilder();
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings {Top = 10},
                DocumentTitle = "Maintenance Report",
                DPI = 200
            };
            sb.Append($@"
                        <html>
                          <body class='container'>
                            <div class='header'><h1>PHIẾU BẢO DƯỠNG</h1></div>
                            <div class='intro'>
                              <h2>Công ty: HOÀ BÌNH MINH</h2>
                              <h2>Chi nhánh: {maintenance.Branch.Name}</h2>
                              <h3>Địa chỉ: {maintenance.Branch.Address}</h3>
                              <h3>Số điện thoại: 088.888.888</h3>
                            </div>
                            <table class='table-info'>
                              <tr>
                                <td>Mã khách hàng: {maintenance.UserVehicle.UserId}</td>
                                <td>Tên khách hàng: {maintenance.UserVehicle.User.FullName}</td>
                                <td>Số điện thoại: {maintenance.UserVehicle.User.PhoneNumber}</td>
                              </tr>
                              <tr>
                                <td>Mã xe: {maintenance.UserVehicleId}</td>
                                <td>Loại xe: {maintenance.UserVehicle.VehicleGroup.Name}</td>
                                <td>Biển số: {maintenance.UserVehicle.PlateNumber}</td>
                              </tr>
                              <tr>
                                <td>Số khung: {maintenance.UserVehicle.ChassisNumber}</td>
                                <td>Số máy: {maintenance.UserVehicle.EngineNumber}</td>
                                <td>Số Km: {maintenance.Odometer}</td>
                              </tr>
                              <tr>
                                <td>Thời gian nhận xe: {maintenance.CreatedDate}</td>
                                <td>Khách lấy lại phụ tùng: {maintenance.SparepartBack}</td>
                                <td>Rửa xe: {maintenance.MotorWash}</td>
                              </tr>
                            </table>

                            <h2>Dịch vụ</h2>

                            <table class='table-bill'>
                              <tr>
                                <th>STT</th>
                                <th>Tên phụ tùng</th>
                                <th>Mã phụ tùng</th>
                                <th>Giá phụ tùng</th>
                                <th>Giá nhân công</th>
                                <th>Số lượng</th>
                                <th>Thành tiền</th>
                              </tr>
                              {BuildBillRows(maintenance.MaintenanceBillDetail.ToList())}
                              <tfoot>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>Tổng tiền</td>
                                <td>{maintenance.MaintenanceBillDetail.Sum(x => x.TotalPrice)}</td>
                              </tfoot>
                            </table>
                            <p class='sign-staff'>
                              Nhân viên
                              <span class='sign-customer'>Khách hàng</span>
                            </p>
                          </body>
                        </html>
                      ");
            var html = sb.ToString();

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "pdfStyles.css")
                },
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = {objectSettings}
            };
            var bytes = _converter.Convert(pdf);

            return _storageManager.UploadMemorySteamToAwsS3(new MemoryStream(bytes));
        }

        public string BuildBillRows(List<MaintenanceBillDetail> bills)
        {
            StringBuilder result = new StringBuilder();
            var count = 1;

            bills.ForEach(bill =>
            {
                result.Append($@"
                              <tr>
                                <td>{count}</td>
                                <td>{bill.BranchServicePrice.MaintenanceService.Name}</td>
                                <td>{bill.BranchServicePrice.MaintenanceService.Id}</td>
                                <td>{bill.BranchServicePrice.SparePartPrice}</td>
                                <td>{bill.BranchServicePrice.LaborCost}</td>
                                <td>{bill.Quantity}</td>
                                <td>{bill.TotalPrice}</td>
                              </tr>
                          ");
                count++;
            });
            return result.ToString();
        }
    }
}