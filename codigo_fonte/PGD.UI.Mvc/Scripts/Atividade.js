function AddRow() {
    var TipoFaixa = $("#TipoFaixa").val();
    var DuracaoPGD = $("#DuracaoPGD").val();
    var DuracaoPresencial = $("#DuracaoPresencial").val();
}

function RemoveRow(id, index) {
    $('#' + id).hide();
    $("#Tipos_" + index + "__Excluir").val('true');
}

function AddTipoAtividade() {
    var index = TiposCount - 1;

    $.ajax({
        type: "post",
        dataType: "html",
        url: ROOT + '/Atividade/AddTipoAtividade',
        data: { pCount: TiposCount },
        success: function (response) {
            var table = $('#tableAtividades > tbody:last-child');
            table.append(response);
            TiposCount++;
        }
    });
}