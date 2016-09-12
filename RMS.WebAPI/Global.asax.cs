using System.Threading;
using System.Timers;
using System.Web.Http;
using RMS.AppServiceLayer;
using RMS.AppServiceLayer.Zktime.Services;

namespace RMS.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private ZkTimeService _zkTimeService;
        private OvertimeHandler _overtimeHandler;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // wire up Unity
            UnityConfig.RegisterComponents();

            // wire up AutoMapper
            AutoMapperWebApiConfig.RegisterMappings();
            AutoMapperAslConfig.RegisterMappings();


            // instantiate the service
            _zkTimeService = new ZkTimeService();
            _overtimeHandler = new OvertimeHandler();

            // start the zktime service
            _zkTimeService.Init();

            // kick off timer to repeat above
            // once first run has completed
            //StartDataPollTimer();
            var dataPollThreadJob = new ThreadStart(StartDataPollTimer);
            var dataPollThread = new Thread(dataPollThreadJob);
            dataPollThread.Start();

            // kick off overtime timer
            //StartOvertimePollTimer();
            var overtimePollThreadJob = new ThreadStart(StartOvertimePollTimer);
            var overtimePollThread = new Thread(overtimePollThreadJob);
            overtimePollThread.Start();
        }

        protected void StartDataPollTimer()
        {
            const int interval = 60000; // 1 minutes

            var dpTimer = new System.Timers.Timer();
            dpTimer.Elapsed += DataPollTimerEvent;
            dpTimer.Interval = interval;
            dpTimer.Enabled = true;
        }

        protected void StartOvertimePollTimer()
        {
            const int interval = 300000;  // 5 minutes

            var otTimer = new System.Timers.Timer();
            otTimer.Elapsed += OvertimePollTimerEvent;  
            otTimer.Interval = interval;
            otTimer.Enabled = true;
        }

        protected void OvertimePollTimerEvent(object source, ElapsedEventArgs e)
        {
            _overtimeHandler.Init();
        }

        protected void DataPollTimerEvent(object source, ElapsedEventArgs e)
        {
            _zkTimeService.Init();
        }
    }
}
