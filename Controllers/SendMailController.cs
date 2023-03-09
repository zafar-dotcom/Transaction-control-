using Microsoft.AspNetCore.Mvc;
using SendEmailViaSMTP.Models;
using System.Net;
using System.Net.Mail;

namespace SendEmailViaSMTP.Controllers
{
   // [Route("api/[controller]")]
    public class SendMailController : Controller
    {
      //  [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult index(MailModel obj)
        {
            if(ModelState.IsValid)
            {
                MailMessage mail=new MailMessage();
                mail.To.Add(obj.To);
                mail.From=new MailAddress(obj.From);
                mail.Subject=obj.Subject;
                string Body=obj.Body;
                mail.Body = Body;
                mail.IsBodyHtml=true;   
                SmtpClient smtp=new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("mohammadzafar12555@gmail.com", "zylffubbehtzpmku");
                smtp.EnableSsl= true;
                smtp.Send(mail);
                TempData["success"] = "Mail has been sent successfully";
                return View("index",obj);
            }
            else
            {
                return View();
            }
        }
       // [Route("Attachfile")]
        public ActionResult MailWithAttachment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MailWithAttachment(MailModelForAttachment obj, IFormFile FormFile)
        {
            if (ModelState.IsValid)
            {
                string from = "mohammadzafar12555@gmail.com";
                using(MailMessage mail=new MailMessage(from, obj.To))
                {
                    mail.Subject = obj.Subject;
                    mail.Body = obj.Body;
                    if (FormFile != null)
                    {
                        string filename = Path.GetFileName(FormFile.FileName);
                        byte[] filebyte;
                        using(MemoryStream ms=new MemoryStream())
                        {
                            FormFile.CopyTo(ms);
                            filebyte = ms.ToArray();
                            var attachment = new Attachment(new MemoryStream(filebyte), filename);
                            mail.Attachments.Add(attachment);
                        }

                    }
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential nc = new NetworkCredential(from, "zylffubbehtzpmku");
                    smtp.UseDefaultCredentials= false;
                    smtp.Credentials= nc;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    TempData["attachment"] = "Mail has been sent successfully";
                    return View("MailWithAttachment", obj);


                }
            }
            else
            {
                return View();
            }
        }
    }
}
