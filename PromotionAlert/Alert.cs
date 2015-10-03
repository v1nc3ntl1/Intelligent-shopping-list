using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PromotionAlert
{
  using System.Collections;
  using System.Collections.ObjectModel;
  using System.Configuration;
  using System.Diagnostics;
  using BusinessLogic;
  using DomainObject;
  using Framework;
  using PromotionAlert.ViewModel;

  public partial class Alert : Form
  {
    private int PollInterval = 1;
    private static int linkColumnIndex = 0;
    private const int MiliSecondsPerMinute = 60000;

    public Alert()
    {
      InitializeComponent();

      var poolSetting = ConfigurationManager.AppSettings["PollInterval"];
      listBox1.Items.Add(string.Format("PollInterval is set at {0}", poolSetting));
      if (!int.TryParse(poolSetting, out PollInterval))
      {
        PollInterval = 60;
      }
      timer1.Interval = PollInterval * MiliSecondsPerMinute;
      SetLayout();
      this.dataGridView1.CellContentClick += DataGridView1_CellContentClick;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      listBox1.Items.Add(string.Format("Start of screen refresh : {0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
      this.dataGridView1.DataSource = null;
      ShowAlert();
      listBox1.Items.Add(string.Format("End of screen refresh : {0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
    }

    private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == linkColumnIndex)
      {
        ProcessStartInfo sInfo = new ProcessStartInfo((string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        Process.Start(sInfo);
      }
    }

    protected override void OnResize(EventArgs e)
    {
      notifyIcon1.BalloonTipTitle = "Minimize to Tray App";
      notifyIcon1.BalloonTipText = "You have successfully minimized your form.";
      listBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
      dataGridView1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;

      if (FormWindowState.Minimized == this.WindowState)
      {
        notifyIcon1.Visible = true;
        notifyIcon1.ShowBalloonTip(500);
        this.Hide();
      }
      else if (FormWindowState.Normal == this.WindowState)
      {
        notifyIcon1.Visible = false;
      }
      SetLayout();
    }

    private void SetLayout()
    {
      int marginLeft, marginRight, marginTop, marginBottom;
      marginLeft = marginRight = marginTop = marginBottom = 10;
      
      if (FormWindowState.Normal == this.WindowState || this.WindowState == FormWindowState.Maximized)
      {
        dataGridView1.Size = new Size(this.Width - marginLeft - marginRight , (this.Size.Height * 2 /3) - marginTop - marginBottom);
        listBox1.Size = new Size(this.Width - marginLeft - marginRight, (this.Size.Height / 3) - marginTop - marginBottom - marginBottom - button1.Size.Height);
        dataGridView1.Location = new Point(marginLeft, marginTop);
        listBox1.Location = new Point(marginLeft, dataGridView1.Height + marginTop + marginBottom + button1.Height);
        button1.Location = new Point(marginLeft, dataGridView1.Height + marginTop + marginBottom);
      }
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.Show();
      this.dataGridView1.Size = new Size(600,600);
      if (this.WindowState == FormWindowState.Minimized)
        this.WindowState = FormWindowState.Normal;

      // Activate the form.
      this.Activate();
    }

    async private void ShowAlert()
    {
      var extrator = SpringResolver.GetObject<IPromotionExtractor>("PromotionExtractorImpl");
      var promotions = await extrator.GetActivePromotion();

      IShoppingListItemExtractor shoppingListItemExtractor =
            SpringResolver.GetObject<IShoppingListItemExtractor>("ShoppingListItemExtractorImpl");
      var listItems = await shoppingListItemExtractor.GetActiveItem();

      Collection<ShoppingListPromotionItemViewModel> matchPromotions = new Collection<ShoppingListPromotionItemViewModel>();
      IEnumerable<Promotion> tempPromotions;
      foreach (var item in listItems)
      {
        tempPromotions = promotions.Where(p => p.PromotionItems != null && p.PromotionItems.Any(pi => string.Equals(pi.ItemName, item.ItemName, StringComparison.InvariantCultureIgnoreCase) ||
                                                                         string.Equals(pi.Tag, item.Tag, StringComparison.InvariantCultureIgnoreCase)));

        if (tempPromotions.GetEnumerator().MoveNext())
        {
          foreach (var tempPromotion in tempPromotions)
          {
            matchPromotions.Add(new ShoppingListPromotionItemViewModel()
            {
              PromotionName = tempPromotion.PromotionName,
              Description = tempPromotion.Description,
              EffectiveDateTime = tempPromotion.EffectiveDateTime,
              Brands = tempPromotion.Brands == null ? "" : tempPromotion.Brands.Aggregate((p, n) => string.Format("{0},{1}", p, n)),
              PromotionItems = tempPromotion.PromotionItems == null ? "" : tempPromotion.PromotionItems.Select(p => p.ItemName).Aggregate((p, n) => string.Format("{0},{1}", p, n)),
              PromotionTags = tempPromotion.PromotionItems == null ? "" : tempPromotion.PromotionItems.Select(p => p.Tag).Aggregate((p, n) => string.Format("{0},{1}", p, n)),
              ListTags = item.Tag,
              ListItems = item.ItemName ,
              Link = tempPromotion.Link,
              Html = tempPromotion.Html
            });
          }
        }
      }

      this.dataGridView1.Columns.Clear();
      this.dataGridView1.DataSource = matchPromotions;
      this.dataGridView1.Columns.Insert(linkColumnIndex, new DataGridViewLinkColumn()
      {
        HeaderText = "PromotionName",
        DataPropertyName = "Link",
        LinkBehavior = LinkBehavior.SystemDefault,
      });
    }

    private void button1_Click(object sender, EventArgs e)
    {
      ShowAlert();
    }
  }
}
