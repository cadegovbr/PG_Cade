using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.Interfaces
{
    public interface IPactoAppService
    {
        PactoViewModel Adicionar(PactoViewModel pactoViewModel, bool isDirigente, UsuarioViewModel usuarioViewModel);
        IEnumerable<PactoViewModel> ObterTodos();
        IEnumerable<PactoViewModel> ObterTodos(string include);
        IEnumerable<PactoViewModel> ObterTodos(PactoViewModel objFiltro, bool incluirUnidadesSubordinadas);
        IEnumerable<CronogramaViewModel> ObterTodosCronogramasCpfLogado(string cpf, List<int> idsSituacoes = null,
            DateTime? dataInicial = null, DateTime? dataFinal = null, int? idUnidade = null);
        List<int> ObterSituacoesPactoValido();
        PactoViewModel ObterPorId(int id);
        PactoViewModel Atualizar(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao);
        PactoViewModel AtualizarStatus(PactoViewModel pactoViewModel, UsuarioViewModel usuario, eAcaoPacto eAcao, bool commit = true);

        bool PodeEditar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeDeletar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeAssinar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeInterromper(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeSuspender(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeVoltarSuspensao(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeNegar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeAvaliar(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeEditarEmAndamento(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeEditarObservacaoProduto(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);

        bool PodeVisualizarAvaliacaoProduto(PactoViewModel pacto, UsuarioViewModel usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);

        PactoViewModel Remover(PactoViewModel pactoViewModel, UsuarioViewModel usuariologado);

        PactoViewModel BuscarPorId(int id);

        bool PossuiPactoPendencias(UsuarioViewModel usuario);

        FeriadoViewModel ObterFeriado(DateTime data);

        IEnumerable<FeriadoViewModel> ObterTodosFeriadosPorData(DateTime dataInicio);
        int BuscaStatusAssinatura(PactoViewModel objFiltro);

        void IniciarAutomaticamente();

        PactoViewModel OrdemVigenteProdutos(OrdemServicoViewModel ordemVigente, PactoViewModel pacto);

        PactoViewModel ZeraRestanteCronograma( PactoViewModel pacto);

        List<PactoViewModel> GetPactosConcorrentes(DateTime dataInicio, DateTime dataFinal, string cpfUsuario, int idPacto);
        ProdutoViewModel AtualizarObservacaoProduto(ProdutoViewModel produto, UsuarioViewModel usuarioVM);

        ProdutoViewModel AtualizarSituacaoProduto(ProdutoViewModel produto, UsuarioViewModel usuarioVM);

        bool PodeCancelarAvaliacao (PactoViewModel pactoVM, UsuarioViewModel user, bool isDirigente, bool unidadePactoESubordinadaUnidadeUsuario);        
        bool PodeVisualizarPactuadoAvaliado(PactoViewModel pactoVM);
        PactoViewModel CancelarAvaliacao(PactoViewModel pactoVM, AvaliacaoProdutoViewModel avaliacaoProdutoVM, UsuarioViewModel user, eAcaoPacto acao);
        ValidationResult ValidarDataHoraSuspensaoInterrupcao(PactoViewModel pactoVM, DateTime dataInicioSuspensao, TimeSpan horasConsideradas, Domain.Enums.Operacao operacao );
        ValidationResult ValidarDataConclusaoAntecipada(PactoViewModel pacto, DateTime dataConclusaoAntecipada);
        
        bool PodeVisualizar(PactoViewModel pactoVM, UsuarioViewModel usuarioVM, bool isDirigente, bool unidadePactoESubordinadaUnidadeUsuario);

    }
}
