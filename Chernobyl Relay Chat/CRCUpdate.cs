using GitHubUpdate;
using Octokit;
using System;
using System.Media;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    class CRCUpdate
    {
        // Note to self: prereleases are ignored

        public static bool CheckFirstUpdate()
        {
            UpdateChecker updateChecker = new UpdateChecker("itsAnchorpoint", "Chernobyl-Relay-Chat-Rebirth", "0.4.0");
            UpdateType updateType;
            try
            {
                updateType = updateChecker.CheckUpdate().Result;
            }
            catch (RateLimitExceededException)
            {
                return false;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    return (ex is HttpRequestException);
                });
                MessageBox.Show(CRCStrings.Localize("update_failed"), CRCStrings.Localize("crc_name"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (updateType != UpdateType.None)
            {
                string releaseNotes = updateChecker.RenderReleaseNotes().Result;
                if (CRCOptions.SoundNotifications)
                    SystemSounds.Asterisk.Play();
                using (UpdateForm updateForm = new UpdateForm((updateType == UpdateType.Major || updateType == UpdateType.Minor), releaseNotes))
                {
                    DialogResult dialogResult = updateForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                        updateChecker.DownloadAsset("CRCR.zip");
                }
                return true;
            }
            return false;
        }

        public static async Task<bool> CheckUpdate()
        {
            UpdateChecker updateChecker = new UpdateChecker("itsAnchorpoint", "Chernobyl-Relay-Chat-Rebirth", "0.4.0");
            UpdateType updateType;
            try
            {
                updateType = await updateChecker.CheckUpdate();
            }
            catch (RateLimitExceededException)
            {
                return false;
            }
            // If it got past the first one this is probably just a temporary issue
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    return (ex is HttpRequestException);
                });
                return false;
            }
            if (updateType != UpdateType.None)
            {
                string releaseNotes = await updateChecker.RenderReleaseNotes();
                if (CRCOptions.SoundNotifications)
                    SystemSounds.Asterisk.Play();
                CRCGame.OnUpdate(CRCStrings.Localize("update_notice"));
                using (UpdateForm updateForm = new UpdateForm((updateType == UpdateType.Major || updateType == UpdateType.Minor), releaseNotes))
                {
                    DialogResult dialogResult = updateForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                        updateChecker.DownloadAsset("CRCR.zip");
                }
                return true;
            }
            return false;
        }
    }
}
