using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;

namespace WindowsService
{
  using System.Collections.ObjectModel;
  using BusinessLogic;
  using DomainObject;
  using Framework;
  using HigLabo.Rss;
  using Parser;

  public partial class PromotionCollector : ServiceBase
  {
    private int eventId;
    private int PollInterval = 1;
    private const int MiliSecondsPerMinute = 60000;
    public PromotionCollector(string[] args)
    {
      InitializeComponent();
      string eventSourceName = "PromotionCollectorSource";
      string logName = "PromotionCollectorNewLog";

      if (args.Any())
      {
        eventSourceName = args[0];
        
      } 
      if (args.Count() > 1)
      {
        logName = args[1];
      }

      eventLog1 = new System.Diagnostics.EventLog();
      if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
      {
        System.Diagnostics.EventLog.CreateEventSource(
          eventSourceName, logName);
      }
      eventLog1.Source = eventSourceName;
      eventLog1.Log = logName;
    }

    private void Init()
    {
      var poolSetting = ConfigurationManager.AppSettings["PollInterval"];
      eventLog1.WriteEntry(string.Format("PollInterval is set at {0}", poolSetting));
      if (!int.TryParse(poolSetting, out PollInterval))
      {
        PollInterval = 60;
      }
    }

    protected override void OnStart(string[] args)
    {
      Init();
      // Update the service state to Start Pending.
      ServiceStatus serviceStatus = new ServiceStatus();
      serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
      serviceStatus.dwWaitHint = 100000;
      SetServiceStatus(this.ServiceHandle, ref serviceStatus);

      eventLog1.WriteEntry("In OnStart");

      var timer = new System.Timers.Timer();
      timer.Interval = PollInterval * MiliSecondsPerMinute; 
      timer.Elapsed += this.OnTimer;
      timer.Start();

      // Update the service state to Running.
      serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
      SetServiceStatus(this.ServiceHandle, ref serviceStatus);
    }

    async private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
    {
      eventLog1.WriteEntry(string.Format("Start Pooling Rss Feed at {0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), EventLogEntryType.Information, eventId++);

      string[] allRssFeedUrl = null;
      try
      {
        allRssFeedUrl = ConfigurationManager.AppSettings["PromotionRSSSites"].Split('|');
      }
      catch (Exception ex)
      {
        eventLog1.WriteEntry("Error getting from settings PromotionRSSSites from appsettings.", EventLogEntryType.Error);
      }

      if (allRssFeedUrl.Length > 0)
      {
        Collection<Promotion> promotions = null;

        try
        {
          var extrator = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
          promotions = await extrator.GetActivePromotion();
        }
        catch (Exception ex)
        {
          eventLog1.WriteEntry(string.Format("Error getting active promotion from database \n {0}", ex.Message), EventLogEntryType.Error);
        }

        var creator = SpringResolver.GetObject<IPromotionCreator>("PromotionCreatorImpl");

        foreach (var feed in allRssFeedUrl)
        {
          RssFeed rssFeed;

          try
          {
            rssFeed = External.HigLabFascade.GetRss(feed.Trim());
          }
          catch (Exception ex)
          {
            eventLog1.WriteEntry(string.Format("Error getting feed from url {0} \n {1}", feed, ex.Message), EventLogEntryType.Error);
            continue;
          }

          if (rssFeed != null)
          {
            var parser = new SNSRssParser<Promotion, RssFeed>();

            IEnumerable<Promotion> result = null;
            try
            {
              result = parser.Parse(rssFeed);
            }
            catch (Exception ex)
            {
              eventLog1.WriteEntry(string.Format("Error Parsing from url {0} \n {1}", feed, ex.Message), EventLogEntryType.Error);
              continue;
            }

            if (result != null)
            {
              if (!promotions.IsNullOrEmpty())
              {
                result = result.Where(r => !promotions.Any(p => string.Equals(p.PromotionName, r.PromotionName, StringComparison.InvariantCultureIgnoreCase)));
              }

              if (result != null)
              {
                foreach (var promo in result)
                {
                  try
                  {
                    await creator.SavePromotion(promo);
                  }
                  catch (Exception ex)
                  {
                    eventLog1.WriteEntry(string.Format("Error Saving promotion {0} from url {1} \n {2}", promo.PromotionName, feed, ex.Message), EventLogEntryType.Error);
                  }
                }
              }
              else
              {
                eventLog1.WriteEntry(string.Format("All Promotion exists in the sysatem. No promotion will be save.", feed), EventLogEntryType.Warning);
              }
            }
            else
            {
              eventLog1.WriteEntry(string.Format("Parsing from {0} resulting in null", feed), EventLogEntryType.Warning);
            }
          }
          else
          {
            eventLog1.WriteEntry(string.Format("No promotions return from website : {0}", feed), EventLogEntryType.Warning);
          }
        }
      }
      eventLog1.WriteEntry(string.Format("End Pooling Rss Feed at {0:yyyy/MM/dd hh:mm:ss}", DateTime.Now), EventLogEntryType.Information, eventId++);
    }

    protected override void OnStop()
    {
      eventLog1.WriteEntry("In onStop.");
    }

    protected override void OnContinue()
    {
      eventLog1.WriteEntry("In OnContinue.");
    }

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

    public enum ServiceState
    {
      SERVICE_STOPPED = 0x00000001,
      SERVICE_START_PENDING = 0x00000002,
      SERVICE_STOP_PENDING = 0x00000003,
      SERVICE_RUNNING = 0x00000004,
      SERVICE_CONTINUE_PENDING = 0x00000005,
      SERVICE_PAUSE_PENDING = 0x00000006,
      SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
      public long dwServiceType;
      public ServiceState dwCurrentState;
      public long dwControlsAccepted;
      public long dwWin32ExitCode;
      public long dwServiceSpecificExitCode;
      public long dwCheckPoint;
      public long dwWaitHint;
    };
  }
}
