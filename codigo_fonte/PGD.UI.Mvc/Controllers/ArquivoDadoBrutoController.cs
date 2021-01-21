using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PGD.Application.Interfaces;
using PGD.Domain.Interfaces.Service;
using PGD.Domain.Entities.RH;
using System.IO;
using System.Configuration;
using PGD.UI.Mvc.Helpers;
using PGD.Domain.Enums;

namespace PGD.UI.Mvc.Controllers
{
    public class ArquivoDadoBrutoController : BaseController
    {
        IArquivoDadoBrutoAppService _arquivoDadoBrutoAppService;

        public ArquivoDadoBrutoController(IUsuarioAppService usuarioAppService, IArquivoDadoBrutoAppService arqDadoBrutoAppService) : base(usuarioAppService)
        {
            _arquivoDadoBrutoAppService = arqDadoBrutoAppService;
        }

        // GET: ArquivoDadoBruto 
        public ActionResult Index()
        {
            return View(_arquivoDadoBrutoAppService.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(SearchArquivosDadosBrutosViewModel obj)
        {
            var arqDadosBrutos = new List<ArquivoDadoBrutoViewModel>();

            if (!obj.Ano.HasValue)
            {
                return View(_arquivoDadoBrutoAppService.ObterTodos());
            }
            else
            {
                return View(_arquivoDadoBrutoAppService.ObterPorAno(obj.Ano.Value));
            }
        }


        public FileResult DownloadFile(string file, string ano)
        {
            file = ConfigurationManager.AppSettings["DiretorioArquivos"].ToString() + file;
            var fileName = Path.GetFileName(file);

            try
            {
                return File(file, "application/force-download", fileName);
            }
            catch(Exception ex)
            {
                // LogManager.LogErro(ex, $"Erro ao fazer download do arquivo: {fileName}" );
                throw;
            }            
        }

        public ActionResult DownloadFileCd(string fileName )
        {
            try
            {
                string filepath = ConfigurationManager.AppSettings["DiretorioArquivos"].ToString() + fileName;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };

                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(filedata, contentType);
            }
            catch (Exception ex)
            {
                // LogManager.LogErro(ex, $"Erro ao fazer download do arquivo: {fileName}");
                throw;
            }
        }
    }
}
