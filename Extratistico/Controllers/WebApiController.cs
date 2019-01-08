using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Areas.Cadastros.Models.Repositorios;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Areas.Extratos.Models.Repositorios;
using Extratistico.Classes.Estatistica;
using Extratistico.Classes.Helpers;
using Extratistico.Models.Entidades;
using Extratistico.Models.EntidadesWebAPI;
using Extratistico.Models.Repositorios;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace Extratistico.Controllers
{
    public class WebApiController : ApiController
    {

        private UsuarioRepository dbUsuario = new UsuarioRepository();
        ExtratoRepository extratoRepository = new ExtratoRepository();
        FrequenciaRepository frequenciaRepository = new FrequenciaRepository();
        CategoriaRepository categoriaRepository = new CategoriaRepository();
        InfoSyncRepository infoSyncRepository = new InfoSyncRepository();

        // GET api/webapi
        public IEnumerable<ExtratoVM> Get()
        {
            return extratoRepository.ExtratoUnificado("rafaelcsmaia");
        }

        // GET api/webapi/5
        public string Get(int id)
        {
            return "value";
        }

        // GET api/webapi/Logon/username?rafael&password?123456
        public string Logon()
        {
            return "value";
        }

        [HttpGet]
        public object AutoPushUpdate(double arg0)
        {
            double serverVersion = 1.72;
            if (serverVersion > arg0)
            {
                return new { Result = true };
            }
            else
            {
                return new { Result = false };
            }
                        
        }

        [HttpGet]
        public object AutoPushAPK(string arg0)
        {

                byte[] _Buffer = null;


                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\AutoPush\\AutoPush.apk"), System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // attach filestream to binary reader
                System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);

                // get total byte length of the file
                long _TotalBytes = new System.IO.FileInfo(System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\AutoPush\\AutoPush.apk")).Length;

                // read entire file into buffer
                _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);

                // close file reader
                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();
            return new { Result = _Buffer };
        }

        [HttpGet]
        public IHttpActionResult DownloadAPK(string arg0)
        {
            //converting Pdf file into bytes array  
            var dataBytes = File.ReadAllBytes(System.Web.HttpContext.Current.Request.MapPath("~\\App_Data\\AutoPush\\AutoPush.apk"));
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);
            return new APKResult(dataStream, Request, "autopush.apk");
        }


        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public object Logon(string arg0, string arg1)
        {
            var user = dbUsuario.SelectSingle(arg0);
            if (user == null){
                return new { Result = "Invalid Username" };
            }
            else if (user.Password != HashHelper.ComputeHash(arg1))
            {
                return new { Result = "Invalid Password" };
            }
            else
            {
                return new { Result = "OK" };
            }
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public IEnumerable<ExtratoVM> GetExtrato(string arg0, int arg1)
        {
            var e = extratoRepository.ExtratoUnificado(arg0);
            return e.OrderByDescending(x => x.Data).Skip((arg1 - 1) * 20).Take(20);
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public InfoExtratoVM GetResumo(string arg0)
        {
            var e = new InfoExtratoVM();
            e.SaldoTotal = Math.Round(extratoRepository.SaldoTotal(arg0) + extratoRepository.CreditoCategorizadoTotal(arg0),2);
            e.DinheiroTotal = Math.Round(extratoRepository.DinheiroTotal(arg0),2);
            e.CartaoTotal = Math.Round(extratoRepository.CartaoCreditoTotal(arg0) - extratoRepository.CreditoCategorizadoTotal(arg0),2);
            return e;
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public IEnumerable<CategoriaVM> GetCategorias(string arg0)
        {            
            return categoriaRepository.Select(arg0);
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public IEnumerable<InfoSync> GetInfoSync(string arg0)
        {
            return infoSyncRepository.GetInfoSync();
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public List<ResumoCategoria> GetResumoDespesasCategoria(string arg0, int arg1, int arg2)
        {
            DateTime dtIni, dtFim;
            if (arg2 != 0)
            {
                dtIni = new DateTime(arg1, arg2, 1);
                dtFim = new DateTime(arg1, arg2, DateTime.DaysInMonth(arg1, arg2));
            }
            else
            {
                dtIni = new DateTime(arg1, 1, 1);
                dtFim = new DateTime(arg1, 12, DateTime.DaysInMonth(arg1, 12));
            }

            return extratoRepository.ResumoDespesasCategoria(dtIni, dtFim, arg0);
        }

        // GET api/webapi/Logon/arg0/arg1
        [HttpGet]
        public List<ProjectedMonth> GetProjecao(string arg0, int arg1)
        {
            List<ProjectedMonth> result = new List<ProjectedMonth>();
            var d = extratoRepository.WebApiReportProjecao(arg0, arg1, frequenciaRepository.Select(arg0));
            foreach (int item in  d.Select(x=>x.Date.Month).Distinct())
	        {
                result.Add(new ProjectedMonth()
                {
                    MonthName = new DateTime(2015, item, 1).ToString("MMM", new CultureInfo("pt-BR")),
                    Dates = d.Where(x => x.Date.Month == item).ToList()
                });
	        }    
            
            return result;
        }

        // POST api/webapi
        [HttpPost]
        public object CadastarCreditoDebitoDinheiro(string arg0, [FromBody]DinheiroVM d)
        {
            if (d == null)
                return new { Result = "Requisição inválida!" };

            d.CriarDataManual();
            extratoRepository.CadastrarGastoDinheiro(d, arg0);
            extratoRepository.Commit();
            return new { Result = "OK" };
        }

        // POST api/webapi
        [HttpPost]
        public object ExcluirExtrato(string arg0, [FromBody]ExtratoVM e)
        {
            if (e == null)
                return new { Result = "Requisição inválida!" };

            extratoRepository.ExcluirExtratoCodigo(arg0, e.Procedencia, e.Documento);
            extratoRepository.Commit();
            return new { Result = "OK" };
        }

        // GET api/webapi/Login/arg0/arg1
        [HttpGet]
        public object Login(string arg0, string arg1)
        {
            string guid = Guid.NewGuid().ToString();
            string url = @"amqp://bfgtgzni:DijKhp7DJsFS3jrt3mA1w3y7ePfkHxm_@jellyfish.rmq.cloudamqp.com/bfgtgzni";
            ConnectionFactory connFactory = new ConnectionFactory();
            connFactory.Uri = url.Replace(@"amqp://", @"amqps://");
            //
            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                // The message we want to put on the queue
                JsonMessage j = new JsonMessage()
                {
                    Sender = guid,
                    Message = "Login",
                    JsonData = new { username = arg0, password = arg1 }
                };

                // the data put on the queue must be a byte array
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(j));
                // ensure that the queue exists before we publish to it
                var queueName = "Request";
                bool durable = true;
                bool exclusive = false;
                bool autoDelete = false;
                channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);
                // publish to the "default exchange", with the queue name as the routing key
                var exchangeName = "";
                var routingKey = "Request";
                channel.BasicPublish(exchangeName, routingKey, null, data);
            }

            int tries = 0;
            while (tries < 60)
            {
                using (var conn = connFactory.CreateConnection())
                using (var channel = conn.CreateModel())
                {
                    // ensure that the queue exists before we access it
                    //channel.QueueDeclare("queue1", false, false, false, null);
                    var queueName = "Response";
                    // do a simple poll of the queue
                    var data = channel.BasicGet(queueName, false);

                    while (data != null)
                    {
                        JsonMessage message = JsonConvert.DeserializeObject<JsonMessage>(Encoding.UTF8.GetString(data.Body));
                        if (message.Sender == guid)
                        {
                            channel.BasicAck(data.DeliveryTag, false);
                            return new { Result = message.Message };
                        }
                        data = channel.BasicGet(queueName, false);
                    }

                }
                tries++;
                Thread.Sleep(1000);

            }
            return new { Result = "Timeout" };
        }


        // PUT api/webapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/webapi/5
        public void Delete(int id)
        {
        }
    }

    public class APKResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public APKResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}
