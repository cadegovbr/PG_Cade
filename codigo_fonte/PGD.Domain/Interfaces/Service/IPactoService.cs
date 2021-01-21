using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IPactoService:IService<Pacto>
    {
        IEnumerable<Pacto> ConsultarPactos(Pacto objFiltro, bool incluirUnidadesSubordinadas = false);
        IEnumerable<Pacto> ObterTodos(string include);
        IEnumerable<Pacto> ObterTodosAtrasados();
        IEnumerable<Pacto> ObterTodosAIniciarHoje();
        IEnumerable<Cronograma> ObterTodosCronogramasCpfLogado(string cpf, List<int> idsSituacoes = null,
            DateTime? dataInicial = null, DateTime? dataFinal = null, int? idUnidade = null);
        List<int> ObterSituacoesPactoValido();
        Pacto BuscarPorId(int id);
        bool PossuiPactoPendencias(Usuario usuario);
        Pacto AtualizarSuspender(Pacto obj, Usuario usuariologado, List<PGD.Domain.Enums.Perfil> Perfis);
        int BuscaStatusAssinatura(Pacto objFiltro);
        Pacto AtualizarStatus(Pacto obj, Usuario usuariologado,eAcaoPacto eAcaoPacto);

        bool PodeEditar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeDeletar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeAssinar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeInterromper(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeSuspender(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeVoltarSuspensao(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeNegar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeAvaliar(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeEditarEmAndamento(Pacto pacto, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        Pacto RemoverAtrasado(Pacto obj);
        Pacto IniciarPacto(Pacto obj);
        Pacto Atualizar(Pacto pacto, int idPacto);
        Pacto AtualizarStatus(Pacto pacto, Usuario usr, eAcaoPacto eAcao, bool isDirigente, bool commit = true);
        void AtualizaEstadoEntidadesRelacionadas(Pacto pacto);

        ValidationResult ValidarDataHoraSuspensaoInterrupcao(Pacto pacto, DateTime dataInicioSuspensao, TimeSpan horasConsideradas, Domain.Enums.Operacao operacao);
        ValidationResult ValidarDataConclusaoAntecipada(Pacto pacto, DateTime dataConclusaoAntecipada);

        List<Pacto> GetPactosConcorrentes(DateTime dataInicio, DateTime dataFinal, string cpfUsuario, int idPacto);
        IEnumerable<Pacto> ObterTodosEmAndamentoComPrazoFinalizado();
        IEnumerable<Pacto> ObterTodosSuspensosComPrazoParaRetorno();
        Pacto FinalizarPacto(Pacto obj, bool commit = false);
        Pacto ReiniciarPacto(Pacto pacto, bool commit = false);
        bool PodeEditarObservacaoProduto(Pacto pactoVM, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);

        bool PodeVisualizarAvaliacaoProduto(Pacto pactoVM, Usuario usr, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);

        bool PodeCancelarAvaliacao(Pacto pacto, Usuario usuariologado, bool isDirigente, bool unidadePactoEhSubordinadaUnidadeUsuario);
        bool PodeVisualizarPactuadoAvaliado(Pacto pacto);
        bool PodeVisualizar(Pacto pacto, Usuario usuario, bool isDirigente, bool unidadePactoESubordinadaUnidadeUsuario);

        DateTime ObterDataMinimaSuspensaoPacto();
    }
}
