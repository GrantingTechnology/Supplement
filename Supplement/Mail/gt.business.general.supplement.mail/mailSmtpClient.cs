using gt.business.log;
using gt.entity.general.model;
using gt.entity.general.Resource;
using gt.entity.general.settings;
using System.Net.Mail;

namespace gt.business.general.supplement.mail
{

    public class mailSmtpClient {

        #region attribute
        private bool _confirmsendmail = false;
        /// <summary>
        /// Validation for single shipping orders, exclusived!
        /// </summary>
        public bool? uniquesend_completed = null;
        #endregion

        #region  Contructors
        public mailSmtpClient() { }
        public mailSmtpClient(bool confirmSend = false) { _confirmsendmail = confirmSend; }
        #endregion


        public List<string> SendToFail = new List<string>();

        /// <summary>
        /// Executes the Request for Email (s) trigger (s)
        /// </summary>
        /// <param name="item_collection">Type:tbl_mail_message, list itens mail</param>
        /// <returns>Type:Bolean return status execution</returns>
        public bool Send(List<mail_message> item_collection) {
            #region run
            try {
                if (item_collection.Count <= 0)
                    throw new System.InvalidOperationException("Collection not found!");
                _get_ItemCollection(ref item_collection);
                if (item_collection.Count == 1) {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    uniquesend_completed = item_collection.ToList().FirstOrDefault().success;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    return (bool)uniquesend_completed;
                }

            } catch { uniquesend_completed = false; return false; }

            return true;
            #endregion
        }

        /// <summary>
        /// Scroll through items for send Email
        /// </summary>
        /// <param name="item_collection">Type:tbl_mail_message, list with itens mail</param>
        private void _get_ItemCollection(ref List<mail_message> item_collection) {
            #region run

            string logsendmail = mail_pt.mail_send_log_fail;

            try {
                MailMessage msg = new MailMessage();
                bool sendMail = false;
                foreach (var item in item_collection) {
                    logsendmail = logsendmail.Replace("[T[MAIL]]",item.to);
                    msg.From = new MailAddress(item.from);
                    msg.To.Add(item.to);
                    if (item.cc != null)
                        msg.CC.Add(item.cc);
                    if (item.co != null)
                        msg.Bcc.Add(item.co);
                    msg.Subject = item.subject;
                    msg.Body = item.body;
                    msg.IsBodyHtml = true;
                    mailSmtpClient._setSend(ref msg,out sendMail); //Send Mail
                    if (!sendMail)
                        _failSend(item);
                    else {
                        item.success = true;
                        logsendmail = mail_pt.mail_send_log_sucess;
                    }
                    if (_confirmsendmail) { _ConfirmSend(item); }
                }
            } catch { } finally {

                Exception e = new Exception(logsendmail);
                Register.Log(this,e.Message);

            }
            #endregion
        }

        /// <summary>
        /// Mail with fail will be send posteriorly
        /// </summary>
        /// <param name="item">Object type mail_message: container item values</param>
        private static void _failSend(mail_message item) {

            //using (Base run = new general.Base()) {
            //    run.invokeroutine = enumerator.invokeRoutine.ins_adm_base_mailuser_fail;
            //    run.controlPopulation(ref item,enumerator.invokeRoutine.ins_adm_base_mailuser_fail);
            //}

        }

        /// <summary>
        /// The attempted firing, smtp configuration, port and send
        /// </summary>
        /// <param name="msg">Type:MailMessage, contains all objects in the itemCollection </param>
        /// <param name="status">Type:Bolean return status of execution</param>
        private static void _setSend(ref MailMessage msg,out bool status) {

            try {
                using (SmtpClient client = new SmtpClient()) {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Host = Settings.SMTPRelay;
                    client.Port = Settings.SMTPPort;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);
                    status = true;
                }
            } catch (Exception ex) {
                status = false;
                Register.Log("Fail send add new user:",ex.Message,true);
            }
        }



        /// <summary>
        /// Resend records on hold
        /// </summary>
        /// <param name="item">Object type mail_message: Items to be resent </param>
        private void _ConfirmSend(mail_message item) {
            //using (Base run = new general.Base()) {
            //    run.controlPopulation(ref item,enumerator.invokeRoutine.sel_adm_base_mailuser_confirm);
            //}
        }


    }


}

