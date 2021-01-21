const form = $('#frm-pesquisar-unidades-tipo-pacto');
var tblUnidadeTipoPacto = $("#tblUnidadeTipoPacto");
var veioDeExclusao = false;
var pageNumber = 0;
var qtdRegistrosPagina = 0;

$(() => {
    tblUnidadeTipoPacto.bootstrapTable({
        pageList: [10, 20, 30, 40, 50]
    });
});

function buscarUnidadesTipoPacto() {
    tblUnidadeTipoPacto.bootstrapTable('refresh');
}

function excluirUnidadeTipoPactoClick(idUnidadeTipoPacto) {
    if (idUnidadeTipoPacto === '0' || !idUnidadeTipoPacto) {
        ShowWarningMessage(Mensagens.REGISTRO_NAO_ENCONTRADO);
        return;
    }

    if (confirm(Mensagens.EXCLUIR_REGISTRO)) {
        excluirUnidadeTipoPacto(idUnidadeTipoPacto);
    }
}

function ajaxTableUnidadesTipoPacto(params) {
    var take = params.data.limit;
    var skip = qtdRegistrosPagina === 1 && veioDeExclusao && pageNumber !== 1 ? ((pageNumber - 2) * take) : params.data.offset;


    $('#hdn-skip').val(skip);
    $('#hdn-take').val(take);
    veioDeExclusao = false;

    var data = form.serialize();

    $.ajax({
        type: 'POST',
        url: '/UnidadeTipoPacto/Index',
        data: data,
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {

            if ((!data.Lista || !data.Lista.length) && !data.camposNaoPreenchidos)
                ShowErrorMessage("Nenhum registro encontrado");

            params.success({
                "rows": data.Lista,
                "total": data.QtRegistros
            });

            qtdRegistrosPagina = data.Lista.length;
            pageNumber = tblUnidadeTipoPacto.bootstrapTable('getOptions').pageNumber;
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });

    return 0;
} 

function excluirUnidadeTipoPacto(idUnidadeTipoPacto) {
    $.ajax({
        type: 'POST',
        url: '/UnidadeTipoPacto/Delete',
        data: { idUnidadeTipoPacto },
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {
            if (!data) {
                ShowWarningMessage(Mensagens.REGISTRO_NAO_ENCONTRADO);
                return;
            }

            ShowOperationSucessMessage();
            veioDeExclusao = true;
            tblUnidadeTipoPacto.bootstrapTable('refresh');
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });
}

function formatterPermitePactoExterior(value, row) {
    return value ? "Sim" : "Não";
}

function formatterAcoes(value, row) {

    return `<a href="UnidadeTipoPacto/Create/${row.IdUnidade_TipoPacto}" class="btn btn-xs btn-primary"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></a>
            <a href="javascript:void(0)" class="btn btn-xs btn-danger" onclick="excluirUnidadeTipoPactoClick(${row.IdUnidade_TipoPacto})">
                <i class="glyphicon glyphicon-remove" aria-hidden="true"></i>
            </a>`;
}