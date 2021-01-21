
function atualizaControleContextual(idControleDestino, controller, acao, parametros, tipoControleDestino, valorControleDestino, doneCallback) {
    ExecutaJson(controller, acao, resolverValoresParametros(parametros), "post").done(function (retorno) {
        if (tipoControleDestino == 1) {
            //dropdown
            var drpDestino = $('#' + idControleDestino);
            drpDestino.empty();
            $.each(retorno,
                function (index, itemData) {
                        drpDestino.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                });
            if (valorControleDestino != "" && valorControleDestino != "0") {
                drpDestino.val(valorControleDestino);
            }
            drpDestino.trigger("change");
        } else if (tipoControleDestino == 2) {
            //textbox
            var controleDestino = $('#' + idControleDestino);
            controleDestino.val(retorno);
        } else {
            doneCallback(retorno);
        }
    });
}

function resolverValoresParametros(parametros) {
    //a partir da string json que define os parâmetros, altera o Id das dropdown pelo valor das mesmas.
    var jsParametros = JSON.parse(parametros);
    Object.keys(jsParametros).forEach(function (k) {
        jsParametros[k] = $('#' + jsParametros[k]).val();
    });
    return jsParametros;
}

function hourTohhmm(hours) {
    var hr = Math.floor(Math.abs(hours));
    var min = Math.floor((Math.abs(hours) * 60) % 60);
    var result = hr + ":" + (min < 10 ? "0" : "") + min;
    return result;
}

function atualizaValorCampo(nomeCampo, valorCampo) {
    $("[name='" + nomeCampo + "']").each(function () {
        this.value = valorCampo;
    });
}