using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OptimizedZXing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScanPage : Page
    {
        ScanPageNavigationParameters Parameters { get; set; }

        public ScanPage()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // If no parameters were passed, we navigated here for some other reason
            // so let's ignore it
            if (e.Parameter == null)
                return;

            Parameters = e.Parameter as ScanPageNavigationParameters;

            if (Parameters != null)
                Parameters.Scanner.ScanPage = this;
            
            scannerControl.TopText = Parameters?.Scanner?.TopText ?? "";
            scannerControl.BottomText = Parameters?.Scanner?.BottomText ?? "";

            scannerControl.ScanningOptions = Parameters?.Options ?? new MobileBarcodeScanningOptions (BarcodeFormat.QR_CODE);
            scannerControl.ContinuousScanning = Parameters?.ContinuousScanning ?? false;

            scannerControl.StartScanning(Parameters?.ResultHandler, Parameters?.Options);
        }
        
        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try { await scannerControl.StopScanningAsync(); }
            catch { }

            base.OnNavigatingFrom(e);
        }

        #region IMobileBarcodeScanner Implementation

        public string TopText
        {
            get { return scannerControl.TopText; }
            set { scannerControl.TopText = value; }
        }

        public string BottomText
        {
            get { return scannerControl.BottomText; }
            set { scannerControl.BottomText = value; }
        }

        public string CancelButtonText
        {
            get { return ""; }
            set { }
        }

        public string FlashButtonText
        {
            get { return ""; }
            set { }
        }

        public string CameraUnsupportedMessage
        {
            get { return ""; }
            set { }
        }

        public bool IsTorchOn
        {
            get { return scannerControl.IsTorchOn; }
        }

        public Task<Result> Scan(MobileBarcodeScanningOptions options)
        {
            var tcsResult = new TaskCompletionSource<Result>();

            scannerControl.ContinuousScanning = false;
            scannerControl.StartScanning(r =>
            {
                scannerControl.StopScanning();

                tcsResult.SetResult(r);
            }, options ?? Parameters?.Options);

            return tcsResult.Task;
        }

        public Task<Result> Scan()
        {
            return Scan(new MobileBarcodeScanningOptions(BarcodeFormat.QR_CODE));
        }

        public void ScanContinuously(MobileBarcodeScanningOptions options, Action<Result> scanHandler)
        {
            scannerControl.ContinuousScanning = true;
            scannerControl.StartScanning(scanHandler, options ?? Parameters?.Options);
        }

        public void ScanContinuously(Action<Result> scanHandler)
        {
            ScanContinuously(new MobileBarcodeScanningOptions(BarcodeFormat.QR_CODE), scanHandler);
        }

        public void Cancel()
        {
            scannerControl?.Cancel();
        }

        public void Torch(bool on)
        {
            scannerControl?.Torch(on);
        }

        public void AutoFocus()
        {
            scannerControl?.AutoFocus();
        }

        public void ToggleTorch()
        {
            scannerControl?.ToggleTorch();
        }

        public void PauseAnalysis()
        {
            scannerControl?.PauseAnalysis();
        }

        public void ResumeAnalysis()
        {
            scannerControl?.ResumeAnalysis();
        }

        #endregion
    }

    public class ScanPageNavigationParameters
    {
        public MobileBarcodeScanner Scanner { get; set; }
        public bool ContinuousScanning { get; set; }
        public MobileBarcodeScanningOptions Options { get; set; }
        public Action<Result> ResultHandler { get; set; }
    }
}
