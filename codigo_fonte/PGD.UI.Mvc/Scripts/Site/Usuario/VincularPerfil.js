const form = $('#frm-vincular');
const tblPerfilUnidade = $('#tbl-perfil-unidade');
const divSelectUnidade = $('#div-slt-unidade');
const sltUnidade = $('#slt-unidade');
const sltPerfil = $('#slt-perfil');
const hiddenUnidade = $('#hdn-id-unidade');
var temUmUltimaPagina = false;
var veioDeExclusao = false;
var pageNumber = 0;
var qtdRegistrosPagina = 0;
var qtdRegistrosTotal = 0;
var veioDeInclusao = false;

$(() => {
    tblPerfilUnidade.bootstrapTable({
        pageList: [10, 20, 30, 40, 50]
    });
    
    sltUnidade.select2({
        ajax: {
            url: '/Usuario/BuscarUnidadesPorNomeOuSigla',
            dataType: 'json',
            delay: 500,
            data: (params) => {
                return {
                    query: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data.Lista,
                        function (obj) {
                            return { id: obj.IdUnidade, text: `${(obj.Sigla ? obj.Sigla + " -" : "")} ${obj.Nome}` };
                        })

                };
            }
        },
        placeholder: 'Digite o nome ou sigla da unidade',
        minimumInputLength: 2,
        allowClear: true,
        language: {
            inputTooShort: function () { return "Inicie a pesquisa com 2 caracteres"; },
            noResults: function () { return "Nenhum resultado encontrado"; },
            searching: () => "Buscando unidades"
        },
        width: '100%'
    });

    registerChangeDropDowns();
});

function registerChangeDropDowns() {
    sltUnidade.change(() => selecionarUnidade(sltUnidade.val()));
    sltUnidade.on("select2:close", () => { sltUnidade.valid() });
    sltPerfil.change(() => {
        if (sltPerfil.val() === Enum.Perfil.Administrador.toString()) {
            divSelectUnidade.hide();
            selecionarUnidade(ID_UNIDADE_SEM_EXERCICIO);
        } else {
            divSelectUnidade.show();
            sltUnidade.val(null).trigger("change");
        }
    });
}

function refreshTabelaPerfisVinculados() {
    tblPerfilUnidade.bootstrapTable('refresh');
}

function vincular() {
    if (!form.valid()) {
        var validate = form.validate();
        var erros = validate.errorList.map(x => x.message).join("|||");
        ShowWarningMessage(erros);
        return;
    }

    var data = form.serialize();

    $.ajax({
        type: 'POST',
        url: '/Usuario/VincularPerfil',
        data: data,
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {
            if (data.camposNaoPreenchidos) {
                ShowWarningMessage(data.camposNaoPreenchidos.join('|||'));
                return;
            }

            ShowValidationResultMessages(data.ValidationResult);
            if (data.ValidationResult.IsValid) {
                veioDeInclusao = true;
                limparCamposVincular();
                refreshTabelaPerfisVinculados();
            }
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });
}

function limparCamposVincular() {
    sltUnidade.val(null).trigger("change");
    resetForm();
    divSelectUnidade.show();
}

function selecionarUnidade(valor) {
    hiddenUnidade.val(valor);
}

function resetForm() {
    var validator = form.validate();
    var errors = form.find(".field-validation-error span");
    errors.each(function () { validator.settings.success($(this)); })
    validator.resetForm();
    form.trigger('reset');
}

function ajaxTablePerfilUnidade(params) {

    var idUsuario = $('#hdn-id-usuario').val();
    var take = params.data.limit;
    var skip = qtdRegistrosPagina === 1 && veioDeExclusao && pageNumber !== 1 ? ((pageNumber - 2) * take) : params.data.offset;

    if (veioDeInclusao && qtdRegistrosTotal > 0) {
        veioDeInclusao = false;

        setTimeout(() => {
            tblPerfilUnidade.bootstrapTable('selectPage', 1);
        }, 10);
        return 0;
    }

    veioDeInclusao = false;
    veioDeExclusao = false;

    $.ajax({
        type: 'POST',
        url: '/Usuario/BuscarUnidadesPerfisUsuario',
        data: { idUsuario, take, skip },
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {
            qtdRegistrosPagina = data.Lista.length;
            qtdRegistrosTotal = data.QtRegistros;
            params.success({
                "rows": data.Lista,
                "total": data.QtRegistros
            });
            pageNumber = tblPerfilUnidade.bootstrapTable('getOptions').pageNumber;
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });

    return 0;
} 

function excluirPerfilUnidadeClick(idUsuarioPerfilUnidade) {
    if (confirm(Mensagens.EXCLUIR_REGISTRO)) {
        excluirPerfilUnidade(idUsuarioPerfilUnidade);
    }
}

function excluirPerfilUnidade(idUsuarioPerfilUnidade) {
    $.ajax({
        type: 'POST',
        url: '/Usuario/ExcluirVinculo',
        data: { idUsuarioPerfilUnidade },
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {
            ShowValidationResultMessages(data.ValidationResult);
            if (data.ValidationResult.IsValid) {
                veioDeExclusao = true;
                tblPerfilUnidade.bootstrapTable('refresh');
            }
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });
}

function formatterAcoes(value, row) {
    if (row.NomePerfil === 'Solicitante')
        return '';

    return `<span class="btn" onclick="excluirPerfilUnidadeClick(${row.Id})"><i class="fas fa-times-circle fa-2x danger"></i></span>`;
}

function formatterUnidade(value, row) {
    return `${(row.SiglaUnidade ? row.SiglaUnidade + " - " : "")}${row.NomeUnidade}`;
}