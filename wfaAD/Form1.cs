using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Data.SqlClient;
using System.IO;


namespace wfaAD
{
    public partial class frmCheckAD : Form
    {
        int c = 0;
        DirectoryEntry adEntry = null;
        public frmCheckAD()
        {
            InitializeComponent();
            SetADInfoAndCredentials();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            this.c = 0;
            txtEmails.Text = "";
            SetADInfoAndCredentials();
            string email = "";
            
            List<ADUser> usuarios =getUsersAD();
            foreach (ADUser u in usuarios) {
                email = u.Email.Substring(0,u.Email.IndexOf('^')).ToLower();
                txtEmails.Text +=email +Environment.NewLine; ;
            }
            //this.checkSubscriptionsEvent();
            while (txtResults.Text.Length == 0) {
                this.checkSubscriptionsEvent();
                this.c++;
            }
        }
        private void checkSubscriptionsEvent(){
            string email = "";
            if (txtEmails.Lines.Length > 0 && c < txtEmails.Lines.Length)
            {
                for (int x = 0; x < txtEmails.Lines.Length; x++) {
                    email = txtEmails.Lines[x];
                    if (x == this.c) {
                        try {
                            checkForSubscriptions(email);
                            lblUser.Text = email;
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message.ToString());
                        }
                        
                    }
                    //SearchForMailInAD(email);
                    //MessageBox.Show(email);
                    //lblCount.Text = x.ToString() + " of " + txtEmails.Lines.Length.ToString();
                    Application.DoEvents();
                    //System.Threading.Thread.Sleep(1000);
                }
                
            }
            //MessageBox.Show("Búsqueda terminada"); 
            lblUser.Text += " Search has FINISHED for this user.";
        }
        private List<ADUser> getUsersAD()
        {
            List<ADUser> lstADUsers = new List<ADUser>();
            try
            {
                
				string DomainPath = "LDAP://OU=Disabled Accounts,OU=Disabled_Resource Accounts,DC=CONTOSO,DC=net";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath); 
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                //search.Filter = "(&(objectClass=user)(objectCategory=person))";
                //search.Filter = "(&(objectClass=user)((userAccountControl:1.2.840.113556.1.4.803:=2)))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("displayname");//first name
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];
                        if (result.Properties.Contains("samaccountname") && 
                                 result.Properties.Contains("mail") && 
                            result.Properties.Contains("displayname"))
                        {
                            ADUser objSurveyUsers = new ADUser();
                            objSurveyUsers.Email = (String)result.Properties["mail"][0] + 
                              "^" + (String)result.Properties["displayname"][0];
                            objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
                            objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
                            lstADUsers.Add(objSurveyUsers);
                        }
                    }
                }
                return lstADUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return lstADUsers;
            }

        }

        private void checkForSubscriptions(string email)
        {
            txtResults.Text = "";
            List<string> subscriptions = new List<string>();
			SqlConnection conn = new SqlConnection("Server=SSRSSQLServerDB;Database=ReportServer;Trusted_Connection=True;");
            string sql = @"WITH subscriptionXmL
              AS (
                   SELECT
                    SubscriptionID ,
                    OwnerID ,
                    Report_OID ,
                    Locale ,
                    InactiveFlags ,
                    ExtensionSettings ,
                    CONVERT(XML, ExtensionSettings) AS ExtensionSettingsXML ,
                    ModifiedByID ,
                    ModifiedDate ,
                    Description ,
                    LastStatus ,
                    EventType ,
                    MatchData ,
                    LastRunTime ,
                    Parameters ,
                    DeliveryExtension ,
                    Version
                   FROM
                    ReportServer.dbo.Subscriptions
                 ),
                     -- Get the settings as pairs
            SettingsCTE
              AS (
                   SELECT
                    SubscriptionID ,
                    ExtensionSettings ,
        -- include other fields if you need them.
                    ISNULL(Settings.value('(./*:Name/text())[1]', 'nvarchar(1024)'),
                           'Value') AS SettingName ,
                    Settings.value('(./*:Value/text())[1]', 'nvarchar(max)') AS SettingValue
                   FROM
                    subscriptionXmL
                    CROSS APPLY subscriptionXmL.ExtensionSettingsXML.nodes('//*:ParameterValue') Queries ( Settings )
                 )
        SELECT
            *
        FROM
            SettingsCTE
        WHERE
            settingName IN ( 'TO', 'CC', 'BCC' ) AND SettingValue LIKE '%"+email+"%'";
            SqlDataReader rdr;
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows) {
                while (rdr.Read()) {
                    txtResults.Text += rdr[0].ToString() + " - " + rdr[3].ToString()+Environment.NewLine;
                    subscriptions.Add(rdr[0].ToString());
                }
            }
            rdr.Close();
            foreach (string s in subscriptions) {
                sql = @"SELECT CAT.[Path] AS ReportPath 
                        FROM dbo.Subscriptions AS SUB 
                             INNER JOIN dbo.Users AS USR 
                                 ON SUB.OwnerID = USR.UserID 
                             INNER JOIN dbo.[Catalog] AS CAT 
                                 ON SUB.Report_OID = CAT.ItemID 
                             INNER JOIN dbo.ReportSchedule AS RS 
                                 ON SUB.Report_OID = RS.ReportID 
                                    AND SUB.SubscriptionID = RS.SubscriptionID 
                             INNER JOIN dbo.Schedule AS SCH 
                                 ON RS.ScheduleID = SCH.ScheduleID 
                        WHERE SUB.SubscriptionID ='"+s+"'";
                cmd = new SqlCommand(sql, conn);
                string reportPath=cmd.ExecuteScalar().ToString();
                if (reportPath.Length > 0) {
                    txtResults.Text += reportPath + Environment.NewLine;
                }
            }
            conn.Close();
        }
        private void SetADInfoAndCredentials()
        {
			adEntry = new DirectoryEntry("DC=CONTOSO,DC=net", 
				"usuario", "password");
        }

        

        private void dgvSubscriptions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmCheckAD_Resize(object sender, EventArgs e)
        {
            txtEmails.Height = this.Height - 75;
            txtResults.Width = this.Width - txtResults.Left - 25;
            txtResults.Height = this.Height - txtResults.Top - 35;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.checkSubscriptionsEvent();
            this.c++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.c--;
            this.checkSubscriptionsEvent();
        }
    }
    public class ADUser
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool isMapped { get; set; }
    }
}
