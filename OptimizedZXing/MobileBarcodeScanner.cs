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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RootFrame.Navigate(typeof(ScanPage), new ScanPageNavigationParameters
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
            var tcsScanResult = new TaskCompletionSource<Result>();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RootFrame.Navigate(typeof(ScanPage), new ScanPageNavigationParameters
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

            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                if (RootFrame.CanGoBack)
                    RootFrame.GoBack();
            });

            return result;
        }
    }
}
