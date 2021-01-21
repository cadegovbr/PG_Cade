using DomainValidation.Validation;
using PGD.Domain.Entities;
using PGD.Domain.Specifications.Pactos;

namespace PGD.Domain.Validations.Pactos
{
    public class PactoValidation : Validator<Pacto>
    {
        public PactoValidation()
        {
            /*CADE
             * Comentado para permitir data de plano de trabalho retroativa
             */
            //var DataInicioPacto = new DataDeveSerMaiorQueAtual();            
            //base.Add("DataInicioPacto", new Rule<Pacto>(DataInicioPacto, "'Data prevista de início do plano de trabalho' não pode ser inferior a atual."));

            var ProdutosPacto = new DevemExistirProdutosNoPacto();
            base.Add("Produtos", new Rule<Pacto>(ProdutosPacto, "Devem existir produtos para o plano de trabalho."));

            var ProdutosAvaliados = new ProdutosAvaliadosComResultado();
            base.Add("ProdutosAvaliados", new Rule<Pacto>(ProdutosAvaliados, "Pelo menos um produto do plano de trabalho deve ter seu resultado avaliado."));

            var NumeroHoras = new NumerodeHorasCronogramaDiferentePacto();
            base.Add("Cronogramas", new Rule<Pacto>(NumeroHoras, "Horas no cronograma incompatíveis com Carga Horária total."));

            var CargaHoraria = new CargaHorariaCronogramaDiferentePacto();
            base.Add("Cronogramas.Horas", new Rule<Pacto>(CargaHoraria, "Cronograma possui dias com horas maiores que a definida no 'Carga Horária' do plano de trabalho."));

            var CargaPacto = new ReducaoDeveSerMenorQueOito();
            base.Add("CargaHoraria", new Rule<Pacto>(CargaPacto, "Caso possuir 'Redução da carga horaria' o valor permitido deverá ser menor que 8 horas e diferente de 0."));

            var TAPPacto = new TAPPacto();
            base.Add("TAP", new Rule<Pacto>(TAPPacto, "O TAP é obrigatório para esse tipo de plano."));

            var PactoNoExterior = new PactoNoExterior();
            base.Add("Pacto no Exterior", new Rule<Pacto>(PactoNoExterior, "O número do processo SEI é obrigatório para planos de trabalho executados no exterior."));

            var Suspensao = new CamposObrigatoriosSuspensao();
            base.Add("MotivoSuspensao", new Rule<Pacto>(Suspensao, "O campo Motivo é obrigatório para cadastro de Suspensão de plano de trabalho."));

            var DataSuspensao = new DataInicioSuspensao();
            base.Add("DataSuspensao", new Rule<Pacto>(DataSuspensao, "A data de início da suspensão deve ser menor que a data prevista de término do plano de trabalho."));

            var RetornoSuspensao = new DataRetornoSuspensaoDeveSerMaiorQueInicio();
            base.Add("DataRetornoSuspensao", new Rule<Pacto>(RetornoSuspensao, "A data de retorno deve ser maior que a data de início da suspensão ."));

            var JustificativaPactoVisualizacaoRestrita = new JustificativaPactoVisualizacaoRestrita();
            base.Add("JustificativaPactoVisualizacaoRestrita", new Rule<Pacto>(JustificativaPactoVisualizacaoRestrita, "É obrigatório informar uma justificativa para restringir a visualização do plano de trabalho."));

            var AvaliacaoDetalhadaValida = new AvaliacaoDetalhadaValida();
            base.Add("AvaliacaoDetalhadaValida", new Rule<Pacto>(AvaliacaoDetalhadaValida, "É obrigatório avaliar todos os critérios."));
        }
    }
}