var listaRequestLoading = [];

function ShowSuccessMessage(pMessage) {
    MostraMensagem(1, pMessage, 'Base', 'Alerta', $('#Messages'));
}
function ShowInfoMessage(pMessage) {
    MostraMensagem(2, pMessage, 'Base', 'Alerta', $('#Messages'));
}
function ShowWarningMessage(pMessage) {
    MostraMensagem(3, pMessage, 'Base', 'Alerta', $('#Messages'));
}
function ShowErrorMessage(pMessage) {
    MostraMensagem(4, pMessage, 'Base', 'Alerta', $('#Messages'));
}

function ShowValidationResultMessages(validationResult) {
    var hasErrors = validationResult.Erros && validationResult.Erros.length;
    var message = hasErrors ? validationResult.Erros.map(x => x.Message).join("|||") : validationResult.Message;
    validationResult.IsValid = hasErrors ? false : validationResult.IsValid;
    validationResult.IsValid ? MostraMensagem(1, message, 'Base', 'Alerta', $('#Messages')) : MostraMensagem(4, message, 'Base', 'Alerta', $('#Messages'));
}

function ShowOperationSucessMessage() {
    MostraMensagem(1, Mensagens.OPERACAO_REALIZADA, 'Base', 'Alerta', $('#Messages'));
}

function MostraMensagem(pTipoMensagem, pMessage, controller, action, div) {
    RenderPartial(controller, action, { tipo: pTipoMensagem, mensagem: pMessage }).done(function (success) {
        var successm = success.split('|||').join("<br/>");
        div.html(successm);
        $('html, body').animate({ scrollTop: 0 }, 'fast');
        div.fadeIn(2).delay(8000).fadeOut(1600);
    });
    $("#Alertas").show();
    $("#Alertas").delay(8000).fadeOut(1600);
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function RenderPartial(controller, action, data, tipo) {
    var url = ROOT.concat("/", controller, "/", action);
    return $.ajax({
        type: tipo === undefined ? "post" : tipo,
        dataType: "html",
        url: url,
        data: data,
        error: function (status) {
            var errorData = $.parseJSON(status.responseText);
            ShowWarningMessage(errorData[0])
        }
    });
}

function ExecutaJson(controller, action, data, tipo) {
    var url = ROOT.concat("/", controller, "/", action);
    return $.ajax({
        type: tipo === undefined ? "post" : tipo,
        dataType: "json",
        url: url,
        data: data,
        beforeSend: () => showLoading(),
        complete: () => hideLoading(),
        error: function (status) {
            hideLoading();
            try {
                var errorData = $.parseJSON(status.responseText);
                MostraMensagem(2, errorData[0]);
            }
            catch (ex) {
                console.log(status);
            }
        }
    });
}

function forceSubmitOnEnter(e, target) {
    if (e && e.keyCode === 13) {
        $("#" + target).click();
    }
}

$.wait = function (callback, seconds) {
    return window.setTimeout(callback, seconds * 1000);
};

function ShowModalGenerica(controller, action, data) {
    $("#modalGenerica").modal('show');
    Loading($("#ContentmodalGenerica"));
    RenderPartial(controller, action, data, "get").done(function (success) {
        $("#ContentmodalGenerica").html(success);
    });
}

function ShowModalGenericaForm(controller, action, data) {
    $("#modalGenericaForm").modal('show');
    Loading($("#ContentmodalGenericaForm"));
    RenderPartial(controller, action, data, "get").done(function (success) {
        $("#ContentmodalGenericaForm").html(success);
    });
}

function CloseModalGenerica()
{
    $("#modalGenerica").modal('hide');
}

function CloseModalGenericaForm() {
    $("#modalGenericaForm").modal('hide');
}

function Loading(div)
{
    div.html('');
}

function showWaiting()
{
    $.blockUI();
}

function hideWaiting() {
    $.unblockUI();
}

function alto_contraste() {
    if ($('#CSSGlobal').prop("disabled")) {
        $('#CSSGlobalEscuro').prop("disabled", true);
        $('#CSSGlobal').prop("disabled", false);
    } else {
        $('#CSSGlobal').prop("disabled", true);
        $('#CSSGlobalEscuro').prop("disabled", false);
    }
         
} 

function ExecutaJsonRedirect(controller, action, data) {
    var url = ROOT.concat("/", controller, "/", action);
    $.ajax({
        type: "post",
        url: url,
        dataType: 'json',
        crossDomain: true,
        data: data,
        success: function (data) {
            window.location.href = data;
        },
        error: function (status) {
            var errorData = $.parseJSON(status.responseText);
            MostraMensagem(2, errorData[0]);
        }
    });
}

function defaultFailureHandler(response) {
    if (response.status == 400) {
        var jsResponse = JSON.parse(response.responseText);
        var msgErro = jsResponse["MsgErro"];
        ShowErrorMessage(msgErro);
        $("#Alertas").show();
        $("#Alertas").delay(8000).fadeOut(1600);
    } else {
        ShowErrorMessage("Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde");
    }

    hideLoading();
}

function showLoading() {
    listaRequestLoading.push('');
    $('#div-loader').show();
}

function hideLoading() {
    listaRequestLoading.pop();
    if(!listaRequestLoading.length)
        $('#div-loader').hide();
}

