using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace OptimizedZXing
{
    public class MobileBarcodeScanner
    {
        internal ScanPage ScanPage { get; set; }
        readonly CoreDispatcher Dispatcher;
        readonly Frame RootFrame;

        public MobileBarcodeScanner(CoreDispatcher dispatcher, Frame rootFrame)
        {
            Dispatcher = dispatcher;
            RootFrame = rootFrame;
        }

        public string TopText { get; set; }
        public string BottomText { get; set; }

        public async void ScanContinuously(MobileBarcodeScanningOptions options, Action<Result> scanHandler)
        {
            //Navigate: /ZxingSharp.WindowsPhone;component/Scan.xaml
            var rootFrame = RootFrame;
            var dispatcher = Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rootFrame.Navigate(typeof(ScanPage), new ScanPageNavigationParameters
                {
                    Options = options,
                    ResultHandler = scanHandler,
                    Scanner = this,
                    ContinuousScanning = true
                });
            });
        }

        public async Task<Result> Scan(MobileBarcodeScanningOptions options)
        {
            var rootFrame = RootFrame;
            var dispatcher = Dispatcher;

            var tcsScanResult = new TaskCompletionSource<Result>();

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rootFrame.Navigate(typeof(ScanPage), new ScanPageNavigationParameters
                {
                    Options = options,
                    ResultHandler = r =>
                    {
                        tcsScanResult.SetResult(r);
                    },
                    Scanner = this,
                    ContinuousScanning = false
                });
            });

            var result = await tcsScanResult.Task;

            await dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                if (rootFrame.CanGoBack)
                    rootFrame.GoBack();
            });

            return result;
        }
    }
}
