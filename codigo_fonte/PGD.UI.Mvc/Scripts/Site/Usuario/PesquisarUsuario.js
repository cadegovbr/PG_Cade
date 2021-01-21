const form = $('#frm-consultar-usuarios');
const tblUsuarios = $('#tbl-usuarios');
const sltUnidade = $('#slt-unidade');

$(() => {
    tblUsuarios.bootstrapTable({
        pageList: [10, 20, 30, 40, 50]
    });
    
    sltUnidade.select2({
        ajax: {
            url: 'Usuario/BuscarUnidadesPorNomeOuSigla',
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

});

function buscarUsuarios() {
    tblUsuarios.data('pode-pesquisar', true);
    tblUsuarios.bootstrapTable('refresh');
}

function ajaxTableUsuarios(params) {
    var podePesquisar = tblUsuarios.data('pode-pesquisar');

    if (!podePesquisar)
        return params.success({
            "rows": [],
            "total": 0
        });

    $('#hdn-skip').val(params.data.offset);
    $('#hdn-take').val(params.data.limit);
    
    var data = form.serialize();

    $.ajax({
        type: 'POST',
        url: '/Usuario/Index',
        data: data,
        dataType: 'json',
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        success: (data) => {
            if (data.camposNaoPreenchidos)
                ShowWarningMessage(data.camposNaoPreenchidos[0]);

            if ((!data.Lista || !data.Lista.length) && !data.camposNaoPreenchidos)
                ShowErrorMessage("Nenhum registro encontrado");

            params.success({
                "rows": data.camposNaoPreenchidos ? [] : data.Lista,
                "total": data.camposNaoPreenchidos ? 0 : data.QtRegistros
            });
        },
        error: err => {
            console.log(err);
            hideLoading();
        }
    });

    return 0;
} 

function redirecionarEditarUsuario(idUsuario) {
    window.location.href = `Usuario/VincularPerfil/${idUsuario}`;
}

function formatterAcoes(value, row) {
    return `<span class="btn" onclick="redirecionarEditarUsuario(${row.IdUsuario})"><i class="fas fa-edit fa-2x"></i></span>`;
}

function formatterStatus(value, row) {
    return row.Inativo
        ? `<i class="fas fa-times-circle fa-2x danger"></i>`
        : `<i class="fas fa-check fa-2x success"></i>`;
}

function formatterUnidade(value, row) {
    var retorno = '';
    
    if (!row.PerfisUnidades || !row.PerfisUnidades.length) return retorno;

    var unidadesInformadas = {};

    row.PerfisUnidades.forEach(x => {

        var unidade = unidadesInformadas[x.NomeUnidade] || [];

        if (!unidade.some(y => y === retornarInicialPerfil(x.NomePerfil))) {
            unidade.push(retornarInicialPerfil(x.NomePerfil));
            unidadesInformadas[x.NomeUnidade] = unidade;
        }
    });

    Object.keys(unidadesInformadas).forEach((x, i) => {
        var tamanhoKeys = Object.keys(unidadesInformadas).length;
        var listaPerfis = unidadesInformadas[x];

        retorno += `${x}(`;
        listaPerfis.forEach((y, j) => {
            retorno += `${y}${(j === listaPerfis.length - 1 ? ')' : ', ' )}`;
        });

        retorno += tamanhoKeys - 1 === i ? '' : '<br/>';
    });

    return retorno;
}

function retornarInicialPerfil(nomePerfil) {
    switch (nomePerfil.toLowerCase()) {
        case 'solicitante':
            return 'S';
        case 'dirigente':
            return 'D';
        case 'administrador':
            return 'A';
        default:
            return '';
    }
}